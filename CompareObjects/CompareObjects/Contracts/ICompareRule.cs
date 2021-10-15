namespace CompareObjects.Contracts
{
    public interface ICompareRule<T>
    {
        CompareSettings OnConfigure(CompareBuilder<T> builder);
    }
}
