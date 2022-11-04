//Copy these details from console.adobe.io integration
using Jose;
using RestSharp;
using System.Security.Cryptography.X509Certificates;
using System.Web.Helpers;

const string CLIENT_ID = "ba9e5a885a344cf1ac83fc2036f8e9af";        
const string CLIENT_SECRET = "p8e-M4YAl1sWp3PAGw8NacDNmqZAeX3I_1Ex";         
const string TECH_ACC_ID = "A4C61F4C63615E710A495E27@techacct.adobe.com";    
const string ORG_ID = "A6A81F756361531A0A495C55@AdobeOrg";               
const string METASCOPES = "https://ims-na1.adobelogin.com/s/ent_reactor_developer_sdk,https://ims-na1.adobelogin.com/s/ent_reactor_admin_sdk"; 

Dictionary<object, object> test = new Dictionary<object, object>();
test.Add("exp", DateTimeOffset.Now.ToUnixTimeSeconds() + 600);
test.Add("iss", ORG_ID);
test.Add("sub", TECH_ACC_ID);
test.Add("aud", "https://ims-na1.adobelogin.com/c/" + CLIENT_ID);
string[] scopes = METASCOPES.Split(',');

foreach (string scope in scopes)
{
    test.Add(scope, true);
}

//You must have generated a certificate using below command and uploaded in the console.adobe.io integration
//openssl req -x509 -sha256 -nodes -days 365 -newkey rsa:2048 -keyout private.key -out certificate_pub.crt

//Create a pfx file using the private key and public certificate generated from the above step
//openssl pkcs12 -export -out mycert.pfx -inkey private.key -in certificate_pub.crt
//Enter export password: password

try
{
    X509Certificate2 cert = new X509Certificate2("C:\\Program Files\\OpenSSL-Win64\\bin/mycert.pfx", "1234Fabrizio+*");

    string token = Jose.JWT.Encode(test, cert.GetRSAPrivateKey(), JwsAlgorithm.RS256);

    Console.WriteLine(token); //Intermediate JWT Token

    var client = new RestClient("https://ims-na1.adobelogin.com/ims/authorize/v2");

    var request = new RestRequest(Method.Post.ToString());
    request.AddHeader("cache-control", "no-cache");
    request.AddHeader("content-type", "application/json");
    request.AddParameter("client_id", CLIENT_ID);
    request.AddParameter("client_secret", CLIENT_SECRET);
    request.AddParameter("jwt_token", token);
  
  /*  request.AddParameter("multipart/form-data; boundary=----boundary",
        "------boundary Content-Disposition: form-data; name=client_id " + CLIENT_ID +
        "------boundary Content-Disposition: form-data; name=client_secret " + CLIENT_SECRET +
        "------boundary\r\nContent-Disposition: form-data; name=jwt_token" + token +
        "------boundary--", ParameterType.RequestBody);*/


    RestResponse response = client.Execute(request);

    if(response.IsSuccessStatusCode)
    Console.WriteLine(response.Content);
    else
        Console.WriteLine(response.Content);

    Console.ReadKey();
}
catch (Exception e)
{
    Console.WriteLine("An error has occured: " + e.Message);
}

