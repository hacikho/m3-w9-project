using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Models
{
    public class TimeCardModel
    {
        public string UserName { get; set; }
        public string Project { get; set; }
        public DateTime StartDate { get; set; }
        public string EndDate { get; set; }
        public string Notes { get; set; }
    }
}