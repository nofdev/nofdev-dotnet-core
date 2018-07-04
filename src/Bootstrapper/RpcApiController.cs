using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nofdev.Core;
using Nofdev.Core.SOA;
using Nofdev.Service;
using Nofdev.Service.Util;

namespace Nofdev.Bootstrapper
{
    [Route("facade")]
    [Route("service")]
    /*
     * the following three annotations to support old pattern URLs
     */
    [Route("json/facade")]
    [Route("json/service")]
    public class RpcApiController : ControllerBase
    {
        private readonly ILogger<RpcApiController> _logger;

        public bool EnableStackTrace { get; set; } = false;

        public RpcApiController(ILogger<RpcApiController> logger)
        {
            _logger = logger;
        }

        [Route("{packageName}/{interfaceName}/{methodName}")]
        [Route("[action]/{packageName}/{interfaceName}/{methodName}")]
        public async Task<JsonResult> Json(string packageName, string interfaceName, string methodName, string @params)
        {
            var httpJsonResponse = new HttpJsonResponse<dynamic> { callId =  GetCurrentCallId() };
            ExceptionMessage exceptionMessage = null;
            try
            {
                var serviceLayer = Request.GetServiceLayer();
                _logger.LogDebug($"ask instance for interface {interfaceName}");

                var serviceType = ServiceScanner.GetServiceType(packageName, interfaceName, serviceLayer);
                var service = HttpContext.RequestServices.GetService(serviceType);
                _logger.LogDebug(
                    $"JSON facade call(callId:{httpJsonResponse.callId}): {interfaceName}.{methodName}{@params}");
                await Invoker.Invoke(serviceType, service, methodName, @params).ContinueWith(it => httpJsonResponse.val = it.Result);
            }
            catch (AbstractBusinessException e)
            {
                _logger.LogInformation(
                    $"Business exception occured when calling JSON facade call(callId:{httpJsonResponse.callId}): {interfaceName}.{methodName}{@params}: {e.Message}");
                Response.StatusCode = 500;
                exceptionMessage = ExceptionMessage.FromException(e, EnableStackTrace);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    $"Unhandled exception occured when calling JSON facade call(callId:{httpJsonResponse.callId}): {interfaceName}.{methodName}{@params}: {e.Message}");
                Response.StatusCode = 500;
                exceptionMessage = ExceptionMessage.FromException(e, EnableStackTrace);
            }
            httpJsonResponse.err = exceptionMessage;
            return new JsonResult(httpJsonResponse);
        }

          string GetCurrentCallId()

          {
              var context = ServiceContext.Current;
            var parentCallId = context?.CallId.Parent;

            return !string.IsNullOrWhiteSpace(parentCallId) ? parentCallId :

                context?.CallId.Id;

        }

    }
}