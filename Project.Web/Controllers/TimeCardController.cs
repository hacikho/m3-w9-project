using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Web.Models;
using Project.Web.DAL;

namespace Project.Web.Controllers
{
    public class TimeCardController : Controller
    {
        private ITimeCardDAL dal;
        public TimeCardController(ITimeCardDAL record)
        {
            this.dal = record;
        }

        // GET: TimeCard
        [HttpGet]
        public ActionResult Login()
        {
            TimeCardModel model = new TimeCardModel();
            return View("Login", model);
        }

        [HttpGet]
        public ActionResult ClockInOut(string username, string note)
        {
            var model = new ClockInOutModel();
            model.Username = username;
            Session["username"] = model.Username;
            model.Note = note;
            Session["notes"] = model.Note;
            model.CanClockIn = !dal.CanClockOut(model.Username); ;
            return View("ClockInOut", model);
        }

        [HttpPost]
        public ActionResult ClockIn(ClockInOutModel input)
        {
            TimeCardModel m = new TimeCardModel();
            m.UserName = input.Username;
            m.Project = input.Project;
            dal.SaveNewRecord(m);
            return RedirectToAction("Report");
        }

        [HttpPost]
        public ActionResult ClockOut(ClockInOutModel input)
        {
            TimeCardModel m = new TimeCardModel();
            m.UserName = input.Username;
            m.Notes = input.Note;

            dal.ClockOut(m);
            return RedirectToAction("Report");
        }

        public ActionResult Report()
        {
            List<TimeCardModel> models = dal.GetAllRecords(Session["username"].ToString());
            return View("Report", models);
        }
    }
}