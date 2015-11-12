namespace QuickReferenceTests
{
    public interface IFoo {
        bool DoSomething(string input);
        int GetCountThing();
        long GetCount();
        bool TryParse(string input, out string outString);
        bool Submit(ref Bar instance);
        string DoSomethingString(string input);
        bool Add(int input);
        string Name { get; set; }
        Bar Bar { get; set; }
        object FooEvent { get; set; }
        Bar Child { get; set; }
        int Value { get; set; }
        event EventBar.MyEventHandler MyEvent;

        bool Execute(string ping);
        bool Execute(int isAny, string ping);
    }
}