using IMS.data;
using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace IMS.Controllers
{
    public class SuppliersController :Controller
    {
        IMSDb database;
        public SuppliersController(IMSDb _database)
        {
            database = _database;
        }
       
        public IActionResult SupplierList()
        {
            List<SuppliersModel> suppl=database.Suppliers.ToList();
            
            return View(suppl);            
        }
        public IActionResult AddSupplier()
        {
            List<SuppliersModel> suppliers=database.Suppliers.ToList();
            
            return View();
        }
        public IActionResult Savesuppliers(SuppliersModel sup)
        {
            database.Suppliers.Add(sup);
            database.SaveChanges();
            return RedirectToAction("SupplierList");
        }
        public IActionResult Detail(int Id)
        {
            SuppliersModel suppliersModel = database.Suppliers.FirstOrDefault(x => x.Id == Id);

            return View(suppliersModel);

        }
        public IActionResult delete(int Id)
        {
            SuppliersModel sup = database.Suppliers.FirstOrDefault(x => x.Id == Id);
            database.Suppliers.Remove(sup);
            database.SaveChanges();
            return RedirectToAction("SupplierList");
        }
        public IActionResult Edit(int Id)
        {
            SuppliersModel sup = database.Suppliers.FirstOrDefault(x => x.Id == Id);
            return View(sup);
        }
        public IActionResult Update(SuppliersModel supplier)
        {
            database.Suppliers.Update(supplier);
            database.SaveChanges();
            return RedirectToAction("SupplierList");
            
            }
       

        }
    }
