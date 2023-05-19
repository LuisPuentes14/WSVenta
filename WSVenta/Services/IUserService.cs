using WSVenta.Models.Request;
using WSVenta.Models.Response;

namespace WSVenta.Services
{
    public interface IUserService
    {
        UserResponse Auth(AuthRequest model);
    }
}
