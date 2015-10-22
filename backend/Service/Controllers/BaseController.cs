namespace Backend
{
    using Microsoft.WindowsAzure.Mobile.Service;
    using System.Web.Http;

    public class BaseController : ApiController
    {
        public ApiServices Services { get; set; }

        protected IHttpActionResult HttpBadRequest()
        {
            return BadRequest();
        }
    }
}