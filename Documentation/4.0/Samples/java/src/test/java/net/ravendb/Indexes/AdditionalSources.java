package net.ravendb.Indexes;

import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;

import java.util.Collections;

public class AdditionalSources {
    private static class Person {

    }

    //region additional_sources_1
    public class People_ByEmail extends AbstractIndexCreationTask {
        public People_ByEmail() {
            map = "docs.People.Select(person => new { " +
                "    Email = PeopleUtil.CalculatePersonEmail(person.Name, person.Age) " +
                "})";

            additionalSources = Collections.singletonMap("PeopleUtil",
                    "  using System; " +
                    "  using NodaTime; /* using an external library */ " +
                    "  using static Raven.Documentation.Samples.Indexes.PeopleUtil; " +
                    "  namespace Raven.Documentation.Samples.Indexes " +
                    "  { " +
                    "      public static class PeopleUtil " +
                    "      { " +
                    "          public static string CalculatePersonEmail(string name, uint age) " +
                    "          { " +
                    "              return $\"{name}.{Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()) " +
                    "                              .ToDateTimeUtc().Year - age}@ayende.com\"; " +
                    "          } " +
                    "      } " +
                    "  }");
        }
    }
    //endregion
}
