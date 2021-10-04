using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using HR.Models;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace HR_CV.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly modelContext _context;

        public EmployeesController(modelContext context)
        {
            _context = context;
        }

    
        // index cu search,paginare si order by name si id
        public async Task<IActionResult> Index(string filter, int page = 1,
                                              string sortExpression = "EmployeeName")
        {
            List<Multi> _multitable = await _context.Multi.AsNoTracking().OrderBy(p => p.Id).ToListAsync();



            var qry = _context.Employee.AsNoTracking().Where(x => x.IsActive > 0).OrderBy(p => p.Id)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(p => p.EmployeeName.Contains(filter) || p.Team.Contains(filter) || p.CompanyShortName.Contains(filter));
            }

            var model = await PagingList.CreateAsync(
                                         qry, 10, page, sortExpression, "EmployeeName");

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
                return View(new Employee());
            else
            {
                var transactionModel = await _context.Employee.FindAsync(id);
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
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id,EmployeeId,EmployeeName,OrganizationId,CorId,EmploymentDate,EndDate,IsActive,Email,UserName,Plant,Team,CompanyShortName")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();


                }
                //Update
                else
                {

                    try
                    {
                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeExists(employee.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }


                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Employee.ToList()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", employee) });
        }





        // GET: Employees/Details/5
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,EmployeeName,OrganizationId,CorId,EmploymentDate,EndDate,IsActive,Email,UserName,Plant,Team,CompanyShortName")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,EmployeeName,OrganizationId,CorId,EmploymentDate,EndDate,IsActive,Email,UserName,Plant,Team,CompanyShortName")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }




        [Authorize(Roles = "Admin")]
        public JsonResult DeleteEmployee(int EmployeeId)
        {
            bool result = false;

            //var employee = _context.Employee.FindAsync(id);
            Employee employee = _context.Employee.Where(x => x.Id == EmployeeId).SingleOrDefault();
            try
            {
                employee.IsActive = 0;
                _context.Update(employee);
                _context.SaveChanges();
            }
            catch { }

            return Json(result);
        }





        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
