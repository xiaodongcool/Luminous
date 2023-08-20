using Luminous;

namespace ServiceA.WebApi.Controllers
{
    public class HomeController : LuminousController
    {
        public Task<object> Index() => LuminousUtil.GetStatus();
    }
}