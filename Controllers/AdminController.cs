using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TouristPlanner.Data;
using TouristPlanner.Models;

namespace TouristPlanner.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // LOGIN PAGE
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "1234")
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // ADMIN PANEL
        public async Task<IActionResult> Index()
        {
            var places = await _context.Places
                .OrderBy(p =>
                    p.Name == "Al-Aqsa Grand Mosque Kattankudy" ? 1 :
                    p.Name == "Kattankudy Beach" ? 2 :
                    p.Name == "Heritage Museum Kattankudy" ? 3 :
                    p.Name == "Gandhi Park Batticaloa" ? 4 :
                    p.Name == "Kallady Beach" ? 5 :
                    p.Name == "Batticaloa Light House" ? 6 :
                    p.Name == "Batticaloa Dutch Fort" ? 7 :
                    p.Name == "St Michael's College" ? 8 :
                    p.Name == "Sri Mangalarama Raja Maha Viharaya" ? 9 :
                    p.Name == "Kokkadicholai Thaanthonreeswarar Temple" ? 10 : 99
                )
                .ToListAsync();

            return View(places);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Place place)
        {
            if (ModelState.IsValid)
            {
                _context.Places.Add(place);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(place);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var place = await _context.Places.FindAsync(id);

            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Place place)
        {
            if (id != place.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(place);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(place);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var place = await _context.Places.FindAsync(id);

            if (place == null)
            {
                return NotFound();
            }

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}