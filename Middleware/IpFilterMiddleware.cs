using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AargonTools.Middleware
{
    public class IpFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationsOptions _applicationOptions;
        private readonly IUserService _userService;
        private readonly IHttpClientFactory _httpClientFactory;
        public IpFilterMiddleware(RequestDelegate next, IOptions<ApplicationsOptions> applicationOptionsAccessor, IUserService userService, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _applicationOptions = applicationOptionsAccessor.Value;
            _userService = userService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress;
            var whiteListIpList = _applicationOptions.Whitelist;
            var finalListOfIpAddress = new List<IPAddress>();
            foreach (var t in whiteListIpList)
            {
                if (t.Contains("/"))
                {
                    var ipList = GetAllIp(t);
                    finalListOfIpAddress.AddRange(ipList);
                }
                else
                {
                    finalListOfIpAddress.Add(IPAddress.Parse((string)t));
                }
            }

            var isInWhiteListIpList = finalListOfIpAddress.Any(a => a.Equals(ipAddress));
            if (!isInWhiteListIpList)
            {


                context.Request.EnableBuffering();
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                var userAgent = context.Request.Headers["User-Agent"].ToString();
                var userLanguage = context.Request.Headers["Accept-Language"].ToString();
                var ipBehindTheMask = await CheckIPQSDatabase(Convert.ToString(ipAddress), userAgent, userLanguage);

                string hostName = null;

                try
                {
                    // Perform reverse DNS lookup
                    var hostEntry = await Dns.GetHostEntryAsync(ipAddress);
                    hostName = hostEntry.HostName;
                }
                catch (SocketException ex)
                {
                    //Serilog.Log.Warning("DNS lookup failed for IP {IpAddress}: {Message}", ipAddress, ex.Message);
                }

                // Check if the hostname belongs to a known search engine
                if (hostName != null && (hostName.EndsWith("googlebot.com") || hostName.EndsWith("bingbot.com")))
                {
                    // It's a legitimate search engine bot
                    Serilog.Log.Warning("Forbidden IP [{IpAddress}], IP Status:{@ipBehindTheMask}, Payload: {Payload}, UserAgent: {UserAgent}, HostName: {HostName} ThreatLevel: Possible search engine bot",
                        ipAddress, ipBehindTheMask, requestBody, userAgent, hostName);
                }
                else
                {
                    // Handle as a potential intruder
                    Serilog.Log.Warning("Forbidden IP [{IpAddress}], IP Status:{@ipBehindTheMask}, Payload: {Payload}, UserAgent: {UserAgent} ThreatLevel: Potential intruder",
                        ipAddress, ipBehindTheMask, requestBody, userAgent);
                }

                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await _next.Invoke(context);
        }

        //VPN experiments 

        private async Task<object> CheckIPQSDatabase(string ipAddress, string userAgent, string userLanguage)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://ipqualityscore.com/api/json/ip/bM92WYJA63au362Y043bdnljfiD7x8XS/{ipAddress}?strictness=1&user_agent={userAgent}&user_language={userLanguage}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IpQualityScoreResponse>(content);
                    return result;
                }
                else
                {
                    // Log the response status code for debugging
                    Serilog.Log.Warning("Failed to get VPN status for IP {IpAddress}. Status Code: {StatusCode}", ipAddress, response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle network-related errors
                Serilog.Log.Error("Network error while checking VPN status for IP {IpAddress}: {Message}", ipAddress, ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                Serilog.Log.Error("Unexpected error while checking VPN status for IP {IpAddress}: {Message}", ipAddress, ex.Message);
            }
            return null;
        }


        public class IpQualityScoreResponse
        {
            public bool Vpn { get; set; }
            public bool proxy { get; set; }
            public string fraud_score { get; set; }
            public string ISP { get; set; }
            public string organization { get; set; }
            public string country_code { get; set; }
            public string city { get; set; }
            public string region { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public TransactionDetails transaction_details { get; set; }
            // Add other relevant properties as needed
        }

        public class TransactionDetails
        {
            [JsonProperty("valid_billing_address")]
            public bool ValidBillingAddress { get; set; }
            [JsonProperty("valid_shipping_address")]
            public bool ValidShippingAddress { get; set; }
            [JsonProperty("valid_billing_email")]
            public bool ValidBillingEmail { get; set; }
            [JsonProperty("valid_shipping_email")]
            public bool ValidShippingEmail { get; set; }
            [JsonProperty("risky_billing_phone")]
            public bool RiskyBillingPhone { get; set; }
            [JsonProperty("risky_shipping_phone")]
            public bool RiskyShippingPhone { get; set; }
            [JsonProperty("fraudulent_behavior")]
            public bool FraudulentBehavior { get; set; }
            [JsonProperty("risk_score")]
            public int RiskScore { get; set; }
            [JsonProperty("risk_factors")]
            public List<string> RiskFactors { get; set; }

        }








        //for ip range implementation

        public IEnumerable<IPAddress> GetAllIp(string ipRange)
        {
            try
            {
                if (ipRange == null)
                    throw new ArgumentNullException();

                if (!TryParseCidrNotation(ipRange) && !TryParseSimpleRange(ipRange))
                    throw new ArgumentException();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }




            var capacity = 1;
            for (int i = 0; i < 4; i++)
                capacity *= _endIp[i] - _beginIp[i] + 1;

            var ips = new List<IPAddress>(capacity);
            for (int i0 = _beginIp[0]; i0 <= _endIp[0]; i0++)
            {
                for (int i1 = _beginIp[1]; i1 <= _endIp[1]; i1++)
                {
                    for (int i2 = _beginIp[2]; i2 <= _endIp[2]; i2++)
                    {
                        for (int i3 = _beginIp[3]; i3 <= _endIp[3]; i3++)
                        {
                            ips.Add(new IPAddress(new byte[] { (byte)i0, (byte)i1, (byte)i2, (byte)i3 }));
                        }
                    }
                }
            }

            return ips;
        }

        /// <summary>
        /// Parse IP-range string in CIDR notation.
        /// For example "12.15.0.0/16".
        /// </summary>
        /// <param name="ipRange"></param>
        /// <returns></returns>
        private bool TryParseCidrNotation(string ipRange)
        {
            string[] x = ipRange.Split('/');

            if (x.Length != 2)
                return false;

            byte bits = byte.Parse(x[1]);
            uint ip = 0;
            String[] ipParts0 = x[0].Split('.');
            for (int i = 0; i < 4; i++)
            {
                ip = ip << 8;
                ip += uint.Parse(ipParts0[i]);
            }

            byte shiftBits = (byte)(32 - bits);
            uint ip1 = (ip >> shiftBits) << shiftBits;

            if (ip1 != ip) // Check correct subnet address
                return false;

            uint ip2 = ip1 >> shiftBits;
            for (int k = 0; k < shiftBits; k++)
            {
                ip2 = (ip2 << 1) + 1;
            }

            _beginIp = new byte[4];
            _endIp = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                _beginIp[i] = (byte)((ip1 >> (3 - i) * 8) & 255);
                _endIp[i] = (byte)((ip2 >> (3 - i) * 8) & 255);
            }

            return true;
        }

        /// <summary>
        /// Parse IP-range string "12.15-16.1-30.10-255"
        /// </summary>
        /// <param name="ipRange"></param>
        /// <returns></returns>
        private bool TryParseSimpleRange(string ipRange)
        {
            var ipParts = ipRange.Split('.');

            _beginIp = new byte[4];
            _endIp = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                var rangeParts = ipParts[i].Split('-');

                if (rangeParts.Length < 1 || rangeParts.Length > 2)
                    return false;

                _beginIp[i] = byte.Parse(rangeParts[0]);
                _endIp[i] = (rangeParts.Length == 1) ? _beginIp[i] : byte.Parse(rangeParts[1]);
            }

            return true;
        }

        private byte[] _beginIp;
        private byte[] _endIp;



    }
}
