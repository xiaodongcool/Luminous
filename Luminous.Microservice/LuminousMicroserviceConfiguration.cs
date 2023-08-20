namespace Luminous
{
    public partial class LuminousMicroserviceConfiguration
    {
        public static NacosConfig? Nacos => Global.GetConfig<NacosConfig>("Luminous:Nacos");
    }
}