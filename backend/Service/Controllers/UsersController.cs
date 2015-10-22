namespace Backend
{
    using Microsoft.WindowsAzure.Mobile.Service;
    using Microsoft.WindowsAzure.Mobile.Service.Security;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class UsersController : BaseController
    {
        protected UserRepository Repository { get; private set; }

        public UsersController(UserRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public async Task<IEnumerable<User>> Get(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                var result = await Repository.GetAll();
                if (result.Any() == false)
                {
                    return new User[] { new User { Id = "-1", Name = "No users yet", SendMoment = false } };
                }

                Services.Log.Info("User found");
         
                return result;
            }
            return new User[] { await Repository.Find(userId) };
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] User model)
        {
            Repository.Add(model);

            Repository.Commit();

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}
