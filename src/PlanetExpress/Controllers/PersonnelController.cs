using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PlanetExpress.Controllers
{
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

        public IEnumerable<Employee> Get()
        {
            return employees;
        }

        public Employee Get(string id)
        {
            return employees.FirstOrDefault(e => e.Id == id);
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
