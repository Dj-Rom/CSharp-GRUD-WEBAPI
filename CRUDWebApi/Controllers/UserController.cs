using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CRUDWebApi.Model;

namespace CRUDWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> _users = new List<User>();

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_users.Count == 0)
                return NoContent();

            return Ok(_users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Email))
                return BadRequest("First name, last name, and email are required.");

            user.Id = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
            user.Email = user.Email.ToLower().Trim();
            user.FirstName = user.FirstName.Trim();
            user.LastName = user.LastName.Trim();
            user.PhoneNumber = user.PhoneNumber?.Replace(" ", "") ?? "";

            _users.Add(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Email))
                return BadRequest("First name, last name, and email are required.");

            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
                return NotFound();

            existingUser.FirstName = user.FirstName.Trim();
            existingUser.LastName = user.LastName.Trim();
            existingUser.Email = user.Email.ToLower().Trim();
            existingUser.PhoneNumber = user.PhoneNumber?.Replace(" ", "") ?? "";

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_users.Count == 0)
                return NoContent();

            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            _users.Remove(user);
            return NoContent();
        }
    }
}