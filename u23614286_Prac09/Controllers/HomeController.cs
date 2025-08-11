using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace u23614286_Prac09.Controllers
{
    public class HomeController : Controller
    {
        // Declare the connection here using your global connection string
        SqlConnection myConnection = new SqlConnection(Globals.ConnectionString);
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoInsert(string FullName, string ClubName, int Age, decimal MembershipFee)
        {

            try
            {
                // Build the SQL command string — but this is unsafe and open to SQL injection!
                string sql = "INSERT INTO ClubMembership (FullName, ClubName, Age, MembershipFee) VALUES ('"
                             + FullName + "', '"
                             + ClubName + "', "
                             + Age + ", "
                             + MembershipFee + ")";

                SqlCommand myInsertCommand = new SqlCommand(sql, myConnection);

                myConnection.Open();
                int rowsAffected = myInsertCommand.ExecuteNonQuery();
                ViewBag.Message = "Success: " + rowsAffected + " row added to the Database.";
            }
            catch (Exception err)
            {
                ViewBag.Message = "Error: " + err.Message;
            }
            finally
            {
                myConnection.Close();
            }
            return View("Index");
        }
        // GET: Show the search form
        public ActionResult Search()
        {
            return View();
        }

        // POST: Handle search form submission
        [HttpPost]
        public ActionResult Search(int id)
        {
            try
            {
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM ClubMembership WHERE Id = " + id, myConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    TempData["Id"] = reader["Id"];
                    TempData["FullName"] = reader["FullName"].ToString();
                    TempData["ClubName"] = reader["ClubName"].ToString();
                    TempData["Age"] = Convert.ToInt32(reader["Age"]);
                    TempData["MembershipFee"] = Convert.ToDecimal(reader["MembershipFee"]);
                    reader.Close();

                    return RedirectToAction("Update");
                }
                else
                {
                    ViewBag.Message = "Member with this ID not found.";
                    reader.Close();
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return View();
            }
            finally
            {
                myConnection.Close();
            }
        }

        // Show initial update page with ID input
        public ActionResult Update()
        {
            // TempData values are already set from Search POST
            return View();
        }

        [HttpPost]
        public ActionResult DoUpdate(int id, string FullName, string ClubName, int Age, decimal MembershipFee)
        {
            try
            {
                string updateQuery = "UPDATE ClubMembership SET FullName = '" + FullName + "', ClubName = '" + ClubName +
                    "', Age = " + Age + ", MembershipFee = " + MembershipFee + " WHERE Id = " + id;

                SqlCommand myUpdateCommand = new SqlCommand(updateQuery, myConnection);

                myConnection.Open();
                int rowsAffected = myUpdateCommand.ExecuteNonQuery();
                ViewBag.Message = "Success: " + rowsAffected + " row updated.";
            }
            catch (Exception err)
            {
                ViewBag.Message = "Error: " + err.Message;
            }
            finally
            {
                myConnection.Close();
            }
            return View("Index");
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoDelete(int id)
        {
            try
            {
                SqlCommand myDeleteCommand = new SqlCommand("Delete from ClubMembership where Id = " + id, myConnection);

                myConnection.Open();
                int rowsAffected = myDeleteCommand.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    ViewBag.Message = "Error: No record found for the given ID!";
                }
                else
                {
                    ViewBag.Message = "Success: " + rowsAffected + " record deleted.";
                }

            }


            catch (Exception err)
            {
                ViewBag.Message = "Error: " + err.Message;
            }
            finally
            {
                myConnection.Close();
            }
            return View("Index");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}