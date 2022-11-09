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

namespace DatosGTMNegocio.Services
{
    public class AdobeExtractInfo
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdobeExtractInfo));
        public static ParametrosModel ExtractInfo (string pathCredenciales, string pathReadFile, string pathFileSave)
        {
            var respuesta = new ParametrosModel();
            respuesta.Estado = false;
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

                var completeName = (DateTime.Now.ToString().Replace("/", "") + DateTime.Now.Millisecond.ToString().Replace(" ", "")).Replace (":", "") + "_";
                pathFileSave = (pathFileSave + "pdfResult_" + completeName + ".zip").Replace(" ", "");

                FileRef result = extractPdfOperation.Execute(executionContext);
                result.SaveAs(pathFileSave);

                respuesta.Estado = true;
                respuesta.PathArchivo= pathFileSave;
                respuesta.Mensaje = "Transaccion Exitosa";
            }
            catch (ServiceUsageException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
            catch (ServiceApiException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
            catch (SDKException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
            catch (IOException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                log.Error("Exception encountered while executing operation", ex);
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
