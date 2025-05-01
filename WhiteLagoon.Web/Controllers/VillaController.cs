﻿using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;  
        public VillaController(ApplicationDbContext db)
        {
            _db  = db;
        }
        public IActionResult Index()
        {
            var villas =_db.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _db.Villas.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Villa");
            }
            return View(obj);
        }

        public IActionResult Update(int VillaId)
        {
            Villa obj = _db.Villas.FirstOrDefault(u => u.Id == VillaId);

            //var VillaList = _db.Villas.Where(u => u.Price > 50 && u.Occupancy > 0);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
    }
}
