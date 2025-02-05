using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TraceLink.Models;
using TraceLink.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TraceLink.Pages.Tagging
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TraceLinkUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<TraceLinkUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public TaggingRequest TaggingRequest { get; set; }

        [BindProperty]
        public IFormFile RCBookFile { get; set; }

        [BindProperty]
        public IFormFile KYCFile { get; set; }

        [BindProperty]
        public IFormFile SurakshaMitrCertificateFile { get; set; }

        public List<string> AvailableIMEIs { get; set; } = new List<string>();
        public List<TaggingStatus> TaggingStatuses { get; set; } = new List<TaggingStatus>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Load the existing tagging request
            TaggingRequest = await _context.TaggingRequests
                
                
                .FirstOrDefaultAsync(t => t.Id == id);

            if (TaggingRequest == null)
            {
                return NotFound();
            }

            // Load available IMEIs and tagging statuses
            AvailableIMEIs = _context.Items
    .Where(i => !((i.IsTagged ?? false) && i.IMEI != TaggingRequest.IMEI)) // Fix: Handle nullable bool
    .Select(i => i.IMEI)
    .ToList();


            TaggingStatuses = await _context.TaggingStatuses.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            

            // Attach the existing tagging request to the context
            _context.Attach(TaggingRequest).State = EntityState.Modified;

            // Save the updated tagging request
            await _context.SaveChangesAsync();

            // Handle file uploads (if any)
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/tagging", TaggingRequest.Id.ToString());
            Directory.CreateDirectory(uploadPath);

            async Task SaveFileAndAddDocument(IFormFile file, string documentType)
            {
                if (file != null && file.Length > 0)
                {
                    var filePath = Path.Combine(uploadPath, $"{documentType}_{file.FileName}");
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var document = new Document
                    {
                        TaggingRequestId = TaggingRequest.Id,
                        FileName = file.FileName,
                        FilePath = $"/uploads/tagging/{TaggingRequest.Id}/{file.FileName}",
                        UploadedDate = DateTime.Now,
                        DocumentType = documentType
                    };
                    _context.Documents.Add(document);
                }
            }

            await SaveFileAndAddDocument(RCBookFile, "RCBook");
            await SaveFileAndAddDocument(KYCFile, "KYC");
            await SaveFileAndAddDocument(SurakshaMitrCertificateFile, "SurakshaMitr");

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}