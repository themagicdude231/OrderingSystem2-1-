using OrderingSystem2.Models;
using OrderingSystem2.Models.Context;
using OrderingSystem2.Models.Tables;
using OrderingSystem2.otherClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderingSystem2.Controllers
{
    public class AdminUserController : Controller
    {
        // GET: AdminUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdminMainPage()
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
        public ActionResult AdminMenuViewPage()
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
        public ActionResult AdminOrderViewPage()
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
        public JsonResult getAdminMenu()
        {
            try
            {
                using(var db = new pickNServeContext())
                {
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var getVendor = db.tbl_Vendors.FirstOrDefault(x => x.User_Id == userId);
                    if(getVendor == null)
                    {
                        return Json(new { success = false });
                    }
                    else
                    {
                        var getMenus = (from Menus in db.tbl_Menus
                                        join Menu_Status in db.tbl_Menus_Status on Menus.Menu_Status_Id equals Menu_Status.Menu_Status_ID
                                        where Menus.Vendor_Id == getVendor.Vendor_ID
                                        select new { Menus, Menu_Status }).ToList();
                        return Json(getMenus, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch(Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException.ToString(), ex.Message);
                return Json(new { success = false });
            }
        }
        public JsonResult addMenu(tbl_menus_model menu)
        {
            try
            {
                using(var db = new pickNServeContext())
                {
                    var Request = HttpContext.Request;
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var getVendor = db.tbl_Vendors.FirstOrDefault(x => x.User_Id == userId);
                    if (db.tbl_Menus.Any(x => x.Vendor_Id == getVendor.Vendor_ID && x.Menu_Name == menu.Menu_Name))
                    {
                        return Json(new { success = false, message = "Item Already Exsisted" });
                    } 
                    else
                    {
                        string menuPhotoFileName = "";
                        if(Request.Files.Count > 0)
                        {
                            var Menu_File = Request.Files["Menu_Image"];
                            if(Menu_File != null && Menu_File.ContentLength > 0)
                            {
                                menuPhotoFileName = Guid.NewGuid() + System.IO.Path.GetExtension(Menu_File.FileName);
                                string filePath = Server.MapPath("~/Content/StoreFiles/");
                                if (!System.IO.Directory.Exists(filePath))
                                {
                                    System.IO.Directory.CreateDirectory(filePath);
                                }
                                string wholePath = Path.Combine(filePath, menuPhotoFileName);
                                Menu_File.SaveAs(wholePath);
                            }
                        }
                        var Menu_Info = new tbl_menus_model()
                        {
                            Vendor_Id = getVendor.Vendor_ID,
                            Menu_Name = Request.Form["Menu_Name"],
                            Menu_Description = Request.Form["Menu_Description"],
                            Menu_Price = decimal.Parse(Request.Form["Menu_Price"]),
                            Menu_Status_Id = int.Parse(Request.Form["Menu_Status"]),
                            Menu_Image = menuPhotoFileName
                        };
                        db.tbl_Menus.Add(Menu_Info);
                        db.SaveChanges();
                        return Json(new { success = true });
                    }
                }
            }
            catch(Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException?.ToString(), ex.Message);
                return Json(new { success = false, message = "an error Occured" });
            }
        }
        public JsonResult delMenu(ShowMenuModel model)
        {
            try
            {
                using (var db = new pickNServeContext())
                {
                    var getMenu = db.tbl_Menus.FirstOrDefault(x => x.Menu_ID == model.Menu_ID);
                    if (getMenu != null)
                    {
                        db.tbl_Menus.Remove(getMenu);
                        db.SaveChanges();
                        return Json(new { success = true });
                    }
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException?.ToString(), ex.Message);
                return Json(new { success = false });
            }
        }
        public JsonResult getAdminOrder()
        {
            try
            {
                using (var db = new pickNServeContext())
                {
                    var userId = Convert.ToInt32(Session["User_ID"]);
                    var getVendor = db.tbl_Vendors.FirstOrDefault(x => x.User_Id == userId);
                    if(getVendor == null)
                    {
                        return Json(new { success = false });
                    }
                    else
                    {
                        var getOrder = db.tbl_Orders.Where(o => o.Vendor_Id == getVendor.Vendor_ID).Select(o => new {
                            Orders = o,
                            User = db.tbl_Users.FirstOrDefault(u => u.User_ID == o.User_Id),
                            Payment = db.tbl_Payments.FirstOrDefault(p => p.Order_Id == o.Order_ID),
                            Payment_Method = (from p in db.tbl_Payments
                                              join pm in db.tbl_Payment_Methods
                                              on p.Payment_Method_Id equals pm.Payment_Method_ID
                                              where p.Order_Id == o.Order_ID
                                              select pm).FirstOrDefault(),
                            getOrderItem = (from oi in db.tbl_OrderItems
                                            join m in db.tbl_Menus on oi.Menu_Id equals m.Menu_ID
                                            where oi.Order_Id == o.Order_ID
                                            select new { OrderItem = oi, Menus = m }).ToList()
                        }).ToList();
                        return Json(getOrder, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch(Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException?.ToString(), ex.Message);
                return Json(new { success = false });
            }
        }
        public JsonResult markOrderComplete(int orderId)
        {
            try
            {
                using (var db = new pickNServeContext())
                {
                    var order = db.tbl_Orders.FirstOrDefault(o => o.Order_ID == orderId);
                    if (order == null)
                    {
                        return Json(new { success = false, message = "Order not found" });
                    }
                    db.tbl_Orders.Remove(order);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException?.ToString(), ex.Message);
                return Json(new { success = false, message = "Error deleting order" });
            }
        }
        public JsonResult getVendorCardStats()
        {
            try
            {
                using (var db = new pickNServeContext())
                {
                    int userId = Convert.ToInt32(Session["User_ID"]);
                    var vendor = db.tbl_Vendors.FirstOrDefault(v => v.User_Id == userId);
                    if (vendor == null)
                    {
                        return Json(new { success = false, message = "Vendor not found" }, JsonRequestBehavior.AllowGet);
                    }
                    var vendorId = vendor.Vendor_ID;
                    var orderItems = (from oi in db.tbl_OrderItems
                                      join m in db.tbl_Menus
                                      on oi.Menu_Id equals m.Menu_ID
                                      where m.Vendor_Id == vendorId
                                      select oi).ToList();
                    decimal totalSales = orderItems.Sum(x => (x.Price * x.Quantity));
                    int totalItemsSold = orderItems.Sum(x => x.Quantity);
                    int totalOrders = orderItems.Select(x => x.Order_Id).Distinct().Count();
                    var topMenu = (from oi in db.tbl_OrderItems
                                   join m in db.tbl_Menus
                                   on oi.Menu_Id equals m.Menu_ID
                                   where m.Vendor_Id == vendorId
                                   group oi by m.Menu_Name into g
                                   select new
                                   {
                                       MenuName = g.Key,
                                       Qty = g.Sum(x => x.Quantity)
                                   }).OrderByDescending(x => x.Qty).FirstOrDefault();
                    return Json(new { success = true, totalSales, totalOrders,
                                      totalItemsSold, topMenuItem = topMenu != null ? topMenu.MenuName : "N/A", topMenuQty = topMenu != null ? topMenu.Qty : 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException?.ToString(), ex.Message);
                return Json(new { success = false, message = "Error loading dashboard" });
            }
        }
        public JsonResult getDashboardCharts()
        {
            try
            {
                using (var db = new pickNServeContext())
                {
                    int userId = Convert.ToInt32(Session["User_ID"]);
                    var vendor = db.tbl_Vendors.FirstOrDefault(v => v.User_Id == userId);
                    if (vendor == null)
                    {
                        return Json(new { success = false });
                    }
                    int vendorId = vendor.Vendor_ID;
                    var items = (from oi in db.tbl_OrderItems
                                 join m in db.tbl_Menus
                                 on oi.Menu_Id equals m.Menu_ID
                                 where m.Vendor_Id == vendorId
                                 select new
                                 {
                                     m.Menu_Name,
                                     oi.Quantity,
                                     oi.Price,
                                     oi.Total,
                                     oi.Created_At
                                 }).ToList();
                    var pie = items
                        .GroupBy(x => x.Menu_Name)
                        .Select(g => new
                        {
                            label = g.Key,
                            value = g.Sum(x => x.Quantity)
                        })
                        .OrderByDescending(x => x.value)
                        .ToList();
                    var bar = items
                        .GroupBy(x => x.Created_At.Month)
                        .Select(g => new
                        {
                            month = g.Key,
                            revenue = g.Sum(x => x.Total)
                        })
                        .OrderBy(x => x.month)
                        .ToList();
                    var line = items
                        .GroupBy(x => x.Created_At.Date)
                        .Select(g => new
                        {
                            date = g.Key,
                            revenue = g.Sum(x => x.Total)
                        })
                        .OrderBy(x => x.date)
                        .ToList();
                    return Json(new { success = true, pieLabels = pie.Select(x => x.label), pieData = pie.Select(x => x.value),
                                      barLabels = bar.Select(x => new DateTime(2000, x.month, 1).ToString("MMMM")), barData = bar.Select(x => x.revenue),
                                      lineLabels = line.Select(x => x.date.ToString("MM-dd")), lineData = line.Select(x => x.revenue) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException?.ToString(), ex.Message);
                return Json(new { success = false });
            }
        }
    }
}