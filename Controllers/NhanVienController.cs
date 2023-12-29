using BTL.Data;
using BTL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL.Models.Process;
using OfficeOpenXml;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;

namespace BTL.Controllers 
{
    public class NhanVienController : Controller
    {
        private readonly  ApplicationDbContext _context;
        public NhanVienController(ApplicationDbContext context)
        {
            _context = context;
        }
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
            var model = _context.NhanVien.ToList().ToPagedList(page ?? 1, pagesize);
            return Task.FromResult<IActionResult>(View(model));
        }
        public IActionResult Create ()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNhanVien, TenNhanVien, ChucVu, MucLuong, SoDienThoai")] NhanVien nhanvien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhanvien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhanvien);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.NhanVien == null)
            {
                return NotFound();
            }

            var nhanvien = await _context.NhanVien.FindAsync(id);
            if (nhanvien == null)
            {
                return NotFound();
            }
            return View(nhanvien);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNhanVien, TenNhanVien, ChucVu, MucLuong, SoDienThoai")] NhanVien nhanvien)
        {
            if (id != nhanvien.MaNhanVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanvien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanvien.MaNhanVien))
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
            return View(nhanvien);
        }
        private bool NhanVienExists(string id)
        {
            return (_context.NhanVien?.Any(e => e.MaNhanVien == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.NhanVien == null)
            {
                return NotFound();
            }

            var nhanvien = await _context.NhanVien.FirstOrDefaultAsync(m => m.MaNhanVien == id);
            if (nhanvien == null)
            {
                return NotFound();
            }
            return View(nhanvien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.NhanVien == null)
            {
                return Problem("Entity set 'ApplicationDbContext.NhanVien' is null.");
            }

            var nhanvien = await _context.NhanVien.FindAsync(id);
            if (nhanvien != null)
            {
                _context.NhanVien.Remove(nhanvien);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
                                var nv = new NhanVien();
                                nv.MaNhanVien = dt.Rows[i][0].ToString();
                                nv.TenNhanVien = dt.Rows[i][1].ToString();
                                nv.ChucVu = dt.Rows[i][2].ToString();
                                nv.MucLuong = dt.Rows[i][3].ToString();
                                nv.SoDienThoai = dt.Rows[i][4].ToString();
                                _context.Add(nv);
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
            var fileName = "DanhSachNhanVien" + ".xlsx";
            using(ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A1"].Value = "MaNhanVien";
                worksheet.Cells["B1"].Value = "TenNhanVien";
                worksheet.Cells["C1"].Value = "ChucVu";
                worksheet.Cells["D1"].Value = "MucLuong";
                worksheet.Cells["E1"].Value = "SoDienThoai";
                var DanhSachNhanVien = _context.NhanVien.ToList();
                worksheet.Cells["A2"].LoadFromCollection(DanhSachNhanVien);
                var stream = new MemoryStream(excelPackage.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName) ;
            }
        }
    }
}