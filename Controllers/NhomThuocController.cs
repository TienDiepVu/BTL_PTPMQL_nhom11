using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL.Data;
using BTL.Models;

namespace BTL.Controllers
{
    public class NhomThuocController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NhomThuocController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NhomThuoc
        public async Task<IActionResult> Index()
        {
            return View(await _context.NhomThuoc.ToListAsync());
        }

        // GET: NhomThuoc/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhomThuoc = await _context.NhomThuoc
                .FirstOrDefaultAsync(m => m.MaNhomThuoc == id);
            if (nhomThuoc == null)
            {
                return NotFound();
            }

            return View(nhomThuoc);
        }

        // GET: NhomThuoc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhomThuoc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNhomThuoc,TenNhomThuoc,MoTa")] NhomThuoc nhomThuoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhomThuoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhomThuoc);
        }

        // GET: NhomThuoc/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhomThuoc = await _context.NhomThuoc.FindAsync(id);
            if (nhomThuoc == null)
            {
                return NotFound();
            }
            return View(nhomThuoc);
        }

        // POST: NhomThuoc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNhomThuoc,TenNhomThuoc,MoTa")] NhomThuoc nhomThuoc)
        {
            if (id != nhomThuoc.MaNhomThuoc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhomThuoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhomThuocExists(nhomThuoc.MaNhomThuoc))
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
            return View(nhomThuoc);
        }

        // GET: NhomThuoc/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhomThuoc = await _context.NhomThuoc
                .FirstOrDefaultAsync(m => m.MaNhomThuoc == id);
            if (nhomThuoc == null)
            {
                return NotFound();
            }

            return View(nhomThuoc);
        }

        // POST: NhomThuoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var nhomThuoc = await _context.NhomThuoc.FindAsync(id);
            if (nhomThuoc != null)
            {
                _context.NhomThuoc.Remove(nhomThuoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhomThuocExists(string id)
        {
            return _context.NhomThuoc.Any(e => e.MaNhomThuoc == id);
        }
    }
}
