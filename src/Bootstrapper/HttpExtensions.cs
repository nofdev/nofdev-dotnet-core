using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Nofdev.Core.SOA;

namespace Nofdev.Bootstrapper
{
    public static class HttpExtensions
    {
        public static string GetServiceLayer(this HttpRequest request)
        {
            var segments = request.Path.Value.Trim('/').Split('/');
            var layers = Enum.GetNames(typeof(ServiceType));
            var i = 0;
            foreach (var segment in segments)
            {
                if (layers.Any(layer => string.Compare(segment, layer, StringComparison.CurrentCultureIgnoreCase) == 0))
                {
                    return segment;
                }
                i++;
                if (i == 2)
                    break;
            }
            throw new NotSupportedException("Cann't find supported service layer name.");
        }
    }
}