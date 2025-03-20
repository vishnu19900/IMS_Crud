using IMS.data;
using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Cryptography;

namespace IMS.Controllers
{
    public class PurchasesController : Controller
    {
        IMSDb database;
        public PurchasesController(IMSDb _database)
        {
            database = _database;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PurchaseList()
        {
            List<PurchasesModel> suppl = database.Purchases.ToList();
            List<PurchaseReportModel> item2 = new List<PurchaseReportModel>();
            foreach (var item in suppl)
            {
                PurchaseReportModel obj2 = new PurchaseReportModel();
                obj2.Id = item.Id;
                obj2.Number_Received = item.Number_Received;
                obj2.SupplierId = item.SupplierId;
                obj2.ProductId = item.ProductId;
                obj2.Supplier_Name = database.Suppliers.FirstOrDefault(x => x.Id == item.SupplierId).Supplier_Name;
                obj2.ProductName = database.Products.FirstOrDefault(x => x.Id == item.ProductId).ProductName;
                obj2.Purchase_date = item.Purchase_date;
                item2.Add(obj2);

            }


            return View(item2);


        }

        public IActionResult AddPurchase()
        {
            List<SuppliersModel> suppList = database.Suppliers.ToList();
            List<SelectListItem> selectedList = new List<SelectListItem>();
            foreach (SuppliersModel item in suppList)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.Supplier_Name;
                obj.Value = item.Id.ToString();
                selectedList.Add(obj);
            }
            ViewBag.SupplierBag = selectedList;

            List<ProductModel> ProductList = database.Products.ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var item in ProductList)
            {
                SelectListItem obj = new SelectListItem();
                obj.Value = item.Id.ToString();
                obj.Text = item.ProductName;

                selectList.Add(obj);

            }
            ViewBag.ProductBag = selectList;


            return View();
        }
        public IActionResult SavePurchases(PurchasesModel sup)
        {
            database.Purchases.Add(sup);
            database.SaveChanges();
            return RedirectToAction("PurchaseList");
        }
        public IActionResult Detail(int Id)
        {
            PurchasesModel purchasesModel = database.Purchases.FirstOrDefault(x => x.Id == Id);

            return View(purchasesModel);

        }
        public IActionResult Delete(int Id)
        {
            PurchasesModel su = database.Purchases.FirstOrDefault(x => x.Id == Id);
            database.Purchases.Remove(su);
            database.SaveChanges();
            return RedirectToAction("PurchaseList");
        }
        public IActionResult Edit(int Id)
        {
            PurchasesModel sup = database.Purchases.FirstOrDefault(x => x.Id == Id);
            List<SuppliersModel> suppLists = database.Suppliers.ToList();
            List<SelectListItem> selectedLists = new List<SelectListItem>();
            foreach (var item in suppLists)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.Supplier_Name;
                obj.Value = item.Id.ToString();
                selectedLists.Add(obj);
            }
            ViewBag.SupplierBag = selectedLists;

            List<ProductModel> ProductList = database.Products.ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var item in ProductList)
            {
                SelectListItem obj = new SelectListItem();
                obj.Value = item.Id.ToString();
                obj.Text = item.ProductName;

                selectList.Add(obj);

            }
            ViewBag.ProductBag = selectList;


            return View(sup);
        }
        public IActionResult Update(PurchasesModel Purchase)
        {
            database.Purchases.Update(Purchase);
            database.SaveChanges();
            return RedirectToAction("PurchaseList");

        }
        public IActionResult PurchaseReport(int SupplierId, int ProductId, int MonthValue, string Text, int PageNumber)
        {
            int PageSize = 10;
            List<PurchasesModel> suppl = new List<PurchasesModel>();


            if (SupplierId > 0 && ProductId > 0)
            {
                suppl = database.Purchases.Where(p => p.SupplierId == SupplierId && p.ProductId == ProductId).ToList();

            }
            else if (SupplierId > 0)
            {
                suppl = database.Purchases.Where(p => p.SupplierId == SupplierId).ToList();
            }
            else if (ProductId > 0)
            {
                suppl = database.Purchases.Where(p => p.ProductId == ProductId).ToList();

            }
            else
            {
                suppl = database.Purchases.ToList();
            }

            // Filter Data month wise 
            if (MonthValue > 0)
            {
                suppl = suppl.Where(p => p.Purchase_date.Month == MonthValue).ToList();
            }


            List<PurchaseReportModel> item2 = new List<PurchaseReportModel>();
            foreach (var item in suppl)
            {
                PurchaseReportModel obj2 = new PurchaseReportModel();
                obj2.Id = item.Id;
                obj2.Number_Received = item.Number_Received;
                obj2.SupplierId = item.SupplierId;
                obj2.ProductId = item.ProductId;
                obj2.Supplier_Name = database.Suppliers.FirstOrDefault(x => x.Id == item.SupplierId).Supplier_Name;
                obj2.ProductName = database.Products.FirstOrDefault(x => x.Id == item.ProductId).ProductName;
                obj2.Purchase_date = item.Purchase_date;
                item2.Add(obj2);

            }
            if (Text != null)
            {
                item2 = item2.Where(p => p.Supplier_Name == Text
                || p.ProductName == Text).ToList();


            }

            // Bind Suppliers

            List<SuppliersModel> suppList = database.Suppliers.ToList();
            List<SelectListItem> selectedList = new List<SelectListItem>();

            selectedList.Add(new SelectListItem { Text = "All", Value = "All" });
            foreach (SuppliersModel item in suppList)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.Supplier_Name;
                obj.Value = item.Id.ToString();
                selectedList.Add(obj);
            }
            ViewBag.SupplierBag = selectedList;

            // Bind Product

            List<ProductModel> ProductList = database.Products.ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            selectList.Add(new SelectListItem { Text = "All", Value = "All" });

            foreach (var item in ProductList)
            {
                SelectListItem obj = new SelectListItem();
                obj.Value = item.Id.ToString();
                obj.Text = item.ProductName;

                selectList.Add(obj);

            }
            ViewBag.ProductBag = selectList;

            List<SelectListItem> selectMonthList = new List<SelectListItem>();
            selectMonthList.Add(new SelectListItem { Text = "All", Value = "All" });
            selectMonthList.Add(new SelectListItem { Text = "Jan", Value = "1" });
            selectMonthList.Add(new SelectListItem { Text = "Feb", Value = "2" });
            selectMonthList.Add(new SelectListItem { Text = "July", Value = "7" });
            selectMonthList.Add(new SelectListItem { Text = "June", Value = "6" });

            ViewBag.MonthList = selectMonthList;
            #region
            ViewBag.TotalPages = item2.Count() / PageSize;
            item2 = item2.Skip(PageNumber * PageSize).Take(PageSize).ToList();

            #endregion


            return View(item2);
        }

        public IActionResult PurchaseView(int SupplierId,int ProductId,String TextSearch,int PageNumber)
        {
            int PageSize = 9;
            List<PurchasesModel> purchasesList = database.Purchases.ToList();

            List<PurchaseReportModel> _purchasesView = new List<PurchaseReportModel>();
          

            if (SupplierId > 0 && ProductId > 0)
            {
                purchasesList = purchasesList.Where(p => p.SupplierId == SupplierId && p.ProductId == ProductId).ToList();
            }
            if (SupplierId > 0)
            {
                purchasesList = purchasesList.Where(p => p.SupplierId == SupplierId).ToList();
            }
            if (ProductId > 0)
            {
                purchasesList = purchasesList.Where(p => p.ProductId ==ProductId).ToList();

            }



            foreach (var item in purchasesList)
            {
                PurchaseReportModel _purchaseReportModel = new PurchaseReportModel();
                _purchaseReportModel.Id = item.Id;
                _purchaseReportModel.Supplier_Name = database.Suppliers.FirstOrDefault(x => x.Id == item.SupplierId).Supplier_Name;
                _purchaseReportModel.ProductName = database.Products.FirstOrDefault(x => x.Id == item.ProductId).ProductName;
                
                 _purchasesView.Add(_purchaseReportModel);
            }


            #region Bind Suppliers & Product

            List<SuppliersModel> suppList = database.Suppliers.ToList();
            List<SelectListItem> selectedList = new List<SelectListItem>();

            selectedList.Add(new SelectListItem { Text = "All", Value = "0" });
            foreach (SuppliersModel item in suppList)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = item.Supplier_Name;
                obj.Value = item.Id.ToString();
                selectedList.Add(obj);
            }
            ViewBag.SupplierBag = selectedList;

            

            //============== Bind Product Started ================

            List<ProductModel> ProductList = database.Products.ToList();
            List<SelectListItem> selectList = new List<SelectListItem>();
            selectList.Add(new SelectListItem { Text = "All", Value = "0" });

            foreach (var item in ProductList)
            {
                SelectListItem obj = new SelectListItem();
                obj.Value = item.Id.ToString();
                obj.Text = item.ProductName;

                selectList.Add(obj);

            }
            ViewBag.ProductBag = selectList;
            //============== Bind Product End ================
            #endregion

            if (TextSearch != null)
            {
                _purchasesView = _purchasesView.Where(p => p.Supplier_Name.ToLower() == TextSearch.ToLower()||
                p.ProductName.ToLower()==TextSearch).ToList();
            }
            #region
            ViewBag.TotalPages = _purchasesView.Count() / PageSize;
            _purchasesView = _purchasesView.Skip(PageNumber * PageSize).Take(PageSize).ToList();
            #endregion
            return View(_purchasesView);



         }
    }
}
