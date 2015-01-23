using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PlanetExpress.Controllers
{
    public class DeliveriesController : ApiController
    {
        public DeliveriesController()
        {
            Deliveries = new List<Delivery>(new[]
            {
                new Delivery
                {
                    Id = "dev1",
                    Summary = "some dev1 summary",
                    Origin = "origin1",
                    Desination = "destination1",
                    PlacedAt = DateTime.Now,
                    PlacedBy = "some_guy1",
                    Status = "Good news, everyone!",
                    HandlerId = "fry"
                }, 
                new Delivery
                {
                    Id = "dev2",
                    Summary = "some dev2 summary",
                    Origin = "origin2",
                    Desination = "destination2",
                    PlacedAt = DateTime.Now,
                    PlacedBy = "some_guy2",
                    Status = "Good news, everyone!",
                    HandlerId = "leela"
                }, 
                new Delivery
                {
                    Id = "dev3",
                    Summary = "some dev3 summary",
                    Origin = "origin3",
                    Desination = "destination3",
                    PlacedAt = DateTime.Now,
                    PlacedBy = "some_guy3",
                    Status = "Good news, everyone!",
                    HandlerId = "bender"
                }
            });

        }

        public IEnumerable<Delivery> Get()
        {   
            return Deliveries;
        }

        [HttpPut]
        public void ChangeStatus(string id, string status)
        {
            var delivery = Deliveries.FirstOrDefault(d => d.Id == id);
            if (delivery != default(Delivery))
            {
                delivery.ChangeStatus(status);
            }

        }

        public IList<Delivery> Deliveries { get; protected set; }
    }

    public class Delivery
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Origin { get; set; }
        public string Desination { get; set; }
        public string Status { get; set; }
        public DateTime PlacedAt { get; set; }
        public string PlacedBy { get; set; }
        public string HandlerId { get; set; }

        public void ChangeStatus(string status)
        {
            Status = status;
        }
    }

    

    

}
