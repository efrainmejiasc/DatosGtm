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
        public async Task<string> ObtenerJWTAsync(string urlAdobePdfApi)
        {
            var respuesta = string.Empty;
            HttpClient client = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,urlAdobePdfApi);
            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("client_id", AdobePdfApi.client_id));
            formData.Add(new KeyValuePair<string, string>("client_secret", AdobePdfApi.client_secret));
            formData.Add(new KeyValuePair<string, string>("jwt_token", AdobePdfApi .certificado_key_filetext ));
             request.Content = new FormUrlEncodedContent(formData);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                respuesta = response.Content.ReadAsStringAsync().Result;
                //token = JsonConvert.DeserializeObject<Token>(cadenaToken);
            }
            else
                respuesta = response.StatusCode.ToString();

            return respuesta;
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
