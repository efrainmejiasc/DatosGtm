using System.Collections.Generic;

namespace AIMatchWeb.Models
{
    public class AuthGptResponseDto: CommonPropertyResponseDto
    {
        public DataGptResponse data { get; set; }

    }

    public class DataGptResponse
    {
        public List<ComparacionGptResponse> comparacion { get; set; }
    }


    public class ComparacionGptResponse
    {
        public string nombreCampo { get; set; }
        public string valorF1 { get; set; }
        public string valorF2 { get; set; }
        public string cantidadF1 { get; set; }
        public string cantidadF2 { get; set; }
        public int numeroRepF1 { get; set; }
        public int numeroRepF2 { get; set; }
    }

    
}
