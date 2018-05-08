using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Threading;

namespace Exp001.Controllers
{
    public class GetterController : Controller
    {
        AdresaDBEntities e = new AdresaDBEntities();
        public string CurrentLangCode { get; protected set; }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (requestContext.HttpContext.Request.Url != null)
            {
                var HostName = requestContext.HttpContext.Request.Url.Authority;
            }

            if (requestContext.RouteData.Values["lang"] != null && requestContext.RouteData.Values["lang"] as string != "null")
            {    
                CurrentLangCode = requestContext.RouteData.Values["lang"] as string;
                var ci = new CultureInfo(CurrentLangCode);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult Get(int page = 1, int itemsPerPage = 3, string sortBy = "Country1", bool reverse = false, string fieldname = null, string filtervalue = null, string firstdate="", string seconddate="")
        {
            
            var fieldName = fieldname;
            //database
            var result = e.Addresses.Select(e => new { e.Country.Country1, e.City1.City1, e.Street1.Street1, e.House, e.Indeks, e.Data }).AsEnumerable();
            //sort
            if (sortBy!=null)
            {
                result = result.OrderBy(sortBy + (reverse ? " descending" : ""));
            }
            //filter
            switch (fieldName)
            {
                case "Country":
                    if (filtervalue != null)
                    { result = result.Where(x => x.Country1.ToLower().Contains(filtervalue.ToLower())); }
                    break;
                case "City":
                    if (filtervalue != null)
                    { result = result.Where(x => x.City1.ToLower().Contains(filtervalue.ToLower())); }
                    break;
                case "Street":
                    if (filtervalue != null)
                    { result = result.Where(x => x.Street1.ToLower().Contains(filtervalue.ToLower())); }
                    break;
                case "House":
                    if (filtervalue != null)
                    { result = result.Where(x => x.House.Value.ToString().Contains(filtervalue)); }
                    break;
                case "Indeks":
                    if (filtervalue != null)
                    { result = result.Where(x => x.Indeks.Value.ToString().Contains(filtervalue)); }
                    break;
                case "Data":
                    if ((firstdate != "" && seconddate != "") && (firstdate != seconddate))
                    {
                        result = result.Where(x => x.Data > Convert.ToDateTime(firstdate) && x.Data < Convert.ToDateTime(seconddate));
                    }
                    if ((firstdate == seconddate) && (firstdate != "" && seconddate != ""))
                    {
                        result = result.Where(x => x.Data == Convert.ToDateTime(firstdate));
                    }
                    break;
            }
            //paging
            var resultPaged = result.Skip((page-1)*itemsPerPage).Take(itemsPerPage);
            //json
            var json = new
            {
                count = result.Count(),
                data = resultPaged.Select(e => new {
                    e.Country1,
                    e.City1,
                    e.Street1,
                    e.House,
                    e.Indeks,
                    e.Data
                })
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}