using System.Linq.Expressions;
using System.Reflection;
using LangM.AspNetCore.Enumeration;

namespace LangM.AspNetCore.DbInterface
{
    //  TODO：加上解决方法

    public class NotSupportPredicateException : Exception
    {
        public NotSupportPredicateException() : base("表达式树解析失败") { }
    }

    public class NotSupportOperateException : Exception
    {
        public NotSupportOperateException(ExpressionType type) : base($"不支持的操作符 {type}") { }
    }

    public class NotSupportPropertyException : Exception
    {
        public NotSupportPropertyException(string propertyName) : base($"不能识别的字段 {propertyName}") { }
    }

    public class NotSupportMethodException : Exception
    {
        public NotSupportMethodException(string methodName) : base($"不能识别的方法 {methodName}") { }
    }

    public class NotSupportNestException : Exception
    {
        public NotSupportNestException() : base("只支持一级嵌套") { }
    }

    public class NotSupportCrossDbSharedException : Exception
    {
        public NotSupportCrossDbSharedException() : base("禁止跨库操作") { }
    }

    public class NotSupportCrossTbSharedException : Exception
    {
        public NotSupportCrossTbSharedException() : base("禁止跨表操作") { }
    }

    public class NotSharedException<T> : Exception
    {
        public Shared Shared { get; }
        public NotSharedException(Shared shared, string suffix) : base($"实体{typeof(T).FullName}没有指定{shared.Display()}属性，无法查询{shared.Display()}{suffix}")
        {
            Shared = shared;
        }
    }

    public class OutOfChoiceSharedException : Exception
    {
        public Shared Shared { get; }
        public OutOfChoiceSharedException(Shared shared, string entitySuffix = null, string specifiedSuffix = null, string predicateSuffix = null) : base($"不一致的{shared.Display()}， 实体：'{entitySuffix}' 指定：'{specifiedSuffix}' 表达式：'{predicateSuffix}'")
        {
            Shared = shared;
        }
    }

    public class NotSetPrimaryKeyException<T> : Exception
    {
        public NotSetPrimaryKeyException(T entity) : base($"无法为未设置主键的实体对象更新，{JsonConvert.SerializeObject(entity)}")
        {
        }
    }

    public class FailedGetSharedException<T> : Exception
    {
        public Shared Shared { get; }
        public FailedGetSharedException(Shared shared) : base($"对实体{typeof(T).FullName}的操作未能获取到{(shared == Shared.Db ? "分库" : "分表")}信息")
        {
            Shared = shared;
        }
    }

    public class DisableDefaultValueSharedException : Exception
    {
        public Shared Share { get; }

        public DisableDefaultValueSharedException(Type entityType, PropertyInfo property, Shared share) : base($"实体 {entityType.FullName} 的字段 {property.Name} 禁止默认值  {share.Display()}")
        {
            Share = share;
        }
    }
}
