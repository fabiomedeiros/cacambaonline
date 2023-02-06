using cacambaonline.Data;
using cacambaonline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace cacambaonline.Controllers
{
    public class HomeController : Controller
    {


        private readonly MeuDbContext _context;

      

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MeuDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //var meuDbContext = _context.CTR.Include(c => c.Cacambas).Include(c => c.Destinatarios).Include(c => c.Transportadores);
            return View();
        }

        public IActionResult Papel()
        {
            //var meuDbContext = _context.CTR.Include(c => c.Cacambas).Include(c => c.Destinatarios).Include(c => c.Transportadores);
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Geolocalizacao()
        {
            return View();
        }

        public IActionResult Geo()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}