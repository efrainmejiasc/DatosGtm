using Adobe.PDFServicesSDK.auth;
using Adobe.PDFServicesSDK.exception;
using Adobe.PDFServicesSDK.io;
using Adobe.PDFServicesSDK.options.extractpdf;
using Adobe.PDFServicesSDK.pdfops;
using Adobe.PDFServicesSDK;
using DatosGTMNegocio.DTOs;
using log4net.Config;
using log4net.Repository;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DatosGTMNegocio.Helpers;

namespace DatosGTMNegocio.Services
{
    public class AdobeSplitFile
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdobeExtractInfo));
        public static ParametrosModel SplitFile(string pathCredenciales, string pathReadFile,string identificador)
        {
            var respuesta = new ParametrosModel();
            respuesta.Estado = false;
            ConfigureLogging();
            try
            {
                ClientConfig clientConfig = ClientConfig.ConfigBuilder().FromFile(pathCredenciales).Build();

                Credentials credentials = Credentials.ServiceAccountCredentialsBuilder().FromFile(pathCredenciales).Build();

                Adobe.PDFServicesSDK.ExecutionContext executionContext = Adobe.PDFServicesSDK.ExecutionContext.Create(credentials);
                SplitPDFOperation splitPDFOperation = SplitPDFOperation.CreateNew();

                var pathFromFile = FileRef.CreateFromLocalFile(pathReadFile);
                FileRef sourceFileRef = pathFromFile;
                splitPDFOperation.SetInput(sourceFileRef);

                splitPDFOperation.SetPageCount(20);

                List<FileRef> result = splitPDFOperation.Execute(executionContext);

                var partesPath = pathReadFile.Split("\\");
                var nombreArchivo = partesPath[partesPath.Length - 1];
                var pathSaveFile = pathReadFile.Replace(nombreArchivo, "");
                respuesta.PathFile = new List<string>();
                int index = 0;
                var path = string.Empty;     
                foreach (FileRef fileRef in result)
                {
                    path = pathSaveFile + nombreArchivo + "_" + index.ToString() + "_.pdf";
                    fileRef.SaveAs(path);
                    respuesta.PathFile.Add(path);
                    index++;
                }

                respuesta.Estado = true;
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
