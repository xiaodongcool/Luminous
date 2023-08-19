namespace Luminous.Microservice
{
    public class NacosConfig
    {
        /// <summary>
        ///     服务名
        /// </summary>
        public string ServiceName { get; set; } = null!;
        /// <summary>
        ///     Nacos 服务地址
        /// </summary>
        public List<string> ServerAddresses { get; set; } = null!;
        /// <summary>
        ///     命名空间
        /// </summary>
        public string Namespace { get; set; } = null!;
    }
}