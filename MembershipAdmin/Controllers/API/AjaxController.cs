using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MembershipAdmin.Controllers
{
    public class AjaxController : Controller
    {

        public ActionResult SetClanarinaAjax(DateTime zaMjesec, int UserID = 0)
        {
            DBHelper.setClanarina(zaMjesec, UserID);


            return null;
        }
        //public static string GetClanarinaPayedcurrentMonthAjax()
        //{
        //    var zaMjesec = DateTime.Now.Year;
        //    var result = DBHelper.GetClanarinaPayed();

        //    return result;
        //}
    }
}