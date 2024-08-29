using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FileUploadApp.Data;
using FileUploadApp.Data.Models;
using Microsoft.AspNetCore.Hosting;

namespace FileUploadApp.Controllers
{
    public class FilesController : Controller
    {
        private readonly AppDbContext _context;
        private const string UploadPath = "C:\\Users\\todor.chankov\\source\\repos\\FileUploadApp\\FileUploadApp\\wwwroot\\"; // Define the path where you want to save the images

        public FilesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Files
        public async Task<IActionResult> Index()
        {
            return View(await _context.Images.ToListAsync());
        }

        // GET: Files/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @file = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@file == null)
            {
                return NotFound();
            }

            return View(@file);
        }

        // GET: Files/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ContentType,Data")] Data.Models.Image @file)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@file);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@file);
        }

        // GET: Files/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @file = await _context.Images.FindAsync(id);
            if (@file == null)
            {
                return NotFound();
            }
            return View(@file);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ContentType,Data")] Data.Models.Image @file)
        {
            if (id != @file.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@file);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(@file.Id))
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
            return View(@file);
        }

        // GET: Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @file = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@file == null)
            {
                return NotFound();
            }

            return View(@file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @file = await _context.Images.FindAsync(id);
            if (@file != null)
            {
                _context.Images.Remove(@file);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }

        [HttpPost, ActionName("Upload")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage([Bind("Name")] Image image, IFormFile ImageFile)
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Please select an image.");
            }

            if (ImageFile.ContentType.ToLower() != "image/jpeg" && ImageFile.ContentType.ToLower() != "image/jpg")
            {
                ModelState.AddModelError("ImageFile", "Only JPEG files are allowed.");
                return View(image);
            }

            if (ModelState.IsValid)
            {
                // Define the path where you want to save the images
                string uploadsFolder = Path.Combine(UploadPath, "uploads");
                // Create the folder if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                // Create a unique file name
                string uniqueFileName = Guid.NewGuid().ToString() + ".jpeg";
                // Combine the folder and file name to create the full path
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file to the specified path
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                // Set the FilePath property to the relative path of the image
                image.FilePath = "/uploads/" + uniqueFileName;

                // Save the image record in the database
                _context.Images.Add(image);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); // Redirect after successful upload
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // or use a logger to log the errors
                }
            }

            return View(image);
        }


    }
}
