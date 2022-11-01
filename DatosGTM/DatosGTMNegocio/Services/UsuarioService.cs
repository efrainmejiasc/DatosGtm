
using DatosGTMModelo.DataModel;
using DatosGTMModelo.IRepositories;
using DatosGTMNegocio.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMNegocio.Services
{
    public class UsuarioService: IUsuarioService
    {
        private readonly IUsuarioRepository usuarioRepository;


        public UsuarioService(IUsuarioRepository _usuarioRepository)
        {
            this.usuarioRepository = _usuarioRepository;
        }

        public Usuario GetUserData( string userMail, string password)
        {
            var usuario = new Usuario();
            usuario = this.usuarioRepository.GetUserData(userMail, password);

            return usuario;
        }

    }
}
