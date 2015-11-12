using System.Text.RegularExpressions;
using Moq;

namespace QuickReferenceTests.MOQ_Reference
{
    class MOQ_Matching_Arguments
    {
        private void Example()
        {
            var mock = new Mock<IFoo>();

            // any value
            mock.Setup(foo => foo.DoSomething(It.IsAny<string>())).Returns(true);


            // matching Func<int>, lazy evaluated
            mock.Setup(foo => foo.Add(It.Is<int>(i => i % 2 == 0))).Returns(true);


            // matching ranges
            mock.Setup(foo => foo.Add(It.IsInRange<int>(0, 10, Range.Inclusive))).Returns(true);


            // matching regex
            mock.Setup(x => x.DoSomethingString(It.IsRegex("[a-d]+", RegexOptions.IgnoreCase))).Returns("foo");
        }
    }
}
