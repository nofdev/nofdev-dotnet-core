using System;
using System.Linq;
using System.Net.Http;
using Nofdev.Core.SOA;

namespace Nofdev.Service.Util
{
    public static class HttpContextHelper
    {
        //public static string GetServiceLayer(this HttpContent hc)
        //{
        //    var segments = hc.Request.Path.Value.Trim('/').Split('/');
        //    var layers = Enum.GetNames(typeof(ServiceType));
        //    var i = 0;
        //    foreach (var segment in segments)
        //    {
        //        if (layers.Any(layer => string.Compare(segment, layer, StringComparison.CurrentCultureIgnoreCase) == 0))
        //        {
        //            return segment;
        //        }
        //        i++;
        //        if (i == 2)
        //            break;
        //    }
        //    throw new NotSupportedException("Cann't find supported service layer name.");
        //}


        public static  string GetCurrentCallId(this ServiceContext context)
        {
            var parentCallId = context?.CallId.Parent;
            return !string.IsNullOrWhiteSpace(parentCallId) ? parentCallId :
               context?.CallId.Id;
        }
    }
}
