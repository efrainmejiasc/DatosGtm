using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System.Diagnostics.Metrics;
using System.Text;
using Newtonsoft.Json;
using AIMatchWeb.Models;
using AIMatchWeb.Business.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AIMatchWeb.Business.Class
{
    public class PdfToJsonBusiness : IPdfToJsonBusiness
    {
        public string ConvertPDFToJson(string filePath,string fileName)
        {
            var pdfText = new StringBuilder();

            using (var reader = new PdfReader(filePath))
            {
                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    var text = PdfTextExtractor.GetTextFromPage(reader, page);
                    pdfText.AppendLine(text);
                }
            }

            var result = new PdfResultModel
            {
                FileName = fileName,
                Content = pdfText.ToString()
            };

           return JsonConvert.SerializeObject(result);
        }
        public string ExtractTextFromPdf(IFormFile file)
        {
            StringBuilder text = new StringBuilder();

            using (var reader = new PdfReader(file.OpenReadStream()))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    // Agrega un delimitador para cada página
                    text.AppendLine($"[PÁGINA {i} DE {reader.NumberOfPages}]");

                    // Extrae el texto de la página actual
                    string pageText = PdfTextExtractor.GetTextFromPage(reader, i);

                    // Estructura básica del texto, se puede personalizar más
                    text.AppendLine(StructureText(pageText));
                }
            }

            return text.ToString();
        }

        private string StructureText(string pageText)
        {
            StringBuilder structuredText = new StringBuilder();

            // Opcional: separa encabezados o secciones por palabras clave conocidas
            string[] lines = pageText.Split('\n');

            foreach (var line in lines)
            {
                // Puedes agregar lógica para identificar y etiquetar secciones
                if (line.Contains("PROVEEDOR:"))
                {
                    structuredText.AppendLine("[ENCABEZADO]");
                }
                else if (line.StartsWith("CÓDIGO"))
                {
                    structuredText.AppendLine("[ITEMS]");
                }
                else if (line.Contains("Fecha de entrega:"))
                {
                    structuredText.AppendLine("[CONDICIONES]");
                }

                // Agrega la línea actual al texto estructurado
                structuredText.AppendLine(line.Trim());
            }

            return structuredText.ToString();
        }


    }
}
