using Eli.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eli.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult IndexReport()
        {       
                return View();
        }

        public ActionResult FinanceFactorDebatorsReport()
        {
            List<ViewFinanceFactorDebator>items=new List<ViewFinanceFactorDebator>();
            string connectionString = "Data Source=Eden-PC\\SQLEXPRESS1;Initial Catalog=Eli;Integrated Security=True";

            using (var con= new SqlConnection(connectionString))
            {
                string qry="SELECT * FROM ViewFinanceFactorDebator";
                var cmd= new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                using (SqlDataReader objReader = cmd.ExecuteReader())
                {
                    if (objReader.HasRows)
                    {              
                        while (objReader.Read())
                        {
                          //I would also check for DB.Null here before reading the value.
                           string item= objReader.GetString(objReader.GetOrdinal("*"));
                           //items.Add(item);                  
                        }
                    }
                }
            }
            EliManagerDB db = new EliManagerDB();

            return View(db.getAllDebators());
        }

        public ActionResult GeneratePDF()
        {
            return new Rotativa.ActionAsPdf("FinanceFactorDebatorsReport");

        }
    }
}
