﻿using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EggsAndHoney.WebApi.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var bodyAsString = ReadRequestBody(request);

            if ((HttpMethods.IsPut(request.Method) || HttpMethods.IsPost(request.Method)) && BodyIsEmptyOrNull(bodyAsString))
            {
                context.Result = new BadRequestObjectResult("Empty or null body is not allowed!");
            }
            else if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        private static bool BodyIsEmptyOrNull(string body)
        {
            return string.IsNullOrEmpty(body) || body == "null";
        }

		private static string ReadRequestBody(HttpRequest httpRequest)
		{
            httpRequest.EnableRewind();
            var bodyStream = new StreamReader(httpRequest.Body);
			bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
			var bodyText = bodyStream.ReadToEnd();
			return bodyText;
		}
    }
}
