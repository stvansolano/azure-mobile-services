// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    using Microsoft.AspNet.Mvc;
    using System.Collections.Generic;

    [Route("/[controller]")]
    public partial class FriendsController : Controller
    {
        protected FriendshipRepository Repository { get; private set; }
        protected CloudContext CloudContext { get; set; }

        public FriendsController(CloudContext context, FriendshipRepository repository)
        {
            Repository = repository;
            CloudContext = context;
        }
        
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public IEnumerable<User> Get(bool getFriends, string userId)
        {
            if (getFriends)
            {
                var results = Repository.Where(friendshipRequest => friendshipRequest.Accepted && friendshipRequest.UserId == userId);
            }

            /*var employeeQuery = await table.CreateQuery<FriendshipEntity>();
            var query = (from employee in employeeQuery
                         where employee.Accepted
                         select employee).AsTableQuery();

            var queryResults = query.Execute();

            var results = (from item in queryResults
                           select new User { Id = item.UserId, Name = "Esteban Solano G." }).ToArray();

            return results;*/

            return new User[] { new User { Id = "1234", Name = "Esteban Solano G." } };
        }
    }
}