using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;  
        public VillaNumberController(ApplicationDbContext db)
        {
            _db  = db;
        }
        public IActionResult Index()
        {
            var villaNumers =_db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                villaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {

            //ModelState.Remove("Villa");

            bool roomNumberExists =_db.VillaNumbers.Any(u=>u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The villa Number has been created successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }

            if(roomNumberExists)
            {
                TempData["error"] = "The villa Number already exists.";
            }
            obj.villaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Update(int VillaNumberId)
        {

            VillaNumberVM villaNumberVM = new()
            {
                villaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == VillaNumberId)
            };

            //var VillaList = _db.Villas.Where(u => u.Price > 50 && u.Occupancy > 0);
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");

            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {

            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The villa Number has been updated successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }


            villaNumberVM.villaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);      
        }

        public IActionResult Delete(int VillaNumberId)
        {

            VillaNumberVM villaNumberVM = new()
            {
                villaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == VillaNumberId)
            };

            //var VillaList = _db.Villas.Where(u => u.Price > 50 && u.Occupancy > 0);
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");

            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb =_db.VillaNumbers.
                FirstOrDefault(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                _db.VillaNumbers.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa number could not be deleted.";
            return View();
        }
    }
}
