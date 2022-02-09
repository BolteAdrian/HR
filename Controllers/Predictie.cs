using HR.Models;
using HRML.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR.Controllers
{
    public class Predictie : Controller
    {
        private readonly modelContext _context;

        public Predictie(modelContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index(string filter, int page = 1,
                                             string sortExpression = "Id")
        {
            List<PersonCv> _multitable = await _context.PersonCv.AsNoTracking().OrderBy(p => p.Id).ToListAsync();

          

            var qry = _context.PersonCv.AsNoTracking().OrderBy(p => p.Id)
                .AsQueryable();




            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(p => p.Name.Contains(filter) || p.CityAddress.Contains(filter) || p.CountyAddress.Contains(filter));
            }

            var model = await PagingList.CreateAsync(
                                         qry, 10, page, sortExpression, "Name");

            model.RouteValue = new RouteValueDictionary {
        { "filter", filter}
    };


            List<Functions> f = await _context.Functions.AsNoTracking().OrderBy(p => p.Id).ToListAsync();
            
            List<ModelInput> ModeIn = new List<ModelInput>();
            foreach (var input in _multitable)
            {
                if (input.Status != 2)
                {
                    ModelInput aux = new ModelInput();

                    var dateTimeNow = (DateTime)input.BirthDate;
                    var dateOnlyString = dateTimeNow.ToShortDateString();
                    Convert.ToString(dateOnlyString);

                    

                    aux.Id = input.Id;
                    aux.BirthDate = dateOnlyString.ToString();

                    long y = (long)input.FunctionApply;
                    foreach (var fi in f)
                    {
                        if (fi.Id == y)
                        {
                            aux.Name_function = fi.NameFunction;
                            List<Departments> dep = await _context.Departments.AsNoTracking().OrderBy(p => p.Id).ToListAsync();
                            foreach (var fi2 in dep)
                            {
                                if (fi2.Id == fi.IdDepartment)
                                {
                                    aux.Name_department = fi2.NameDepartment;
                                }
                            }

                        }


                    }
                    
                        aux.ModeApply = (float)input.ModeApply;
                    aux.Studies = input.Studies;
                    aux.Experience = input.Experience;
                    aux.Observation = input.Observation;
                    ModeIn.Add(aux);
                    
                }
                else
                    continue;
            }

            Dictionary<int, float> p = new Dictionary<int, float>();

            
           
            foreach (var input in ModeIn)
            {
               
                // Load model and predict output of sample data
                ModelOutput result = ConsumeModel.Predict(input);

                p.Add((int)input.Id, (result.Score[0])*100);
                
                

            }
            List<Functions> fw = _context.Functions.ToList();

            ViewData["FunctionApply"] = fw;
            ViewData["FunctionMatch"] = fw;


            ViewBag.Prediction = p;




            return View(model);
        }




    }
}
