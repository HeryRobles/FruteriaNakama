using DevilFruits.DTO;

namespace DevilFruits.BLL.Services.IServices
{
    public interface IFrutaService
    {
        Task<List<FrutaDTO>> ListadoFrutas();
        Task<FrutaDTO> ObtenerFrutaAsync(int id);
    }
}
