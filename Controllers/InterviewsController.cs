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
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using HR.Repository;
using HR.DataModels;
using HR.Utils;

namespace HR.Controllers
{
    /// <summary>
    /// Handles CRUD operations and other functionalities related to Interview CVs.
    /// </summary>
    public class InterviewsController : Controller
    {
        private readonly modelContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterviewsController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public InterviewsController(modelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves interview statistics data.
        /// </summary>
        /// <returns>A JSON result containing interview statistics.</returns>
        public ActionResult GetData()
        {
            try
            {
                var qry = _context.JobApplicationDetails
                    .AsNoTracking()
                    .GroupBy(p => p.EmploymentDate)
                    .Select(g => new { name = g.Key, count = g.Sum(w => w.Id) })
                    .ToList();

                return new JsonResult(new { data = qry });
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves and displays statistics based on employment dates.
        /// </summary>
        /// <returns>A view containing the statistics.</returns>
        public ActionResult Statistics()
        {
            try
            {
                var months = new int[12];
                var employmentDates = _context.JobApplicationDetails
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .Select(x => x.EmploymentDate)
                    .ToList();

                foreach (var date in employmentDates)
                {
                    if (date != null)
                    {
                        months[date.Value.Month - 1]++;
                    }
                }

                ViewData["EmploymentDates"] = months.ToList();
                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Delete an interview records for a specified id.
        /// </summary>
        /// <param name="interviewId">The ID of the interview that are to be deleted.</param>
        /// <returns>A JSON result indicating success or failure.</returns>
        [Authorize(Roles = "Admin")]
        public JsonResult DeleteInterview(long interviewId)
        {
            try
            {
                bool result = false;

                var interviews = _context.Interviews.Where(x => x.Id == interviewId).ToList();
                foreach (var interview in interviews)
                {
                    var interviewTeams = _context.InterviewTeams.Where(x => x.InterviewId == interview.Id).ToList();
                    foreach (var team in interviewTeams)
                    {
                        _context.InterviewTeams.Remove(team);
                    }
                    _context.Interviews.Remove(interview);
                    interview.OfferStatus = 0;
                }
                _context.SaveChanges();
                TempData["AlertMessage"] = Constants.INTERVIEW.DeleteSuccess;
                result = true;

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(StatusCode(500, string.Format(Constants.InternalServerError, ex.Message)));
            }
        }

        /// <summary>
        /// Calculates the acceptance percentage of job applications.
        /// </summary>
        /// <param name="jobApplications">The list of job applications.</param>
        /// <returns>A string representing the acceptance percentage.</returns>
        public string Percentage(List<JobApplicationDetails> jobApplications)
        {
            try
            {
                var totalApplications = jobApplications.Count;
                var acceptedApplications = jobApplications.Count(ja => ja.OfferStatus == 1);

                if (totalApplications == 0)
                    return "0";

                return ((double)acceptedApplications / totalApplications * 100).ToString("F2");
            }
            catch (Exception ex)
            {
                return Constants.InternalServerError;
            }
        }

        /// <summary>
        /// Deletes an employee from the team.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to be deleted.</param>
        /// <returns>A JSON result indicating success or failure.</returns>
        [Authorize(Roles = "Admin")]
        public JsonResult DeleteEmployeeFromTeam(int employeeId)
        {
            try
            {
                bool result = false;

                var teams = _context.InterviewTeams.Where(x => x.EmployeeId == employeeId).ToList();
                foreach (var team in teams)
                {
                    var employees = _context.Employees.Where(x => x.EmployeeId == employeeId).ToList();
                    foreach (var employee in employees)
                    {
                        _context.Employees.Remove(employee);
                    }
                    _context.InterviewTeams.Remove(team);
                }
                _context.SaveChanges();
                TempData["AlertMessage"] = Constants.INTERVIEW.DeleteSuccess;
                result = true;

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(StatusCode(500, string.Format(Constants.InternalServerError, ex.Message)));
            }
        }

        /// <summary>
        /// Displays the interview form for creating an interview record.
        /// </summary>
        /// <returns>A view with the interview form.</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            try
            {
                PopulateViewData();
                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Handles the creation an interview record.
        /// </summary>
        /// <param name="id">The ID of the interview to be updated (if applicable).</param>
        /// <param name="model">The interview details to be saved.</param>
        /// <returns>A redirect to the index view after saving.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(int id, [Bind("Id,CandidateId,InterviewDate,FunctionApply,Accepted,TestResult,RefusedReason,Comments,DateAnswer,OfferStatus,EmploymentDate,InterviewId,EmployeeId")] JobApplicationAggregate model)
        {
            try
            {
                await InsertInterview(model);
                TempData["AlertMessage"] = Constants.INTERVIEW.OperationSuccess;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        static List<JobApplicationDetails> jobApplicationDetailsList = new List<JobApplicationDetails>();

        /// <summary>
        /// Exports interview records to an Excel file.
        /// </summary>
        /// <returns>A file result containing the Excel file.</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult ExportToExcel()
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Interviews");
                    PopulateWorksheetHeader(worksheet);

                    int row = 2; // Start at the second row (since the first row is for headers)

                    foreach (var record in jobApplicationDetailsList)
                    {
                        PopulateWorksheetRow(worksheet, record, row); // Pass the row number to the method
                        row++; // Increment the row number for the next record
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Interviews.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Displays a list of job applications with optional filtering and sorting.
        /// </summary>
        /// <param name="filter">A filter string to apply to the list of job applications.</param>
        /// <param name="page">The page number to display.</param>
        /// <param name="sortExpression">The sort expression for the list.</param>
        /// <returns>A view displaying the list of job applications.</returns>
        [Authorize]
        public async Task<IActionResult> Index(string filter, int page = 1, string sortExpression = "Id")
        {
            try
            {
                var jobApplications = await _context.JobApplicationDetails
                    .AsNoTracking()
                    .ToListAsync();

                jobApplicationDetailsList = jobApplications;

                var query = _context.JobApplicationDetails
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query = query.Where(p => p.CandidateName.Contains(filter) || p.Function.Contains(filter) || p.Department.Contains(filter));
                }

                var model = await PagingList.CreateAsync(query, 10, page, sortExpression, "Name");
                model.RouteValue = new RouteValueDictionary { { "filter", filter } };

                ViewData["Procent"] = Percentage(jobApplications);
                return View(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Displays the form for editing an interview record.
        /// </summary>
        /// <param name="id">The ID of the interview to be edited.</param>
        /// <returns>A view with the interview edit form.</returns>
        [Authorize(Roles = "Admin")]
        public Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return Task.FromResult<IActionResult>(NotFound(Constants.INTERVIEW.NotFound));
            }

            try
            {

                var interview = _context.Interviews.FirstOrDefault(x => x.Id == id);
                if (interview == null)
                {
                    return Task.FromResult<IActionResult>(NotFound(Constants.INTERVIEW.NotFound));
                }

                var interviewTeam = _context.InterviewTeams.FirstOrDefault(x => x.InterviewId == id);
                if (interviewTeam == null)
                {
                    return Task.FromResult<IActionResult>(NotFound(Constants.INTERVIEW.NotFound));
                }

                var aggregate = CreateJobApplicationAggregate(interview, (int)interviewTeam.Id, interviewTeam.EmployeeId);

                PopulateViewData(aggregate);
                return Task.FromResult<IActionResult>(View(aggregate));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(StatusCode(500, string.Format(Constants.InternalServerError, ex.Message)));
            }
        }

        /// <summary>
        /// Updates an existing interview record.
        /// </summary>
        /// <param name="id">The ID of the interview to be updated.</param>
        /// <param name="interviewCv">The updated interview record.</param>
        /// <returns>A redirect to the index view after updating.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CandidateId,InterviewDate,FunctionApply,Accepted,TestResult,RefusedReason,RefusedObservation,Comments,DateAnswer,OfferStatus,EmploymentDate,InterviewId,EmployeeId")] JobApplicationAggregate model)
        {
            if (id != model.Id)
            {
                return NotFound(Constants.INTERVIEW.NotFound);
            }

            if (!ModelState.IsValid)
            {
                TempData["AlertMessage"] = Constants.INTERVIEW.InvalidModel;
                PopulateViewData();
                return View(model);
            }

            try
            {

                await UpdateInterview(id, model);

                TempData["AlertMessage"] = Constants.INTERVIEW.OperationSuccess;
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterviewCvExists(model.Id))
                {
                    return NotFound(Constants.INTERVIEW.NotFound);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Checks if an interview record exists.
        /// </summary>
        /// <param name="id">The ID of the interview to check.</param>
        /// <returns>True if the interview exists; otherwise, false.</returns>
        private bool InterviewCvExists(long id)
        {
            return _context.Interviews.Any(e => e.Id == id);
        }

        /// <summary>
        /// Populates the view data for the interview form.
        /// </summary>
        /// <param name="model">The model containing interview details.</param>
        private void PopulateViewData(JobApplicationAggregate model = null)
        {
            ViewData["OfferStatus"] = new SelectList(Constants.OfferStatus);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", model?.EmployeeId);
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "Id", "Name", model?.CandidateId);
            ViewData["FunctionApply"] = new SelectList(_context.Functions, "Id", "Name", model?.FunctionApply);
            ViewData["DepartamentApply"] = new SelectList(_context.Departments, "Id", "Name", model?.DepartamentApply);
            ViewData["Accepted"] = new SelectList(new[] { new { Value = "1", Text = "Accepted" }, new { Value = "0", Text = "Not Accepted" } }, "Value", "Text", model?.Accepted.ToString());
        }

        /// <summary>
        /// Creates a job application aggregate from an interview record.
        /// </summary>
        /// <param name="interview">The interview record.</param>
        /// <returns>A job application aggregate representing the interview record.</returns>
        private JobApplicationAggregate CreateJobApplicationAggregate(Interviews interview, int interviewCvId, int employeeId)
        {
            return new JobApplicationAggregate
            {
                CandidateId = interview.CandidateId,
                InterviewDate = interview.InterviewDate,
                FunctionApply = (int)interview.FunctionApply,
                DepartamentApply = (int)interview.DepartamentApply,
                Accepted = interview.Accepted,
                TestResult = interview.TestResult,
                RefusedReason = interview.RefusedReason,
                RefusedObservation = interview.RefusedObservation,
                Comments = interview.Comments,
                DateAnswer = interview.DateAnswer,
                OfferStatus = interview.OfferStatus,
                EmploymentDate = interview.EmploymentDate,
                InterviewId = interviewCvId,
                EmployeeId = employeeId
            };
        }

        /// <summary>
        /// Inserts a new interview record into the database.
        /// </summary>
        /// <param name="model">The interview record to insert.</param>
        private async Task InsertInterview(JobApplicationAggregate model)
        {
            try
            {
                var function = await _context.Functions.FirstOrDefaultAsync(f => f.Id == model.FunctionApply);

                var newInterview = new Interviews
                {
                    CandidateId = model.CandidateId,
                    InterviewDate = model.InterviewDate,
                    FunctionApply = model.FunctionApply,
                    DepartamentApply = function.DepartmentId,
                    Accepted = model.Accepted,
                    TestResult = model.TestResult,
                    RefusedReason = model.RefusedReason,
                    RefusedObservation = model.RefusedObservation,
                    Comments = model.Comments,
                    DateAnswer = model.DateAnswer,
                    OfferStatus = model.OfferStatus,
                    EmploymentDate = model.EmploymentDate,
                };

                _context.Add(newInterview);
                await _context.SaveChangesAsync();

                InterviewTeams interviuTeam = new InterviewTeams
                {
                EmployeeId = model.EmployeeId != null ? model.EmployeeId : 0,
                    InterviewId = newInterview.Id
                };
                _context.InterviewTeams.Add(interviuTeam);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.INTERVIEW.OperationFailed, ex);
            }
        }


        /// <summary>
        /// Updates an existing interview record.
        /// </summary>
        /// <param name="id">The ID of the interview to update.</param>
        /// <param name="model">The updated interview details.</param>
        private async Task UpdateInterview(long id, JobApplicationAggregate model)
        {
            try
            {
                var interview = await _context.Interviews.FindAsync(id);
                if (interview == null)
                {
                    throw new Exception(Constants.INTERVIEW.NotFound);
                }
                var function = await _context.Functions.FirstOrDefaultAsync(f => f.Id == model.FunctionApply);

                interview.CandidateId = model.CandidateId;
                interview.InterviewDate = model.InterviewDate;
                interview.FunctionApply = model.FunctionApply;
                interview.DepartamentApply = function.DepartmentId;
                interview.Accepted = model.Accepted;
                interview.TestResult = model.TestResult;
                interview.RefusedReason = model.RefusedReason;
                interview.RefusedObservation = model.RefusedObservation;
                interview.Comments = model.Comments;
                interview.DateAnswer = model.DateAnswer;
                interview.OfferStatus = model.OfferStatus;
                interview.EmploymentDate = model.EmploymentDate;
                interview.AddedBy = model.AddedBy;
                interview.AddedAt = model.AddedAt;
                interview.UpdatedBy = model.UpdatedBy;
                interview.UpdatedAt = model.UpdatedBy;

                _context.Update(interview);

                var interviuTeam = _context.InterviewTeams.FirstOrDefault(t => t.InterviewId == (int)id && t.EmployeeId == model.EmployeeId);
                interviuTeam.EmployeeId = model.EmployeeId != null ? model.EmployeeId : 0;
                interviuTeam.InterviewId = interview.Id;

                _context.InterviewTeams.Update(interviuTeam);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.INTERVIEW.OperationFailed, ex);
            }
        }

        /// <summary>
        /// Populates the worksheet header with column names.
        /// </summary>
        /// <param name="worksheet">The worksheet to populate.</param>
        private void PopulateWorksheetHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell(1, 1).Value = "Candidate Name";
            worksheet.Cell(1, 2).Value = "Interview Date";
            worksheet.Cell(1, 3).Value = "Function Apply";
            worksheet.Cell(1, 4).Value = "Departament Apply";
            worksheet.Cell(1, 5).Value = "Test Result";
            worksheet.Cell(1, 6).Value = "Accepted";
            worksheet.Cell(1, 7).Value = "Refused Reason";
            worksheet.Cell(1, 8).Value = "Comments";
            worksheet.Cell(1, 9).Value = "Date Answer";
            worksheet.Cell(1, 10).Value = "Offer Status";
            worksheet.Cell(1, 11).Value = "Employment Date";
            worksheet.Cell(1, 12).Value = "Interview Id";
            worksheet.Cell(1, 13).Value = "EmployeeId";
            worksheet.Cell(1, 13).Value = "Employee Name";
        }

        /// <summary>
        /// Populates a row in the worksheet with interview record data.
        /// </summary>
        /// <param name="worksheet">The worksheet to populate.</param>
        /// <param name="record">The interview record data.</param>
        /// <param name="row">The row number to populate.</param>
        private void PopulateWorksheetRow(IXLWorksheet worksheet, JobApplicationDetails record, int row)
        {
            worksheet.Cell(row, 1).Value = record.CandidateName;
            worksheet.Cell(row, 2).Value = record.InterviewDate;
            worksheet.Cell(row, 3).Value = record.Function;
            worksheet.Cell(row, 4).Value = record.Department;
            worksheet.Cell(row, 5).Value = record.TestResult;
            worksheet.Cell(row, 6).Value = record.Accepted;
            worksheet.Cell(row, 7).Value = record.RefusedReason;
            worksheet.Cell(row, 8).Value = record.Comments;
            worksheet.Cell(row, 9).Value = record.DateAnswer;
            worksheet.Cell(row, 10).Value = record.OfferStatus;
            worksheet.Cell(row, 11).Value = record.EmploymentDate;
            worksheet.Cell(row, 12).Value = record.Id;
            worksheet.Cell(row, 13).Value = record.EmployeeId;
            worksheet.Cell(row, 13).Value = record.EmployeeName;
        }
    }
}
