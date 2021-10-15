using System.Collections.Generic;

namespace CompareObjects.Contracts
{
    public interface ICompareProvider <T>
    {
        bool IsEqual(T object1, T object2);
        List<Difference> GetDifferences(T object1, T object2);
        string GetDifferencesText(T object1, T object2);
    }
}
