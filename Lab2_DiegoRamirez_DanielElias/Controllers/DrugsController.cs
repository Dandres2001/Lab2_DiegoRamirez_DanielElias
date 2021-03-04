﻿using Microsoft.AspNetCore.Hosting;
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
using LibreriaRD2;

namespace Lab2_DiegoRamirez_DanielElias.Controllers
{
    public class DrugsController : Controller
    {

        public static string preorderinfo = "";
        Binarytree<Drug> index = new Binarytree<Drug>();
        Manual_List<Drug> listita = new Manual_List<Drug>();
   public static   string search;
        IWebHostEnvironment hostingEnvironment;
        public DrugsController(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

        }
         

        [HttpGet]
        public ActionResult AddToOrder(int id)
        {
            Drug selected;


            var newDrug3 = new Models.Drug();

            newDrug3.Name = search;

            selected = Singleton.Instance.DrugsList.ElementAt(Singleton.Instance.Drugindex.find(newDrug3, Singleton.Instance.Drugindex.root).Data.ID);

            return View(selected);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToOrder(int id, IFormCollection collection)
        {
            var newDrug3 = new Models.Drug();

            newDrug3.Name = search;

            int i;
            try
            {
                Drug selected;

              

                int quantity = Convert.ToInt32(collection["OrderedQuantity"]);
                selected = Singleton.Instance.DrugsList.ElementAt(Singleton.Instance.Drugindex.find(newDrug3, Singleton.Instance.Drugindex.root).Data.ID);
                selected.OrderedQuantity = quantity;

                if ((selected.OrderedQuantity <= selected.Stock)&& quantity != 0)
                {
                    selected.Stock = selected.Stock - quantity ;
                    Singleton.Instance.OrderedDrugs.AddLast(selected);
                    Drugadded();
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
        public ActionResult csvok()
        {
            TempData["alertMessage"] = "Csv imported successfully!";
            return View();
        }
        public ActionResult Drugadded()
        {
            TempData["alertMessage"] = "Drug added to order!";
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
            OrderText += "Address: " + collection["Address"] + "\n";
            OrderText += "NIT: " + collection["NIT"] + "\n";
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
                        var newDrug2  = new Models.Drug();
                        newDrug.ID = Convert.ToInt32(fields[0]);
                        newDrug.Name = fields[1];
                        newDrug.Description = fields[2];
                        newDrug.Factory = fields[3];
                        newDrug.Price = fields[4];
                        newDrug.Stock = Convert.ToInt32(fields[5]);

                        newDrug2.ID = Convert.ToInt32(fields[0])-1;
                        newDrug2.Name = fields[1];
                        object obj = newDrug2;
                        Func<Drug, int> Comparer = x => x.Name.CompareTo(newDrug2.Name);
                        string name  = newDrug2.Name;
                        Singleton.Instance.DrugsList.AddLast(newDrug);
                        Singleton.Instance.Drugindex.insert(newDrug2);
                        index.insert(newDrug2);
                       preorderinfo= Singleton.Instance.Drugindex.preorder(index.root, preorderinfo);
                  
                    }
              

                }
            }
            csvok();
            return RedirectToAction("Index");

        }

        public ActionResult GetLog()
        {


            return File(Encoding.UTF8.GetBytes(preorderinfo), "text/csv", "Log.txt");

        }
        // GET: DrugsController
        public ActionResult Index()
        {
          
          
            return View(Singleton.Instance.DrugsList);
        }

        // GET: DrugsController/Details/5
      
        public ActionResult Client()
        {
            return View();
        }
        // POST: DrugsController/Delete/5
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(IFormCollection collection)
        {

            try
            {


                search = collection["Name"]; 

                

              
             
              
                return RedirectToAction(nameof(AddToOrder));

            }
            catch
            {
                return View();
            }

        }

    }
}
