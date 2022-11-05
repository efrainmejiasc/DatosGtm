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
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), AdobePdfApi.urlAdobePdfApiToken))
                {
                    request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(new StringContent(AdobePdfApi .client_id ), "client_id");
                    multipartContent.Add(new StringContent(AdobePdfApi.client_secret ), "client_secret");
                    multipartContent.Add(new StringContent("eyJhbGciOiJSUzI1NiJ9.eyJleHAiOjE2Njc3NjE5MzIsImlzcyI6IkE2QTgxRjc1NjM2MTUzMUEwQTQ5NUM1NUBBZG9iZU9yZyIsInN1YiI6IkE0QzYxRjRDNjM2MTVFNzEwQTQ5NUUyN0B0ZWNoYWNjdC5hZG9iZS5jb20iLCJodHRwczovL2ltcy1uYTEuYWRvYmVsb2dpbi5jb20vcy9lbnRfZG9jdW1lbnRjbG91ZF9zZGsiOnRydWUsImF1ZCI6Imh0dHBzOi8vaW1zLW5hMS5hZG9iZWxvZ2luLmNvbS9jL2JhOWU1YTg4NWEzNDRjZjFhYzgzZmMyMDM2ZjhlOWFmIn0.sTQLF_JzZ0LwvApjJ-183yfiyhZP4dFF4Ek4MxM6c-uKuLn1sb0_ppk_sFmbuofGvd1UH7_X6W-RkUUjdjMv2URc8GcLR4SYJPRMcSpl7E-0HcWqpNWMavRhvkyUdtcLAq5DyTv_zKoLIwFCDj0c3VDTnntXaVEpvAqGp5ImlFTryY5tU2SNZTV2Q_xeo3bfUb-3J-xwmH3Zn008UhPpRISNfNYDBdsD84mQGS8upK0AfL2xaoGt8kvBChKWrQ48WYAtvGhE1DhlsqmbUu5KkqyU2VTadh3ROKWEpx3-HNlYhewumsaX0IN-UT-C0_RL15EMiExI7S6sep-xmF25LA"), "jwt_token");
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
