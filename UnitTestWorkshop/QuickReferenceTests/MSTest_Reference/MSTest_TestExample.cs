using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuickReferenceTests
{
    [TestClass]
    public class MSTest_TestExample
    {
        [TestMethod]
        public void TestMethod1()
        {
            //arrange - Setup inputs, mocks, expected outputs and the like.
            var length = 42;
            var hight = 24;
            var expectedArea = 1008;

            //act - execute method/function being tested.
            var actualArea = (length*hight);

            //assert - assert evaluations post execution
            Assert.AreEqual(expectedArea, actualArea);
        }
    }
}
