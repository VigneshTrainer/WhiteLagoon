﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize(Roles=SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties:"Villa");
            return View(amenities);
        }

        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                villaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {

            //ModelState.Remove("Villa");


            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction("Index", "Amenity");
            }

          
            obj.villaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Update(int AmenityId)
        {

            AmenityVM amenityVM = new()
            {
                villaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == AmenityId)
            };

            //var VillaList = _db.Villas.Where(u => u.Price > 50 && u.Occupancy > 0);
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");

            }
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been updated successfully.";
                return RedirectToAction("Index", "Amenity");
            }


            amenityVM.villaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(amenityVM);      
        }

        public IActionResult Delete(int AmenityId)
        {

            AmenityVM amenityVM = new()
            {
                villaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == AmenityId)
            };

            //var VillaList = _db.Villas.Where(u => u.Price > 50 && u.Occupancy > 0);
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");

            }
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? objFromDb = _unitOfWork.Amenity
                .Get(u => u.Id == amenityVM.Amenity.Id);
            if (objFromDb is not null)
            {
                _unitOfWork.Amenity.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The  amenity has been deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The amenity could not be deleted.";
            return View();
        }
    }
}
