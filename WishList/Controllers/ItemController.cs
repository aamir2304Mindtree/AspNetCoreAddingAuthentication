using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishList.Data;
using WishList.Models;

namespace WishList.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserAsync(HttpContext.User).Id;
            var model = _context.Items.Where(item => item.Id == 20).ToList();

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Models.Item item)
        {
            item.User = _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult();
            _context.Items.Add(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var item = _context.Items.FirstOrDefault(e => e.Id == id);
            if (item.User == _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult())
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
