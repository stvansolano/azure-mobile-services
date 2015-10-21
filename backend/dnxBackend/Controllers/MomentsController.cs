// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    using Microsoft.AspNet.Mvc;
    using System.Linq;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.IO;
    using System.Diagnostics;

    [Route("/[controller]")]
    public class MomentsController : Controller
    {
        protected MomentRepository Repository { get; set; }
        protected CloudContext CloudContext { get; set; }
        protected MediaStorageRepository MediaStorage { get; set; }

        public MomentsController(CloudContext context, MomentRepository repository, MediaStorageRepository mediaStorage)
        {
            Repository = repository;
            CloudContext = context;
            MediaStorage = mediaStorage;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetSentToMe(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return HttpBadRequest();
            }
            var result = await Repository.FindSentTo(userId);

            if (result.Any() == false)
            {
                result = new Moment[] { new Moment { Id = "No moments yet!" } };
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<HttpStatusCodeResult> Post([FromBody]MomentBody body)
        {
            if (ModelState.IsValid == false)
            {
                return HttpBadRequest();
            }
            var recipients = body.Recipients;

            if (body.IsValid() == false)
            {
                return HttpBadRequest();
            }
            var validRecipients = body.SanitizeRecipients();
            if (validRecipients.Any() == false)
            {
                return HttpBadRequest();
            }
            if (body.ContainsAttachedContent)
            {
                body.Url = await StoreImageBlob(body.Attached);
            }

            foreach (var user in validRecipients)
            {
                var moment = new Moment
                {
                    MomentUrl = body.Url ?? string.Empty,
                    SenderUserId = body.SenderId,
                    //SenderName = body.SenderName,
                    SenderProfileImage = body.SenderProfileImage ?? string.Empty,
                    RecipientUserId = user,
                    DisplayTime = body.DisplayTime,
                    TimeSent = DateTime.Now
                };

                Repository.Add(moment);
            }
            Repository.Commit();

            return new HttpStatusCodeResult((int)HttpStatusCode.Created);
        }

        [HttpGet("_Share")]
        public ActionResult _Share()
        {
            return View();
        }

        [HttpGet("_Download")]
        public async Task<ActionResult> Download(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpBadRequest();
            }
            string rawContents;

            using (var stream = new MemoryStream())
            {
                await MediaStorage.Download(id, stream);

                stream.Position = 0;

                rawContents = new StreamReader(stream).ReadToEnd();
            }
            var parts = rawContents.Split(',');
            const string OCTET = "application/octet-stream";
            // MediaTypeNames.Application.Octet

            if (parts.Any() == false)
            {
                return File(rawContents, OCTET, "file"); ;
            }
            if (parts.Length == 1)
            {
                return File(rawContents, OCTET, "file"); ;
            }

            var header = parts.First();
            var body = parts.ElementAt(1);
            var fileName = "file";

            try
            {
                var fileContents = Convert.FromBase64String(body);

                if (header.StartsWith("data:image/jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    fileName += ".jpeg";
                }
                return File(fileContents, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return File(rawContents, "application/octet-stream", fileName);
            }
        }

        private Task<string> StoreImageBlob(string contents)
        {
            return MediaStorage.Add(contents);
        }
    }
}