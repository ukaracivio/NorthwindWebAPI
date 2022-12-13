using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NorthwindWebAPI.Models;
using System.Data;
using System.Data.OleDb;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;



namespace NorthwindWebAPI.Controllers
{
    public class TransferController : Controller
    {
        private IWebHostEnvironment Environment;
        private IConfiguration Configuration;

        public TransferController(IWebHostEnvironment _environment, IConfiguration _configuration)
        {
            Environment = _environment;
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile file)
        {
            if (file != null) // Eğer benim dosyam view ekranından geldiyse...
            {
                // 1. Gelen dosyayı kendime saklamak istiyorum.

                // ilk önce dbir directory yaratıyorum.
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads"); // wwwwroot/Uploads

                if (!Directory.Exists(path)) // Tanımlamış olduğum directory yoksa --> yarat
                {
                    Directory.CreateDirectory(path); // burada ilgili dizinde alt directory oluşturuluyor.
                }

                // Burada dosyayı ve yazacağı yeri oluşturuyor.
                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(path, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // 2. Bu gelen dosya içeriğini VT na yazma.

                try
                {
                    // İlkönce excel için gerekli connstr okuması(System.Data.OleDB)

                    string ExcelConnStr = this.Configuration.GetConnectionString("ExcelConnStr");

                    DataTable dt = new DataTable(); // Excel verisinin dolacağı DataTable
                    ExcelConnStr = string.Format(ExcelConnStr, filePath);

                    // artık geldi sıra veriyi okumaya 
                    using (OleDbConnection connExcel = new OleDbConnection(ExcelConnStr))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                // Excel dosyası içersindeki sheet ismini öğrenme

                                connExcel.Open();

                                DataTable dtExcelSchema;

                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString(); // sheet adı

                                cmdExcel.CommandText = "SELECT * FROM [" + sheetName + "]"; // aynı sql serverdaki gibi select * from ["sheetname"]

                                odaExcel.SelectCommand = cmdExcel;

                                odaExcel.Fill(dt); // Adapterden gelen bilgi data direkt datatablea yerleştiriliyor.

                                connExcel.Close();

                            }
                        }
                    }

                    // sıra geldi SQL'e yazmaya

                    string SQLConnStr = this.Configuration.GetConnectionString("ConnStr");

                    //Excel'den okuduğum veriyi SQL'e ve ilgili tabloya postalıyorum
                    using (SqlConnection con = new SqlConnection(SQLConnStr))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //veritabanındaki tablonun adı
                            sqlBulkCopy.DestinationTableName = "dbo.Products"; // SQL tarafındaki tablo

                            //Opsiyonel : veritabanı tablo kolonları ile excel tablo kolonlarını map etme
                            //sqlBulkCopy.ColumnMappings.Add("Id", "CustomerId");
                            sqlBulkCopy.ColumnMappings.Add("ProductName", "ProductName");
                            sqlBulkCopy.ColumnMappings.Add("SupplierID", "SupplierID");
                            sqlBulkCopy.ColumnMappings.Add("CategoryID", "CategoryID");
                            sqlBulkCopy.ColumnMappings.Add("QuantityPerUnit", "QuantityPerUnit");
                            sqlBulkCopy.ColumnMappings.Add("UnitPrice", "UnitPrice");
                            sqlBulkCopy.ColumnMappings.Add("UnitsInStock", "UnitsInStock");
                            sqlBulkCopy.ColumnMappings.Add("UnitsOnOrder", "UnitsOnOrder");
                            sqlBulkCopy.ColumnMappings.Add("ReorderLevel", "ReorderLevel");
                            sqlBulkCopy.ColumnMappings.Add("Discontinued", "Discontinued");

                            con.Open(); // sql bağlantısını açmak

                            sqlBulkCopy.WriteToServer(dt); // excel verilerinin bulunduğu data table ı direkt olarak sql tarafına postalama

                            con.Close();
                        }
                    }

                    ViewBag.Message = "Excel tablosu başarıyla Import edildi....Veritabanını inceleyebilirsiniz....";
                }
                catch (Exception ex)
                {

                    ViewBag.Message = "Opps : Bir yerlerde bir hata var...." + ex.ToString();

                }

            }

            return View();
        }

    }
}
