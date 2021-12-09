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
    public class DepartmentsController : Controller
    {
        private readonly modelContext _context;

        public DepartmentsController(modelContext context)
        {
            _context = context;
        }





        [Authorize]
        public async Task<IActionResult> Index(string filter, int page = 1,
                                             string sortExpression = "Id")
        {
          



            var qry = _context.Departments.AsNoTracking().OrderBy(p => p.Id)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(p => p.NameDepartment.Contains(filter) );
            }

            var model = await PagingList.CreateAsync(
                                         qry, 10, page, sortExpression, "NameDepartment");

            model.RouteValue = new RouteValueDictionary {
        { "filter", filter}
    };

            return View(model);
        }







        // GET: Transaction/AddOrEdit(Insert)
        // GET: Transaction/AddOrEdit/5(Update)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Departments());
            else
            {
                var transactionModel = await _context.Departments.Where(x=>x.Id==id).SingleOrDefaultAsync();
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
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id,NameDepartment")] Departments departments)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    List<long> Tablou = _context.Departments
.Select(u => u.Id)
.ToList();
                    int aux2 = ((int)Tablou.LastOrDefault() + 1);


                    Departments e = new Departments();
                    e.Id = aux2;
                    e.NameDepartment = departments.NameDepartment;
                  


                    _context.Departments.AddRange(e);

                    await _context.SaveChangesAsync();




                }
                //Update
                else
                {

                    try
                    {
                        _context.Update(departments);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DepartmentsExists(departments.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }


                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Departments.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", departments) });
        }





        [Authorize(Roles = "Admin")]
        public JsonResult DeleteEmployee(int EmployeeId)
        {
            bool result = false;

            //var employee = _context.Employee.FindAsync(id);
            Departments d = _context.Departments.Where(x => x.Id == EmployeeId).SingleOrDefault();
            try
            {
                List<Functions> fu = _context.Functions.Where(y => y.IdDepartment == d.Id).ToList();
                foreach(var f in fu)
                {
                    _context.Functions.Remove(f);
                    _context.SaveChanges();
                }
                _context.Departments.Remove(d);
                _context.SaveChanges();
            }
            catch { }

            return Json(result);
        }





        private bool DepartmentsExists(long id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
