using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        List<string> whiteListIPList = _applicationOptions.Whitelist;

        var isInwhiteListIPList = whiteListIPList
            .Where(a => IPAddress.Parse(a)
            .Equals(ipAddress))
            .Any();
        if (!isInwhiteListIPList)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }
        await _next.Invoke(context);
    }
    }
}
