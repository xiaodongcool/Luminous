namespace Luminous
{
    public interface IRowObject
    {
        ICellObject this[int columnIndex] { get; }
        string[] GetValues();
    }
}
