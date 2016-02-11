using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using NReco.PdfGenerator;

namespace NeedleWork2016.Filters
{
    //I use filter for common data representation to generate a pdf report
    //I use NReco library for generate PDF
    public class ConvertToPdfFilterAttribute : ActionFilterAttribute
    {
        //Directly PDF generation with using the NReco library
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Query.ContainsKey("convert"))
            {
                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                htmlToPdf.GeneratePdfFromFile(
                    GetUrl(context.HttpContext.Request).Split('?')[0], null,
                    context.HttpContext.Response.Body);
                htmlToPdf.Size = PageSize.Default;
                context.Result = new EmptyResult();
                context.HttpContext.Response.ContentType = "application/pdf";
                return;
            }
            base.OnActionExecuting(context);
        }
        //Get the URL of html page which we want to convert to PDF
        private string GetUrl(HttpRequest request)
        {
            return Microsoft.AspNet.Http.Extensions.UriHelper.Encode(request.Scheme, request.Host, request.PathBase, request.Path, request.QueryString);
        }
    }
}