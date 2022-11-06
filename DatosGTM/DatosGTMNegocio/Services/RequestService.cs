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
                    multipartContent.Add(new StringContent("eyJhbGciOiJSUzI1NiIsIng1dSI6Imltc19uYTEta2V5LWF0LTEuY2VyIiwia2lkIjoiaW1zX25hMS1rZXktYXQtMSIsIml0dCI6ImF0In0.eyJpZCI6IjE2Njc2ODYwNDI0NjRfNWMwNmU4NGItYWU4Ny00YzhmLWEyYzctODczZWVhM2IwMDk4X3VlMSIsInR5cGUiOiJhY2Nlc3NfdG9rZW4iLCJjbGllbnRfaWQiOiI5NWEyNzM0MGI1MzE0ZjA1YWQwZDdlODIwNDI5N2M1OCIsInVzZXJfaWQiOiIyNDU0MjA1QTYzNjZDRkI0MEE0OTVGOTVAdGVjaGFjY3QuYWRvYmUuY29tIiwiYXMiOiJpbXMtbmExIiwiYWFfaWQiOiIyNDU0MjA1QTYzNjZDRkI0MEE0OTVGOTVAdGVjaGFjY3QuYWRvYmUuY29tIiwiY3RwIjowLCJmZyI6Ilc1T0M3SUdQRlBFNUlYVUtHTVFGWUhZQU5FPT09PT09IiwibW9pIjoiYzVmZDYxN2QiLCJleHBpcmVzX2luIjoiODY0MDAwMDAiLCJzY29wZSI6Im9wZW5pZCxEQ0FQSSxBZG9iZUlELGFkZGl0aW9uYWxfaW5mby5vcHRpb25hbEFncmVlbWVudHMiLCJjcmVhdGVkX2F0IjoiMTY2NzY4NjA0MjQ2NCJ9.XnnSJTSO36EszkxE4b6rZpl5-hzQ7lG-7cLsd_FFge0v_3My7C0T21bOqJ0DPjpgH04sltxBYNsoLubuD1yGC6DaQEg8Sw-I6KrhuP9FrV4fFBai10mat1Ft2Z8zLR9phJ2JLPE_LkFW-DhrMmJw8gmtHNJqUazXD-w4XJD9l1ZIfBepLd-2SkfWcEj968xRDJqO6c57HLhqJOClEiaPD_v9ENewaqRgxD5HIB_cekntYDlWhlfQETH3_nozdU-oTvII-Igsj5ICBw3fhnunZT1qrPvuPOKZtgrQLbIBwxiHBs6h9tmtv7tvQ862wkZ9IoI4sYt8xDpcSnEt1SvQ4g"), "jwt_token");
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
