using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AargonTools.Middleware
{
    public class IpFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationsOptions _applicationOptions;
        public IpFilterMiddleware(RequestDelegate next, IOptions<ApplicationsOptions> applicationOptionsAccessor)
        {
            _next = next;
            _applicationOptions = applicationOptionsAccessor.Value;
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



            var isInWhiteListIpList = finalListOfIpAddress
                .Any(a => a
                    .Equals(ipAddress));
            if (!isInWhiteListIpList)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await _next.Invoke(context);
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
