using System;
using System.Collections.Generic;
using Moq;

namespace QuickReferenceTests.MOQ_Reference
{
    class MOQ_Callbacks
    {
        private void Example()
        {
            var mock = new Mock<IFoo>();
            decimal calls = 0;
            mock.Setup(foo => foo.Execute("ping"))
                .Returns(true)
                .Callback(() => calls++);

            // access invocation arguments
            var calls1 = new List<string>();
            mock.Setup(foo => foo.Execute(It.IsAny<string>()))
                .Returns(true)
                .Callback((string s) => calls1.Add(s));

            // alternate equivalent generic method syntax
            mock.Setup(foo => foo.Execute(It.IsAny<string>()))
                .Returns(true)
                .Callback<string>(s => calls1.Add(s));

            // access arguments for methods with multiple parameters
            mock.Setup(foo => foo.Execute(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(true)
                .Callback<int, string>((i, s) => calls1.Add(s));

            // callbacks can be specified before and after invocation
            mock.Setup(foo => foo.Execute("ping"))
                .Callback(() => Console.WriteLine("Before returns"))
                .Returns(true)
                .Callback(() => Console.WriteLine("After returns")); 
        }
    }
}
