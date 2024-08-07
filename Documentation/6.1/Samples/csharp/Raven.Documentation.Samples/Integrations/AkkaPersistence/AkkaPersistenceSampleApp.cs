using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Hosting;
using Akka.Persistence;
using Akka.Persistence.Hosting;
using Akka.Persistence.Query;
using Akka.Persistence.RavenDb.Hosting;
using Akka.Persistence.RavenDb.Query;
using Akka.Streams;
using Microsoft.Extensions.Hosting;

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
        
        #region queries
        void QueryEventsById(ActorSystem system)
        {
            var _readJournal = PersistenceQuery.Get(system)
                .ReadJournalFor<RavenDbReadJournal>(RavenDbReadJournal.Identifier);

            var eventsSource = _readJournal.CurrentEventsByPersistenceId("sales-actor", 0L, long.MaxValue);
            var materializer = system.Materializer();

            ConsoleHelper.WriteToConsole(ConsoleColor.DarkBlue,
                "Query results for CurrentEventsByPersistenceId:");
            
            eventsSource.RunForeach(envelope =>
            {
                var saleEvent = (Sale)envelope.Event;
                ConsoleHelper.WriteToConsole(ConsoleColor.DarkBlue,
                    $"CurrentEventsByPersistenceId: " +
                    $"Sale Event - Brand: {saleEvent.Brand}, Price: {saleEvent.Price}");
            }, materializer).Wait();
        }
        
        void QueryAllEvents(ActorSystem system)
        {
            var _readJournal = PersistenceQuery.Get(system)
                .ReadJournalFor<RavenDbReadJournal>(RavenDbReadJournal.Identifier);

            var eventsSource = _readJournal.CurrentAllEvents(NoOffset.Instance);
            var materializer = system.Materializer();

            ConsoleHelper.WriteToConsole(ConsoleColor.DarkBlue,
                "Query results for CurrentAllEvents:");
            
            eventsSource.RunForeach(envelope =>
            {
                var saleEvent = (Sale)envelope.Event;
                ConsoleHelper.WriteToConsole(ConsoleColor.DarkBlue,
                    $"CurrentAllEvents: " +
                    $"Sale Event - Brand: {saleEvent.Brand}, Price: {saleEvent.Price}");
            }, materializer).Wait();
        }
        #endregion
    }
}
