using DevilFruits.DTO;
using DevilFruits.Model.Entities;

namespace DevilFruits.BLL.Services.IServices
{
    public interface IJwtService
    {
        string GenerarToken(Usuario usuario);
    }
}
