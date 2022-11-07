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

namespace DatosGTMNegocio.Services
{
    public class AdobeExtractInfo
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdobeExtractInfo));
        public static void ExtractInfo (string pathCredenciales, string fileReadPath, string fileSave)
        {
            ConfigureLogging();
            try
            {
                var pathTime = Directory.GetCurrentDirectory() + "/pdfservices-api-credentials.json";
                ClientConfig clientConfig = ClientConfig.ConfigBuilder().FromFile(pathTime).Build();

                Credentials credentials = Credentials.ServiceAccountCredentialsBuilder().FromFile(pathCredenciales).Build();

                Adobe.PDFServicesSDK.ExecutionContext executionContext = Adobe.PDFServicesSDK.ExecutionContext.Create(credentials);
                ExtractPDFOperation extractPdfOperation = ExtractPDFOperation.CreateNew();

                var pathFromFile = FileRef.CreateFromLocalFile(fileReadPath);
                FileRef sourceFileRef = pathFromFile;
                extractPdfOperation.SetInputFile(sourceFileRef);

                ExtractPDFOptions extractPdfOptions = ExtractPDFOptions.ExtractPDFOptionsBuilder()
                    .AddElementsToExtract(new List<ExtractElementType>(new[] { ExtractElementType.TEXT, ExtractElementType.TABLES }))
                    .Build();
                extractPdfOperation.SetOptions(extractPdfOptions);

                FileRef result = extractPdfOperation.Execute(executionContext);

                result.SaveAs(fileSave);
            }
            catch (ServiceUsageException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (ServiceApiException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (SDKException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (IOException ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
            catch (Exception ex)
            {
                log.Error("Exception encountered while executing operation", ex);
            }
        }

        static void ConfigureLogging()
        {
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
    }
}
