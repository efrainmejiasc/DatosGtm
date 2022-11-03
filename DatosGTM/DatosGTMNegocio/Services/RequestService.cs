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
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,urlAdobePdfApi);
            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("client_id", AdobePdfApi.client_id));
            formData.Add(new KeyValuePair<string, string>("client_secret", AdobePdfApi.client_secret));
            formData.Add(new KeyValuePair<string, string>("grant_type", "grant_type"));
            formData.Add(new KeyValuePair<string, string>("organization_id", AdobePdfApi .organization_id));
            formData.Add(new KeyValuePair<string, string>("account_id", AdobePdfApi .account_id ));
            formData.Add(new KeyValuePair<string, string>("private_key_file",AdobePdfApi.private_key_filetext));
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
    }
}
