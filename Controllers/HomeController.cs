using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMovie.Data;
using WebMovie.Models;

namespace WebMovie.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var movies = await _context.Movies.OrderByDescending(m => m.CreatedAt).ToListAsync();
        return View(movies);
    }

    [HttpGet]
    public async Task<IActionResult> GetEpisodes(int id)
    {
        var episodes = await _context.EpisodeLinks
            .Where(e => e.MovieId == id)
            .OrderBy(e => e.Episode)
            .Select(e => new { e.Episode, e.Link })
            .ToListAsync();
            
        return Json(episodes);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
