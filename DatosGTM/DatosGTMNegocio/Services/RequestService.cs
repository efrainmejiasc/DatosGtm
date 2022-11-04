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
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://ims-na1.adobelogin.com/ims/exchange/jwt/"))
                {
                    request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(new StringContent(AdobePdfApi .client_id ), "client_id");
                    multipartContent.Add(new StringContent(AdobePdfApi.client_secret ), "client_secret");
                    multipartContent.Add(new StringContent("eyJhbGciOiJSUzI1NiJ9.eyJleHAiOjE2Njc2NTI5NzksImlzcyI6IkE2QTgxRjc1NjM2MTUzMUEwQTQ5NUM1NUBBZG9iZU9yZyIsInN1YiI6IkE0QzYxRjRDNjM2MTVFNzEwQTQ5NUUyN0B0ZWNoYWNjdC5hZG9iZS5jb20iLCJodHRwczovL2ltcy1uYTEuYWRvYmVsb2dpbi5jb20vcy9lbnRfZG9jdW1lbnRjbG91ZF9zZGsiOnRydWUsImF1ZCI6Imh0dHBzOi8vaW1zLW5hMS5hZG9iZWxvZ2luLmNvbS9jL2JhOWU1YTg4NWEzNDRjZjFhYzgzZmMyMDM2ZjhlOWFmIn0.c5wS-J30h-yRJZavHWQmDMzicIB6VqZbYKGN4vFXiSX2ebXUJNKDrrbp6JdPh0h4L4tUiLx-aRAduRVZcZEnqqIZVc6bFyl6S1JNG1O6DdfyV4NQoPR6MHV6Ix6kFH5Db1-GKYjKqaDafTB4QpxPZCa75SR8LucmJz5rvyPYDpeCvRiq7kHr2CaUKzumE4iPscJj8WzS5KS9oUFEY4XtPCH0F6h0NGW0avqYj9HrOL4JZmtUSgKtlQQtw8f0TPmwb43y-lVcJlKYSdMafouFRv6CF4wn8dYwnVNpNXOdhnbePeAF0nysL_42nfwOkAPv7GTDDleOybbzu-RbA3cfBQ"), "jwt_token");
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

        public async Task<string> ObtenerJWTPostAsync(string urlAdobePdfApi)
        {

            var jsonDocumento = JsonConvert .SerializeObject (SetParametro());
            var respuesta = string.Empty;
            HttpClient client = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
            HttpResponseMessage response = await client.PostAsync(urlAdobePdfApi , new StringContent(jsonDocumento, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                respuesta = response.Content.ReadAsStringAsync().Result;
            }
            else
                respuesta = response.StatusCode.ToString();

            return respuesta;
        }

        private ParametroModel SetParametro()
        {
            var parametro = new ParametroModel()
            {
                client_id = AdobePdfApi.client_id,
                client_secret = AdobePdfApi.client_secret,
                jwt_token = AdobePdfApi.certificado_key_filetext
            };

            return parametro;
        }
    }
}
