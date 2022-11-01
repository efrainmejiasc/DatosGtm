using DatosGTMModelo.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMModelo.IRepositories
{
    public interface IUsuarioRepository
    {
        Usuario GetUserData(string userMail, string password);
    }
}
