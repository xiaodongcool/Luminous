namespace LangM.AspNetCore
{
    public class PagingAssignmentOptions
    {
        public string PageIndexName { get; set; } = "pageindex";
        public string PageSizeName { get; set; } = "pagesize";
        public int DefaultPageSize { get; set; } = 20;
    }
}
