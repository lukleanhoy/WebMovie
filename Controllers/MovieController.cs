using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebMovie.Data;
using WebMovie.Models;

namespace WebMovie.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly AppDbContext _context;

        public MovieController(AppDbContext context)
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
        public async Task<IActionResult> Create(Movie model, IFormFile poster, IFormFile thumbnail)
        {
            // Remove manual file validations since we handle them manually
            ModelState.Remove("poster");
            ModelState.Remove("thumbnail");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Create uploads folder inside wwwroot
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Process Poster Upload
            if (poster != null && poster.Length > 0)
            {
                var posterFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(poster.FileName);
                var posterPath = Path.Combine(uploadsFolder, posterFileName);
                using (var stream = new FileStream(posterPath, FileMode.Create))
                {
                    await poster.CopyToAsync(stream);
                }
                model.PosterPath = "/uploads/" + posterFileName;
            }

            // Process Thumbnail Upload
            if (thumbnail != null && thumbnail.Length > 0)
            {
                var thumbFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(thumbnail.FileName);
                var thumbPath = Path.Combine(uploadsFolder, thumbFileName);
                using (var stream = new FileStream(thumbPath, FileMode.Create))
                {
                    await thumbnail.CopyToAsync(stream);
                }
                model.ThumbnailPath = "/uploads/" + thumbFileName;
            }

            model.CreatedAt = DateTime.UtcNow;

            _context.Movies.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Movie '{model.Title}' created successfully! You can now link episodes to it.";
            return RedirectToAction("UploadEpisode");
        }

        [HttpGet]
        public async Task<IActionResult> UploadEpisode()
        {
            var moviesList = await _context.Movies.OrderBy(m => m.Title).ToListAsync();
            ViewBag.Movies = new SelectList(moviesList, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadEpisode(EpisodeLink model)
        {
            ModelState.Remove("Movie");

            if (!ModelState.IsValid)
            {
                var moviesList = await _context.Movies.OrderBy(m => m.Title).ToListAsync();
                ViewBag.Movies = new SelectList(moviesList, "Id", "Title", model.MovieId);
                return View(model);
            }

            model.CreatedAt = DateTime.UtcNow;

            _context.EpisodeLinks.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Episode streaming link added successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}
