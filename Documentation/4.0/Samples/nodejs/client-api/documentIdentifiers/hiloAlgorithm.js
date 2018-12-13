import { 
    DocumentStore 
} from "ravendb";

class Employee {}

async function example() {

    //region return_hilo_1
    const store = new DocumentStore();

    {
        const session = store.openSession();
        // Store an entity will give us the hilo range (ex. 1-32)
        const employee = new Employee("John", "Doe");

        await session.store(employee);

        await session.saveChanges();
    }

    store.dispose(); // returning unused range [last=1, max=32]
    //endregion

    {
        //region return_hilo_2
        const newStore = new DocumentStore();

        const session = newStore.openSession();
        // Store an entity will give us the hilo range (ex. 1-32)
        const employee = new Employee("John", "Doe");

        // Store an entity after closing the last store will give us  (ex. 2-33)
        await session.store(employee);

        await session.saveChanges();
        //endregion
    }
}
