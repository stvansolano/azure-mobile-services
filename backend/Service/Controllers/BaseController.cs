namespace Backend
{
    using System.Web.Http;

    public class BaseController : ApiController
    {
        protected IHttpActionResult HttpBadRequest()
        {
            return BadRequest();
        }
    }
}