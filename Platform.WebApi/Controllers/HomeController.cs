using Luminous;

namespace Platform.WebApi.Controllers
{
    public class HomeController : LuminousController
    {
        public Task<object> Index() => LuminousUtil.GetStatus();
    }
}