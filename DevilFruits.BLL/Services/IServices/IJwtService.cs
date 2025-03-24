using DevilFruits.DTO;

namespace DevilFruits.BLL.Services.IServices
{
    public interface IJwtService
    {
        string GenerarToken(LoginDTO usuario);
    }
}
