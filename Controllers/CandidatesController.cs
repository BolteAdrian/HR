using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HR.Models;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Routing;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using HR.Repository;
using HR.Utils;
using static HR.Utils.Constants;

namespace HR.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly modelContext _context;

        public CandidatesController(modelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a paginated list of person CVs based on the provided search and pagination options.
        /// </summary>
        /// <param name="filter">The search filter keyword.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="sortExpression">The sorting expression.</param>
        /// <returns>Returns a paginated list of person CVs if successful. If no CVs are found, returns a NotFound response with an appropriate message. If an error occurs during processing, returns a StatusCode 500 response with an error message.</returns>
        [Authorize]
        public async Task<IActionResult> Index(string filter, int page = 1, string sortExpression = "Id")
        {
            try
            {
                var persons = _context.Candidates.AsNoTracking().OrderBy(p => p.Id).AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    persons = persons.Where(p => p.Name.Contains(filter) || p.City.Contains(filter) || p.County.Contains(filter));
                }

                var model = await PagingList.CreateAsync(persons, 10, page, sortExpression, "Name");
                model.RouteValue = new RouteValueDictionary { { "filter", filter } };

                List<Functions> functions = _context.Functions.ToList();
                ViewData["FunctionApply"] = functions;
                ViewData["FunctionMatch"] = functions;

                return View(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves statistics based on the number of applications by month.
        /// </summary>
        /// <returns>Returns a view displaying statistics of applications by month.</returns>
        public ActionResult Statistics()
        {
            try
            {
                var months = new int[12];

                var persons = _context.Candidates.AsNoTracking().OrderBy(x => x.Id).Select(x => x.DateApply).ToList();

                foreach (var person in persons)
                {
                    months[person.Value.Month - 1]++;
                }

                ViewData["ApplicationsByMonths"] = months;
                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves data for visualization, grouping by application date and count.
        /// </summary>
        /// <returns>Returns JSON data of grouped application counts by date.</returns>
        public ActionResult GetData()
        {
            try
            {
                var persons = _context.Candidates.AsNoTracking()
                    .GroupBy(p => p.DateApply)
                    .Select(g => new { name = g.Key, count = g.Sum(w => w.Id) })
                    .ToList();

                return new JsonResult(new { data = persons });
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Deletes a candidate and related documents.
        /// </summary>
        /// <param name="candidateId">The ID of the candidate to delete.</param>
        /// <returns>Returns a JSON result indicating success or failure of the deletion.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> DeleteCandidateAsync(long candidateId)
        {
            try
            {
                var persons = _context.Candidates.Where(x => x.Id == candidateId).SingleOrDefault();

                if (persons == null)
                {
                    return Json(false);
                }

                List<Interviews> interviews = _context.Interviews.Where(x => x.CandidateId == candidateId).ToList();
                if (interviews != null)
                {
                    foreach (var interview in interviews)
                    {
                        List<InterviewTeams> interviewTeams = _context.InterviewTeams.Where(x => x.InterviewId == interview.Id).ToList();
                        if (interviewTeams != null)
                        {
                            foreach (var interviewTeam in interviewTeams)
                            {
                                _context.InterviewTeams.Remove(interviewTeam);
                                _context.SaveChanges();
                            }

                            _context.Interviews.Remove(interview);
                        }
                    }
                }

                await DeleteCandidateFolderAsync((int)candidateId);
                _context.Candidates.Remove(persons);
                var response = _context.SaveChanges();

                TempData["AlertMessage"] = Constants.CANDIDATE.DeletedWithSuccess;
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(StatusCode(500, string.Format(Constants.InternalServerError, ex.Message)));
            }
        }

        /// <summary>
        /// Exports candidate details to an Excel file.
        /// </summary>
        /// <returns>Returns an Excel file containing person CV data.</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult ExportToExcel()
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Cvs");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Id";
                    worksheet.Cell(currentRow, 2).Value = "Name";
                    worksheet.Cell(currentRow, 3).Value = "Date Apply";
                    worksheet.Cell(currentRow, 4).Value = "Function Apply";
                    worksheet.Cell(currentRow, 5).Value = "Function Match";
                    worksheet.Cell(currentRow, 6).Value = "Studies";
                    worksheet.Cell(currentRow, 7).Value = "Experience";
                    worksheet.Cell(currentRow, 8).Value = "Observation";
                    worksheet.Cell(currentRow, 9).Value = "Mode Apply";
                    worksheet.Cell(currentRow, 10).Value = "County";
                    worksheet.Cell(currentRow, 11).Value = "City";
                    worksheet.Cell(currentRow, 12).Value = "BirthDate";
                    worksheet.Cell(currentRow, 13).Value = "Status";

                    var cvs = _context.Candidates.ToList();

                    foreach (var cv in cvs)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = cv.Id;
                        worksheet.Cell(currentRow, 2).Value = cv.Name;
                        worksheet.Cell(currentRow, 3).Value = Convert.ToString(cv.DateApply);

                        List<Functions> functions = _context.Functions.ToList();
                        foreach (var function in functions)
                        {
                            if (cv.FunctionApply == function.Id)
                            {
                                worksheet.Cell(currentRow, 4).Value = function.Name;
                            }

                            if (cv.FunctionMatch == function.Id)
                            {
                                worksheet.Cell(currentRow, 5).Value = function.Name;
                            }
                        }

                        worksheet.Cell(currentRow, 6).Value = cv.Studies;
                        worksheet.Cell(currentRow, 7).Value = cv.Experience;
                        worksheet.Cell(currentRow, 8).Value = cv.Observation;
                        worksheet.Cell(currentRow, 9).Value = cv.ModeApply == 2 ? "Paper" : "Email";

                        var dateTimeNow = (DateTime)cv.BirthDate;
                        var dateOnlyString = dateTimeNow.ToShortDateString();

                        worksheet.Cell(currentRow, 10).Value = cv.County;
                        worksheet.Cell(currentRow, 11).Value = cv.City;
                        worksheet.Cell(currentRow, 12).Value = Convert.ToString(dateOnlyString);

                        worksheet.Cell(currentRow, 13).Value = cv.Status == 2 ? "Inactive" : "Active";
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CV.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Handles uploading files for a specific candidate.
        /// </summary>
        /// <param name="files">The uploaded files.</param>
        /// <param name="candidateId">The ID of the candidate.</param>
        /// <returns>Returns an empty result after processing.</returns>
        [Authorize(Roles = "Admin")]
        private async Task<bool> UploadFiles(List<IFormFile> files, int candidateId, string observation)
        {
            try
            {
                if (files == null || files.Count == 0)
                {
                    return false;
                }

                var maxFileSize = 5000000; // 5 MB
                string basePath = Path.Combine(Directory.GetCurrentDirectory(), CANDIDATE.rootFolder, candidateId.ToString());

                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                foreach (var file in files)
                {
                    if (file.Length > maxFileSize)
                    {
                        return false;
                    }

                    var fileName = file.FileName;
                    if (fileName.Length > 50) // Adjust the length according to your column size
                    {
                        fileName = fileName.Substring(0, 50); // Truncate the filename
                    }

                    var filePath = Path.Combine(basePath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);

                        Documents document = new Documents
                        {
                            DateAdded = DateTime.Today,
                            Name = fileName,
                            CandidateId = candidateId,
                            Observation = observation
                        };

                        _context.Documents.AddRange(document);
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Deletes a document from both the database and file system.
        /// </summary>
        /// <param name="documentId">The ID of the document to be deleted.</param>
        /// <returns>Returns a JSON result indicating success or failure of the deletion.</returns>
        [Authorize(Roles = "Admin")]
        public JsonResult DeleteCandidateDocument(int documentId)
        {
            try
            {
                var document = _context.Documents.SingleOrDefault(x => x.Id == documentId);

                if (document == null)
                {
                    return Json(false);
                }

                // Files to be deleted    
                string authorsFile = document.Name;
                string personId = document.CandidateId + "\\";
                string rootFolder = CANDIDATE.rootFolder + document.CandidateId;

                try
                {
                    var filePath = Path.Combine(rootFolder, authorsFile);

                    // Check if file exists with its full path 
                    if (System.IO.File.Exists(filePath))
                    {
                        // If file found, delete it    
                        System.IO.File.Delete(filePath);
                        _context.Documents.Remove(document);
                        _context.SaveChanges();
                        TempData["AlertMessage"] = CANDIDATE.DeletedWithSuccess;
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                catch (Exception ex)
                {
                    return Json(string.Format(Constants.CANDIDATE.DocumentDeletionError, ex.Message));
                }
            }
            catch (Exception ex)
            {
                return Json(string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Deletes a folder from both the file system.
        /// </summary>
        /// <param name="candidateId">The ID of the candidate who s file will be deleted.</param>
        /// <returns>Returns a JSON result indicating success or failure of the deletion.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> DeleteCandidateFolderAsync(int candidateId)
        {
            try
            {
                string rootFolder = CANDIDATE.rootFolder + candidateId;

                try
                {
                    // Check if directory exists with its full path 
                    if (System.IO.Directory.Exists(rootFolder))
                    {
                        // If directory found, delete it and its contents recursively    
                        System.IO.Directory.Delete(rootFolder, true);

                        var document = await _context.Documents.Where(d => d.CandidateId == candidateId).ToListAsync();

                        if (document == null)
                        {
                            return Json(false);
                        }

                        _context.Documents.RemoveRange(document);
                        await _context.SaveChangesAsync();

                        TempData["AlertMessage"] = CANDIDATE.DeletedWithSuccess;
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                catch (Exception ex)
                {
                    return Json(string.Format(Constants.CANDIDATE.DocumentDeletionError, ex.Message));
                }
            }
            catch (Exception ex)
            {
                return Json(string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// Retrieves documents related to a person CV for display in a grid.
        /// </summary>
        /// <param name="candidateId">The ID of the person CV.</param>
        /// <returns>Returns JSON data of documents related to the person CV.</returns>
        public ActionResult GetDocuments(int candidateId)
        {
            try
            {
                var documentsQuery = _context.Documents
                    .Where(x => x.CandidateId == candidateId);

                var totalRecords = documentsQuery.Count();

                var documents = documentsQuery
                    .Select(d => new
                    {
                        d.Id,
                        d.Name,
                        DateAdded = d.DateAdded.ToString("yyyy-MM-dd"),
                        d.Observation
                    })
                    .ToList();

                return Json(new
                {
                    data = documents,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords 
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = $"Error fetching documents: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Retrieves details of interviews for a specific person.
        /// </summary>
        /// <param name="candidateId">The ID of the person.</param>
        /// <param name="start">The starting index for pagination.</param>
        /// <param name="length">The number of records to fetch.</param>
        /// <param name="sortOrder">The order of sorting (asc/desc).</param>
        /// <param name="sortColumn">The column to sort by.</param>
        /// <returns>Returns a JSON object containing interview details.</returns>
        public async Task<ActionResult> GetInterviewsAsync(int candidateId)
        {
            try
            {
                var interviewsQuery = await _context.JobApplicationDetails.Where(j=> j.CandidateId == candidateId).ToListAsync();
                var totalRecords = interviewsQuery.Count();

                return Json(new
                {
                    data = interviewsQuery,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords // Presupunem că nu există filtrare suplimentară
                });
            }
            catch (Exception ex)
            {
                // Logăm excepția și returnăm un mesaj de eroare
                return Json(new
                {
                    error = true,
                    message = $"Error fetching interviews: {ex.Message}"
                });
            }
        }

        // GET: Candidates/Details/:id
        /// <summary>
        /// Retrieves details of a specific person CV.
        /// </summary>
        /// <param name="id">The ID of the person CV.</param>
        /// <returns>Returns a view displaying details of the person CV.</returns>
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var personCv = await _context.Candidates
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (personCv == null)
                {
                    return NotFound();
                }

                return View(personCv);
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }

        // GET: Candidates/Create
        /// <summary>
        /// Displays a form to create a new person CV.
        /// </summary>
        /// <returns>Returns a view for creating a new person CV.</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            try
            {
                var modeApplySelectList = Constants.ModeApply.Select(m => new SelectListItem
                {
                    Value = m.Key.ToString(),
                    Text = m.Value
                }).ToList();
                ViewData["ModeApply"] = new SelectList(modeApplySelectList, "Value", "Text");

                var statusSelectList = Constants.Status.Select(s => new SelectListItem
                {
                    Value = s.Key.ToString(),
                    Text = s.Value
                }).ToList();
                ViewData["Status"] = new SelectList(statusSelectList, "Value", "Text");

                ViewData["FunctionApply"] = new SelectList(_context.Functions, "Id", "Name");
                ViewData["FunctionMatch"] = new SelectList(_context.Functions, "Id", "Name");

                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }


        // POST: Candidates/Create
        /// <summary>
        /// Handles creation of a new person CV.
        /// </summary>
        /// <param name="model">The person CV model to create.</param>
        /// <returns>Returns a redirect to the Index action if successful, otherwise returns the Create view with errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,DateApply,FunctionApply,FunctionMatch,Observation,ModeApply,County,City,BirthDate,Studies,Status,Experience")] Candidates model, List<IFormFile> files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (files == null)
                    {
                        TempData["AlertMessage"] = Constants.CANDIDATE.AddCvBeforePerson;
                        return View(model);
                    }

                    var candidate = new Candidates
                    {
                        Name = model.Name,
                        DateApply = model.DateApply,
                        FunctionApply = model.FunctionApply,
                        FunctionMatch = model.FunctionMatch,
                        Observation = model.Observation,
                        ModeApply = model.ModeApply,
                        County = model.County,
                        City = model.City,
                        BirthDate = ((DateTime)model.BirthDate).Date,
                        Status = model.Status,
                        Experience = model.Experience,
                        Studies = model.Studies
                    };

                    _context.Candidates.AddRange(candidate);
                    await _context.SaveChangesAsync();

                    var lastCandidate = await _context.Candidates
                                        .OrderByDescending(c => c.Id)
                                        .FirstOrDefaultAsync();

                    // Handle file uploads
                    await UploadFiles(files, (int)lastCandidate.Id, model.Observation);

                    TempData["AlertMessage"] = Constants.CANDIDATE.PersonAddedSuccessfully;
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.CANDIDATE.CreatingPersonCvError, ex.Message));
            }
        }

        // GET: Candidates/Edit/:id
        /// <summary>
        /// Displays a form to edit details of a specific person CV.
        /// </summary>
        /// <param name="id">The ID of the person CV.</param>
        /// <returns>Returns a view for editing details of the person CV.</returns>
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(long? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var candidate = _context.Candidates.FirstOrDefault(x => x.Id == id);
                if (candidate == null)
                {
                    return NotFound();
                }

                var modeApplySelectList = Constants.ModeApply.Select(m => new SelectListItem
                {
                    Value = m.Key.ToString(),
                    Text = m.Value,
                    Selected = m.Key == candidate.ModeApply
                }).ToList();
                ViewData["ModeApply"] = new SelectList(modeApplySelectList, "Value", "Text");

                var statusSelectList = Constants.Status.Select(s => new SelectListItem
                {
                    Value = s.Key.ToString(),
                    Text = s.Value,
                    Selected = s.Key == candidate.Status
                }).ToList();
                ViewData["Status"] = new SelectList(statusSelectList, "Value", "Text");

                ViewData["FunctionApply"] = new SelectList(_context.Functions, "Id", "Name", candidate.FunctionApply);
                ViewData["FunctionMatch"] = new SelectList(_context.Functions, "Id", "Name", candidate.FunctionMatch);

                return View(candidate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.InternalServerError, ex.Message));
            }
        }
    
        // POST: Candidates/Edit/:id
        /// <summary>
        /// Handles updating details of a specific person CV.
        /// </summary>
        /// <param name="id">The ID of the person CV.</param>
        /// <param name="personCv">The updated person CV model.</param>
        /// <returns>Returns a redirect to the Index action if successful, otherwise returns the Edit view with errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,DateApply,FunctionApply,FunctionMatch,Observation,ModeApply,County,City,BirthDate,Status,Studies,Experience")] Candidates model, List<IFormFile> files)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();


                    if (files != null)
                    {
                        // Handle file uploads
                        await UploadFiles(files, (int)id, model.Observation);
                    }

                    TempData["AlertMessage"] = Constants.CANDIDATE.PersonUpdatedSuccessfully;
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidateExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, string.Format(Constants.CANDIDATE.UpdatingPersonCvError, ex.Message));
            }
        }

        /// <summary>
        /// Checks if a person CV with the specified ID exists in the database.
        /// </summary>
        /// <param name="id">The ID of the person CV to check.</param>
        /// <returns>True if a person CV with the specified ID exists; otherwise, false.</returns>
        private bool CandidateExists(long id)
        {
            return _context.Candidates.Any(e => e.Id == id);
        }
    }
}
