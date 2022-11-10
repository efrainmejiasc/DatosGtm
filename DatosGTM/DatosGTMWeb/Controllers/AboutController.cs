using DatosGTMlWeb.Models;
using DatosGTMModelo.DataModel;
using DatosGTMNegocio.DTOs;
using DatosGTMNegocio.Helpers;
using DatosGTMNegocio.IServices;
using DatosGTMNegocio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using System;
using System.IO.Compression;
using System.Runtime.CompilerServices;

namespace DatosGTMWeb.Controllers
{
    public class AboutController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IRequestService _requestService;
        private readonly IReadFileService _readFileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITerceroService  _terceroService;

        public AboutController(ILogger<HomeController> logger, IHttpContextAccessor httpContext, IRequestService requestService, 
            IWebHostEnvironment webHostEnvironment, IReadFileService readFileService, ITerceroService terceroService)
        {
            this._logger = logger;
            this._httpContext = httpContext;
            this._requestService = requestService;
            this._readFileService = readFileService;
            this._webHostEnvironment = webHostEnvironment;
            this._terceroService = terceroService;
        }
        public IActionResult Index()
        {
            return View();
        }

   

        [HttpPost]
        //[AllowAnonymous]
        //[Route("api/UploadFileMethod")]
        public IActionResult UploadFileMethod(IFormFile file)
        {
            var respuesta = new ParametrosModel();
            respuesta.Estado = false;
            if (file != null)
            {
                try
                {
                    var p = file.FileName.Replace("_", "").Replace("/", "").Split('.');
                    var name = p[0] + "_" + DateTime.Now.ToString().Replace("/","") + DateTime.Now.Millisecond  + ".pdf".Replace(" ", "");
                    name = name.Replace(":", "").Replace (" ","");
                    if (file.FileName.ToUpper().Contains(".PDF"))
                    {
                        var pathReadFile = this._webHostEnvironment.WebRootPath + AdobePdfApi.pdf_filesToWrite + name;
                        var stream = System.IO.File.Create(pathReadFile);
                        file.CopyTo(stream);
                        stream.Dispose();

                        var pathCredenciales = this._webHostEnvironment.ContentRootPath + AdobePdfApi.file_credentials;
                        var pathFileSave = this._webHostEnvironment.WebRootPath + AdobePdfApi.pdf_filesExtract;
                        var pathReadJson = this._webHostEnvironment.WebRootPath + AdobePdfApi.pdf_filesToRead;
                        respuesta= AdobeExtractInfo.ExtractInfo(pathCredenciales, pathReadFile, pathFileSave);
                        if (respuesta.Estado)
                        {
                            ZipFile.ExtractToDirectory(respuesta.PathArchivo, pathReadJson, true);
                            respuesta.Tercero = this._readFileService.LeerArchivo(pathReadJson + "structuredData.json");
                        }
                        else
                        {
                            return Json(respuesta);
                        }   
                    }
                    else
                    {
                        respuesta.Mensaje  = "El archivo debe ser de de extencion .pdf";
                        return Json(respuesta);
                    }

                }
                catch (Exception ex)
                {
                    respuesta.Mensaje = ex.Message;
                    return Json(respuesta);
                }
            }
            else
            {
                respuesta.Mensaje = "El valor no puede ser nulo";
            }

            respuesta.Mensaje = "Archivo cargado correctamente";
            respuesta.Estado = true;
            return Json(respuesta);
        }


        #region PRUEBAS _CONCEPTUALES 


        [HttpGet]
        public async Task<IActionResult> RequestAsync()
        {
            var respuesta = new RespuestaModel();

            try
            {
                AdobePdfApi.certificado_key_filetext = Helper.GetKeyCertificado(this._webHostEnvironment.WebRootPath + AdobePdfApi.certificado);
                var adobePdfApiTokenModel = await this._requestService.ObtenerJWTAsync(AdobePdfApi.urlAdobePdfApiToken);
                respuesta.Mensaje = adobePdfApiTokenModel.token_type == "bearer" ? "Token Obtenido Correctamente" : "Fallo Obtener el Token";
                respuesta.Estado = adobePdfApiTokenModel.token_type == "bearer" ? true : false; ;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                respuesta.Estado = false;
            }

            return Json(respuesta);
        }


        [HttpPost]
        public IActionResult ExtraerInfo()
        {
            var respuesta = new ParametrosModel();
            try
            {
                var nameFile = "Agentes30.pdf";
                var pathCredenciales = this._webHostEnvironment.ContentRootPath + AdobePdfApi.file_credentials;
                var pathReadFile = this._webHostEnvironment.WebRootPath + AdobePdfApi.pdf_filesToWrite + nameFile;
                var pathFileSave = this._webHostEnvironment.WebRootPath + AdobePdfApi.pdf_filesExtract;
                var pathReadJson = this._webHostEnvironment.WebRootPath + AdobePdfApi.pdf_filesToRead;
                respuesta = AdobeExtractInfo.ExtractInfo(pathCredenciales, pathReadFile, pathFileSave);
                if (respuesta.Estado)
                {
                    ZipFile.ExtractToDirectory(respuesta.PathArchivo, pathReadJson, true);
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                return Json(respuesta);
            }

            return Json(respuesta);
        }




        [HttpPost]
        public IActionResult LeerArchivo()
        {
            var respuesta = new RespuestaModel();
            respuesta.Estado = false;
            var infoPdf = new ExtractPdfInfoModel();
            var elements = new List<Element>();
            var infoOrdenada = new InformacionPdfOrdenada();
            try
            {
                var texto = Helper.ReadFile(this._webHostEnvironment.WebRootPath + AdobePdfApi.pdf_filesToRead + "structuredData.json");
                if (!string.IsNullOrEmpty(texto))
                {
                    infoPdf = JsonConvert.DeserializeObject<ExtractPdfInfoModel>(texto);
                    elements = infoPdf.elements.Where(x => x.Text != null && (x.Text.Trim() != "No." && x.Text.Trim() != "NIT" && x.Text.Trim() != "NOMBRE" && x.Text.Trim() != "FECHA INICIO" && !x.Text.Contains("LISTADO DE AGENTES DE RETENCIÓN DEL IVA"))).ToList();
                    infoOrdenada.Nombre = elements.Where(x => x.Path.Substring(x.Path.Length - 4, 4).Trim() == "TD/P").Select(x => x.Text).ToList();
                    infoOrdenada.Nit = elements.Where(x => x.Path.Substring(x.Path.Length - 7, 7).Trim() == "TD[2]/P").Select(x => x.Text).ToList();
                    infoOrdenada.Nombre = elements.Where(x => x.Path.Substring(x.Path.Length - 7, 7).Trim() == "TD[3]/P").Select(x => x.Text.Replace("\"", "")).ToList();
                    infoOrdenada.FechaInicio = elements.Where(x => x.Path.Substring(x.Path.Length - 7, 7).Trim() == "TD[4]/P").Select(x => x.Text).ToList();
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                return Json(respuesta);
            }



            respuesta.Mensaje = "Datos Obtenidos correctamente";
            respuesta.Estado = true;
            return Json(respuesta);
        }


        [HttpGet]
        public IActionResult GetTerceros()
        {
            var terceros = new List<Tercero>();

            try
            {
                terceros = this._terceroService.GetTerceros();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(terceros);
        }

        #endregion


    }
}
