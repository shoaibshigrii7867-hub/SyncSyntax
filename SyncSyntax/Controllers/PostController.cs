using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SyncSyntax.Data;
using SyncSyntax.Models;
using SyncSyntax.Models.ViewModels;
using System.Threading.Tasks;

namespace SyncSyntax.Controllers
{
    

    public class PostController : Controller
    {
        private readonly ApDbContext context;
        
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string[] _allowedExtension = { ".jpg", ".jpeg", ".png", ".gif"  };

        public PostController(ApDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet]
        
        public IActionResult Index( int? categoryId)
        {
            var postQuery = context.Posts.Include(p => p.Category).AsQueryable();
            if (categoryId.HasValue)
            {
                postQuery =postQuery.Where(p=>p.CategoryId== categoryId.Value);
            }
             var posts = postQuery.ToList();
            ViewBag.Categories = context.Categories.ToList();
            return View(posts);
        }   
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            var postViewModel = new PostViewModel();
            postViewModel.Categories = context.Categories.Select(c =>
            new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }
            ).ToList();

            return View(postViewModel);
        }
       
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create(PostViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                var inpuFileExtension = Path.GetExtension(postViewModel.FutureImage.FileName);
                bool isallowed = _allowedExtension.Contains(inpuFileExtension);
                if (!isallowed)
                {
                    ModelState.AddModelError("FutureImage", "Only .jpg, .jpeg, .png, .gif .files are allowed.");
                    return View(postViewModel);
                }
                postViewModel.Post.FutureImageURL = await UploadFilefolder(postViewModel.FutureImage);
                await context.Posts.AddAsync(postViewModel.Post);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            postViewModel.Categories = context.Categories.Select(c =>
                  new SelectListItem
                  {
                     Value = c.Id.ToString(),
                     Text = c.Name
                  }
            ).ToList();
            return View(postViewModel);
        }
        [Authorize]
                
        public JsonResult AddComment([FromBody]Comments comment)
        {
            comment.CommentDate = DateTime.Now;
            context.Comments.Add(comment);
            context.SaveChanges();

            return Json(new
            {
                username = comment.UserName,
                commentdate = comment.CommentDate.ToString("yyyy-MM-dd"),
                content = comment.Content
            });
        }
        [HttpGet]

        [Authorize(Roles = "Admin")]

        public async  Task<IActionResult> Edits(int id)
        {
          if(id == null)
            {
                return NotFound();
            }
           var postfDb = await context.Posts.FirstOrDefaultAsync(p=>p.Id ==id);
            if(postfDb == null)
            {
                return NotFound();
            }
            var editViewModel = new EditViewModel
            {
                Post = postfDb,
                Categories = context.Categories.Select(c =>
                  new SelectListItem
                  {
                      Value = c.Id.ToString(),
                      Text = c.Name
                  }
            ).ToList()
           
            };
            return View(editViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async  Task<IActionResult> Edits(EditViewModel editViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editViewModel);
            }
            var postFromdb = await context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == editViewModel.Post.Id);

            if (postFromdb == null)
            { 
                return NotFound();
            }
            if (editViewModel.FutureImage!=null)
            {
                var inpuFileExtension = Path.GetExtension(editViewModel.FutureImage.FileName).ToLower();
                bool isAllowed = _allowedExtension.Contains(inpuFileExtension);
                if (!isAllowed)
                {
                    ModelState.AddModelError("FutureImage", "Only .jpg, .jpeg, .png, .gif files are allowed.");
                    return View(editViewModel);
                }
                var ExistingFilePath = Path.Combine(_webHostEnvironment.WebRootPath,"Images", Path.GetFileName(postFromdb.FutureImageURL));
                if (System.IO.File.Exists(ExistingFilePath))
                {
                    System.IO.File.Delete(ExistingFilePath);
                }
                editViewModel.Post.FutureImageURL= await UploadFilefolder(editViewModel.FutureImage);
            }
            else
            {
                editViewModel.Post.FutureImageURL = postFromdb.FutureImageURL;
            }
            context.Posts.Update(editViewModel.Post);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }   
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {
          
            var postfDb = await context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (postfDb == null)
            {
                return NotFound();
            }
           return View(postfDb);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeletePost(int id)
        {
            var postfDb = await context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (postfDb == null)
            {
                return NotFound();
            }
            var ExistingFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", Path.GetFileName(postfDb.FutureImageURL));
            if (System.IO.File.Exists(ExistingFilePath))
            {
                System.IO.File.Delete(ExistingFilePath);
            }
            context.Posts.Remove(postfDb);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task<string> UploadFilefolder(IFormFile file)
        {
            var uploadFolder = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString() + uploadFolder;
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var imagesFolderPath = Path.Combine(wwwRootPath, "images");
            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }
            var filePath = Path.Combine(imagesFolderPath, fileName);
            try
            {
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while uploading the file: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }
            return "/images/" + fileName;
        }
        [HttpGet]
        
        public IActionResult Details(int id)
        {
          
            if (id == null)
            {
                return NotFound();
            }
            var post = context.Posts.Include(p => p.Category).Include(p => p.Comments)
                .FirstOrDefault(p=>p.Id==id);
                
            if(post == null)
            {
                return NotFound();
            }

            return View(post);
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

