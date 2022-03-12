using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Mvc;
using CrudOperationADONet.Models;

namespace CrudOperationADONet.Controllers
{
    public class ProductController : Controller
    {
        private IConfiguration con;

          public ProductController(IConfiguration connection)
        {
            con = connection;
        }
        public IActionResult Index()
        {
            List<Product> lstprd = new List<Product>();
            SqlConnection conn = new SqlConnection(con.GetConnectionString("DbConnection"));
            SqlCommand cmd = new SqlCommand("Select * from Products", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstprd.Add(new Product
                {
                    ProductId = Convert.ToInt32(dt.Rows[i]["ProductId"]),
                    Name = dt.Rows[i]["Name"].ToString(),
                    Description = dt.Rows[i]["Description"].ToString(),
                    UnitPrice = Convert.ToDecimal(dt.Rows[i]["UnitPrice"]),
                    CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryId"])
                    });
                }
                
            return View(lstprd);
        }
            public IActionResult Create()
        {
            SqlConnection conn = new SqlConnection(con.GetConnectionString("DbConnection"));
            SqlCommand cmd = new SqlCommand("Select * from Categories", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Category> cat = new List<Category>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cat.Add(new Category
                {
                    CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryId"]),
                    Name = dt.Rows[i]["Name"].ToString()
                });
            }
            ViewBag.Categories = cat;
            //Product lstprd = LoadProduct();
            return View();
        }
        public Product LoadProduct()
        {
            Product lstproduct = new Product();

            SqlConnection conn = new SqlConnection(con.GetConnectionString("DbConnection"));
            SqlCommand cmd = new SqlCommand("Select * from Products", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for(int i=0;i<dt.Rows.Count;i++)
            {
                Product obj = new Product();
                //obj.ProductId = Convert.ToInt32(dt.Rows[i]["ProductId"]);
                //obj.Name = dt.Rows[i]["Name"].ToString();
                //obj.Description = dt.Rows[i]["Description"].ToString();
                //obj.UnitPrice = Convert.ToDecimal(dt.Rows[i]["UnitPrice"]);
                //obj.CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryId"]);
                //lstproduct.Add(obj);

                //    lstproduct.Add(new Product {
                //    ProductId = Convert.ToInt32(dt.Rows[i]["ProductId"]),
                //    Name = dt.Rows[i]["Name"].ToString(),
                //    Description = dt.Rows[i]["Description"].ToString(),
                //    UnitPrice = Convert.ToDecimal(dt.Rows[i]["UnitPrice"]),
                //    CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryId"])
                //});

                lstproduct = new Product
                {
                    ProductId = Convert.ToInt32(dt.Rows[i]["ProductId"]),
                    Name = dt.Rows[i]["Name"].ToString(),
                    Description = dt.Rows[i]["Description"].ToString(),
                    UnitPrice = Convert.ToDecimal(dt.Rows[i]["UnitPrice"]),
                    CategoryId = Convert.ToInt32(dt.Rows[i]["CategoryId"])
                };
            
        }
            return lstproduct;
        }

        [HttpPost]
        public IActionResult Create(Product model)
        {
            ModelState.Remove("ProductId");
            SqlConnection conn = new SqlConnection(con.GetConnectionString("DbConnection"));
            conn.Open();
            SqlCommand cmd = new SqlCommand("AddUpdateProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", model.Name);
            cmd.Parameters.AddWithValue("@Description", model.Description);
            cmd.Parameters.AddWithValue("@UnitPrice", model.UnitPrice);
            cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
            cmd.Parameters.AddWithValue("@action", "add");
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
            return View();
        }
    }
}
