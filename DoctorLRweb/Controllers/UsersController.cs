using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorLRweb.Data;
using DoctorLRweb.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using DoctorLRweb.Services;

namespace DoctorLRweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Context _context;
        private readonly IUserService _userService;
        private readonly IAuth _Auth;

        public UsersController(Context context, IUserService _userService, IAuth Auth) 
        {
            this._context = context;
            this._userService = _userService;
            this._Auth = Auth;
        }
        

        //public UsersController(Context context)
        //{
        //    _context = context;
        //}

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            await _userService.CreateUser(user); // This handles auto-generating 4-digit UserId
            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
       
        [HttpGet("search")]
        public IActionResult GetByIdentifier(string identifier)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Email == identifier || u.UserId.ToString() == identifier);
            if (user == null)
                return NotFound();
            user.Password = null;
            return Ok(user);
        }
        [HttpGet("by-role")]
        public IActionResult GetUsersByRole(string role)
        {
            var users = _userService.GetUsersByRole(role);
            return Ok(users);
        }
       
        // [AllowAnonymous]
        // POST api/<UsersController>/authentication
        /*  [HttpPost("authentication")]
          public IActionResult Authentication([FromBody] User user)
          {
              var token = _Auth.Authentication(user.UserId.ToString(), user.Password);
              if (token == null)
                  return Unauthorized();
              return Ok(new { Token = token });
          }*/
    }
}
