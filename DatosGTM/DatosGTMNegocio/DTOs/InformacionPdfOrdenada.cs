using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMNegocio.DTOs
{
    public  class InformacionPdfOrdenada
    {
        public List<string>  Numero { get; set; }
        public List<string> Nit { get; set; }
        public List<string> Nombre { get; set; }
        public List<string> FechaInicio { get; set; }
    }
}
