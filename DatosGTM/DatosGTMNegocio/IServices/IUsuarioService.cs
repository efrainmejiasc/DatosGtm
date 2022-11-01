
using DatosGTMModelo.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMNegocio.IServices
{
    public interface IUsuarioService
    {
        Usuario GetUserData(string userMail, string password);
    }
}
