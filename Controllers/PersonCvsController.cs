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
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace HR.Controllers
{
    public class PersonCvsController : Controller
    {
        private readonly modelContext _context;

        public PersonCvsController(modelContext context)
        {
            _context = context;
        }


        [Authorize]
        // index cu search,paginare si order by name si id
        public async Task<IActionResult> Index(string filter, int page = 1,
                                               string sortExpression = "Id")
        {
            List<PersonCv> pers = await _context.PersonCv.AsNoTracking().OrderBy(p => p.Id).ToListAsync();


            
            var qry = _context.PersonCv.AsNoTracking().OrderBy(p => p.Id)
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(p => p.Name.Contains(filter) || p.FunctionApply.Contains(filter) || p.CityAddress.Contains(filter) || p.CountyAddress.Contains(filter));
            }

            var model = await PagingList.CreateAsync(
                                         qry, 10, page, sortExpression, "Name");

            model.RouteValue = new RouteValueDictionary {
        { "filter", filter}
    };

            return View(model);
        }




        //delete person

        [Authorize(Roles = "Admin")]
        public JsonResult DeleteEmployee(int EmployeeId)
        {

            bool result = false;


            //List<InterviewTeam> s2 = new List<InterviewTeam>();
            PersonCv s = _context.PersonCv.Where(x => x.Id == EmployeeId).SingleOrDefault();

            List<InterviewCv> s1 = _context.InterviewCv.Where(x => x.PersonCvid == EmployeeId).ToList();
            if (s1 != null)
            {
                foreach (var item in s1)
                {
                    List<InterviewTeam> s2 = _context.InterviewTeam.Where(x => x.InterviewCvid == item.Id).ToList();
                    if (s2 != null)
                    {
                        foreach (var item2 in s2)
                        {


                            _context.InterviewTeam.Remove(item2);

                            _context.SaveChanges();
                        }

                        _context.InterviewCv.Remove(item);
                    }
                }
            }

            List<Documents> d = _context.Documents.Where(x => x.PersonCvid == EmployeeId).ToList();
            if (d != null)
            {
                foreach (var item in d)
                {
                    _context.Documents.Remove(item);
                }
                }
                _context.PersonCv.Remove(s);
            _context.SaveChanges();
            //TODO: inactivare interview team si interview dupa scaffold

            return Json(result);

        }






        //export button
        //static List<PersonCv> multitable = new List<PersonCv>();
        [Authorize(Roles = "Admin")]
        public IActionResult ExportToExcel()
        {

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Cvs");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "DateApply";
                worksheet.Cell(currentRow, 4).Value = "FunctionApply";
                worksheet.Cell(currentRow, 5).Value = "Observation";
                worksheet.Cell(currentRow, 6).Value = "ModeApply";
                worksheet.Cell(currentRow, 7).Value = "CountyAddress";
                worksheet.Cell(currentRow, 8).Value = "CityAddress";
                worksheet.Cell(currentRow, 9).Value = "BirthDate";
                worksheet.Cell(currentRow, 10).Value = "Status";

                foreach (var x in _context.PersonCv)
                { 
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = x.Id;
                    worksheet.Cell(currentRow, 2).Value = x.Name;
                    worksheet.Cell(currentRow, 3).Value = Convert.ToString(x.DateApply);
                    worksheet.Cell(currentRow, 4).Value = x.FunctionApply;
                    worksheet.Cell(currentRow, 5).Value = x.Observation;
                    if (x.ModeApply == 1)
                    {
                        worksheet.Cell(currentRow, 6).Value = "Email";
                    }
                    else worksheet.Cell(currentRow, 6).Value = "Paper";

                    var dateTimeNow = (DateTime)x.BirthDate;
                    var dateOnlyString = dateTimeNow.ToShortDateString();

                    worksheet.Cell(currentRow, 7).Value = x.CountyAddress;
                    worksheet.Cell(currentRow, 8).Value = x.CityAddress;
                    worksheet.Cell(currentRow, 9).Value = Convert.ToString(dateOnlyString);

                   

                    if (x.Status == 1)
                    {
                        worksheet.Cell(currentRow, 10).Value = "Active";
                    }
                    else worksheet.Cell(currentRow, 10).Value = "Inactive";


                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CV.xlsx");
                }

            }
        }




        //upload button for CV-s la editare person

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UploadSmallFile(IFormFile smallFile)
        {

            PersonCv pers = _context.PersonCv.Where(x => x.Id == aux).SingleOrDefault();


            Documents d = new Documents();
            try
            {
                var maxFileSize = 4000000;
                if (smallFile.Length < maxFileSize)
                {

                    var path = Path.Combine(@"D:\Programe\New folder\htdocs\CV-uri\", pers.Id.ToString());
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    using (var fileStream = System.IO.File.Create(Path.Combine(path, smallFile.FileName)))
                    {
                        smallFile.CopyTo(fileStream);
                    }
                }

            }
            catch
            {
                Response.StatusCode = 400;
            }



            List<long> Names = _context.Documents
  .Where(u => u.Id > 0)
  .Select(u => u.Id)
  .ToList();
            int lastID = ((int)(Names.LastOrDefault() + 1));




           
            d.Id = lastID;
            d.DateAdded = DateTime.Today;
            d.DocumentName = smallFile.FileName;
            d.PersonCvid = aux;
            _context.Documents.Add(d);
            _context.SaveChanges();



            return new EmptyResult();
        }

        
        static string NumeF;
        //upload button for CV-s la creere person

        [HttpPost]
        public ActionResult UploadSmallFile2(IFormFile smallFile)
        {

           

            List<long> Tablou = _context.PersonCv
.Select(u => u.Id)
.ToList();
            int aux3 = ((int)Tablou.LastOrDefault() + 1);

            try
            {
                var maxFileSize = 4000000;
                if (smallFile.Length < maxFileSize)
                {

                    var path = Path.Combine(@"D:\Programe\New folder\htdocs\CV-uri\", aux3.ToString());
                    
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    using (var fileStream = System.IO.File.Create(Path.Combine(path, smallFile.FileName)))
                    {
                        smallFile.CopyTo(fileStream);
                    }
                }

            }
            catch
            {
                Response.StatusCode = 400;
            }

            NumeF = smallFile.FileName;
            return new EmptyResult();
        }




        static int aux;


        static string rootFolder = @"D:\Programe\New folder\htdocs\CV-uri\";


        //delete document din baza de date si director
        [Authorize(Roles = "Admin")]
        public JsonResult DeleteEmployee2(int EmployeeId)
        {

            bool result = false;
            Documents s = _context.Documents.Where(x => x.Id == EmployeeId).SingleOrDefault();


            _context.Documents.Remove(s);
            _context.SaveChanges();


            // Files to be deleted    
            string authorsFile = s.DocumentName;
            rootFolder += s.Id;
            try
            {
                // Check if file exists with its full path    
                if (System.IO.File.Exists(Path.Combine(rootFolder, authorsFile)))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(Path.Combine(rootFolder, authorsFile));

                }

            }
            catch
            {

            }



            return Json(result);

        }


        //Grid Dcuments edit/details
        public ActionResult GetDocuments(int PersonCVId, DataSourceLoadOptions loadOptions)
        {
            aux = PersonCVId;
            var documents = _context.Documents.Where(x => x.PersonCvid == PersonCVId).ToList();
            //return new JsonConvert(DataSourceLoader.Load(interviews, loadOptions), "application/json");
            return Content(JsonConvert.SerializeObject(DataSourceLoader.Load(documents, loadOptions)), "application/json");
        }




        //Grid Interview
        public ActionResult GetInterview(int PersonCVId, DataSourceLoadOptions loadOptions)
        {
            aux = PersonCVId;
            //var interviews = _context.InterviewCv.Where(x => x.Id == x.PersonCvid).ToList();
            var interviews = _context.InterviewCv.Where(x => x.PersonCvid == PersonCVId).ToList();
            //return new JsonConvert(DataSourceLoader.Load(interviews, loadOptions), "application/json");
            return Content(JsonConvert.SerializeObject(DataSourceLoader.Load(interviews, loadOptions)), "application/json");
        }







        // GET: PersonCvs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personCv = await _context.PersonCv
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personCv == null)
            {
                return NotFound();
            }

            return View(personCv);
        }

        // GET: PersonCvs/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ModeApply"] = new SelectList(_context.Auxi, "Id", "ModeApply");
            ViewData["Status"] = new SelectList(_context.Auxi, "Id", "Status");
            return View();
        }









        // POST: PersonCvs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,DateApply,FunctionApply,FunctionMatch,Observation,ModeApply,CountyAddress,CityAddress,BirthDate")] PersonCv m)
        {

            List<long> Tablou = _context.PersonCv
.Select(u => u.Id)
.ToList();
            int aux2 = ((int)Tablou.LastOrDefault() + 1);

           
            using (var transaction = _context.Database.BeginTransaction())
            {


                DateTime dt = (DateTime)m.BirthDate;
                

                PersonCv personCv = new PersonCv();
                personCv.Id = aux2;
                personCv.Name = m.Name;
                personCv.DateApply = m.DateApply;
                personCv.FunctionApply = m.FunctionApply;
                personCv.FunctionMatch = m.FunctionMatch;
                personCv.Observation = m.Observation;
                personCv.ModeApply = m.ModeApply;
                personCv.CountyAddress = m.CountyAddress;
                personCv.CityAddress = m.CityAddress;
                personCv.BirthDate = dt.Date;
                personCv.Status = m.Status;




                _context.PersonCv.AddRange(personCv);
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CV.PersonCV ON;");
                await _context.SaveChangesAsync();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CV.PersonCV OFF;");
                transaction.Commit();


                List<long> Names = _context.Documents
      .Where(u => u.Id > 0)
      .Select(u => u.Id)
      .ToList();
                int lastID = ((int)(Names.LastOrDefault() + 1));

                Documents d = new Documents();

                d.Id = lastID;
                d.DateAdded = DateTime.Today;
                d.DocumentName = NumeF;
                d.PersonCvid = aux2;
                _context.Documents.AddRange(d);
                await _context.SaveChangesAsync();
           
            }

return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "Admin")]
        // GET: PersonCvs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            ViewData["ModeApply"] = new SelectList(_context.Auxi, "Id", "ModeApply");
            ViewData["Status"] = new SelectList(_context.Auxi, "Id", "Status");
            if (id == null)
            {
                return NotFound();
            }

            var personCv = await _context.PersonCv.FindAsync(id);
            if (personCv == null)
            {
                return NotFound();
            }
            return View(personCv);
        }

        // POST: PersonCvs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,DateApply,FunctionApply,FunctionMatch,Observation,ModeApply,CountyAddress,CityAddress,BirthDate,Age,AddedBy,AddedAt,UpdatedBy,UpdatedAt,Status,CvreciveDate")] PersonCv personCv)
        {
            if (id != personCv.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personCv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonCvExists(personCv.Id))
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
            return View(personCv);
        }

     //verifica daca persoana exista in baza de date
        private bool PersonCvExists(long id)
        {
            return _context.PersonCv.Any(e => e.Id == id);
        }
    }
}
