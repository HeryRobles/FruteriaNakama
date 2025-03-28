using DevilFruits.BLL.Response;
using DevilFruits.DTO.ExternalModel;

namespace DevilFruits.BLL.Services.IServices
{
    public interface IFrutaService
    {
        Task<HttpResponseWrapper<List<FrutaDTO>>> ListadoFrutas();
        Task<HttpResponseWrapper<FrutaDTO>> ObtenerFrutaAsync(int id);
    }
}
