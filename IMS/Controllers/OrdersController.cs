using IMS.data;
using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace IMS.Controllers
{
    public class OrdersController : Controller
    {
        IMSDb database;
        public OrdersController(IMSDb _database)
        {
            database = _database;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrdersList()
        {
            List<OrdersModel> ord = database.Orders.ToList();
            List<OrdersViewModel> ord2 = new List<OrdersViewModel>();
            foreach (var item in ord)
            {
                OrdersViewModel obj = new OrdersViewModel();
                obj.Id = item.Id;
                obj.Title = item.Title;
                obj.First = item.First;
                obj.Middle = item.Middle;
                obj.Last = item.Last;
                obj.MobileNo = item.MobileNo;
                obj.ProductId = item.ProductId;
                obj.ProductName = database.Products.FirstOrDefault(x => x.Id == item.ProductId).ProductName;
                obj.SuppliersId = item.SuppliersId;
                obj.Supplier_Name = database.Suppliers.FirstOrDefault(x => x.Id == item.SuppliersId).Supplier_Name;
                obj.NumberShipped = item.NumberShipped;
                obj.OrderDate = item.OrderDate;
                ord2.Add(obj);

            }

            return View(ord2);
        }
        public IActionResult AddOrders()
        {
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


            List<SuppliersModel> supplier = database.Suppliers.ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in supplier)
            {
                SelectListItem item2 = new SelectListItem();
                item2.Value = item.Id.ToString();
                item2.Text = item.Supplier_Name;

                items.Add(item2);
            }
            ViewBag.SuppliersBag = items;
            return View();
        }
        public IActionResult SaveOrders(OrdersModel sub)
        {
            database.Orders.Add(sub);
            database.SaveChanges();
            return RedirectToAction("OrdersList");
        }
        public IActionResult Detail(int Id)
        {
            OrdersModel ordersModel = database.Orders.FirstOrDefault(x => x.Id == Id);
            return View(ordersModel);
        }
        public IActionResult Delete(int Id)
        {
            OrdersModel sub = database.Orders.FirstOrDefault(y => y.Id == Id);
            database.Orders.Remove(sub);
            database.SaveChanges();
            return RedirectToAction("OrdersList");
        }
        public IActionResult Edit(int Id)
        {
            OrdersModel ordersModel = database.Orders.FirstOrDefault(y => y.Id == Id);
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


            List<SuppliersModel> supplier = database.Suppliers.ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in supplier)
            {
                SelectListItem item2 = new SelectListItem();
                item2.Value = item.Id.ToString();
                item2.Text = item.Supplier_Name;

                items.Add(item2);
            }
            ViewBag.SuppliersBag = items;
            return View(ordersModel);
        }


        public IActionResult Update(OrdersModel subs)
        {
            database.Orders.Update(subs);
            database.SaveChanges();
            return RedirectToAction("OrdersList");
        }


        public IActionResult OrderReport(int SuppliersId, int ProductId, int Month, string TextSearch, int PageNumber)
        {
            int PageSize = 4;
            

            List<OrdersModel> ord = database.Orders.ToList();            

            #region Filter Order Behalf of SuppliersId, ProductId
            if (SuppliersId > 0 && ProductId > 0)
            {
                ord = ord.Where(Id => Id.SuppliersId == SuppliersId && Id.ProductId == ProductId).ToList();

            }
            else if (SuppliersId > 0)
            {
                ord = ord.Where(Id => Id.SuppliersId == SuppliersId).ToList();
            }
            else if (ProductId > 0)
            {
                // select * from order where ProductId = 2
                ord = ord.Where(Id => Id.ProductId == ProductId).ToList();
            }

            #endregion

            #region Filter Data behalf of selected Mont

            if (Month > 0)
            {
                ord = ord.Where(p => p.OrderDate.Month == Month).ToList();
            }
            #endregion

            #region Add Name of Supplier & Product 

            List<OrderReportModel> _orderReportModelList = new List<OrderReportModel>();
            foreach (var item in ord)
            {
                OrderReportModel _orderReportModel = new OrderReportModel();
                _orderReportModel.SuppliersId = item.SuppliersId;
                _orderReportModel.ProductId = item.ProductId;
                _orderReportModel.Supplier_Name = database.Suppliers.FirstOrDefault(x => x.Id == item.SuppliersId).Supplier_Name;
                _orderReportModel.ProductName = database.Products.FirstOrDefault(x => x.Id == item.ProductId).ProductName;
                _orderReportModel.MobileNo = item.MobileNo;
                _orderReportModel.Id = item.Id;
                _orderReportModel.First = item.First;
                _orderReportModel.Last = item.Last;
                _orderReportModel.OrderDate = item.OrderDate;

                _orderReportModelList.Add(_orderReportModel);
            }
            #endregion

            #region Filter by Text

            if (TextSearch != null)
            {
                _orderReportModelList = _orderReportModelList.Where(p => 
                                       p.First?.ToLower() == TextSearch.ToLower()
                                       //p.First.ToLower().Contains(TextSearch.ToLower())
                                       || p.Last?.ToLower() == TextSearch.ToLower()
                                       || p.Middle?.ToLower() == TextSearch.ToLower()
                                       || p.ProductName.ToLower() == TextSearch.ToLower()
                                       || p.Supplier_Name.ToLower() == TextSearch.ToLower()
                                        ).ToList();
            }

            #endregion

            #region Bind Supplier

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

            #endregion

            #region Bind Product

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

            #region Bind Month

            List<SelectListItem> selectMonthList = new List<SelectListItem>();
            selectMonthList.Add(new SelectListItem { Text = "All", Value = "0" });
            selectMonthList.Add(new SelectListItem { Text = "Jan", Value = "1" });
            selectMonthList.Add(new SelectListItem { Text = "Feb", Value = "2" });
            selectMonthList.Add(new SelectListItem { Text = "June", Value = "6" });
            selectMonthList.Add(new SelectListItem { Text = "July", Value = "7" });

            ViewBag.MonthList = selectMonthList;
            #endregion

            #region Pagging
            ViewBag.TotalPages = _orderReportModelList.Count() / PageSize;
            _orderReportModelList = _orderReportModelList.Skip(PageNumber * PageSize).Take(PageSize).ToList();
            
            #endregion

            return View(_orderReportModelList);
        }
        public IActionResult OrderView(int SuppliersId,int ProductId, string TextSearch,int Month,int pageNumber)
        {
            int PageSize = 4;


            #region show the name supplier And Product
            List<OrdersModel> _Ord = database.Orders.ToList();
            
            List<OrdersViewModel> _OrdersViewModelList = new List<OrdersViewModel>();
            foreach (var item in _Ord)
            { 
             OrdersViewModel _name = new OrdersViewModel();
                _name.Title = item.Title;
                _name.First = item.First;
                _name.Last = item.Last;
                _name.SuppliersId=item.SuppliersId;
                _name.Supplier_Name = database.Suppliers.FirstOrDefault(x => x.Id == item.SuppliersId).Supplier_Name;
                _name.ProductId = item.ProductId;
                _name.ProductName = database.Products.FirstOrDefault(x => x.Id == item.ProductId).ProductName;
                _name.OrderDate = item.OrderDate;
                _OrdersViewModelList.Add(_name);
            }
            if (SuppliersId > 0 && ProductId > 0)
            {
                _OrdersViewModelList = _OrdersViewModelList.Where(p=>p.SuppliersId==SuppliersId && p.ProductId == ProductId).ToList();
            }
            #endregion
            if(ProductId >0)
            {
                _OrdersViewModelList = _OrdersViewModelList.Where(p => p.ProductId == ProductId).ToList();
            }
            if (SuppliersId > 0)
            {
                _OrdersViewModelList = _OrdersViewModelList.Where(p => p.SuppliersId == SuppliersId).ToList();
            }

            if (TextSearch != null)
            {
                _OrdersViewModelList = _OrdersViewModelList.Where(p=>p.First.ToLower() == TextSearch.ToLower()
                ||p.Last.ToLower()==TextSearch.ToLower()
                ||p.Supplier_Name ==TextSearch.ToLower()
                ||p.ProductName.ToLower()==TextSearch.ToLower()).ToList();
            }
            if(Month >0 )
            {
                _OrdersViewModelList = _OrdersViewModelList.Where(p=>p.OrderDate.Month==Month).ToList();
            }
            
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

            List<ProductModel> _prod =database.Products.ToList();
            List<SelectListItem> _selectLists=new List<SelectListItem>();
            _selectLists.Add(new SelectListItem { Text = "All", Value = "0" });
            foreach (ProductModel item in _prod)
            { 
            SelectListItem objs = new SelectListItem();
                objs.Text = item.ProductName;
                objs.Value = item.Id.ToString();
                _selectLists.Add(objs);
            }
            ViewBag.Products = _selectLists;

            List<SelectListItem> _selectedList = new List<SelectListItem>();
            _selectedList.Add(new SelectListItem { Text = "All", Value="0" });
            _selectedList.Add(new SelectListItem { Text = "Jan", Value = "1" });
            _selectedList.Add(new SelectListItem { Text = "june", Value = "6" });
            _selectedList.Add(new SelectListItem { Text = "july", Value = "7" });
            ViewBag.SelectedMonth = _selectedList;

            
            ViewBag.Totalpages = _OrdersViewModelList.Count() / PageSize;
            _OrdersViewModelList=_OrdersViewModelList.Skip(PageSize).Take(pageNumber*PageSize).ToList();
            return View(_OrdersViewModelList);


        }


    }
}
