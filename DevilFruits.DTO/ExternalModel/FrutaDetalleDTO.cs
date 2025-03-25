using DevilFruits.DTO.Models;

namespace DevilFruits.DTO.ExternalModel
{
    public class FrutaDetalleDTO : FrutaDTO
    {
        public List<ResenaDTO> Resenas { get; set; }
        public int PuntajePromedio { get; set; }
    }
}
