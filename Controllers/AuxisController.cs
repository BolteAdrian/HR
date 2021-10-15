using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HR.Models;

namespace HR.Controllers
{
    public class AuxisController : Controller
    {
        private readonly modelContext _context;

        public AuxisController(modelContext context)
        {
            _context = context;
        }

        // GET: Auxis
        public async Task<IActionResult> Index()
        {
            return View(await _context.Auxi.ToListAsync());
        }

        // GET: Auxis/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auxi = await _context.Auxi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auxi == null)
            {
                return NotFound();
            }

            return View(auxi);
        }

        // GET: Auxis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Auxis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RefusedReason,OffertStatus,ModeApply,Status,FunctionCv,Department")] Auxi auxi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(auxi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(auxi);
        }

        // GET: Auxis/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auxi = await _context.Auxi.FindAsync(id);
            if (auxi == null)
            {
                return NotFound();
            }
            return View(auxi);
        }

        // POST: Auxis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,RefusedReason,OffertStatus,ModeApply,Status,FunctionCv,Department")] Auxi auxi)
        {
            if (id != auxi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auxi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuxiExists(auxi.Id))
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
            return View(auxi);
        }

        // GET: Auxis/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auxi = await _context.Auxi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auxi == null)
            {
                return NotFound();
            }

            return View(auxi);
        }

        // POST: Auxis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var auxi = await _context.Auxi.FindAsync(id);
            _context.Auxi.Remove(auxi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuxiExists(long id)
        {
            return _context.Auxi.Any(e => e.Id == id);
        }
    }
}
