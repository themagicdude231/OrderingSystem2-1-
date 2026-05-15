using OrderingSystem2.Models.Context;
using OrderingSystem2.Models.Tables;
using OrderingSystem2.otherClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace OrderingSystem2.Controllers
{
    public class UserAccountsController : Controller
    {
        // GET: UserAccounts
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginPage()
        {
            return View();
        }
        public ActionResult RegisterPage()
        {
            return View();
        }
        public ActionResult StoreSetupPage()
        {
            return View();
        }
        public JsonResult RegisterUser(tbl_users_model user)
        {
            using (var db = new pickNServeContext())
            {
                try
                {
                    if (db.tbl_Users.Any(x => x.User_Email == user.User_Email || x.User_Username == user.User_Username))
                    {
                        return Json(new { success = false, message = "An user account with this username /or email has already exsisted" });
                    }
                    var userData = new tbl_users_model()
                    {
                        User_Fullname = user.User_Fullname,
                        User_Email = user.User_Email,
                        User_Address = user.User_Address,
                        User_Username = user.User_Username,
                        User_Password = user.User_Password,
                        User_isVendor = user.User_isVendor,
                        Created_At = DateTime.Now,
                        Edited_At = DateTime.Now
                    };
                    db.tbl_Users.Add(userData);
                    db.SaveChanges();
                    return Json(new { success = true, message = "User successfully registered" });
                }
                catch (Exception ex)
                {
                    errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException.ToString(), ex.Message);
                    return Json(new { success = false, message = "an error occured. failed to register user" });
                }
            }
        }
        public JsonResult RegisterVendor()
        {
            using(var db = new pickNServeContext())
            {
                try
                {
                    var Request = HttpContext.Request;
                    string Store_File_Name = "";
                    var userData = new tbl_users_model()
                    {
                        User_Fullname = Request.Form["User_Fullname"],
                        User_Email = Request.Form["User_Email"],
                        User_Address = Request.Form["User_Address"],
                        User_Username = Request.Form["User_Username"],
                        User_Password = Request.Form["User_Password"],
                        User_isVendor = Request.Form["User_isVendor"] == "true",
                        Created_At = DateTime.Now,
                        Edited_At = DateTime.Now
                    };
                    db.tbl_Users.Add(userData);
                    db.SaveChanges();
                    if (Request.Files.Count > 0)
                    {
                        var Store_File = Request.Files["Vendor_Storepicture"];
                        if (Store_File != null && Store_File.ContentLength > 0)
                        {
                            Store_File_Name = Guid.NewGuid() + System.IO.Path.GetExtension(Store_File.FileName);
                            string filePath = Server.MapPath("~/Content/StoreFiles/");
                            if (!System.IO.Directory.Exists(filePath))
                            {
                                System.IO.Directory.CreateDirectory(filePath);
                            }
                            string wholePath = Path.Combine(filePath, Store_File_Name);
                            Store_File.SaveAs(wholePath);
                        }
                    }
                    var vendorData = new tbl_vendors_model()
                    {
                        User_Id = userData.User_ID,
                        Store_Category_Id = Convert.ToInt32(Request.Form["Store_Category_Id"]),
                        Vendor_Storename = Request.Form["Vendor_Storename"],
                        Vendor_Store_Address = Request.Form["Vendor_Store_Address"],
                        Vendor_Store_Picture = Store_File_Name,
                        Created_At = DateTime.Now,
                        Edited_At = DateTime.Now
                    };
                    db.tbl_Vendors.Add(vendorData);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Vendor successfully registered" });
                }
                catch (Exception ex)
                {
                    errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException.ToString(), ex.Message);
                    return Json(new { success = false, message = "an error occured. failed to register vendor" });
                }
            }
        }
        public JsonResult checkUser(tbl_users_model tempUser)
        {
            using(var db =  new pickNServeContext())
            {
                try
                {
                    if (db.tbl_Users.Any(x => x.User_Username == tempUser.User_Username || x.User_Email == tempUser.User_Email))
                    {
                        return Json(new { success = false });
                    }
                    else
                    {
                        return Json(new { success = true });
                    }
                }
                catch (Exception ex)
                {
                    errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException.ToString(), ex.Message);
                    return Json(new { success = false, message = "an error occured. failed to register vendor" });
                }
            }
        }
        public JsonResult authenticateUser(tbl_users_model authInfo)
        {
            using (var db = new pickNServeContext())
            {
                try
                {
                    var userRole = db.tbl_Users.FirstOrDefault(x => x.User_Username == authInfo.User_Username && x.User_Password == authInfo.User_Password);
                    if (userRole != null)
                    {
                        Session["User_ID"] = userRole.User_ID;
                        Session["User_Fullname"] = userRole.User_Fullname;
                        Session["User_Email"] = userRole.User_Email;
                        Session["User_Address"] = userRole.User_Address;
                        string role;
                        if (userRole.User_isVendor)
                        {
                            role = "Vendor";
                        }
                        else
                        {
                            role = "Normal";
                        }
                        return Json(new { success = true, role = role });
                    }
                    else
                    {
                        return Json(new { success = false, message = "User not Available" });
                    }
                }
                catch (Exception ex)
                {
                    errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException.ToString(), ex.Message);
                    return Json(new { success = false, message = "An unexpected Error Occur" });
                }
            }
        }
        // for normal user (potenital most likely for admin user as well)
        public ActionResult getUserInfo()
        {
            try
            {
                return Json(new { User_ID = Session["User_ID"], User_Fullname = Session["User_Fullname"], User_Email = Session["User_Email"], User_Address = Session["User_Address"] }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException.ToString(), ex.Message);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        // for editing user profile
        public JsonResult updateUserInfo(tbl_users_model User)
        {
            try
            {
                using (var db = new pickNServeContext())
                {
                    var getUser = db.tbl_Users.FirstOrDefault(x => x.User_ID == User.User_ID);
                    if (getUser == null)
                    {
                        return Json(new { success = false, message = "User Not Found" });
                    }
                    else
                    {
                        getUser.User_Fullname = User.User_Fullname;
                        getUser.User_Address = User.User_Address;
                        getUser.User_Email = User.User_Email;
                        getUser.Edited_At = DateTime.Now;
                        db.SaveChanges();
                        Session["User_Fullname"] = getUser.User_Fullname;
                        Session["User_Email"] = getUser.User_Email;
                        Session["User_Address"] = getUser.User_Address;
                        return Json(new { success = true });
                    }
                }
            }
            catch (Exception ex)
            {
                errorHandlerClass.ErrorHandler(ex.StackTrace, ex.InnerException.ToString(), ex.Message);
                return Json(new { success = false, message = "Failed to Update User Information" });
            }
        }
    }
}