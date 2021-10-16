using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HR.Models;
using ClosedXML.Excel;
using System.IO;
using ReflectionIT.Mvc.Paging;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace HR.Controllers
{
    public class InterviewCvsController : Controller
    {
        private readonly modelContext _context;

        public InterviewCvsController(modelContext context)
        {
            _context = context;
        }



        //delete interview
        [Authorize(Roles = "Admin")]
        public JsonResult DeleteEmployee(int EmployeeId)
        {
            bool result = false;


            List<InterviewCv> s1 = _context.InterviewCv.Where(x => x.Id == EmployeeId).ToList();
            foreach (var item in s1)
            {
                List<InterviewTeam> s2 = _context.InterviewTeam.Where(x => x.InterviewCvid == item.Id).ToList();
                if(s2!=null)
                foreach (var item2 in s2)
                {
                    _context.InterviewTeam.Remove(item2);
                    _context.SaveChanges();
                }
                _context.InterviewCv.Remove(item);
                item.OffertStatus = 0;
                _context.SaveChanges();
            }
            
            _context.SaveChanges();

            return Json(result);
        }



        //delete interview din details person
        [Authorize(Roles = "Admin")]
        public JsonResult DeleteEmployee3(int EmployeeId)
        {
            bool result = false;


            List<InterviewCv> s1 = _context.InterviewCv.Where(x => x.Id == EmployeeId).ToList();
            foreach (var item in s1)
            {
                List<InterviewTeam> s2 = _context.InterviewTeam.Where(x => x.InterviewCvid == item.Id).ToList();
                if (s2 != null)
                    foreach (var item2 in s2)
                    {
                        _context.InterviewTeam.Remove(item2);
                        _context.SaveChanges();
                    }
                _context.InterviewCv.Remove(item);
                item.OffertStatus = 0;
                _context.SaveChanges();
            }

            _context.SaveChanges();

            return Json(result);
        }



        //delete employee
        [Authorize(Roles = "Admin")]
        public JsonResult DeleteEmployee2(int EmployeeId)
        {
            bool result = false;

            List<InterviewTeam> s = _context.InterviewTeam.Where(x => x.EmployeeId == EmployeeId).ToList();
            foreach (var item in s)
            {
                List<Employee> s2 = _context.Employee.Where(x => x.EmployeeId == EmployeeId).ToList();
                foreach (var item2 in s2)
                {
                    _context.Employee.Remove(item2);
                    _context.SaveChanges();
                }
                _context.InterviewTeam.Remove(item);
                _context.SaveChanges();
            }
            _context.SaveChanges();
            return Json(result);
        }

        //create/edit  interview
        [Authorize(Roles = "Admin")]
        public IActionResult Interview(int id)
        {
            
                if (id != 0)
            {

                var a = new MultiTable();
                InterviewCv i = _context.InterviewCv.Where(x => x.Id == id).FirstOrDefault();
                a.Id = i.Id;
                a.PersonCvid = i.PersonCvid;
                a.InterviewDate = i.InterviewDate;
                a.FunctionApply = i.FunctionApply;
                a.DepartamentApply = i.DepartamentApply;
                a.Accepted = i.Accepted;
                a.TestResult = i.TestResult;
                a.RefusedReason = i.RefusedReason;
                a.RefusedObservation = i.RefusedObservation;
                a.Comments = i.Comments;
                a.DateAnswer = i.DateAnswer;
                a.OffertStatus = i.OffertStatus;
                a.EmploymentDate = i.EmploymentDate;
                a.AddedBy = i.AddedBy;
                a.AddedAt = i.AddedAt;
                a.UpdatedBy = i.UpdatedBy;
                a.UpdatedAt = i.UpdatedAt;


                InterviewTeam t = _context.InterviewTeam.Where(x => x.InterviewCvid == id).FirstOrDefault();

                a.EmployeeId = t.EmployeeId;//
                a.InterviewCvid = t.InterviewCvid;



                ViewData["PersonCvid"] = new SelectList(_context.PersonCv, "Id", "Name");
                ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "EmployeeName");
                ViewData["InterviewCvid"] = new SelectList(_context.InterviewCv, "Id", "Id");

                return View(a);
            }
            ViewData["OffertStatus"] = new SelectList(_context.Auxi, "Id", "OffertStatus");
            ViewData["PersonCvid"] = new SelectList(_context.PersonCv, "Id", "Name");
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "EmployeeName");
            ViewData["InterviewCvid"] = new SelectList(_context.InterviewCv, "Id", "Id");
            // ViewData["Id"] = new SelectList(_context.InterviewTeams, "InterviewCVId");
            return View();
        }

        //create/edit  interview
        // POST: InterviewCvs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Interview(int id, [Bind("Id,PersonCvid,InterviewDate,FunctionApply,DepartamentApply,Accepted,TestResult,RefusedReason,RefusedObservation,Comments,DateAnswer,OffertStatus,EmploymentDate,AddedBy,AddedAt,UpdatedBy,UpdatedAt,InterviewCvid,EmployeeId")] MultiTable obj)
        {
           

                //Insert
                if (id == 0)
                {

                List<long> Tablou = _context.InterviewCv
.Select(u => u.Id)
.ToList();
                int aux2 = ((int)Tablou.LastOrDefault() + 1);


                using (var transaction = _context.Database.BeginTransaction())
                {

                    InterviewCv i = new InterviewCv();
                    i.Id = aux2;
                    i.PersonCvid = obj.PersonCvid;
                    i.InterviewDate = obj.InterviewDate;
                    i.FunctionApply = obj.FunctionApply;
                    i.DepartamentApply = obj.DepartamentApply;
                    i.Accepted = obj.Accepted;
                    i.TestResult = obj.TestResult;
                    i.RefusedReason = obj.RefusedReason;
                    i.RefusedObservation = obj.RefusedObservation;
                    i.Comments = obj.Comments;
                    i.DateAnswer = obj.DateAnswer;
                    i.OffertStatus = obj.OffertStatus;
                    i.EmploymentDate = obj.EmploymentDate;
                    i.AddedBy = obj.AddedBy;
                    i.AddedAt = obj.AddedAt;
                    i.UpdatedBy = obj.UpdatedBy;
                    i.UpdatedAt = obj.UpdatedBy;
                    

                    _context.InterviewCv.AddRange(i);
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CV.InterviewCV ON;");
                    await _context.SaveChangesAsync();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CV.InterviewCV OFF;");
                    transaction.Commit();
                }



                InterviewTeam t = new InterviewTeam();

                    t.EmployeeId = obj.EmployeeId ?? 0;
                t.InterviewCvid = aux2;
                    _context.InterviewTeam.Add(t);
                    await _context.SaveChangesAsync();




                }
                //Update
                else
                {

                    try
                    {

                        InterviewCv i = _context.InterviewCv.Where(x => x.Id == id).FirstOrDefault();

                        i.PersonCvid = obj.PersonCvid;
                        i.InterviewDate = obj.InterviewDate;
                        i.FunctionApply = obj.FunctionApply;
                        i.DepartamentApply = obj.DepartamentApply;
                        i.Accepted = obj.Accepted;
                        i.TestResult = obj.TestResult;
                        i.RefusedReason = obj.RefusedReason;
                        i.RefusedObservation = obj.RefusedObservation;
                        i.Comments = obj.Comments;
                        i.DateAnswer = obj.DateAnswer;
                        i.OffertStatus = obj.OffertStatus;
                        i.EmploymentDate = obj.EmploymentDate;
                        i.AddedBy = obj.AddedBy;
                        i.AddedAt = obj.AddedAt;
                        i.UpdatedBy = obj.UpdatedBy;
                        i.UpdatedAt = obj.UpdatedBy;
                        _context.InterviewCv.Update(i);
                        await _context.SaveChangesAsync();

                        InterviewTeam t = _context.InterviewTeam.Where(x => x.InterviewCvid == id).FirstOrDefault();

                        t.EmployeeId = obj.EmployeeId ?? 0;
                        t.InterviewCvid = i.Id;
                        _context.InterviewTeam.Update(t);
                        await _context.SaveChangesAsync();

                    }
                    catch
                    {
                        if (!InterviewTeamExists(id) && InterviewCvExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                }
                return RedirectToAction(nameof(Index));
            



        }





        //verifica daca exista in team
        private bool InterviewTeamExists(long id)
        {
            return _context.InterviewTeam.Any(e => e.Id == id);
        }






        //export button
        
        [Authorize(Roles = "Admin")]
        public IActionResult ExportToExcel()
        {

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Interviews");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "InterviewDate";
                worksheet.Cell(currentRow, 4).Value = "FunctionApply";
                worksheet.Cell(currentRow, 5).Value = "DepartamentApply";
                worksheet.Cell(currentRow, 6).Value = "EmployeeName";
                worksheet.Cell(currentRow, 7).Value = "Accepted";
                worksheet.Cell(currentRow, 8).Value = "TestResult";
                worksheet.Cell(currentRow, 9).Value = "RefusedReason";
                worksheet.Cell(currentRow, 10).Value = "DateAnswer";
               
                worksheet.Cell(currentRow, 11).Value = "OffertStatus";

                worksheet.Cell(currentRow, 12).Value = "EmploymentDate";

                foreach (var x in multitable)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = x.Id;
                    worksheet.Cell(currentRow, 2).Value = x.Name;
                    worksheet.Cell(currentRow, 3).Value = x.InterviewDate;
                    worksheet.Cell(currentRow, 4).Value = x.FunctionApply;
                    worksheet.Cell(currentRow, 5).Value = x.DepartamentApply;
                    worksheet.Cell(currentRow, 6).Value = x.EmployeeName;
                    worksheet.Cell(currentRow, 7).Value = x.Accepted;
                    worksheet.Cell(currentRow, 8).Value = x.TestResult;
                    worksheet.Cell(currentRow, 9).Value = x.RefusedReason;
                    worksheet.Cell(currentRow, 10).Value = x.DateAnswer;

                    if (x.OffertStatus == 1)
                    {
                        worksheet.Cell(currentRow, 11).Value = "Offered";
                    }
                    else worksheet.Cell(currentRow, 11).Value = "Signed";


                    worksheet.Cell(currentRow, 12).Value = Convert.ToString(x.EmploymentDate);
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Interviews.xlsx");
                }

            }
        }



        static List<Multi> multitable = new List<Multi>();
        [Authorize]
        //index cu paginare search si order
        public async Task<IActionResult> Index(string filter, int page = 1,
                                               string sortExpression = "Name")
        {
            List<Multi> _multitable = await _context.Multi.AsNoTracking().OrderBy(p => p.Id).ToListAsync();

            multitable.AddRange(_multitable);

            var qry = _context.Multi.AsNoTracking().OrderBy(p => p.Id)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(p => p.Name.Contains(filter) || p.FunctionApply.Contains(filter) || p.DepartamentApply.Contains(filter));
            }

            var model = await PagingList.CreateAsync(
                                         qry, 10, page, sortExpression, "Name");

            model.RouteValue = new RouteValueDictionary {
        { "filter", filter}
    };

            return View(model);
        }


        //verifica daca exista interviul
        private bool InterviewCvExists(long id)
        {
            return _context.InterviewCv.Any(e => e.Id == id);
        }


        // GET: InterviewCvs1/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interviewCv = await _context.InterviewCv.FindAsync(id);
            if (interviewCv == null)
            {
                return NotFound();
            }
            ViewData["PersonCvid"] = new SelectList(_context.PersonCv, "Id", "Name", interviewCv.PersonCvid);
            return View(interviewCv);
        }

        // POST: InterviewCvs1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,PersonCvid,InterviewDate,FunctionApply,DepartamentApply,Accepted,TestResult,RefusedReason,RefusedObservation,Comments,DateAnswer,OffertStatus,EmploymentDate,AddedBy,AddedAt,UpdatedBy,UpdatedAt")] InterviewCv interviewCv)
        {
            if (id != interviewCv.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interviewCv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InterviewCvExists(interviewCv.Id))
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
            ViewData["PersonCvid"] = new SelectList(_context.PersonCv, "Id", "Name", interviewCv.PersonCvid);
            return View(interviewCv);
        }



     

    }
}
