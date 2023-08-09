namespace Luminous.Npoi
{
    public interface IRowObject
    {
        ICellObject this[int columnIndex] { get; }
        string[] GetValues();
    }
}
