using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace IMS.Controllers
{
    public class ClcController : Controller
    {
        public IActionResult ClcIndex()
        {
            return View(new ClcModel());
        }

        public ActionResult Calulator(ClcModel c, string Calculate)
        {
            if (Calculate == "add")//Add is button
            {
                c.Total = c.No1 + c.No2;
            }
            else if (Calculate == "min")
            {
                c.Total = c.No1 - c.No2;
            }
            else if (Calculate == "sub")
            {
                c.Total = c.No1 * c.No2;
            }
            else if (Calculate == "divv")
            {
                c.Total = c.No1 / c.No2;
            }

            return View("ClcIndex", c);
        }



    }
}

