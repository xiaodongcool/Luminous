namespace Luminous
{
    public interface IColumnObject
    {
        ICellObject this[int rowIndex] { get; }
        string[] GetValues();
    }
}
