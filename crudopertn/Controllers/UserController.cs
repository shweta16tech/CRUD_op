using Microsoft.AspNetCore.Mvc;
using crudopertn.Models;

namespace crudopertn.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserDataAccessLayer _dal;

        public UserController(IConfiguration configuration)
        {
            _dal = new UserDataAccessLayer(configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = _dal.GetAllUsers();
            return Ok(users);
        }
        
        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _dal.AddUser(user);
                return Ok();
            }

            return View(user);
        }
    }
}