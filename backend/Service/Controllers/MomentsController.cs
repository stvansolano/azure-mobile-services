// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend
{
    //using Microsoft.AspNet.Mvc;
    using System.Linq;
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.IO;
    using System.Diagnostics;
    using System.Web.Http;
    using System.Collections.Generic;
    using Microsoft.WindowsAzure.Mobile.Service.Security;

    [AuthorizeLevel(AuthorizationLevel.Anonymous)]

    public class MomentsController : BaseController
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

        [HttpGet]
        public async Task<IHttpActionResult> GetSentToMe(string userId)
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
    }
}