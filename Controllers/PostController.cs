using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMovie.Data;
using WebMovie.Models;

namespace WebMovie.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post model)
        {
            // Remove navigation property validations if present in ModelState
            ModelState.Remove("User");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Retrieve current logged in user's ID from claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                model.UserId = userId;
            }

            model.CreatedAt = DateTime.UtcNow;

            _context.Posts.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
