using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

public class CustomersController : ODataController
    {
        private static readonly Random Random = new();

        private static readonly List<Customer> Customers = new(
            Enumerable.Range(1, 3).Select(idx => new Customer
            {
                Id = $"{idx.ToString()}/test",
                Name = $"Customer {idx}",
            }));

        [EnableQuery]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            return Ok(Customers);
        }

        [EnableQuery]
        public ActionResult<Customer> Get([FromRoute] string key)
        {
            var item = Customers.SingleOrDefault(d => d.Id.Equals(key));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        public ActionResult<Customer> Post([FromBody] Customer customer)
        {
            if (Customers.Any(c => c.Id == customer.Id))
            {
                return BadRequest("Customer with same id already exists");
            }

            Customers.Add(customer);
            return Created(customer);
        }

    }