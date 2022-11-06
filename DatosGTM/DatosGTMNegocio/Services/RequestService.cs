using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DatosGTMNegocio.Helpers;
using DatosGTMNegocio.IServices;
using System.Xml;
using DatosGTMNegocio.DTOs;

namespace DatosGTMNegocio.Services
{
    public  class RequestService:IRequestService
    {
        public async Task<AdobePdfApiTokenModel> ObtenerJWTAsync(string urlAdobePdfApi)
        {
            var respuesta = string.Empty;
            var adobePdfApiTokenModel = new AdobePdfApiTokenModel();
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), AdobePdfApi.urlAdobePdfApiJwt))
                {
                    request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(new StringContent(AdobePdfApi.client_id ), "client_id");
                    multipartContent.Add(new StringContent(AdobePdfApi.client_secret ), "client_secret");
                    multipartContent.Add(new StringContent(AdobePdfApi.certificado_key_filetext), "jwt_token");
                    request.Content = multipartContent;

                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        respuesta = response.Content.ReadAsStringAsync().Result;
                        adobePdfApiTokenModel = JsonConvert.DeserializeObject<AdobePdfApiTokenModel>(respuesta);
                    }
                }
            }
            return adobePdfApiTokenModel;
        }
    }
}
