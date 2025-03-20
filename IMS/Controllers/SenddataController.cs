using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    public class SenddataController : Controller
    {
        public IActionResult Index()
        {
            string[] furits = { "Apple","Banana","PineApple"};
            ViewData["furitList"] = furits;
            ViewData["Meassage"] = "HEllo World";
            string[] vegetable = { "Potato", "Tomato", "Brinjal","Onion","Mashroom","Pumpkin" };
            ViewData["vegetable"]= vegetable;
            return View();
           
        
        
        }
    }
}
