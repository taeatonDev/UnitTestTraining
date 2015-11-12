using System;
using Moq;

namespace QuickReferenceTests.MOQ_Reference
{
    public class MOQ_Methods
    {

        private void Examples()
        {
            var mock = new Mock<IFoo>();

            // Set Mock to Return a value
            mock.Setup(foo => foo.DoSomething("ping")).Returns(true);


            // out arguments
            var outString = "ack";
            // TryParse will return true, and the out argument will return "ack", lazy evaluated
            mock.Setup(foo => foo.TryParse("ping", out outString)).Returns(true);


            // ref arguments
            var instance = new Bar();
            // Only matches if the ref argument to the invocation is the same instance
            mock.Setup(foo => foo.Submit(ref instance)).Returns(true);


            // access invocation arguments when returning a value
            mock.Setup(x => x.DoSomethingString(It.IsAny<string>()))
            .Returns((string s) => s.ToLower());
            // Multiple parameters overloads available


            // throwing when invoked
            mock.Setup(foo => foo.DoSomething("reset")).Throws<InvalidOperationException>();
            mock.Setup(foo => foo.DoSomething("")).Throws(new ArgumentException("command"));


            // lazy evaluating return value
            long count = 1;
            mock.Setup(foo => foo.GetCount()).Returns(() => count);


            // returning different values on each invocation
            var calls = 0;
            mock.Setup(foo => foo.GetCountThing())
                .Returns(() => calls)
                .Callback(() => calls++);
        }
    }

    
}