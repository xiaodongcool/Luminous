namespace Luminous.Npoi
{
    public interface IColumnObject
    {
        ICellObject this[int rowIndex] { get; }
        string[] GetValues();
    }
}
