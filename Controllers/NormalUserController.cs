using OrderingSystem2.Models;
using OrderingSystem2.Models.Context;
using OrderingSystem2.Models.Tables;
using OrderingSystem2.otherClasses;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderingSystem2.Controllers
{
    public class NormalUserController : Controller
    {
        // GET: NormalUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NormalMainPage()
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("LoginPage", "UserAccounts");
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            return View();
        }
        public ActionResult MenuPage(int Vendor_ID)
        {
            ViewBag.Vendor_ID = Vendor_ID;
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("LoginPage", "UserAccounts");
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            return View();
        }
        public ActionResult NormalCartPage()
        {
            if (Session["User_ID"] == null)
            {
                return RedirectToAction("LoginPage", "UserAccounts");
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            return View();
        }
        public JsonResult getRestoInfo()
        {
            try
            {
                using (var db = new pickNServeContext())
                {
                    var getRestoInfo = (from Vendors in db.tbl_Vendors
                                        join Store_Category in db.tbl_Store_Categories on Vendors.Store_Category_Id equals Store_Category.Store_Category_ID
                                        select new { Vendors, Store_Category }).ToList();
                    return Json(getRestoInfo, JsonRequestBehavior.AllowGet);
                }
            } 
            catch(Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException?.ToString(), ex.Message);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getStoreCategory()
        {
            try
            {
                using (var db = new pickNServeContext())
                {
                    var getStoreCat = db.tbl_Store_Categories.Select(x => x).ToList();
                    List<StoreCategoryModel> storeCatBtns = new List<StoreCategoryModel>();
                    foreach(var storeCat in getStoreCat)
                    {
                        var storeCategory = new StoreCategoryModel()
                        {
                            StoreCategoryID = storeCat.Store_Category_ID,
                            StoreCategoryDesc = storeCat.Category_Description
                        };
                        storeCatBtns.Add(storeCategory);
                    }
                    return Json(storeCatBtns, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException?.ToString(), ex.Message);
                return Json(new { success = false });
            }
        }
        public JsonResult LogOut()
        {
            try
            {
                Session.Clear();
                Session.Abandon();

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException.ToString(), ex.Message);
                return Json(new { success = false });
            }
        }
        public JsonResult getfilterStoreList(FilterStoreModel RestoID)
        {
            using(var db = new pickNServeContext())
            {
                try
                {
                    var getStoreData = (from Vendors in db.tbl_Vendors
                                        join Store_Category in db.tbl_Store_Categories on Vendors.Store_Category_Id equals Store_Category.Store_Category_ID
                                        select new { Vendors, Store_Category });
                    if(RestoID.restoInfo != 0)
                    {
                        getStoreData = getStoreData.Where(x => x.Vendors.Store_Category_Id == RestoID.restoInfo);
                    }
                    return Json(getStoreData.ToList(), JsonRequestBehavior.AllowGet);
                }
                catch(Exception ex)
                {
                    errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException.ToString(), ex.Message);
                    return Json(new { success = false });
                }
            }
        }
        public JsonResult getSearchResult(SearchStoreModel userSearchResto)
        {
            try
            {
                using (var db = new pickNServeContext())
                {
                    var getRestoInfo = (from Vendors in db.tbl_Vendors
                                        join Store_Category in db.tbl_Store_Categories on Vendors.Store_Category_Id equals Store_Category.Store_Category_ID
                                        select new { Vendors, Store_Category }).ToList();
                    if (!string.IsNullOrEmpty(userSearchResto.userSearch))
                    {
                        var searchResto = userSearchResto.userSearch.ToLower();
                        getRestoInfo = getRestoInfo.Where(x => x.Vendors.Vendor_Storename.ToLower().Contains(searchResto)).ToList();
                    }
                    return Json(getRestoInfo, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException.ToString(), ex.Message);
                return Json(new { success = false });
            }
        }
        public JsonResult getRestoMenus(ShowMenuModel vendorId)
        {
            try
            {
                using(var db = new pickNServeContext())
                {
                    var getVendor = (from Vendors in db.tbl_Vendors
                                     join Category in db.tbl_Store_Categories on Vendors.Store_Category_Id equals Category.Store_Category_ID
                                     where Vendors.Vendor_ID == vendorId.Vendor_ID
                                     select new { Vendors, Category }).FirstOrDefault();
                    var getMenus = db.tbl_Menus.Where(x => x.Vendor_Id == vendorId.Vendor_ID).ToList();
                    return Json(new { Vendor = getVendor, Menu = getMenus }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException.ToString(), ex.Message);
                return Json(new { success = false });
            }
        }
        public JsonResult executeOrder(userOrder userWants)
        {
            try
            {
                using(var db = new pickNServeContext())
                {
                    if (userWants.groupedOrders == null || userWants.groupedOrders.Count == 0)
                    {
                        return Json(new { success = false, message = "Cart is empty" });
                    }
                    int userId = Convert.ToInt32(Session["User_ID"]);
                    foreach (var vendorGroup in userWants.groupedOrders)
                    {
                        int vendorId = vendorGroup.Vendor_ID;
                        var items = vendorGroup.Items;
                        var order = new tbl_orders_model
                        {
                            User_Id = userId,
                            Vendor_Id = vendorId,
                            Order_Total_Amount = items.Sum(i => i.Menu_Price * i.Quantity),
                            Order_Date = DateTime.Now,
                            Created_At = DateTime.Now,
                            Edited_At = DateTime.Now
                        };
                        db.tbl_Orders.Add(order);
                        db.SaveChanges();
                        var payment = new tbl_payments_model
                        {
                            Order_Id = order.Order_ID,
                            Payment_Method_Id = userWants.paymentMethodId,
                            Created_At = DateTime.Now,
                            Edited_At = DateTime.Now
                        };
                        db.tbl_Payments.Add(payment);
                        foreach (var item in items)
                        {
                            var orderItem = new tbl_orderitems_model
                            {
                                Order_Id = order.Order_ID,
                                Menu_Id = item.Menu_ID,
                                Quantity = item.Quantity,
                                Price = item.Menu_Price,
                                Total = item.Menu_Price * item.Quantity,
                                Created_At = DateTime.Now,
                                Edited_At = DateTime.Now
                            };
                            db.tbl_OrderItems.Add(orderItem);
                        }
                    }
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            catch(Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException?.ToString(), ex.Message);
                return Json(new { success = false, message = "an unkown error occur while processing your order" });
            }
        }
    }
}