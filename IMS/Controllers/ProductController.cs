using IMS.data;
using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Controllers
{
    public class ProductController : Controller
    {
        IMSDb database;
        public ProductController(IMSDb bd)
        {
            database = bd;
        }
        public ActionResult ProductList()
        {
            List<ProductModel> vishnusList = database.Products.ToList();
            return View(vishnusList);
        }
        public ActionResult AddProduct()
        {
            return View();
        }
        public ActionResult SaveProduct(ProductModel cus)
        {
            database.Products.Add(cus);
            database.SaveChanges();
            return RedirectToAction("ProductList");
        }
        public IActionResult detail(int Id)
        {
            ProductModel productModel = database.Products.FirstOrDefault(x => x.Id == Id);
            return View(productModel);
        }
        public IActionResult Delete(int Id)
        {
            ProductModel cus = database.Products.FirstOrDefault(x => x.Id == Id);
            database.Products.Remove(cus);
            database.SaveChanges();
            return RedirectToAction("ProductList");
        }
        public IActionResult Edit(int Id)
        {
            ProductModel cus = database.Products.FirstOrDefault(x => x.Id == Id);
            return View(cus);
        }
        public IActionResult Update(ProductModel custo)
        {
            database.Products.Update(custo);
            database.SaveChanges();
            return RedirectToAction("ProductList");
        }
    }

}
