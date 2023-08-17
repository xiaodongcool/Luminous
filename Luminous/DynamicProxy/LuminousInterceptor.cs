using AspectCore.DynamicProxy;
using System.Reflection;

namespace Luminous.DynamicProxy
{
    public abstract class LuminousInterceptor : IInterceptor
    {
        public bool AllowMultiple { get; }

        public bool Inherited { get; set; }

        public int Order { get; set; }

        public LuminousInterceptor()
        {
            Console.WriteLine(GetHashCode());
        }

        public abstract Task Invoke(AspectContext context, AspectDelegate next);

        protected virtual T? GetCustomerAttributeOnMethod<T>(AspectContext context) where T : Attribute
        {
            return context.ServiceMethod.GetCustomAttribute<T>();
        }

        protected virtual T? GetCustomerAttributeOnInterface<T>(AspectContext context) where T : Attribute
        {
            return context.ServiceMethod.DeclaringType?.GetCustomAttribute<T>();
        }

        protected Type GetMethodReturnType(AspectContext context)
        {
            return context.ServiceMethod.ReturnType;
        }

        protected ParameterInfo[] GetMethodParameters(AspectContext context)
        {
            return context.ServiceMethod.GetParameters();
        }

        protected object[] GetMethodParameterValues(AspectContext context)
        {
            return context.Parameters;
        }
    }
}
