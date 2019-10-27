using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManager.DataAccess;
using StudentManager.Models;

namespace StudentManager.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentsDbContext _context;

        public StudentsController(StudentsDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string name, DateTime? date, string address)
        {
            var students = from m in _context.Students
                           select m;

            if (!String.IsNullOrEmpty(name))
            {
                students = students.Where(s => s.Name.Contains(name));
            }
            if (!String.IsNullOrEmpty(address))
            {
                students = students.Where(s => s.Address.Contains(address));
            }

            if (date != null)
            {
                students = students.Where(s => DateTime.Compare((DateTime)s.MemberSince, (DateTime)date) >= 0);
            }
            return View(await students.ToListAsync());
        }

        public async Task<IActionResult> EmptyFilters()
        {
            return View("Index", await _context.Students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(m => m.Allergies)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            Student student = new Student();
            return View(student);
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,Name,MemberSince,CurrentlyStudying,Address")] Student student, List<Allergy> Allergies)
        {
            student.Allergies = Allergies;
            if (ModelState.IsValid)
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        public IActionResult _AddAllergyRow(int id)
        {
            ViewData["ID"] = id;
            return PartialView("_AllergyRow", new Allergy());
        }

        public IActionResult _RemoveAllergyRow(int id, List<Allergy> Allergies)
        {

            return PartialView("_AllergyRow", new Allergy());
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(m => m.Allergies)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentID,Name,MemberSince,CurrentlyStudying,Address,Allergies")] Student student)
        {
            if (id != student.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldAllergies = await _context.Allergies
                        .Where(a => a.StudentID == id)
                        .ToListAsync();
                    _context.RemoveRange(oldAllergies);
                    await _context.SaveChangesAsync();

                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentID))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentID == id);
        }

    }
}
