namespace CampusOrdering.Models
{
    public class CustomerRepository
    {
        private List<Customer> customers = new List<Customer>();

        public CustomerRepository()
        {
            customers.Add(new Customer { Id = 0, Name = "Jane", Birthdate = DateTime.Parse("1990-01-15"), Email = "Jane@example.com", Password = "password0" });

            customers.Add(new Customer { Id = 1, Name = "Alice", Birthdate = DateTime.Parse("1990-01-15"), Email = "alice@example.com", Password = "password1" });
            customers.Add(new Customer { Id = 2, Name = "Bob", Birthdate = DateTime.Parse("1985-07-22"), Email = "bob@example.com", Password = "password2" });
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return customers;
        }

        public Customer GetCustomerById(int id)
        {
            return customers.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateCustomer(Customer updatedCustomer)
        {
            var existingCustomer = customers.FirstOrDefault(c => c.Id == updatedCustomer.Id);
            if (existingCustomer != null)
            {
                existingCustomer.Name = updatedCustomer.Name;
                existingCustomer.Birthdate = updatedCustomer.Birthdate;
                existingCustomer.Email = updatedCustomer.Email;
                existingCustomer.Password = updatedCustomer.Password;
            }
        }
    }

}
