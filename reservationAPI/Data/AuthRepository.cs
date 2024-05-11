using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using reservationAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace reservationAPI.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext context, IConfiguration configuration) 
        {
            _context = context; 
            _configuration = configuration;
        }

        //Метод дял входу клієнта
        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            //Шукаємо користувача за його email
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email.ToLower().Equals(email.ToLower()));
            //Перевірка коректності даних
            if (user is null)
            {
                response.Success = false;
                response.Message = $"User with email:{email} not found";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password!";
            }
            else 
            {
                // В випадку коректного введення користувача створюємо токен та відправляємо його
                response.Data = CreateToken(user);
            }

            return response;
        }

        //Метод для реєстрації користувача
        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            //Перевірка чи користувач вже зареєстрований 
            if (await UserExist(user.Email))
            { 
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }

            //Хешуємо пароль
            CreatePasswordhash(password, out byte[] passwordHash, out byte[] passwordSalt);

            //Додаємо користувача до БД
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //Відправка відповіді
            response.Data=user.Id;
            response.Message = "New user created";
            return response;
        }

        //Перевірка чи користувач вже існує
        public async Task<bool> UserExist(string email)
        {
            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;
        }

        //Створення хешу для пароля
        private void CreatePasswordhash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        //Перевірка хешу пароля
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedhash.SequenceEqual(passwordHash);
            }
        }

        //Метод для створення JWT токена
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if (appSettingsToken is null)
                throw new Exception("token is null");

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
