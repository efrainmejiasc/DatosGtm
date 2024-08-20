namespace AIMatchWeb.Models
{
    public class ComparacionItemDto
    {
        public string Codigo { get; set; }
        public string DescripcionDocumento1 { get; set; }
        public string DescripcionDocumento2 { get; set; }
        public string CantidadDocumento1 { get; set; }
        public string CantidadDocumento2 { get; set; }
        public string PrecioUnitarioDocumento1 { get; set; }
        public string PrecioUnitarioDocumento2 { get; set; }
        public string ImporteTotalDocumento1 { get; set; }
        public string ImporteTotalDocumento2 { get; set; }
        public string Coincidencia { get; set; }  // Puede ser 'Total', 'Parcial', 'Ninguna' o 'Similar'

    }
}
