using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using System.Text;

using DoctorLRweb.Models;

using DoctorLRweb.Data;

namespace DoctorLRweb

{

    public class Auth : IAuth

    {

        private readonly string key;

        private readonly Context _context;

        public Auth(string key, Context context)

        {

            this.key = key;

            _context = context;

        }

        public string Authentication(string identifier, string password, string role)

        {

            var user = _context.Users.SingleOrDefault(u =>

                (u.UserId.ToString() == identifier || u.Email == identifier) &&

                u.Password == password &&

                u.Role.ToLower() == role.ToLower());

            if (user == null)

                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor

            {

                Subject = new ClaimsIdentity(new Claim[]

                {

                    new Claim(ClaimTypes.Name, user.UserId.ToString()),

                    new Claim(ClaimTypes.Role, user.Role)

                }),

                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(

                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

    }

}