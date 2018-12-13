import { 
    DocumentStore,
    AbstractIndexCreationTask,
    IndexDefinition,
    PutIndexesOperation
} from "ravendb";

const store = new DocumentStore();


    class Person { }

    //region additional_sources_1
    class People_ByEmail extends AbstractIndexCreationTask {
        constructor() {
            super();

            this.map = "docs.People.Select(person => new { " +
                "    Email = PeopleUtil.CalculatePersonEmail(person.Name, person.Age) " +
                "})";

            this.additionalSources = 
                    { 
                        "PeopleUtil":  `using System; 
                                      using NodaTime; /* using an external library */ 
                                      using static Raven.Documentation.Samples.Indexes.PeopleUtil; 
                                      namespace Raven.Documentation.Samples.Indexes 
                                      { 
                                          public static class PeopleUtil 
                                          { 
                                              public static string CalculatePersonEmail(string name, uint age) 
                                              { 
                                                  return $"{name}.{Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()) 
                                                                  .ToDateTimeUtc().Year - age}@ayende.com"; 
                                              } 
                                          } 
                                      }`
                    };
        }
    }
    //endregion

