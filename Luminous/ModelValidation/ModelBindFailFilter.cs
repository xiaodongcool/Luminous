using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Luminous
{
    /// <summary>
    ///     模型绑定失败处理
    /// </summary>
    public class ModelBindFailFilter : IActionFilter
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ModelBindFailFilter(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

            var errors = GetErrors(context);

            IResult<object> result;

            var message = errors.SelectMany(x => x.Errors).FirstOrDefault(x => !string.IsNullOrEmpty(x.Message))?.Message;

            if (_webHostEnvironment.IsProduction())
            {
                result = new Result<object>(ResultStatus.ParameterError, null, message);
            }
            else
            {
                result = new DebugResult<object>(ResultStatus.ParameterError, null, message, null, errors);
            }

            context.Result = new JsonResult(result);
        }

        /// <summary>
        ///     获取所有绑定失败的字段及原因
        /// </summary>
        protected List<ModelBindFail> GetErrors(ActionExecutingContext context)
        {
            var errors = new List<ModelBindFail>();

            foreach (var (key, value) in context.ModelState)
            {
                if (value.Errors.Any())
                {
                    errors.Add(new ModelBindFail
                    {
                        Name = key,
                        Value = value.RawValue,
                        Errors = value.Errors.Select(x => new ModelBindFailDetail
                        {
                            Exception = x.Exception,
                            Message = x.ErrorMessage
                        }).ToArray()
                    });
                }
            }

            return errors;
        }
    }
}
