//using IMS.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Reflection.Metadata.Ecma335;

//namespace IMS.Controllers
//{
//    public class CalcuController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View(new Calcu());
//        }

//        public ActionResult Calulator(Calcu c, string Calculate)
//        {
//            if (Calculate == "add")
//            {
//                c.Total = c.N1 + c.N2;
//            }
//            else if (Calculate == "min")
//            {
//                c.Total = c.N1 - c.N2;
//            }

//            return View("Index", c);
//        }

//        public IActionResult Calculator()
//        {

//            if (Symbol == "add")
//            {
//                ViewBag.Result = N1 + N2;
//            }
//            else if (Symbol == "min")
//            {
//                ViewBag.Result = N1 - N2;
//            }


//            return View("Index");
//        }

//    }
//}
