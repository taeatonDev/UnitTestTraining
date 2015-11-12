namespace QuickReferenceTests
{
    public class Bar
    {
        public Bar Baz { get; set; }
        public string Name { get; set; }
        public EventBar First { get; set; }
    }

    public abstract class EventBar
    {
        public delegate void MyEventHandler(int i, bool b);

        public object FooEvent { get; set; }
    }
}