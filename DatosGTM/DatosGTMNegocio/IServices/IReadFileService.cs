using DatosGTMModelo.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMNegocio.IServices
{
    public interface IReadFileService
    {
        List<Tercero> LeerArchivo(string path, Guid identificador);
    }
}
