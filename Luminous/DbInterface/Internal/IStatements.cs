namespace LangM.AspNetCore.DbInterface
{
    public interface IStatements
    {
        string TableName { get; }
        Type EntityType { get; }
        string PrefixParameter(string paramName);
        string BuildInsert(string[] columnNames, string[] paramNames);
        string BuildUpdate(string[] columnNames, string[] paramNames);
        string BuildSelect(string[] columnNames, string[] paramNames);
    }
}
