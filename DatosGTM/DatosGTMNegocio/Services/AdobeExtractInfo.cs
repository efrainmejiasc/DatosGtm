using Adobe.PDFServicesSDK.auth;
using Adobe.PDFServicesSDK.exception;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.options.extractpdf;
using Adobe.PDFServicesSDK.pdfops;
using Adobe.PDFServicesSDK;
using log4net.Config;
using log4net.Repository;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DatosGTMNegocio.DTOs;
using DatosGTMModelo.DataModel;
using DatosGTMNegocio.Helpers;

namespace DatosGTMNegocio.Services
{
    public class AdobeExtractInfo
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdobeExtractInfo));
        public static ParametrosModel ExtractInfo(string pathCredenciales, string pathReadFile, string pathFileSave, string identificador, string logPath, string indice ="")
        {
            var respuesta = new ParametrosModel();
            respuesta.Estado = false;
            var partesPath = pathReadFile.Split("\\");
            ConfigureLogging();

            try
            {
                ClientConfig clientConfig = ClientConfig.ConfigBuilder().FromFile(pathCredenciales).Build();
                Credentials credentials = Credentials.ServiceAccountCredentialsBuilder().FromFile(pathCredenciales).Build();
                Adobe.PDFServicesSDK.ExecutionContext executionContext = Adobe.PDFServicesSDK.ExecutionContext.Create(credentials);
                ExtractPDFOperation extractPdfOperation = ExtractPDFOperation.CreateNew();
                var pathFromFile = FileRef.CreateFromLocalFile(pathReadFile);
                FileRef sourceFileRef = pathFromFile;
                extractPdfOperation.SetInputFile(sourceFileRef);
                ExtractPDFOptions extractPdfOptions = ExtractPDFOptions.ExtractPDFOptionsBuilder()
                    .AddElementsToExtract(new List<ExtractElementType>(new[] { ExtractElementType.TEXT, ExtractElementType.TABLES }))
                    .Build();
                extractPdfOperation.SetOptions(extractPdfOptions);

                indice = string.IsNullOrEmpty(indice) ? indice : "_" + indice;
                var nombreArchivo = ("pdfResult_" + identificador +  indice + "_.zip");
                pathFileSave = pathFileSave + nombreArchivo;
                FileRef result = extractPdfOperation.Execute(executionContext);
                result.SaveAs(pathFileSave);

                respuesta.Estado = true;
                respuesta.PathArchivo = pathFileSave;
                respuesta.NombreArchivo = nombreArchivo;
                respuesta.Mensaje = "Transaccion Exitosa";
            }
            catch (ServiceUsageException ex)
            {
                Helper.WriteFileLog(logPath, ex.ToString());
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
            catch (ServiceApiException ex)
            {
                Helper.WriteFileLog(logPath, ex.ToString());
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
            catch (SDKException ex)
            {
                Helper.WriteFileLog(logPath, ex.ToString());
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
            catch (IOException ex)
            {
                Helper.WriteFileLog(logPath, ex.ToString());
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                Helper.WriteFileLog(logPath, ex.ToString());
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        static void ConfigureLogging()
        {
            var ensamblado = Assembly.GetEntryAssembly();
            ILoggerRepository logRepository = LogManager.GetRepository(ensamblado);
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
    }
}
