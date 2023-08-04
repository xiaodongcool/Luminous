namespace LangM.AspNetCore.DbInterface
{
    public class OrderColumn
    {
        public OrderColumn() { }
        public OrderColumn(string column, OrderType sortType)
        {
            Column = column;
            SortType = sortType;
        }
        public string Column { get; set; }
        public OrderType SortType { get; set; }
    }
}
