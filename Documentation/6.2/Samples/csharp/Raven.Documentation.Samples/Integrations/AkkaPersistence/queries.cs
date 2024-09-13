using System;
using Akka;
using Akka.Streams.Dsl;
#region basic_hosting_config
using Akka.Persistence.Query;

namespace Raven.Documentation.Samples.Integrations.AkkaPersistence.QuerySyntax
{
    public class Queries
    {
        #region syntax_1
        public Source<string, NotUsed> PersistenceIds()
        #endregion
        {
            throw new NotImplementedException();
        }
        
        #region syntax_1_current
        public Source<string, NotUsed> CurrentPersistenceIds()
        #endregion
        {
            throw new NotImplementedException();
        }
        
        #region syntax_2
        public Source<EventEnvelope, NotUsed> EventsByPersistenceId(string persistenceId,
            long fromSequenceNr,
            long toSequenceNr)
        #endregion
        {
            throw new NotImplementedException();
        }
        
        #region syntax_2_current
        public Source<EventEnvelope, NotUsed> CurrentEventsByPersistenceId(string persistenceId,
                long fromSequenceNr,
                long toSequenceNr)
        #endregion
        {
            throw new NotImplementedException();
        }

        #region syntax_3
        public Source<EventEnvelope, NotUsed> EventsByTag(string tag, Offset offset)
        #endregion
        {
            throw new NotImplementedException();
        }
        
        #region syntax_3_current
        public Source<EventEnvelope, NotUsed> CurrentEventsByTag(string tag, Offset offset)
        #endregion
        {
            throw new NotImplementedException();
        }
        
        #region syntax_4
        public Source<EventEnvelope, NotUsed> AllEvents(Offset offset)
        #endregion
        {
            throw new NotImplementedException();
        }
        
        #region syntax_4_current
        public Source<EventEnvelope, NotUsed> CurrentAllEvents(Offset offset)
        #endregion
        {
            throw new NotImplementedException();
        }
    }
}
#endregion

