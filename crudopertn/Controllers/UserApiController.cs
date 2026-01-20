using Microsoft.AspNetCore.Mvc;
using crudopertn.Models;

namespace crudopertn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly UserDataAccessLayer _dal;

        public UserApiController(IConfiguration configuration)
        {
            _dal = new UserDataAccessLayer(configuration);
        }

        //GetById

        // GET: api/UserApi
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _dal.GetAllUsers();
            return Ok(users);
        }

        // POST: api/UserApi
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (user == null)
                return BadRequest();

            _dal.AddUser(user);
            return Ok(user);
        }

        //Get:api/UserApi/5
        [HttpGet("{id}")]
        //This maps the url like GET/api/UserApi/5
        public IActionResult GetById(int id)
        {
            var user = _dal.GetUserById(id);
            if (user == null)
                return NotFound($"User with ID {id} not found");
            return Ok(user);
        }



        //UpdateById

        //PUT: api/UserApi/5
        [HttpPut()]
        public IActionResult UpdatedUser(User user)
        {
            if (user.ID == 0 )
                return BadRequest("User ID Mismatch");
            
            var isUpdated = _dal.UpdatedUser(user);
            if (!isUpdated)
                return NotFound($"User with ID  not found");
            return Ok("User updated successfully");
        }



        // Delete

        //DELETE : api/UserApi/5
        [HttpDelete("{id}")]
        public IActionResult DeletedUser(int id)
        {
            var existingUser = _dal.GetUserById(id);
            if(existingUser == null)
                return NotFound($"User with ID{ id} does not exist");


            var isDeleted = _dal.DeletedUser(id);
            if (!isDeleted)
                return StatusCode(500, "Unable to delete user");

            return Ok("User deleted successfully");
        }




    }
}