using DatosGTMModelo.DataModel;

namespace DatosGTMNegocio.DTOs
{
    public class ParametrosModel
    {

        public string Mensaje { get; set; }
        public bool Estado{ get; set; }
        public string PathArchivo {get;set; }
        public string NombreArchivo { get; set; }
        public List<string> PathFile { get; set; }
        public List<Tercero> Tercero{ get; set; }
    }
}
