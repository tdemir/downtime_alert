using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebUI
{
    public static class ExtensionMethods
    {
        public static string GetId(this System.Security.Principal.IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null && claimsIdentity.Claims != null)
            {
                var idClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (idClaim != null)
                {
                    return idClaim.Value;
                }
            }
            return string.Empty;
        }

        public static string Serialize(this object obj)
        {
            try
            {
                return System.Text.Json.JsonSerializer.Serialize(obj);
            }
            catch (Exception)
            {

            }
            return string.Empty;
        }

        public static string GetClientIpAddress(this HttpContext httpContext)
        {
            try
            {
                return httpContext.Connection.RemoteIpAddress.ToString();
            }
            catch (Exception)
            {

            }
            return string.Empty;
        }

        public static string GetCurrentUrl(this HttpContext context, bool addRequestMethod = false, bool addQueryStr = false)
        {
            var _url = string.Empty;
            try
            {
                //var _routeValues = context.Request.RouteValues;
                // var _controllerName = _routeValues["controller"].ToString();
                // var _actionName = _routeValues["action"].ToString();
                var _request = context.Request;
                var sb = new StringBuilder();
                if (addRequestMethod)
                {
                    sb.Append(_request.Method);
                    sb.Append(" ");
                }
                sb.Append(_request.Scheme);
                sb.Append("://");
                if (_request.Host.HasValue)
                {
                    sb.Append(_request.Host.Value);
                }
                if (_request.Path.HasValue)
                {
                    sb.Append(_request.Path.Value);
                }
                if (addQueryStr && _request.QueryString.HasValue)
                {
                    sb.Append(_request.QueryString.Value);
                }
                _url = sb.ToString();
            }
            catch (Exception)
            {

            }
            return _url.Trim();
        }
    }
}
