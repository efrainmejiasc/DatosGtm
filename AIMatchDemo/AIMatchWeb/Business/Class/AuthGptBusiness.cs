using AIMatchWeb.Business.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Net.Http.Headers;
using AIMatchWeb.Models;
using System.Collections.Generic;

namespace AIMatchWeb.Business.Class
{
    public class AuthGptBusiness: IAuthGptBusiness
    {
        public async Task<TOutput> PostHttpRequestAuthApiGpt<TInput, TOutput>(TInput model) where TOutput : CommonPropertyResponseDto, new()
        {
            var url = "https://api.openai.com/v1/chat/completions";
            var responseModel = new TOutput();
            var respuesta = string.Empty;
            var content = JsonConvert.SerializeObject(model);

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "apiKey");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Crear contenido de la solicitud
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    respuesta = await response.Content.ReadAsStringAsync();
                    responseModel = JsonConvert.DeserializeObject<TOutput>(respuesta);
                    responseModel.httpCode = Convert.ToInt32(response.StatusCode);
                    responseModel.httpStatus = true;
                }
                else
                {
                    respuesta = await response.Content.ReadAsStringAsync();
                    responseModel = JsonConvert.DeserializeObject<TOutput>(respuesta);
                    responseModel.httpCode = Convert.ToInt32(response.StatusCode);
                    responseModel.httpStatus = true;
                }
            }

            return responseModel;
        }

        public async Task<string> PostHttpRequestAuthApiGpt(AuthGptRequestDto model)
        {
            var url = "https://api.openai.com/v1/chat/completions";
            var respuesta = string.Empty;

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "apiKey");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var textPlain = JsonConvert.SerializeObject(model);
                // Crear contenido de la solicitud
                var httpContent = new StringContent(textPlain , Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    respuesta = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    respuesta = await response.Content.ReadAsStringAsync();
                }
            }

            return respuesta;
        }

    }

}

