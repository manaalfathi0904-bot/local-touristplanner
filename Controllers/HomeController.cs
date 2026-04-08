using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TouristPlanner.Data;
using TouristPlanner.Models;

namespace TouristPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search, string category)
        {
            var places = _context.Places.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                places = places.Where(p => p.Name.Contains(search));
            }

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                places = places.Where(p => p.Category == category);
            }

           var orderedPlaces = await places
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

ViewBag.CurrentCategory = string.IsNullOrEmpty(category) ? "All" : category;

return View(orderedPlaces);
        }
        public async Task<IActionResult> Details(int id)
        {
            var place = await _context.Places.FirstOrDefaultAsync(p => p.Id == id);

            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        [HttpPost]
        public async Task<IActionResult> AddToPlan(int id)
        {
            var place = await _context.Places.FindAsync(id);

            if (place == null)
            {
                return NotFound();
            }

            var planIds = GetPlanIds();

            if (!planIds.Contains(id))
            {
                planIds.Add(id);
                SavePlanIds(planIds);
                TempData["PlanMessage"] = "Place added to your plan successfully.";
            }
            else
            {
                TempData["PlanMessage"] = "This place is already in your plan.";
            }

            return RedirectToAction("Details", new { id = id });
        }

        public async Task<IActionResult> Plan()
        {
            var planIds = GetPlanIds();

            var plannedPlaces = await _context.Places
                .Where(p => planIds.Contains(p.Id))
                .ToListAsync();

            var orderedPlaces = plannedPlaces
                .OrderBy(p => planIds.IndexOf(p.Id))
                .ToList();

            return View(orderedPlaces);
        }

        [HttpPost]
        public IActionResult RemoveFromPlan(int id)
        {
            var planIds = GetPlanIds();

            if (planIds.Contains(id))
            {
                planIds.Remove(id);
                SavePlanIds(planIds);
                TempData["PlanMessage"] = "Place removed from your plan.";
            }

            return RedirectToAction("Plan");
        }

        [HttpPost]
        public IActionResult ClearPlan()
        {
            HttpContext.Session.Remove("PlanIds");
            TempData["PlanMessage"] = "Your plan has been cleared.";
            return RedirectToAction("Plan");
        }

        private List<int> GetPlanIds()
        {
            var sessionData = HttpContext.Session.GetString("PlanIds");

            if (string.IsNullOrEmpty(sessionData))
            {
                return new List<int>();
            }

            return JsonSerializer.Deserialize<List<int>>(sessionData) ?? new List<int>();
        }

        private void SavePlanIds(List<int> planIds)
        {
            HttpContext.Session.SetString("PlanIds", JsonSerializer.Serialize(planIds));
        }
    }
}