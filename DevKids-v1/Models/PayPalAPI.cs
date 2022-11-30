using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NuGet.Versioning;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using static QRCoder.PayloadGenerator;
using static System.Net.Mime.MediaTypeNames;

namespace DevKids_v1.Models
{
    public class PayPalAPI
    {
        private string accessToken = string.Empty;

        // Live strings.
        private string pEndPointURL = "https://api-3t.paypal.com/nvp";
        private string host = "www.paypal.com";

        // Sandbox strings.
        private string pEndPointURL_tokenSB = "https://api-m.sandbox.paypal.com/v1/oauth2/token";
        private string pEndPointURL_SB = "https://api.sandbox.paypal.com/v2/checkout/orders";
        private string host_SB = "https://www.sandbox.paypal.com";

        private readonly string returnURL = "https://localhost:7187/Checkout/CheckoutComplete";
        private readonly string cancelURL = "https://localhost:7187/Checkout/CheckoutCancel";

        //Flag that determines the PayPal environment (live or sandbox)
        private const bool bSandbox = true;
        private const string CVV2 = "CVV2";

        private const string SIGNATURE = "SIGNATURE";
        private const string PWD = "PWD";
        private const string ACCT = "ACCT";

        //Replace <Your API Username> with your API Username
        //Replace <Your API Password> with your API Password
        //Replace <Your Signature> with your Signature
        public string APIUsername = "sb - kaqq522375327@personal.example.com";
        private string APIPassword = "r1(r2A/+";
        private string APISignature = "<Your Signature>";
        private string Subject = "";
        private string BNCode = "PP-ECWizard";

        //HttpWebRequest Timeout specified in milliseconds 
        private const int Timeout = 15000;
        private static readonly string[] SECURED_NVPS = new string[] { ACCT, CVV2, SIGNATURE, PWD };

        public async Task<string> CreateOrder(Purchase orderDetail)
        {
            CultureInfo culture = new CultureInfo("en-EN");
            string body = JsonConvert.SerializeObject(new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        reference_id = orderDetail.Id.ToString(),
                        amount = new
                        {
                            currency_code = "BRL",
                            value = Convert.ToDecimal(orderDetail.Amount, culture),
                        }
                    }
                },
                application_context = new
                {
                    return_url = returnURL,
                    cancel_url = cancelURL
                },
            });

            string order = await HttpPost(pEndPointURL_SB, body);
            JObject jsonOrder = JObject.Parse(order);
            string orderId = string.Empty;
            if (jsonOrder.ContainsKey("status")) 
            {
                if (jsonOrder["status"].ToString() == "CREATED")
                {
                    if (jsonOrder.ContainsKey("id"))
                    {
                        orderId = jsonOrder["id"].ToString();
                    }
                }
            }

            return orderId;
        }

        public string ProcessPayment(string orderId)
        {
            string uri = String.Format("{0}/checkoutnow?token={1}", host_SB, orderId);
            //string data = await HttpGet(uri, string.Empty);
            return uri;
        }

        public async Task<string> CapturePayment(string orderId)
        {
            string uri = String.Format("{0}/{1}/{2}", pEndPointURL_SB, orderId, "capture");
            string data = await HttpPost(uri, string.Empty);
            return data;
        }

        private async Task<string> HttpGet(string uri, string content)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            request.Headers.TryAddWithoutValidation("Authorization", String.Format("Bearer {0}", accessToken));
            request.Headers.TryAddWithoutValidation("content-type", "application/json");
            HttpClient client = new(); // does 'using' here cause socket overload? https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            HttpResponseMessage bearerResult = await client.SendAsync(request);
            string bearerData = await bearerResult.Content.ReadAsStringAsync();

            return bearerData;
        }

        private async Task<string> HttpPost(string uri, string content)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(uri),
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            request.Headers.TryAddWithoutValidation("Authorization", String.Format("Bearer {0}", accessToken));
            request.Headers.TryAddWithoutValidation("content-type", "application/json");
            HttpClient client = new(); // does 'using' here cause socket overload? https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            HttpResponseMessage bearerResult = await client.SendAsync(request);
            string bearerData = await bearerResult.Content.ReadAsStringAsync();

            return bearerData;
        }
       
        public async Task GetAccessToken(string clientID, string clientSecret)
        {
            var encodedClientKey = HttpUtility.UrlEncode(clientID);
            var encodedClientKeySecret = HttpUtility.UrlEncode(clientSecret);
            string encodedPair = Base64Encode(String.Format("{0}:{1}", encodedClientKey, encodedClientKeySecret));

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(pEndPointURL_tokenSB),
                Content = new StringContent("grant_type=client_credentials")
            };

            request.Content.Headers.ContentType = 
                new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };
            request.Headers.TryAddWithoutValidation("Authorization", String.Format("Basic {0}", encodedPair));
            HttpClient client = new();
            HttpResponseMessage bearerResult = await client.SendAsync(request);
            string bearerData = await bearerResult.Content.ReadAsStringAsync();
            JObject response = JObject.Parse(bearerData); 
            if (response.ContainsKey("access_token"))
            {
                accessToken = response["access_token"].ToString();
            }
        }
        
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
