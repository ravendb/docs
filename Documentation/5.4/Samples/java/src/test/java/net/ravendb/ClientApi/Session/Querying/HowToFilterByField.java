public class HowToPerformProximitySearch {

    private interface IFoo<T> {
        //region whereexists_1
        IDocumentQuery<T> whereExists(String fieldName);
        //endregion
    }

    public void sample1(IDocumentSession session) {
        //region whereexists_2
        List<Employee> results = session
                .advanced()
                .documentQuery(Employee.class)
                .whereExists("FirstName")
                .toList();
        //endregion

        //region whereexists_3
        List<Employee> results = session
		        .advanced()
		        .documentQuery(Employee.class)
		        .whereExists("Address.Location.Latitude")
		        .toList();
        //endregion
    }
