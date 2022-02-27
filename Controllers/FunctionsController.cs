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

        [Authorize(Roles = "Admin")]
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
                TempData["AlertMessage"] = "Deleted with success";
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







        // GET: Transaction/AddOrEdit(Insert)
        // GET: Transaction/AddOrEdit/5(Update)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0) { 
                ViewData["IdDepartment"] = new SelectList(_context.Departments, "Id", "NameDepartment");
            return View(new Functions());
            
        }
            else
            {
                var transactionModel = await _context.Functions.Where(x => x.Id == id).SingleOrDefaultAsync();

                ViewData["IdDepartment"] = new SelectList(_context.Departments, "Id", "NameDepartment");
                if (transactionModel == null)
                {
                    return NotFound();
                }
                return View(transactionModel);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id, NameFunction, IdDepartment")] Functions function)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    List<long> Tablou = _context.Functions
.Select(u => u.Id)
.ToList();
                    int aux2 = ((int)Tablou.LastOrDefault() + 1);
                    using (var transaction = _context.Database.BeginTransaction())
                    {

                        Functions e = new Functions();
                        e.Id = aux2;
                    e.IdDepartment = function.IdDepartment;
                        e.NameFunction = function.NameFunction;


                        _context.Functions.AddRange(e);
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Functions ON;");
                        await _context.SaveChangesAsync();
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Functions OFF;");
                        transaction.Commit();
                        


                        TempData["AlertMessage"] = "Inserted with success";

                    }
                }
                //Update
                else
                {

                    try
                    {

                        _context.Update(function);
                        await _context.SaveChangesAsync();
                        TempData["AlertMessage"] = "Updated with success";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FunctionsExists(function.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }


                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Functions.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", function) });
        }










        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,NameFunction,IdDepartment")] Functions functions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(functions);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Inserted with success";
                return RedirectToAction(nameof(Index));
            }
            return View(functions);
        }

        // GET: Functions/Edit/5
        [Authorize(Roles = "Admin")]
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
                    TempData["AlertMessage"] = "Updated with success";
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
