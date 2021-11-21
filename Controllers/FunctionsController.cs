using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace HR.Controllers
{
    public class FunctionsController : Controller
    {
        private readonly modelContext _context;

        public FunctionsController(modelContext context)
        {
            _context = context;
        }

        // GET: Functions

        [Authorize]
        public async Task<IActionResult> Index(string filter, int page = 1,
                                            string sortExpression = "Id")
        {




            var qry = _context.Functions.AsNoTracking().OrderBy(p => p.Id)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(p => p.NameFunction.Contains(filter));
            }

            var model = await PagingList.CreateAsync(
                                         qry, 10, page, sortExpression, "NameFunction");

            model.RouteValue = new RouteValueDictionary {
        { "filter", filter}
    };
            List<Departments> f = _context.Departments.ToList();

            ViewData["IdDepartment"] = f;
            return View(model);
        }






        public JsonResult DeleteEmployee(int EmployeeId)
        {
            bool result = false;


            Functions d = _context.Functions.Where(x => x.Id == EmployeeId).SingleOrDefault();
            try
            {

                _context.Functions.Remove(d);
                _context.SaveChanges();
            }
            catch { }

            return Json(result);
        }


        // GET: Functions/Create
        public IActionResult Create()
        {
            ViewData["IdDepartment"] = new SelectList(_context.Departments, "Id", "NameDepartment");
            return View();
        }

        // POST: Functions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameFunction,IdDepartment")] Functions functions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(functions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(functions);
        }

        // GET: Functions/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functions = await _context.Functions.FindAsync(id);
            if (functions == null)
            {
                return NotFound();
            }
            ViewData["IdDepartment"] = new SelectList(_context.Departments, "Id", "NameDepartment");
            return View(functions);
        }

        // POST: Functions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,NameFunction,IdDepartment")] Functions functions)
        {
            if (id != functions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(functions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionsExists(functions.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(functions);
        }

   

        private bool FunctionsExists(long id)
        {
            return _context.Functions.Any(e => e.Id == id);
        }
    }
}
