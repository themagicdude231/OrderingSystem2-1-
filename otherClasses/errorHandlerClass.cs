using OrderingSystem2.Models.Context;
using OrderingSystem2.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.otherClasses
{
    public static class errorHandlerClass
    {
        public static void ErrorHandler(string stackTrace, string innerException, string message)
        {
            using (var db = new pickNServeContext())
            {
                try
                {
                    var errorLog = new tbl_error_logs_model()
                    {
                        Error_Description = $"{stackTrace} | {innerException} | {message}",
                        Created_At = DateTime.Now,
                        Edited_At = DateTime.Now
                    };
                    db.tbl_ErrorLogs.Add(errorLog);
                    db.SaveChanges();
                }
                catch
                {

                }
            }
        }
    }
}