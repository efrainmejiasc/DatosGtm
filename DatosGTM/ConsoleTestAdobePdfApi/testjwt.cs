using Jose;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestAdobePdfApi
{
    public class testjwt
    {
        private JwtBearerTokenSettings _jwtBearerTokenSettings = new JwtBearerTokenSettings();
        const string CLIENT_ID = "ba9e5a885a344cf1ac83fc2036f8e9af";
        const string CLIENT_SECRET = "p8e-M4YAl1sWp3PAGw8NacDNmqZAeX3I_1Ex";
        const string TECH_ACC_ID = "A4C61F4C63615E710A495E27@techacct.adobe.com";
        const string ORG_ID = "A6A81F756361531A0A495C55@AdobeOrg";
        const string METASCOPES = "https://ims-na1.adobelogin.com/s/ent_documentcloud_sdk";

        public string  GenerarToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var Jwt = new JwtSecurityToken();
            var handler = new JwtSecurityTokenHandler();

            // reading the content of a private key PEM file, PKCS8 encoded 
            string privateKeyPem = File.ReadAllText("C:\\Users\\EfrainMejiasC\\Documents\\GitHub\\DatosGtm\\DatosGTM\\DatosGTMWeb\\wwwroot\\adobe_pdf_api\\private.key");

            // keeping only the payload of the key 
            privateKeyPem = privateKeyPem.Replace("-----BEGIN PRIVATE KEY-----", "");
            privateKeyPem = privateKeyPem.Replace("-----END PRIVATE KEY-----", "");

            byte[] privateKeyRaw = Convert.FromBase64String(privateKeyPem);

            // creating the RSA key 
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.ImportPkcs8PrivateKey(new ReadOnlySpan<byte>(privateKeyRaw), out _);
            RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(provider);

            try
            {
                string[] scopes = METASCOPES.Split(',');
                var sub = string.Empty;
                foreach (string scope in scopes)
                {
                    sub= scope + ": " + "true";
                }
                var aud = "https://ims-na1.adobelogin.com/c/" + CLIENT_ID;

                X509Certificate2 cert = new X509Certificate2("C:\\Users\\EfrainMejiasC\\Documents\\GitHub\\DatosGtm\\DatosGTM\\DatosGTMWeb\\wwwroot\\adobe_pdf_api\\mycert.pfx", "1234Fabrizio+*");
                var Claims = new Claim[]
                {
                   new Claim("exp", (DateTimeOffset.Now.AddDays (1).ToUnixTimeSeconds() + 600).ToString ()),
                   new Claim("iss",ORG_ID),
                   new Claim("sub",TECH_ACC_ID),
                   new Claim("aud",aud)
                };

                Jwt = new JwtSecurityToken(claims:Claims,signingCredentials: new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256));

                //var signingkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is secret phrase"));
                // var SigningCredntials = new SigningCredentials(signingkey, SecurityAlgorithms.HmacSha256);
                //Jwt = new JwtSecurityToken(claims: Claims, signingCredentials: SigningCredntials)

   
            

            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }


            //return new AccessToken(tokenString, exp, true);
            //return new JwtSecurityTokenHandler().WriteToken(Jwt);
           return  handler.WriteToken(Jwt);

        }
    }

    public sealed class AccessToken
    {
        public string Token { get; }
        public DateTime Expires { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Cadena del token</param>
        /// <param name="expiresIn">Tiempo de expiracion</param>
        public AccessToken(string token, int expiresIn, bool UsExterno = false)
        {
            Token = token;
            Expires = UsExterno ? System.DateTime.Now.AddMinutes(expiresIn) : System.DateTime.Now.AddDays(expiresIn);
        }
    }

    public class JwtBearerTokenSettings
    {
        public string exp {get;set; }
        public string iss { get; set; }
        public string sub { get; set; }
        public string aud { get; set; }

        /// <summary>
        /// Llave de encriptacion
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Solicitantes
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gerador de token
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Tiempo de expiracion en dias
        /// </summary>
        public int ExpiryTimeInDays { get; set; }

        /// <summary>
        /// Tiempo de expiracion en minutos
        /// </summary>
        public int ExpiryTimeInMinutes { get; set; }
    }
}
