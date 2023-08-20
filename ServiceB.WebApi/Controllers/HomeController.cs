using Luminous;

namespace ServiceB.WebApi.Controllers
{
    public class HomeController : LuminousController
    {
        public Task<object> Index() => LuminousUtil.GetStatus();
    }
}