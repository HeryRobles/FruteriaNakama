using DevilFruits.BLL.Services.IServices;
using DevilFruits.DTO;

namespace DevilFruits.BLL.Services.Acciones
{
    public interface IFrutaResenaService
    {
        Task<FrutaDetalleDTO> ObtenerFrutaConResenasAsync(int id);
    }
    public class FrutaResenaService : IFrutaResenaService
    {
        private readonly IFrutaService _frutaService;
        private readonly IResenaService _resenaService;
        public FrutaResenaService(IFrutaService frutaService, IResenaService resenaService)
        {
            _frutaService = frutaService;
            _resenaService = resenaService;
        }
        public async Task<FrutaDetalleDTO> ObtenerFrutaConResenasAsync(int id)
        {
            var frutaTask = _frutaService.ObtenerFrutaAsync(id);
            var resenasTask = _resenaService.ObtenerResenasPorFrutaAsync(id);

            await Task.WhenAll(frutaTask, resenasTask);

            var fruta = await frutaTask;
            var resenas = await resenasTask;

            if (fruta == null) return null!;

            var frutaDetalle = new FrutaDetalleDTO
            {
                Id = fruta.Id,
                Name = fruta.Name,
                Type = fruta.Type,
                Description = fruta.Description,
                Roman_Name = fruta.Roman_Name,
                Resenas = resenas,
                PuntajePromedio = resenas.Any() ? (int)resenas.Average(x => x.Puntaje) : 0
            };
            return frutaDetalle;
        }
    }
}
