//Copy these details from console.adobe.io integration
using ConsoleTestAdobePdfApi;
using Jose;
using RestSharp;
using System.IO;
using System.Security.Cryptography.X509Certificates;


const string CLIENT_ID = "ba9e5a885a344cf1ac83fc2036f8e9af";        
const string CLIENT_SECRET = "p8e-M4YAl1sWp3PAGw8NacDNmqZAeX3I_1Ex";         
const string TECH_ACC_ID = "A4C61F4C63615E710A495E27@techacct.adobe.com";    
const string ORG_ID = "A6A81F756361531A0A495C55@AdobeOrg";               
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

    token = "eyJhbGciOiJSUzI1NiJ9.eyJleHAiOjE2Njc3NjE5MzIsImlzcyI6IkE2QTgxRjc1NjM2MTUzMUEwQTQ5NUM1NUBBZG9iZU9yZyIsInN1YiI6IkE0QzYxRjRDNjM2MTVFNzEwQTQ5NUUyN0B0ZWNoYWNjdC5hZG9iZS5jb20iLCJodHRwczovL2ltcy1uYTEuYWRvYmVsb2dpbi5jb20vcy9lbnRfZG9jdW1lbnRjbG91ZF9zZGsiOnRydWUsImF1ZCI6Imh0dHBzOi8vaW1zLW5hMS5hZG9iZWxvZ2luLmNvbS9jL2JhOWU1YTg4NWEzNDRjZjFhYzgzZmMyMDM2ZjhlOWFmIn0.sTQLF_JzZ0LwvApjJ-183yfiyhZP4dFF4Ek4MxM6c-uKuLn1sb0_ppk_sFmbuofGvd1UH7_X6W-RkUUjdjMv2URc8GcLR4SYJPRMcSpl7E-0HcWqpNWMavRhvkyUdtcLAq5DyTv_zKoLIwFCDj0c3VDTnntXaVEpvAqGp5ImlFTryY5tU2SNZTV2Q_xeo3bfUb-3J-xwmH3Zn008UhPpRISNfNYDBdsD84mQGS8upK0AfL2xaoGt8kvBChKWrQ48WYAtvGhE1DhlsqmbUu5KkqyU2VTadh3ROKWEpx3-HNlYhewumsaX0IN-UT-C0_RL15EMiExI7S6sep-xmF25LA";
    var respuesta = string.Empty;
    using (var httpClient = new HttpClient())
    {
        using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://ims-na1.adobelogin.com/ims/exchange/jwt/"))
        {
            
            request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent(CLIENT_ID), "client_id");
            multipartContent.Add(new StringContent(CLIENT_SECRET), "client_secret");
            multipartContent.Add(new StringContent("eyJhbGciOiJSUzI1NiJ9.eyJleHAiOjE2Njc3NjE5MzIsImlzcyI6IkE2QTgxRjc1NjM2MTUzMUEwQTQ5NUM1NUBBZG9iZU9yZyIsInN1YiI6IkE0QzYxRjRDNjM2MTVFNzEwQTQ5NUUyN0B0ZWNoYWNjdC5hZG9iZS5jb20iLCJodHRwczovL2ltcy1uYTEuYWRvYmVsb2dpbi5jb20vcy9lbnRfZG9jdW1lbnRjbG91ZF9zZGsiOnRydWUsImF1ZCI6Imh0dHBzOi8vaW1zLW5hMS5hZG9iZWxvZ2luLmNvbS9jL2JhOWU1YTg4NWEzNDRjZjFhYzgzZmMyMDM2ZjhlOWFmIn0.sTQLF_JzZ0LwvApjJ-183yfiyhZP4dFF4Ek4MxM6c-uKuLn1sb0_ppk_sFmbuofGvd1UH7_X6W-RkUUjdjMv2URc8GcLR4SYJPRMcSpl7E-0HcWqpNWMavRhvkyUdtcLAq5DyTv_zKoLIwFCDj0c3VDTnntXaVEpvAqGp5ImlFTryY5tU2SNZTV2Q_xeo3bfUb-3J-xwmH3Zn008UhPpRISNfNYDBdsD84mQGS8upK0AfL2xaoGt8kvBChKWrQ48WYAtvGhE1DhlsqmbUu5KkqyU2VTadh3ROKWEpx3-HNlYhewumsaX0IN-UT-C0_RL15EMiExI7S6sep-xmF25LA"), "jwt_token");
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
