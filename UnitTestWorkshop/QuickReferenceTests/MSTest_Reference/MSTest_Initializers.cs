using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace QuickReferenceTests
{
    [TestClass]
    public class MSTest_Initializers
    {
        private static TestSubject _subject;
        private static Mock<IMockableInterface> _mockInterface;

        [ClassInitialize]
        [Description("Initializes only once per full test run.")]
        //You can name this whatever you want, InitializeMocks is just an example.
        public static void InitializeMocks(TestContext context) 
        {
            _mockInterface = new Mock<IMockableInterface>();
        }

        [TestInitialize]
        [Description("Initializes before every Test.")]
        //You can name this whatever you want, InitializeSubject is just an example.
        public void InitializeSubject()
        {
            _subject = new TestSubject(_mockInterface.Object);
        }
    }
}
