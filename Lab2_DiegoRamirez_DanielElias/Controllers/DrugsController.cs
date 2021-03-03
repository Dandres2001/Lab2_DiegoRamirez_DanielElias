using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Lab2_DiegoRamirez_DanielElias.Models;
using Lab2_DiegoRamirez_DanielElias.Models.Data;
using Microsoft.VisualBasic;
using System.IO;
using Microsoft.VisualBasic.FileIO;


namespace Lab2_DiegoRamirez_DanielElias.Controllers
{
    public class DrugsController : Controller
    {
        
        IWebHostEnvironment hostingEnvironment;
        public DrugsController(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

        }
         

        [HttpGet]
        public ActionResult AddToOrder(int id)
        {
            Drug selected;
            int i;
            for ( i = 0; Singleton.Instance.DrugsList.Length > i; i++)
            {
                selected = Singleton.Instance.DrugsList.ElementAt(i);


                if (selected.ID == id)
                { 
                    break;
                }

            }

            selected = Singleton.Instance.DrugsList.ElementAt(i);
            return View(selected);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToOrder(int id, IFormCollection collection)
        {


            int i;
            try
            {
                Drug selected;

                for (i = 0; Singleton.Instance.DrugsList.Length > i; i++)
                {
                    selected = Singleton.Instance.DrugsList.ElementAt(i);
                    if (selected.ID == id)
                    { 
                        break;
                    }
                }

                int quantity = Convert.ToInt32(collection["OrderedQuantity"]);
                selected = Singleton.Instance.DrugsList.ElementAt(i);
                selected.OrderedQuantity = quantity;

                if ((selected.OrderedQuantity <= selected.Stock)&& quantity != 0)
                {
                    selected.Stock = selected.Stock - quantity ;
                    Singleton.Instance.OrderedDrugs.AddLast(selected);
                }
                else
                {
                    OutOfStockMessage();
                }

                return RedirectToAction(nameof(AddToOrder));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult OutOfStockMessage()
        {
            TempData["alertMessage"] = "OUT OF STOCK!";
            return View();
        }
        public ActionResult ExportOrder()
        {
            TempData["alertMessage"] = "Order made successfully!";
            return View();
        }
        [HttpGet]

        [HttpGet]
        public ActionResult FinalOrder()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalOrder(IFormCollection collection)
        {
            
            Drug selected;
            double totalPrice = 0; ;
            string OrderText = "";
            OrderText += "Name: " + collection["Name"] + "\n";
            OrderText += "Address" + collection["Address"] + "\n";
            OrderText += "NIT " + collection["NIT"] + "\n";
            OrderText +=  "\n";
            OrderText += "Listed order: " + "\n" + "\n";


            for (int i = 0; Singleton.Instance.OrderedDrugs.Length > i; i++)
            {
                double price;

                selected = Singleton.Instance.OrderedDrugs.ElementAt(i);
                price = Convert.ToDouble(selected.Price.Substring(1, selected.Price.Length-1));
                price = price * selected.OrderedQuantity;
               
                OrderText += selected.Name + " x " + selected.OrderedQuantity.ToString() + ": $" + price.ToString() + "\n";
                totalPrice += price;
            }
           
            OrderText += "\n" + "Total: $" + totalPrice.ToString();


            return File(Encoding.UTF8.GetBytes(OrderText), "text/csv", "Order.txt");
            return View();
        }
      

        public ActionResult ReadFile()
        {

            return View();
        }
        [HttpPost]
        public IActionResult ReadFile(FileClass model)
        {
            

            if (ModelState.IsValid)
            {
                string uFileName = null;
                if (model.csv != null)
                {
                    string uploadFolder = Path.Combine(this.hostingEnvironment.WebRootPath, "csv");
                    uFileName = Guid.NewGuid().ToString() + model.csv.FileName;
                    string filePath = Path.Combine(uploadFolder, uFileName);

                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        model.csv.CopyTo(fileStream);
                        fileStream.Flush();

                    }
                    string[] lines = System.IO.File.ReadAllLines(filePath);

                    TextReader reader = new StreamReader(filePath);
                    TextFieldParser csvReader = new TextFieldParser(reader);
                    csvReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    csvReader.SetDelimiters(",");
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] fields;

                    while (!csvReader.EndOfData)
                    {
                        fields = csvReader.ReadFields();
                        var newDrug = new Models.Drug();
                        newDrug.ID = Convert.ToInt32(fields[0]);
                        newDrug.Name = fields[1];
                        newDrug.Description = fields[2];
                        newDrug.Factory = fields[3];
                        newDrug.Price = fields[4];
                        newDrug.Stock = Convert.ToInt32(fields[5]);
                        Singleton.Instance.DrugsList.AddLast(newDrug);
                        
                    }

                       
               }
            }

            return RedirectToAction("Index");

        }


        // GET: DrugsController
        public ActionResult Index()
        {
            return View(Singleton.Instance.DrugsList);
        }

        // GET: DrugsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DrugsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DrugsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DrugsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DrugsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DrugsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DrugsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
