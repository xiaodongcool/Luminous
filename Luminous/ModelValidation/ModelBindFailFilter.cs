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
        private ActionExecutingContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IContactProvider _conventionProvider;

        public ModelBindFailFilter(IWebHostEnvironment env, IContactProvider conventionProvider)
        {
            _env = env;
            _conventionProvider = conventionProvider;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _context = context;

            if (context.ModelState.IsValid)
            {
                return;
            }

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

            var errors = GetErrors();

            var response = _conventionProvider.Create<object>(WebApiStatusCode.ParameterError, null, errors.SelectMany(_ => _.Errors).FirstOrDefault(_ => !string.IsNullOrEmpty(_.Message)).Message, errors);

            context.Result = new JsonResult(response);
        }

        /// <summary>
        ///     获取所有绑定失败的字段及原因
        /// </summary>
        protected IList<ModelBindFailEntry> GetErrors()
        {
            var errors = new List<ModelBindFailEntry>();

            foreach (var (key, value) in _context.ModelState)
            {
                if (value.Errors.Any())
                {
                    errors.Add(new ModelBindFailEntry
                    {
                        PropertyName = key,
                        RawValue = value.RawValue,
                        Errors = value.Errors.Select(_ => new ModelBindFailEntry.ErrorItem
                        {
                            Exception = _env.IsDevelopment() ? _.Exception : null,
                            Message = _.ErrorMessage
                        }).ToList()
                    });
                }
            }

            return errors;
        }
    }
}
