using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuickReferenceTests
{
    [TestClass]
    public class MSTest_Cleanup
    {
        [ClassCleanup]
        [Description("Runs after all tests have been run.")]
        public static void ClassCleanup()
        {
            // do your disposing or test data cleanup.
        }

        [TestCleanup]
        [Description("Runs after each test has been run.")]
        public static void TestCleanup()
        {
            // do your disposing or test data cleanup.
        }
    }
}
