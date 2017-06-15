using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using IdeaDomain.ServiceLayer;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using GeneralUtilities.Utilities.Unity;
using IdeaDomain.DomainLayer;
using IdeaDomain.DomainLayer.Entities;
using System.Reflection;
using MVCWebUIComponent.Models;

namespace MVCWebUIComponent.Controllers
{
    public class SeatController : Controller
    {
        public ActionResult Index()
        {

            return View("Map");
        }

        public ActionResult Map()
        {
          
            return View();
        }

        public JsonResult SeatData()
        {
            var list = new List<Booking>();
            var today = DateTime.Now;

            var ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            var idea = ideaService.GetIdeaByName("SeatBooking");
            if (idea != null)
            {
                var ideaDetail = ideaService.GetIdeaData(idea.IdeaId, "", "", 0, 10000, null);
                foreach (var row in ideaDetail.Rows)
                {
                    list.Add(new Booking()
                        {
                            Begin = row.Values[5].ToString(),
                            Building = row.Values[1].ToString(),
                            Coordinate = row.Values[3].ToString(),
                            End = row.Values[6].ToString(),
                            Floor = Convert.ToInt32(row.Values[2]),
                            SeatCode = row.Values[0].ToString(),
                            Employee = row.Values[4].ToString()
                        });
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }

            #region :: Fake Data ::

            //#region :: First Floor ::
            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(20).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "92,73",
            //    End = today.AddDays(37).ToShortDateString(),
            //    Floor = 0,
            //    SeatCode = "ARHK_0_001",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(2).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "92,106",
            //    End = today.AddDays(7).ToShortDateString(),
            //    Floor = 0,
            //    SeatCode = "ARHK_0_002",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(-20).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "130,73",
            //    End = today.AddDays(-7).ToShortDateString(),
            //    Floor = 0,
            //    SeatCode = "ARHK_0_003",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(-5).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "130,106",
            //    End = today.AddDays(3).ToShortDateString(),
            //    Floor = 0,
            //    SeatCode = "ARHK_0_004",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(-50).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "92,152",
            //    End = today.AddDays(5).ToShortDateString(),
            //    Floor = 0,
            //    SeatCode = "ARHK_0_006",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(10).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "122,152",
            //    End = today.AddDays(37).ToShortDateString(),
            //    Floor = 0,
            //    SeatCode = "ARHK_0_007",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(-7).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "12,125",
            //    End = today.AddDays(12).ToShortDateString(),
            //    Floor = 0,
            //    SeatCode = "ARHK_0_008",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(-7).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "40,125",
            //    End = today.AddDays(12).ToShortDateString(),
            //    Floor = 0,
            //    SeatCode = "ARHK_0_009",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(-7).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "12,152",
            //    End = today.AddDays(12).ToShortDateString(),
            //    Floor = 0,
            //    SeatCode = "ARHK_0_010",
            //    User = "archer.chen@volvo.com"
            //});

            //#endregion

            //#region :: Second Floor ::
            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(-17).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "95,100",
            //    End = today.AddDays(2).ToShortDateString(),
            //    Floor = 1,
            //    SeatCode = "ARHK_1_081",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(-7).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "120,100",
            //    End = today.AddDays(12).ToShortDateString(),
            //    Floor = 1,
            //    SeatCode = "ARHK_1_081",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(-71).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "95,127",
            //    End = today.AddDays(-12).ToShortDateString(),
            //    Floor = 1,
            //    SeatCode = "ARHK_1_081",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(2).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "120,127",
            //    End = today.AddDays(12).ToShortDateString(),
            //    Floor = 1,
            //    SeatCode = "ARHK_1_081",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(1).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "95,149",
            //    End = today.AddDays(3).ToShortDateString(),
            //    Floor = 1,
            //    SeatCode = "ARHK_1_081",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(7).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "120,149",
            //    End = today.AddDays(19).ToShortDateString(),
            //    Floor = 1,
            //    SeatCode = "ARHK_1_081",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(5).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "95,175",
            //    End = today.AddDays(32).ToShortDateString(),
            //    Floor = 1,
            //    SeatCode = "ARHK_1_081",
            //    User = "archer.chen@volvo.com"
            //});

            //list.Add(new Booking()
            //{
            //    Begin = today.AddDays(-7).ToShortDateString(),
            //    Building = "ARHK",
            //    Coordinate = "120,175",
            //    End = today.AddDays(12).ToShortDateString(),
            //    Floor = 1,
            //    SeatCode = "ARHK_1_081",
            //    User = "archer.chen@volvo.com"
            //});
            //#endregion

            #endregion

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NewBooking(string newBooking)
        {
            var userId = 0; 
            var response = new Dictionary<string, string>();

            if (Request.RequestContext.HttpContext.Session["UserId"] == null)
            {
                response.Add("Fail", "session timeout, please logon again!");
                return Json(response);
            }
                
            else
                userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);

            var array = JArray.Parse(newBooking);

            var list = new List<Booking>();
            foreach (dynamic d in array)
            {
                var booking = new Booking();

                booking.Begin = d.Begin;
                booking.End = d.End;
                booking.SeatCode = d.SeatCode;
                booking.Floor = d.Floor;
                booking.Building = d.Building;
                booking.Coordinate = d.Coordinate;
                booking.Employee = Environment.UserName;

                list.Add(booking);
            }

            
            var ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            var rowService = PolicyInjection.Wrap<IRowService>(Container.Resolve<IRowService>());
            var idea = ideaService.GetIdeaByName("SeatBooking");
            var pis = typeof(Booking).GetProperties();

            var row = new Row()
            {
                IdeaId = idea.IdeaId,
                UserId = userId,
                Columns = idea.Columns
            };
            foreach (var booking in list)
            {
                foreach (var column in row.Columns)
                {
                    foreach (var pi in pis)
                    {
                        if (column.ColumnName.Contains(pi.Name))
                        {
                            row.Values.Add(pi.GetValue(booking, null));
                            break;
                        }
                    }
                }
            }
            var result = rowService.AddRow(row).ToString();

            if (result== "True")
                response.Add("Success", "true");
            else
                response.Add("Fail", "false");

            return Json(response);
        }

    }
}
