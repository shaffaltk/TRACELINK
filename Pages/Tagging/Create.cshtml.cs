using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TraceLink.Models;
using TraceLink.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TraceLink.Pages.Tagging
{

    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TraceLinkUser> _userManager;


        public CreateModel(ApplicationDbContext context, UserManager<TraceLinkUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public TaggingRequest TaggingRequest { get; set; } = new TaggingRequest();

        [BindProperty]
        
        public IFormFile RCBookFile { get; set; }

        [BindProperty]
        public IFormFile KYCFile { get; set; }

        [BindProperty]
        public IFormFile SurakshaMitrCertificateFile { get; set; }


        public List<string> AvailableIMEIs { get; set; } = new List<string>();
        public List<TaggingStatus> TaggingStatuses { get; set; } = new List<TaggingStatus>();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            AvailableIMEIs = _context.Items
     .Where(i => !(i.IsTagged ?? false)) // ✅ Convert `null` to `false`
     .Select(i => i.IMEI)
     .ToList();

            TaggingStatuses = await _context.TaggingStatuses.ToListAsync();

            return Page();
        }
        private string GenerateTagNumber()
        {
            // Prefix for the tag number
            string prefix = "TGR";

            // Count existing tagging requests to generate a unique identifier
            int tagCount = _context.TaggingRequests.Count();

            // Combine prefix and count to generate a unique TagNumber
            string tagNumber = $"{prefix}-{(tagCount + 1):D6}"; // Format: TGR-000001

            // Ensure uniqueness by checking existing tag numbers
            while (_context.TaggingRequests.Any(t => t.TagNumber == tagNumber))
            {
                tagCount++;
                tagNumber = $"{prefix}-{tagCount:D6}";
            }

            return tagNumber;
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var DealerId = user.DealerId.Value;
            TaggingRequest.DealerId = DealerId;
            TaggingRequest.SubDealerId = user.SubDealerId.Value;
            TaggingRequest.RequestDate = DateTime.Now;

            // Generate Tag Number using the new method
            TaggingRequest.TagNumber = GenerateTagNumber();

            // Save the tagging request
            _context.TaggingRequests.Add(TaggingRequest);
            await _context.SaveChangesAsync();

            // Create the upload directory for the tagging request
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/tagging", TaggingRequest.Id.ToString());
            Directory.CreateDirectory(uploadPath);

            // Helper method to save a file and add a document entry
            async Task SaveFileAndAddDocument(IFormFile file, string documentType)
            {
                if (file != null && file.Length > 0)
                {
                    // Create a unique file name and save the file
                    var filePath = Path.Combine(uploadPath, $"{documentType}_{file.FileName}");
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Add an entry to the Document table
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

            // Save each file and add corresponding document entries
            await SaveFileAndAddDocument(RCBookFile, "RCBook");
            await SaveFileAndAddDocument(KYCFile, "KYC");
            await SaveFileAndAddDocument(SurakshaMitrCertificateFile, "SurakshaMitr");

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }



    }
}