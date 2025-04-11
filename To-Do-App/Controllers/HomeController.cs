using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_Do_App.Models;

namespace To_Do_App.Controllers
{
    public class HomeController : Controller
    {
        private ToDoContext _context;

        public HomeController(ToDoContext context)
        {
            _context = context;
        }

        public IActionResult Index(string id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Statuses = _context.Statuses.ToList();
            ViewBag.DueFilters = Filters.DueFiltersValues;

            IQueryable<ToDo> todos = _context.ToDos.Include(t => t.Category).Include(t => t.Status);

            if (filters.HasCategory)
            {
                todos = todos.Where(t => t.CategoryId == filters.CategoryId);
            }

            if (filters.HasStatus)
            {
                todos = todos.Where(t => t.StatusId == filters.StatusId);
            }

            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if (filters.IsPast)
                {
                    todos = todos.Where(t => t.DueDate < today);
                }
                else if (filters.IsFuture)
                {
                    todos = todos.Where(t => t.DueDate > today);
                }
                else if (filters.IsToday)
                {
                    todos = todos.Where(t => t.DueDate == today);
                }
            }

            var tasks = todos.OrderBy(t => t.DueDate).ToList();
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Statuses = _context.Statuses.ToList();
            var task = new ToDo() { StatusId = "open" };
            return View(task);
        }

        [HttpPost]
        public IActionResult Add(ToDo task)
        {
            if (ModelState.IsValid)
            {
                _context.ToDos.Add(task);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Statuses = _context.Statuses.ToList();
            return View(task);
        }

        [HttpPost]
        public IActionResult Filter(string[,] filters)
        {
            string id = string.Join('-', filters);
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]
        public IActionResult MarkComplete([FromRoute] string id, ToDo selected)
        {
            selected = _context.ToDos.Find(selected.Id);
            if (selected != null)
            {
                selected.StatusId = "closed";
                _context.SaveChanges();
            }
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var toDelete = _context.ToDos.Where(t => t.StatusId == "closed").ToList();
            foreach (var task in toDelete)
            {
                _context.ToDos.Remove(task);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", new {ID = id});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
