namespace Luminous.Serializer
{
    /// <summary>
    ///     json 序列化全局配置
    /// </summary>
    public class SerializerOptions
    {
        /// <summary>
        ///     命名规则
        /// </summary>
        public SerializerNamed Named { get; set; } = SerializerNamed.Hump;
        /// <summary>
        ///     时间格式化
        /// </summary>
        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        ///     忽略 null 值
        /// </summary>
        public bool IgnoreNullProperty { get; set; } = true;
    }
}
