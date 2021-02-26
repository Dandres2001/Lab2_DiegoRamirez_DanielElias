﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Lab2_DiegoRamirez_DanielElias.Models.Data;

using System.IO;


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

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] fields = lines[i].Split(",");
                        var newDrug = new Models.Drug();
                        newDrug.ID = Convert.ToInt32(fields[0]);
                        newDrug.Name = fields[1];
                        newDrug.Description = fields[2];
                        newDrug.Factory = fields[3];
                        newDrug.Price = Convert.ToDouble(fields[4]);
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
