using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using WSVenta.Models;
using WSVenta.Models.Common;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using WSVenta.Tools;

namespace WSVenta.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }


        public UserResponse Auth(AuthRequest model)
        {
            UserResponse userResponse = new UserResponse();
            using (var db = new VentaRealContext())
            {

                string password = Encrypt.GetSHA256(model.Password);

                var usuario = db.Usuarios.Where(d => d.Email == model.Email && d.Password == password).FirstOrDefault();

                if (usuario == null) return null;

                userResponse.Email = usuario.Email;
                userResponse.Token = getToken(usuario);

            }

            return userResponse;

        }

        private string getToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(

                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier,usuario.Id.ToString() ),
                        new Claim(ClaimTypes.Email, usuario.Email )
                    }
                    ),
               Expires= DateTime.UtcNow.AddDays(60),
               SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(llave),SecurityAlgorithms.HmacSha256Signature)


            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
