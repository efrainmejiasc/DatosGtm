using DatosGTMlWeb.Models;
using DatosGTMNegocio.Helpers;
using DatosGTMNegocio.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DatosGTMWeb.Controllers
{
    public class AboutController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IRequestService _requestService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AboutController(ILogger<HomeController> logger, IHttpContextAccessor httpContext, IRequestService requestService, IWebHostEnvironment webHostEnvironment)
        {
            this._logger = logger;
            this._httpContext = httpContext;
            this._requestService = requestService;
            this._webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RequestAsync()
        {
            var respuesta = new RespuestaModel();
            respuesta.Estado = false;
            try
            {
                AdobePdfApi .certificado_key_filetext = Helper.GetKeyCertificado(this._webHostEnvironment.WebRootPath + AdobePdfApi.certificado );
                var adobePdfApiTokenModel = await this._requestService.ObtenerJWTAsync(AdobePdfApi.urlAdobePdfApiToken);
                respuesta.Mensaje = adobePdfApiTokenModel.token_type == "bearer" ? "Token Obtenido Correctamente" : "Fallo Obtener el Token";
                respuesta.Estado = adobePdfApiTokenModel.token_type == "bearer" ? true : false; ;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Json(respuesta);
        }


        [HttpPost]
        //[AllowAnonymous]
        //[Route("api/UploadFileMethod")]
        public RespuestaModel UploadFileMethod(IFormFile file)
        {
            var respuesta = new RespuestaModel();
            respuesta.Estado = false;
            if (file != null)
            {
                try
                {
                    var p = file.FileName.Replace("_", "").Replace("/", "").Split('.');
                    var name = p[0] + "_" + DateTime.Now.ToString().Replace("/","") + DateTime.Now.Millisecond  + ".pdf".Replace(" ", "");
                    name = name.Replace(":", "");
                    if (file.FileName.ToUpper().Contains(".PDF"))
                    {
                        string path = this._webHostEnvironment.WebRootPath+ AdobePdfApi.pdf_files + name;
                        var stream = System.IO.File.Create(path);
                        file.CopyTo(stream);
                        stream.Dispose();
                    }
                    else
                    {
                        respuesta.Mensaje  = "El archivo debe ser de de extencion .pdf";
                        return respuesta;
                    }

                }
                catch (Exception ex)
                {
                    respuesta.Mensaje = ex.Message;
                    return respuesta;
                }
            }
            else
            {
                respuesta.Mensaje = "El valor no puede ser nulo";
            }

            respuesta.Mensaje = "Archivo cargado correctamente";
            respuesta.Estado = true;
            return respuesta;
        }

    }
}
