using DatosGTMModelo.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMModelo.IRepositories
{
    public interface ITerceroRepository
    {
        List<Tercero> GetTerceros();
        List<Tercero> GetTerceros(Guid identificador);
        List<Tercero> GuardarRangoTercero(List<Tercero> model);
    }
}
