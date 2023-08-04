namespace LangM.AspNetCore
{
    /// <summary>
    ///     aspnet 入参出参序列化方式
    /// </summary>
    public interface IGlobalSerializer
    {
        public string SerializeObject<T>(T model);
        public T DeserializeObject<T>(string json);
    }

    /// <summary>
    ///     全局序列化配置
    /// </summary>
    public class GlobalSerializer : IGlobalSerializer
    {
        public static JsonSerializerSettings Default = new JsonSerializerSettings();

        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Default);
        }

        public string SerializeObject<T>(T model)
        {
            return JsonConvert.SerializeObject(model, Default);
        }
    }
}
