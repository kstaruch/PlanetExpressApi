using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using PlanetExpress.Models;

//TODO: client
//TODO: templates

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
                    Status = "Good news, everyone!",
                    HandlerId = "fry",
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
                    HandlerId = "leela",
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
                    HandlerId = "bender",
                }
            });

        }

        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            var deliveries = Deliveries.Select(d =>
            {
                d.Links = CreateLinks(d);
                return d;
            });

            return Ok(deliveries);
        }

        private IEnumerable<Link> CreateLinks(Delivery delivery)
        {
            var links = new[]
            {
                new Link
                {
                    Method = "GET",
                    Rel = "self",
                    Href = Url.Link("GetDeliveryById", new {id = delivery.Id})
                },
                new Link
                {
                    Method = "GET",
                    Rel = "handler",
                    Href = Url.Link("GetEmployeeById", new {id = delivery.HandlerId})
                },
                new Link
                {
                    Method = "PUT",
                    Rel = "status-delivered",
                    //Href = Url.Link("ChangeStatusById", new {id = delivery.Id, status = "delivered"})
                    //Href = this.GetTemplatedHref("ChangeStatusById")
                    Href =  Url.Template("GetEmployeeById")
                }
            };
            return links;
        }


        [HttpGet, Route("{id}", Name = "GetDeliveryById")]
        public IHttpActionResult Get(string id)
        {

            var delivery = Deliveries.FirstOrDefault(d => d.Id == id);
            if (delivery == default(Delivery))
            {
                return NotFound();
            }
            return Ok(delivery);
        }

        [HttpGet, Route("{id}/status", Name = "GetStatusById")]
        public IHttpActionResult GetStatusOfDelivery(string id)
        {
            var delivery = Deliveries.FirstOrDefault(d => d.Id == id);
            if (delivery == default(Delivery))
            {
                return NotFound();
            }
            return Ok(delivery.Status);
        }

        [HttpPut, Route("{id}/status{?status}", Name = "ChangeStatusById")]
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

    public interface ITemplatedLink
    {
        string Href { get; }
        ITemplatedLink WithQueryParam(string queryParamName);
        ITemplatedLink WithQueryParam(string queryParamName, object queryParamValue);
        ITemplatedLink WithRouteValue(string paramName, object paramValue);
    }

    class TemplatedLink : ITemplatedLink
    {
        private readonly string _routeName;
        private readonly IDictionary<string, object> _routeValues;
        private readonly IList<string> _queryParams;
        private readonly IDictionary<string, object> _queryParamsValues;

        public TemplatedLink(string routeName)
        {
            _routeName = routeName;
            _routeValues = new HttpRouteValueDictionary();
            _queryParamsValues = new HttpRouteValueDictionary();
            _queryParams = new List<string>();
        }

        public TemplatedLink(string routeName, IDictionary<string, object> routeValues, IList<string> queryParams, IDictionary<string, object> queryParamsValues)
        {
            _routeName = routeName;
            _routeValues = routeValues;
            _queryParams = queryParams;
            _queryParamsValues = queryParamsValues;
        }

        public virtual string Href
        {
            get { return GenerateHref(); }
        }

        protected virtual string GenerateHref()
        {
            throw new NotImplementedException();
        }

        public virtual ITemplatedLink WithQueryParam(string queryParamName)
        {
            var qp = new List<string>(_queryParams) {queryParamName};
            return new TemplatedLink(_routeName, _routeValues, qp, _queryParamsValues);
        }

        public virtual ITemplatedLink WithQueryParam(string queryParamName, object queryParamValue)
        {
            var qpv = new HttpRouteValueDictionary(_queryParamsValues) {{queryParamName, queryParamValue}};
            return new TemplatedLink(_routeName, _routeValues, _queryParams, qpv);
        }

        public virtual ITemplatedLink WithRouteValue(string paramName, object paramValue)
        {
            var rpv = new HttpRouteValueDictionary(_routeValues) { { paramName, paramValue } };
            return new TemplatedLink(_routeName, rpv, _queryParams, _queryParamsValues);
        }
    }


    public static class UrlHelperExtensions
    {
        public static string Template(this UrlHelper url, string routeName)
        {
            return Template(url, routeName, new HttpRouteValueDictionary());
        }

        public static string Template(this UrlHelper url, string routeName, object routeValues)
        {
            return Template(url, routeName, new HttpRouteValueDictionary(routeValues));
        }

        public static string Template(this UrlHelper url, string routeName, IDictionary<string, object> routeValues, string controllerName = "")
        {
            var configuration = url.Request.GetConfiguration();
            var routeTemplate = configuration.Routes[routeName].RouteTemplate;
            
            var baseUri = new Uri(url.Request.RequestUri.GetLeftPart(UriPartial.Authority));
            var templatedHref = new Uri(baseUri, routeTemplate).ToString();
            
            return string.IsNullOrEmpty(controllerName) ?
                templatedHref :
                templatedHref.Replace("{controller}", controllerName.ToLower());
        }
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
        public Link Handler { get; set; }
        public IEnumerable<Link> Links;

        public void ChangeStatus(string status)
        {
            Status = status;
        }
    }

    

    

}
