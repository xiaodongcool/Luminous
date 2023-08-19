namespace Luminous.Microservice
{
    public partial class LuminousMicroserviceConfiguration
    {
        public static NacosConfig? Nacos => Global.GetConfig<NacosConfig>("Luminous:Nacos");
    }
}