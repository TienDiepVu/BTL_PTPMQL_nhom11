using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL.Data;
using BTL.Models;
using X.PagedList;
using OfficeOpenXml;
using BTL.Models.Process;

namespace BTL.Controllers
{
    public class ThuocController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThuocController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Thuoc
        private ExcelProcess _excelPro = new ExcelProcess();
        public Task<IActionResult> Index(int? page, int? PageSize)
        {
            ViewBag.PageSize = new List<SelectListItem>()
            {
                new SelectListItem() {Value = "3" , Text = "3"},
                new SelectListItem() {Value = "5" , Text = "5"},
                new SelectListItem() {Value = "10" , Text = "10"},
                new SelectListItem() {Value = "15" , Text = "15"},
                new SelectListItem() {Value = "25" , Text = "25"},
                new SelectListItem() {Value = "50" , Text = "50"},
            };
            int pagesize = (PageSize ?? 5);
            ViewBag.psize = pagesize;
            var model = _context.Thuoc.ToList().ToPagedList(page ?? 1, pagesize);
            return Task.FromResult<IActionResult>(View(model));
        }

        // GET: Thuoc/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuoc
                .FirstOrDefaultAsync(m => m.MaThuoc == id);
            if (thuoc == null)
            {
                return NotFound();
            }

            return View(thuoc);
        }

        // GET: Thuoc/Create
        public IActionResult Create()
        {
            ViewBag.NhomThuoc = _context.NhomThuoc;
            ViewBag.NhaCungCap = _context.NhaCungCap;
            return View();
        }

        // POST: Thuoc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaThuoc,TenThuoc,SoLuong,GiaBan,NhomThuoc,NhaCungCap,CongDung")] Thuoc thuoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thuoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(thuoc);
        }

        // GET: Thuoc/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.NhomThuoc = _context.NhomThuoc;
            ViewBag.NhaCungCap = _context.NhaCungCap;
            if (id == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuoc.FindAsync(id);
            if (thuoc == null)
            {
                return NotFound();
            }
            return View(thuoc);
        }

        // POST: Thuoc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaThuoc,TenThuoc,SoLuong,GiaBan,NhomThuoc,NhaCungCap,CongDung")] Thuoc thuoc)
        {
            if (id != thuoc.MaThuoc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thuoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThuocExists(thuoc.MaThuoc))
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
            return View(thuoc);
        }

        // GET: Thuoc/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuoc
                .FirstOrDefaultAsync(m => m.MaThuoc == id);
            if (thuoc == null)
            {
                return NotFound();
            }

            return View(thuoc);
        }

        // POST: Thuoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var thuoc = await _context.Thuoc.FindAsync(id);
            if (thuoc != null)
            {
                _context.Thuoc.Remove(thuoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThuocExists(string id)
        {
            return _context.Thuoc.Any(e => e.MaThuoc == id);
        }
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    ModelState.AddModelError("", "Please choose excel file to upload!");
                }
                else
                {
                    //rename file when upload to server
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", "File" + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Millisecond + fileExtension);
                    var fileLocation = new FileInfo(filePath).ToString();
                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            //save file to server
                            await file.CopyToAsync(stream);
                            //read data from file and write to database
                            var dt = _excelPro.ExcelToDataTable(fileLocation);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                var t = new Thuoc(); 
                                t.MaThuoc = dt.Rows[i][0].ToString();
                                t.TenThuoc = dt.Rows[i][1].ToString();
                                t.SoLuong = int.Parse((string)dt.Rows[i][2]);
                                t.GiaBan = int.Parse((string)dt.Rows[i][3]);
                                t.NhomThuoc = dt.Rows[i][4].ToString();
                                t.NhaCungCap = dt.Rows[i][5].ToString();
                                t.CongDung = dt.Rows[i][6].ToString();
                                _context.Add(t);
                            }
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }

            return View();
        }
        public IActionResult Download()
        {
            var fileName = "Danhsachthuoc" + ".xlsx";
            using(ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A1"].Value = "MaThuoc";
                worksheet.Cells["B1"].Value = "TenThuoc";
                worksheet.Cells["C1"].Value = "SoLuong";
                worksheet.Cells["D1"].Value = "GiaBan";
                worksheet.Cells["E1"].Value = "NhomThuoc";
                worksheet.Cells["F1"].Value = "NhaCungCap";
                worksheet.Cells["G1"].Value = "CongDung";
                var Danhsachthuoc = _context.Thuoc.ToList();
                worksheet.Cells["A2"].LoadFromCollection(Danhsachthuoc);
                var stream = new MemoryStream(excelPackage.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName) ;
            }
        }
    }
}
