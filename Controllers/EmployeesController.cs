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
using ClosedXML.Excel;
using System.IO;

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
        [Authorize]
        public async Task<IActionResult> Index(string filter, int page = 1,
                                              string sortExpression = "EmployeeId")
        {
           



            var qry = _context.Employee.AsNoTracking().OrderBy(p => p.Id)
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
        public async Task<IActionResult> AddOrEdit(int id, [Bind("Id,EmployeeId,EmployeeName,OrganizationId,EmploymentDate,Email,Team,CompanyShortName")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    List<int> Tablou = _context.Employee
.Select(u => u.Id)
.ToList();
                    int aux2 = ((int)Tablou.LastOrDefault() + 1);
                    

                        Employee e = new Employee();
                        e.Id = aux2;
                        e.EmployeeId = employee.EmployeeId;
                        e.EmployeeName = employee.EmployeeName;
                        e.EmploymentDate = employee.EmploymentDate;
                        e.OrganizationId = employee.OrganizationId;
                        e.EmploymentDate = employee.EmploymentDate;
                        e.Email = employee.Email;
                        e.Team = employee.Team;
                        e.CompanyShortName = employee.CompanyShortName;
                        e.IsActive = 1;
                        e.Plant = 1;


                        _context.Employee.AddRange(e);
                       
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



        //export button
        
        [Authorize(Roles = "Admin")]
        public IActionResult ExportToExcel()
        {

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Employees");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "EmployeeId";
                worksheet.Cell(currentRow, 3).Value = "EmployeeName";
                worksheet.Cell(currentRow, 4).Value = "OrganizationId";
                worksheet.Cell(currentRow, 5).Value = "EmploymentDate";
                worksheet.Cell(currentRow, 6).Value = "Email";
                worksheet.Cell(currentRow, 7).Value = "Team";
                worksheet.Cell(currentRow, 8).Value = "CompanyShortName";
                

                foreach (var x in _context.Employee)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = x.Id;
                    worksheet.Cell(currentRow, 2).Value = x.EmployeeId;
                    worksheet.Cell(currentRow, 3).Value = x.EmployeeName;
                    worksheet.Cell(currentRow, 4).Value = x.OrganizationId;
                    var dateTimeNow = (DateTime)x.EmploymentDate;
                    var dateOnlyString = dateTimeNow.ToShortDateString();
                    worksheet.Cell(currentRow, 5).Value = Convert.ToString(dateOnlyString);


                    worksheet.Cell(currentRow, 6).Value = x.Email;
                    worksheet.Cell(currentRow, 7).Value = x.Team;
                    worksheet.Cell(currentRow, 8).Value = x.CompanyShortName;



                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
                }

            }
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

                _context.Employee.Remove(employee);
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
