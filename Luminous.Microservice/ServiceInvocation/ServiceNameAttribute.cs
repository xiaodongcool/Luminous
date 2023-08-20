using Luminous;

namespace Luminous
{
    /// <summary>
    /// 服务名称选择器
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class ServiceNameAttribute : Attribute
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public ServiceNameAttribute(string name)
        {
            ArgumentGuard.CheckForNull(name, nameof(name));
            Name = name;
        }
    }
}
