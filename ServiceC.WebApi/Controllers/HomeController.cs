using Luminous;

namespace ServiceC.WebApi.Controllers
{
    public class HomeController : LuminousController
    {
        public Task<object> Index() => LuminousUtil.GetStatus();
    }
}