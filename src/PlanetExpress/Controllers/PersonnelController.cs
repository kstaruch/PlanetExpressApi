using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PlanetExpress.Controllers
{
    [RoutePrefix("api/personnel")]
    public class PersonnelController : ApiController
    {
        private readonly IEnumerable<Employee> employees;

        public PersonnelController()
        {
            employees = new[]
            {
                new Employee("fry", "Philip J. Fry"), 
                new Employee("leela", "Turanga Leela"),
                new Employee("bender", "Bender Bending Rodriguez"),
            };

        }

        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            return Ok(employees);
        }

        [HttpGet, Route("{id}", Name = "GetEmployeeById")]
        public IHttpActionResult Get(string id)
        {
            var emp = employees.FirstOrDefault(e => e.Id == id);
            if (emp == default(Employee))
            {
                return NotFound();
            }
            return Ok(emp);
        }

    }

    public class Employee
    {
        public Employee(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
