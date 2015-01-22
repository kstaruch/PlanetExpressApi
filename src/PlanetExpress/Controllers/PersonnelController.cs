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
            this.employees = new Employee[]
            {
                new Employee("Philip J. Fry"), 
                new Employee("Turanga Leela"),
                new Employee("Bender Bending Rodriguez"),
            };

        }

        public IEnumerable<Employee> Get()
        {
            return employees;
        }

    }

    public class Employee
    {
        public Employee(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
