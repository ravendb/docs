using System;
using System.Threading.Tasks;
using Akka;
using Akka.Actor;
using Akka.Hosting;
using Akka.Persistence;
using Akka.Persistence.Hosting;
using Akka.Persistence.Query;
using Akka.Persistence.RavenDb.Hosting;
using Akka.Persistence.RavenDb.Query;
using Akka.Streams;
using Akka.Streams.Dsl;
using Microsoft.Extensions.Hosting;
using Raven.Client.Documents.Session;

namespace AkkaPersistenceSampleApp
{
    #region classes
    // A sale EVENT to be persisted 
    public class Sale(long pricePaid, string productBrand)
    {
        public long Price { get; set; } = pricePaid;
        public string Brand { get; set; } = productBrand;
    }
    
    // MESSAGES for the simulator actor
    public class StartSimulate { }
    public class StopSimulate { }
    
    // Internal state that will be persisted in a SNAPSHOT 
    class SalesActorState
    {
        public long totalSales { get; set; }

        public override string ToString()
        {
            return $"[SalesActorState: Total sales are {totalSales}]";
        }
    }
    
    public class ConsoleHelper
    {
        public static void WriteToConsole(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
    #endregion
    
    #region sales_actor
    public class SalesActor: ReceivePersistentActor
    {
        // The unique actor id
        public override string PersistenceId => "sales-actor";
        
        // The state that will be persisted in SNAPSHOTS
        private SalesActorState _state;
        
        public SalesActor(long expectedProfit, TaskCompletionSource<bool> taskCompletion)
        {
            _state = new SalesActorState
            {
                totalSales = 0
            }; 
            
            // Process a sale:
            Command<Sale>(saleInfo =>
            {
                if (_state.totalSales < expectedProfit)
                {
                    // Persist an EVENT to RavenDB
                    // ===========================
                    
                    // The handler function is executed after the EVENT was saved successfully
                    Persist(saleInfo, _ =>
                    {
                        // Update the latest state in the actor
                        _state.totalSales += saleInfo.Price;

                        ConsoleHelper.WriteToConsole(ConsoleColor.Black,
                            $"Sale was persisted. Phone brand: {saleInfo.Brand}. Price: {saleInfo.Price}");

                        // Store a SNAPSHOT every 5 sale events
                        // ====================================
                        
                        if (LastSequenceNr != 0 && LastSequenceNr % 5 == 0)
                        {
                            SaveSnapshot(_state.totalSales);
                        }
                    });
                }
                else if (!taskCompletion.Task.IsCompleted)
                {
                    Sender.Tell(new StopSimulate());
                    
                    ConsoleHelper.WriteToConsole(ConsoleColor.DarkMagenta,
                        $"Sale not persisted: " +
                        $"Total sales have already reached the expected profit of {expectedProfit}");
                    
                    ConsoleHelper.WriteToConsole(ConsoleColor.DarkMagenta,
                        _state.ToString());
                    
                    taskCompletion.TrySetResult(true);
                }
            });
            
            // Handle a SNAPSHOT success msg
            Command<SaveSnapshotSuccess>(success =>
            {
                ConsoleHelper.WriteToConsole(ConsoleColor.Blue,
                    $"Snapshot saved successfully at sequence number {success.Metadata.SequenceNr}");
                
                // Optionally, delete old snapshots or events here if needed
                // DeleteMessages(success.Metadata.SequenceNr);
            });
            
            // Recover an EVENT
            Recover<Sale>(saleInfo =>
            {
                _state.totalSales += saleInfo.Price;
                
                ConsoleHelper.WriteToConsole(ConsoleColor.DarkGreen,
                    $"Event was recovered. Price: {saleInfo.Price}");
            });
            
            // Recover a SNAPSHOT
            Recover<SnapshotOffer>(offer =>
            {
                var salesFromSnapshot = (long) offer.Snapshot;
                _state.totalSales = salesFromSnapshot;
                
                ConsoleHelper.WriteToConsole(ConsoleColor.DarkGreen,
                    $"Snapshot was recovered. Total sales from snapshot: {salesFromSnapshot}");
            });
        }
    }
    #endregion
    
    #region sales_simulator_actor
    public class SalesSimulatorActor : ReceiveActor
    {
        private readonly IActorRef _salesActor;
        private ICancelable scheduler;

        public SalesSimulatorActor(IActorRef salesActor)
        {
            _salesActor = salesActor;

            // Schedule the first sale simulation immediately and then every 2 seconds:
            scheduler = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.Zero, 
                TimeSpan.FromSeconds(2), Self, new StartSimulate(), Self);
            
            Receive<StartSimulate>(HandleStart);
            Receive<StopSimulate>(HandleStop);
        }

        private void HandleStart(StartSimulate message)
        {
            ConsoleHelper.WriteToConsole(ConsoleColor.Black,
                $"About to simulate a sale...");

            Random random = new Random();
            string[] products = { "Apple", "Google", "Nokia", "Xiaomi", "Huawei" };

            var randomBrand = products[random.Next(products.Length)];
            var randomPrice = random.Next(1, 6) * 100; // 100, 200, 300, 400, or 500

            var nextSale = new Sale(randomPrice, randomBrand);
            _salesActor.Tell(nextSale);
        }
        
        private void HandleStop(StopSimulate message)
        {
            scheduler.Cancel();
            ConsoleHelper.WriteToConsole(ConsoleColor.DarkRed,
                "Simulation stopped");
        }
    }
    #endregion
    
    class Program
    {
        #region main
        static void Main(string[] args)
        {
            var host = new HostBuilder().ConfigureServices((context, services) =>
            {
                // Configure the RavenDB plugin using Hosting:
                //============================================
                
                services.AddAkka("SalesActorSystem", (builder, provider) =>
                {
                    builder.WithRavenDbPersistence(
                        urls: new[] { "http://localhost:8080" },
                        databaseName: "AkkaStorage_PhoneSales",
                        // Use both akka.persistence.journal and akka.persistence.snapshot-store
                        mode: PersistenceMode.Both);

                    builder.WithActors((system, registry) =>
                    {
                        var taskCompletion = new TaskCompletionSource<bool>();
                        long expectedProfit = 1_500;
                        
                        // Create actors:
                        // ==============
                        
                        var salesActor = system.ActorOf(Props.Create(() => 
                            new SalesActor(expectedProfit, taskCompletion)), "sales-actor");
                        
                        var salesSimulatorActor = system.ActorOf(Props.Create(() => 
                            new SalesSimulatorActor(salesActor)), "sales-simulator-actor");
                        
                        // Exit app when sales reach the 'expectedProfit'
                        taskCompletion.Task.Wait();
                        system.Terminate();
                    });
                });
            });
            
            var app = host.Build();
            app.Run();
        }
        #endregion
        
        void QueryPersistenceId(ActorSystem system)
        {
            #region query_1
            // Obtain the RavenDB read journal
            // ===============================
            RavenDbReadJournal readJournal = PersistenceQuery
                .Get(system) // system is your 'ActorSystem' param
                .ReadJournalFor<RavenDbReadJournal>(RavenDbReadJournal.Identifier);
            
            // Issue query 'CurrentPersistenceIds' to the journal
            // ==================================================
            Source<string, NotUsed> allPersistenceIds = readJournal.CurrentPersistenceIds();
            
            // The materializer handles data flow from the persistence storage through the query pipeline
            // ==========================================================================================
            ActorMaterializer materializer = system.Materializer();
            
            // Execute the query and consume the results
            // =========================================
            allPersistenceIds.RunForeach(persistenceId =>
            {
                Console.WriteLine($"ActorID: {persistenceId}");
            }, materializer).Wait();
            #endregion
        }
        
        void QueryEventsById(ActorSystem system)
        {
            #region query_2
            RavenDbReadJournal readJournal = PersistenceQuery
                .Get(system)
                .ReadJournalFor<RavenDbReadJournal>(RavenDbReadJournal.Identifier); 

            // Issue query 'CurrentEventsByPersistenceId'
            Source<EventEnvelope, NotUsed> eventsSource = readJournal
                .CurrentEventsByPersistenceId("sales-actor", 0L, long.MaxValue);
            
            ActorMaterializer materializer = system.Materializer();
            eventsSource.RunForeach(envelope =>
            {
                var saleEvent = (Sale)envelope.Event;
                Console.WriteLine($"Sale Event - Brand: {saleEvent.Brand}, Price: {saleEvent.Price}");
            }, materializer).Wait();
            #endregion
        }
        
        void QueryEventsByTag(ActorSystem system)
        {
            #region query_3
            RavenDbReadJournal _readJournal = PersistenceQuery.Get(system)
                .ReadJournalFor<RavenDbReadJournal>(RavenDbReadJournal.Identifier);

            // Define an offset after which to return results.
            // See the available offset options in the syntax below..
            ChangeVectorOffset cvOffset = 
                new ChangeVectorOffset("RAFT:1-hJ9jo4rRBEKs/kqNXV107Q TRXN:1169-5LEbeyPG40eQiq6fnnCthA");
                
            // Issue query 'CurrentEventsByTag'
            var eventsSource = _readJournal.CurrentEventsByTag("some-tag", cvOffset);

            ActorMaterializer materializer = system.Materializer();
            eventsSource.RunForeach(envelope =>
            {
                var saleEvent = (Sale)envelope.Event;
                Console.WriteLine($"Sale Event - Brand: {saleEvent.Brand}, Price: {saleEvent.Price}");
            }, materializer).Wait();
            #endregion
        }
        
        void QueryAllEvents(ActorSystem system)
        {
            #region query_4
            RavenDbReadJournal readJournal = PersistenceQuery.Get(system)
                .ReadJournalFor<RavenDbReadJournal>(RavenDbReadJournal.Identifier);

            // Issue query 'CurrentAllEvents'
            var eventsSource = readJournal.CurrentAllEvents(Offset.NoOffset());
            
            ActorMaterializer materializer = system.Materializer();
            eventsSource.RunForeach(envelope =>
            {
                var saleEvent = (Sale)envelope.Event;
                Console.WriteLine($"Sale Event - Brand: {saleEvent.Brand}, Price: {saleEvent.Price}");
            }, materializer).Wait();
            #endregion
        }
    }
}
