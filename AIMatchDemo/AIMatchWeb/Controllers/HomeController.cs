using AIMatchWeb.Business.Interfaces;
using AIMatchWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace AIMatchWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPdfToJsonBusiness _pdfToJsonBusiness;
        private readonly IAuthGptBusiness _authGptBusiness;

        public HomeController(IPdfToJsonBusiness pdfToJsonBusiness, IAuthGptBusiness authGptBusiness)
        {
            this._pdfToJsonBusiness = pdfToJsonBusiness;
            this._authGptBusiness = authGptBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {

            var response = new AuthGptResponseDto()
            {
                httpCode = 400,
                httpStatus = false
            };

            if (files == null || files.Count < 2)
                return Json(response);

            var fileContents = new List<FileContentModelDto>();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var content = this._pdfToJsonBusiness.ExtractTextFromPdf(file);

                    fileContents.Add(new FileContentModelDto
                    {
                        FileName = file.FileName,
                        Content = content
                    });
                }
            }

            var model = CrearSolicitudComparacionFacturas(fileContents);
            var result = await this._authGptBusiness.PostHttpRequestAuthApiGpt(model);

            var objFullResponse = JsonConvert.DeserializeObject<FullResponseGptDto>(result);

            // Navegar a través de 'choices' hasta llegar al contenido que necesitas
            //var content1 = resultJson["choices"][0]["message"]["content"].ToString();
            var content1 = objFullResponse.choices[0].message.content;

            // Ahora puedes deserializar el contenido JSON dentro del mensaje a tu clase C#
            var startIndex = content1.IndexOf("{");
            var endIndex = content1.LastIndexOf("}");
            var jsonString = content1.Substring(startIndex, endIndex - startIndex + 1);

            // Deserializa el contenido JSON a la clase ComparacionFacturasDto
            var comparacionFacturas = JsonConvert.DeserializeObject<ComparacionFacturasDto>(jsonString);

            return Json(comparacionFacturas);

        }

        public AuthGptRequestDto CrearSolicitudComparacionFacturas(List<FileContentModelDto> fileContents)
        {
            var prompt = new StringBuilder("Eres un sistema que compara los ítems de una factura con los ítems de una orden de compra para verificar la consistencia de la facturación." +
                " Debes asegurarte de que cada ítem de la factura coincida con un ítem en la orden de compra en términos de **alta similitud** de las descripciones, cantidades y precios." +
                " Considera las descripciones similares aunque no sean idénticas, y determina si la facturación es consistente con la orden de compra." +
                " Te explico:  el archivo 1 contiene informacion de una factutra el archivo 2 contiene informacion de una OC , nesecito hacer el analisis y comparacion de los items de  " +
                " la factura contra los items de la orden de compra para asegurar que los articulos de la facturan coincidan  ademas de descripcion en cantidad y precio  para determinar que la " +
                " facturacion  sea  consistente  con la orden de compra ,Tenemos ademas que tener encuenta  que la descripciones pueden ser similares y no identicas  entonces alli interviene tu inteligencia artificial para optimizar este proceso" +
                " Proporciona el análisis minucioso de todo el conjunto de items comparados, de forma estructurada en el siguiente formato JSON:\n\n\n\n");

            prompt.AppendLine("```json");
            prompt.AppendLine("{");
            prompt.AppendLine("  \"encabezado_documento_1\": {");
            prompt.AppendLine("    \"proveedor\": \"\",");
            prompt.AppendLine("    \"direccion\": \"\",");
            prompt.AppendLine("    \"fecha_emision\": \"\",");
            prompt.AppendLine("    \"total_facturado\": \"\"");
            prompt.AppendLine("  },");
            prompt.AppendLine("  \"encabezado_documento_2\": {");
            prompt.AppendLine("    \"proveedor\": \"\",");
            prompt.AppendLine("    \"direccion\": \"\",");
            prompt.AppendLine("    \"fecha_emision\": \"\",");
            prompt.AppendLine("    \"total_facturado\": \"\"");  // Cambiado de "total_orden_compra" a "total_facturado"
            prompt.AppendLine("  },");
            prompt.AppendLine("  \"comparacion_detallada\": {");
            prompt.AppendLine("    \"items\": [");
            prompt.AppendLine("      {");
            prompt.AppendLine("        \"codigo_documento_1\": \"\",");
            prompt.AppendLine("        \"codigo_documento_2\": \"\",");  // Añadido este campo para la comparación
            prompt.AppendLine("        \"descripcion_documento_1\": \"\",");
            prompt.AppendLine("        \"descripcion_documento_2\": \"\",");
            prompt.AppendLine("        \"cantidad_documento_1\": \"\",");
            prompt.AppendLine("        \"cantidad_documento_2\": \"\",");
            prompt.AppendLine("        \"precio_unitario_documento_1\": \"\",");
            prompt.AppendLine("        \"precio_unitario_documento_2\": \"\",");
            prompt.AppendLine("        \"importe_total_documento_1\": \"\",");
            prompt.AppendLine("        \"importe_total_documento_2\": \"\",");
            prompt.AppendLine("        \"coincidencia\": \"\"  // Puede ser 'Total', 'Parcial' o 'Ninguna' basado en la similitud de las descripciones, cantidades y precios");
            prompt.AppendLine("      }");
            prompt.AppendLine("      // Repite para cada ítem");
            prompt.AppendLine("    ]");
            prompt.AppendLine("  }");
            prompt.AppendLine("}");
            prompt.AppendLine("```");

            prompt.AppendLine("\n Aquí están los documentos para comparar: \n");

            foreach (var file in fileContents)
            {
                prompt.AppendLine($"Archivo: {file.FileName}\n");
                prompt.AppendLine($"Contenido:\n{file.Content}\n");
            } 

            prompt.AppendLine("\n NOTA: Para mejor la espera en la comunicacion agregamos a nuestro codigo: 'client.Timeout = TimeSpan.FromMinutes(5);' " +
                "                        para que puedas tomar mas tiempo en tu analisis y respuesta. \n");

            var requestModel = new AuthGptRequestDto
            {
                Model = "gpt-3.5-turbo",
                Messages = new List<Message>
        {
            new Message { Role = "system", Content = "Eres un asistente del departamento de compras que compara ítems de facturas con órdenes de compra." +
                                                     "Tu tarea es analizar si los artículos facturados coinciden con los de la orden de compra en descripción, " +
                                                     "cantidad y precio, considerando descripciones similares como coincidencias" },
            new Message { Role = "user", Content = prompt.ToString() }
        }
            };

            return requestModel;
        }

    }
}
