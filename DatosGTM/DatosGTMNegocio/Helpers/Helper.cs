using Jose;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DatosGTMNegocio.Helpers
{
    public class Helper
    {
        public static string EnCodeBase64(string cadena)
        {
            var bytes = Encoding.UTF8.GetBytes(cadena);
            return Convert.ToBase64String(bytes);
        }

        public static string DecodeBase64(string cadena)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(cadena);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static Guid CreateUniqueidentifier()
        {
            Guid g = CreateGuid();
            if (g == Guid.Empty)
                g = CreateUniqueidentifier();

            return g;
        }

        private static Guid CreateGuid()
        {
            return Guid.NewGuid();
        }
        public static string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }

        public static bool CreateDirectory (string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return true;
            }

            return false;
        }

        public static void CreateFolder(string rootPath,string identificador)
        {
            CreateDirectory (rootPath + AdobePdfApi.pdf_filesToWrite + "F_" + identificador);
            CreateDirectory(rootPath + AdobePdfApi.pdf_filesExtract + "F_" + identificador);
            CreateDirectory(rootPath + AdobePdfApi.pdf_filesToRead + "F_" + identificador);
        }

        public static bool WriteFileLog( string logPath, string excepcion)
        {
            var resultado = false;
            var strLog = "FECHA: " + DateTime.Now.ToString()+ " EXCEPCION: " + excepcion  + Environment.NewLine;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logPath, true))
            {
                file.WriteLine(strLog);
                resultado = true;
            }

            return resultado;
        }

        public static string GetKeyCertificado(string path)
        {
            Dictionary<object, object> test = new Dictionary<object, object>();
            test.Add("exp", DateTimeOffset.Now.AddDays(1).ToUnixTimeSeconds() + 600);
            test.Add("iss", AdobePdfApi.organization_id );
            test.Add("sub", AdobePdfApi.account_id );
            string[] scopes = AdobePdfApi.metascope .Split(',');

            foreach (string scope in scopes)
            {
                test.Add(scope, true);
            }
            test.Add("aud", AdobePdfApi.urlAudience + AdobePdfApi.client_id);

            X509Certificate2 cert = new X509Certificate2(path, AdobePdfApi.passcertificado );
            string token = Jose.JWT.Encode(test, cert.GetRSAPrivateKey(), JwsAlgorithm.RS256);

            return token;
        }
    }
}
