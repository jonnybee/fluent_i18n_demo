using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using FluentI18NDemo.Models;

namespace FluentI18NDemo.Controllers
{
    public class CountryController : Controller
    {
        private static List<Country> countryList = new List<Country>
        {
            new Country() {Id = 1, CountryCode = "en-US", CountryName = "America", TwoLetterIsoCode = "US"},
            new Country() {Id = 2, CountryCode = "nb-NO", CountryName = "Norge", TwoLetterIsoCode = "NO"},
        };

        // GET: Country
        public ActionResult Index()
        {
            return View(countryList);
        }

        // GET: Country/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Country/Create
        public ActionResult Create()
        {
            var country = new Country();
            return View(country);
        }

        // POST: Country/Create
        [HttpPost]
        public ActionResult Create(Country country)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var id = countryList.Max(p => p.Id) + 1;
                    country.Id = id;
                    countryList.Add(country);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Country/Edit/5
        public ActionResult Edit(int id)
        {
            var country = countryList.First(p => p.Id == id);
            return View(country);
        }

        // POST: Country/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //// GET: Country/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Country/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
