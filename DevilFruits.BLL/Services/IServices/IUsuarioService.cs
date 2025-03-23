using DevilFruits.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevilFruits.BLL.Services.IServices
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> ListaUsuarios();
        Task<UsuarioDTO> ObtenerUsuario(int id);
        Task<UsuarioDTO> CrearUsuario(UsuarioDTO usuario);
        Task<bool> EditarUsuario(UsuarioDTO usuario, int usuarioActual);
        Task<bool> EliminarUsuario(int id);
    }
}
