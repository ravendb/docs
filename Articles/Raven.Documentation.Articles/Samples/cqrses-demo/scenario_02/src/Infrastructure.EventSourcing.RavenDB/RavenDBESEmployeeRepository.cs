using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Messaging;
using Payroll.Domain.Model;
using Payroll.Domain.Repositories;
using Raven.Abstractions.Commands;
using Raven.Abstractions.Data;
using Raven.Client.Converters;
using Raven.Client.Document;
using Raven.Imports.Newtonsoft.Json;
using Raven.Json.Linq;

namespace Infrastructure.EventSourcing.RavenDB
{
    #region article_cqrs_8
    public class RavenDBESEmployeeRepository :
        IEmployeeRepository
    {
        public IBus Bus { get; }
        private DocumentStore _store;
        private JsonSerializer _serializer;

        public RavenDBESEmployeeRepository(IBus bus)
        {
            Bus = bus;
            _store = new DocumentStore
            {
                Url = "http://localhost:8080/", // server URL
                DefaultDatabase = "EmployeeES"
            };
            
            _store.Initialize();

            _serializer = _store.Conventions.CreateSerializer();
            _serializer.TypeNameHandling = TypeNameHandling.All;

            _store.Conventions.IdentityTypeConvertors = new List<ITypeConverter>
            {
                new GuidConverter()
            };

            _store.Conventions.FindTypeTagName = t => "Employees";

        }

        const string EmployeeEntityVersion =
            "Employee-Entity-Version";

        public int GetStoredVersionOf(JsonDocumentMetadata head)
        {
            return head
                ?.Metadata[EmployeeEntityVersion]
                .Value<int>() ?? 0;
        }

        public JsonDocumentMetadata GetHead(Guid id)
        {
            string localId = $"employees/{id}";
            return _store.DatabaseCommands.Head(localId);
        }
        
        public void Save(Employee employee)
        {
            var head = GetHead(employee.Id);
            var storedVersion = GetStoredVersionOf(head);

            if (storedVersion != (employee.Version - employee.PendingEvents.Count()))
                throw new InvalidOperationException("Invalid object state.");

            if (head == null)
            {
                SaveNewEmployee(employee);
            }
            else
            {
                SaveEmployeeEvents(employee);
            }

            foreach (var evt in employee.PendingEvents)
                Bus.RaiseEvent(evt);
        }

        private void SaveEmployeeEvents(
            Employee employee            )
        {
            var patches = new List<PatchRequest>();
            foreach (var evt in employee.PendingEvents)
            {
                patches.Add(new PatchRequest
                { 
                    Type = PatchCommandType.Add,
                    Name = "Events",
                    Value = RavenJObject.FromObject(evt, _serializer)
                });
            }
            var localId = $"employees/{employee.Id}";

            var addEmployeeEvents = new PatchCommandData()
            {
                Key = localId,
                Patches = patches.ToArray()
            }; 

            var updateMetadata = new ScriptedPatchCommandData()
            {
                Key = localId,
                Patch = new ScriptedPatchRequest
                {
                    Script = $"this['@metadata']['{EmployeeEntityVersion}'] = {employee.Version}; "
                }
            };
            
            _store.DatabaseCommands.Batch(new ICommandData[]
            {
                addEmployeeEvents,
                updateMetadata
            });
        }

        private void SaveNewEmployee(Employee employee)
        {
            using (var session = _store.OpenSession())
            {
                var document = new EmployeeEvents(
                    employee.Id,
                    employee.PendingEvents
                    );
                session.Store(document);

                session.Advanced.GetMetadataFor(document)
                    .Add(EmployeeEntityVersion, employee.Version);

                session.SaveChanges();
            }
        }

        public Employee Load(Guid id)
        {
            EmployeeEvents data;
            using (var session = _store.OpenSession())
            {
                data = session.Load<EmployeeEvents>(id);
            }
            return new Employee(id, data.Events);
        }

        class EmployeeEvents
        {
            public EmployeeEvents(Guid id, IEnumerable<IVersionedEvent<Guid>> events)
            {
                Id = id;
                Events = events.ToArray();
            }

            public Guid Id { get; private set; }
            public IVersionedEvent<Guid>[] Events { get; private set; }
        }
    }
    #endregion
}
