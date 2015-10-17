// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    using Microsoft.AspNet.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    [Route("/[controller]")]
    public class UsersController : Controller
    {
        protected UserRepository Repository { get; private set; }
        protected CloudContext CloudContext { get; set; }

        public UsersController(CloudContext context, UserRepository repository)
        {
            Repository = repository;
            CloudContext = context;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> Get(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                var result = await Repository.GetAll();
                if (result.Any() == false)
                {
                    return new User[] { new User { Id = "-1", Name = "No users yet", SendMoment = false } };
                }
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