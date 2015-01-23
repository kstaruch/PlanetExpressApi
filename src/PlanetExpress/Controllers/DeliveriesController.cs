using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PlanetExpress.Controllers
{
    [RoutePrefix("api/deliveries")]
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
                    Status = "Good news, everyone!"
                }, 
                new Delivery
                {
                    Id = "dev2",
                    Summary = "some dev2 summary",
                    Origin = "origin2",
                    Desination = "destination2",
                    PlacedAt = DateTime.Now,
                    PlacedBy = "some_guy2",
                    Status = "Good news, everyone!"
                }, 
                new Delivery
                {
                    Id = "dev3",
                    Summary = "some dev3 summary",
                    Origin = "origin3",
                    Desination = "destination3",
                    PlacedAt = DateTime.Now,
                    PlacedBy = "some_guy3",
                    Status = "Good news, everyone!"
                }
            });

        }

        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            return Ok(Deliveries);
        }

        [HttpGet, Route("{id}")]
        public IHttpActionResult Get(string id)
        {

            var delivery = Deliveries.FirstOrDefault(d => d.Id == id);
            if (delivery == default(Delivery))
            {
                return NotFound();
            }
            return Ok(delivery);
        }

        [HttpGet, Route("{id}/status")]
        public IHttpActionResult GetStatusOfDelivery(string id)
        {
            var delivery = Deliveries.FirstOrDefault(d => d.Id == id);
            if (delivery == default(Delivery))
            {
                return NotFound();
            }
            return Ok(delivery.Status);
        }

        [HttpPut, Route("{id}/status")]
        public IHttpActionResult ChangeStatus(string id, [FromUri]string status)
        {
            var delivery = Deliveries.FirstOrDefault(d => d.Id == id);
            if (delivery == default(Delivery))
            {
                return NotFound();
            }
            delivery.ChangeStatus(status);
            return Ok();
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

        public void ChangeStatus(string status)
        {
            Status = status;
        }
    }

    

    

}
