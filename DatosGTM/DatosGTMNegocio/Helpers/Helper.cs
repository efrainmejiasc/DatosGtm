﻿using Jose;
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

        public static string GetKeyCertificado(string path)
        {
            Dictionary<object, object> test = new Dictionary<object, object>();
            test.Add("exp", DateTimeOffset.Now.ToUnixTimeSeconds() + 600);
            test.Add("iss", AdobePdfApi.organization_id );
            test.Add("sub", AdobePdfApi.account_id );
            test.Add("aud", AdobePdfApi.urlAudience  + AdobePdfApi .client_id );
            string[] scopes = AdobePdfApi.metascope .Split(',');

            foreach (string scope in scopes)
            {
                test.Add(scope, true);
            }

            X509Certificate2 cert = new X509Certificate2(path, AdobePdfApi .passcertificado );
            string token = Jose.JWT.Encode(test, cert.GetRSAPrivateKey(), JwsAlgorithm.RS256);

            return token;
        }
    }
}
