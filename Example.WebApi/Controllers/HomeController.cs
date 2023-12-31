using Luminous;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Example.WebApi.Controllers
{
    public class HomeController : LuminousController
    {
        [HttpGet]
        public Task<object> Index() => LuminousUtil.GetStatus();
    }
}