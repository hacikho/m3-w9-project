using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Models;
namespace Project.Web.DAL
{
    public interface ITimeCardDAL
    {
        List<TimeCardModel> GetAllRecords(string username);
        bool SaveNewRecord(TimeCardModel record);
        bool CanClockOut(string username);
        bool ClockOut(TimeCardModel record);
    }
}