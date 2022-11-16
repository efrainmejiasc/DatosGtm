using DatosGTMlWeb.Models;
using DatosGTMModelo.DataModel;
using DatosGTMNegocio.DTOs;
using DatosGTMNegocio.Helpers;
using DatosGTMNegocio.IServices;
using DatosGTMNegocio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf;
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
        private readonly IReadFileService _readFileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITerceroService  _terceroService;
        private Usuario usuario ;

        public AboutController(ILogger<HomeController> logger, IHttpContextAccessor httpContext, 
                               IWebHostEnvironment webHostEnvironment, IReadFileService readFileService, ITerceroService terceroService)
        {
            this._logger = logger;
            this._httpContext = httpContext;
            this._readFileService = readFileService;
            this._webHostEnvironment = webHostEnvironment;
            this._terceroService = terceroService;

            if (!string.IsNullOrEmpty(_httpContext.HttpContext.Session.GetString("UsuarioLogin")))
                this.usuario = JsonConvert.DeserializeObject<Usuario>(_httpContext.HttpContext.Session.GetString("UsuarioLogin"));

                
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
            var logPath = this._webHostEnvironment.WebRootPath + AdobePdfApi.log_excepcion;
            Helper.WriteFileLog(logPath, "ENTRADA");

            if (file != null)
            {
                try
                {
                    var p = file.FileName.Replace("_", "").Replace("/", "").Replace(" ", "").Replace("-", "").Split('.');
                    var identificador = Helper.CreateUniqueidentifier();
                    var strIdentificador = identificador.ToString().ToUpper();
                    var name = p[0] + "_" + strIdentificador + "_.pdf".Replace(" ", "");

                    if (file.FileName.ToUpper().Contains(".PDF"))
                    {
                        Helper.CreateFolder(this._webHostEnvironment.WebRootPath, strIdentificador);
                        var pathReadFile = this._webHostEnvironment.WebRootPath + AdobePdfApi.pdf_filesToWrite + "F_" + strIdentificador + "\\" + name;
                        var stream = System.IO.File.Create(pathReadFile);
                        file.CopyTo(stream);
                        stream.Dispose();
                        
                        PdfDocument filePdf = PdfReader.Open(pathReadFile, PdfDocumentOpenMode.InformationOnly );
                        var numberPages = filePdf.PageCount;
                        filePdf.Dispose();

                        var pathCredenciales = this._webHostEnvironment.ContentRootPath + AdobePdfApi.file_credentials;
                        var pathFileSave = this._webHostEnvironment.WebRootPath + AdobePdfApi.pdf_filesExtract + "F_" + strIdentificador + "\\";
                        var pathReadJson = this._webHostEnvironment.WebRootPath + AdobePdfApi.pdf_filesToRead + "F_" + strIdentificador + "\\";
                        
                        if (numberPages > AdobePdfApi.split_number_pages)
                        {
                            respuesta = AdobeSplitFile.SplitFile(pathCredenciales, pathReadFile, strIdentificador,logPath);
                            Helper.WriteFileLog(logPath, "SPLIT");
                            if (respuesta.Estado)
                            {
                                var acumuladorTercero  = new List<Tercero>();
                                int indice = 0;
                                foreach (var pathReadFile_ in respuesta.PathFile)
                                {
                                    respuesta = AdobeExtractInfo.ExtractInfo(pathCredenciales, pathReadFile_, pathFileSave, strIdentificador,logPath, indice.ToString ());
                                    Helper.WriteFileLog(logPath, "EXTRACION Nº: " + indice.ToString());
                                    if (respuesta.Estado)
                                    {
                                        ZipFile.ExtractToDirectory(respuesta.PathArchivo, pathReadJson, true);
                                        var t = this._readFileService.LeerArchivo(pathReadJson + "structuredData.json", identificador);
                                        if (t.Count > 0)
                                        {
                                            Helper.WriteFileLog(logPath, "GUARDANDO REGISTROS Nº: " +indice.ToString ());
                                            acumuladorTercero.AddRange(t);
                                        }
                                        indice++;
                                    }
                                    else
                                    {
                                        Helper.WriteFileLog(logPath, respuesta .Mensaje );
                                        return Json(respuesta);
                                    }
                                }
                                respuesta.Tercero = new List<Tercero>();
                                respuesta.Tercero = acumuladorTercero;
                            }
                            else
                            {
                                return Json(respuesta);
                            }
                        }
                        else
                        {
                            respuesta = AdobeExtractInfo.ExtractInfo(pathCredenciales, pathReadFile, pathFileSave, strIdentificador, logPath);
                            if (respuesta.Estado)
                            {
                                ZipFile.ExtractToDirectory(respuesta.PathArchivo, pathReadJson, true);
                                respuesta.Tercero = this._readFileService.LeerArchivo(pathReadJson + "structuredData.json",identificador);
                            }
                            else
                            {
                                return Json(respuesta);
                            }
                        }
                    }
                    else
                    {
                        Helper.WriteFileLog(logPath, "El archivo debe ser de de extencion .pdf");
                        respuesta.Mensaje  = "El archivo debe ser de de extension .pdf";
                        return Json(respuesta);
                    }

                }
                catch (Exception ex)
                {
                    Helper.WriteFileLog(logPath, ex.ToString());
                    respuesta.Mensaje = ex.Message;
                    return Json(respuesta);
                }
            }
            else
            {
                Helper.WriteFileLog(logPath, "El valor no puede ser nulo");
                respuesta.Mensaje = "El valor no puede ser nulo";
            }

            Helper.WriteFileLog(logPath, "Archivo cargado correctamente");
            respuesta.Mensaje = "Archivo cargado correctamente";
            respuesta.Estado = true;
            return Json(respuesta);
        }


        #region PRUEBAS _CONCEPTUALES 

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
