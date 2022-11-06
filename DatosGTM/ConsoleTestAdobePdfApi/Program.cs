//Copy these details from console.adobe.io integration
using ConsoleTestAdobePdfApi;
using Jose;
using RestSharp;
using System.IO;
using System.Security.Cryptography.X509Certificates;


const string CLIENT_ID = "95a27340b5314f05ad0d7e8204297c58";        
const string CLIENT_SECRET = "p8e-274o2RZeQRlv3_afTrOoT76yBV-rFpY7";         
const string TECH_ACC_ID = "2454205A6366CFB40A495F95@techacct.adobe.com";    
const string ORG_ID = "2CAD1EA96366BFFF0A495E88@AdobeOrg";               
const string METASCOPES = "https://ims-na1.adobelogin.com/s/ent_documentcloud_sdk";



Dictionary<object, object> test = new Dictionary<object, object>();
test.Add("exp", DateTimeOffset.Now.ToUnixTimeSeconds() + 600);
test.Add("iss", ORG_ID);
test.Add("sub", TECH_ACC_ID);
string[] scopes = METASCOPES.Split(',');

foreach (string scope in scopes)
{
    test.Add(scope, true);
}
test.Add("aud", "https:ims-na1.adobelogin.com/c/" + CLIENT_ID);



//You must have generated a certificate using below command and uploaded in the console.adobe.io integration
//openssl req -x509 -sha256 -nodes -days 365 -newkey rsa:2048 -keyout private.key -out certificate_pub.crt

//Create a pfx file using the private key and public certificate generated from the above step
//openssl pkcs12 -export -out mycert.pfx -inkey private.key -in certificate_pub.crt
//Enter export password: password

var t = new testjwt();
var token = t.GenerarToken();
//Console.ReadKey();
try
{
    X509Certificate2 cert = new X509Certificate2("C:\\Users\\EfrainMejiasC\\Documents\\GitHub\\DatosGtm\\DatosGTM\\DatosGTMWeb\\wwwroot\\adobe_pdf_api\\mycert.pfx", "1234Fabrizio+*");

    //string token = Jose.JWT.Encode(test, cert.GetRSAPrivateKey(), JwsAlgorithm.RS256);

    Console.WriteLine(token); //Intermediate JWT Token

    //token = "";
    var respuesta = string.Empty;
    using (var httpClient = new HttpClient())
    {
        using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://ims-na1.adobelogin.com/ims/exchange/jwt/"))
        {
            request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent(CLIENT_ID), "client_id");
            multipartContent.Add(new StringContent(CLIENT_SECRET), "client_secret");
            multipartContent.Add(new StringContent(token), "jwt_token");
            request.Content = multipartContent;

            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                respuesta = response.Content.ReadAsStringAsync().Result;
            }
        }
    }



    Console.ReadKey();
}
catch (Exception e)
{
    Console.WriteLine("An error has occured: " + e.Message);
}
