using System.Collections.Generic;
using Newtonsoft.Json;


namespace AIMatchWeb.Models
{
    public class ComparacionFacturasDto
    {
        public EncabezadoDocumento encabezado_documento_1 { get; set; }
        public EncabezadoDocumento encabezado_documento_2 { get; set; }
        public ComparacionDetallada comparacion_detallada { get; set; }
        public List<ComparacionItemDto> Items { get; set; }  // Asegúrate de que esta propiedad exista
    }

    public class ComparacionDetallada
    {
        public List<Item> items { get; set; }
    }

    public class EncabezadoDocumento
    {
        public string proveedor { get; set; }
        public string direccion { get; set; }  // Corregido: direccin -> direccion
        public string fecha_emision { get; set; }
        public string total_facturado { get; set; }
    }

    public class Item
    {
        public string codigo_documento_1 { get; set; }
        public string codigo_documento_2 { get; set; }
        public string descripcion_documento_1 { get; set; }
        public string descripcion_documento_2 { get; set; }
        public string cantidad_documento_1 { get; set; }
        public string cantidad_documento_2 { get; set; }
        public string precio_unitario_documento_1 { get; set; }
        public string precio_unitario_documento_2 { get; set; }
        public string importe_total_documento_1 { get; set; }
        public string importe_total_documento_2 { get; set; }
        public string coincidencia { get; set; }
    }
}
