namespace CompareObjects
{
    public class Difference
    {
        public string PropertyName { get; set; }
        public object PreviousPropertyValue { get; set; }
        public object NextPropertyValue { get; set; }
        public string ReadableText { get; set; }
    }
}
