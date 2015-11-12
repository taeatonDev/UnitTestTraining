using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace QuickReferenceTests.MOQ_Reference
{
    [TestClass]
    public class MOQ_Properties_Stubs
    {
        [TestMethod]
        public void Example()
        {
            var mock = new Mock<IFoo>();

            
            mock.Setup(foo => foo.Name).Returns("bar");

            // auto-mocking hierarchies (a.k.a. recursive mocks)
            mock.Setup(foo => foo.Bar.Baz.Name).Returns("baz");

            // expects an invocation to set the value to "foo"
            mock.SetupSet(foo => foo.Name = "foo");

            // or verify the setter directly
            mock.VerifySet(foo => foo.Name = "foo");
            
            //Setup a property so that it will automatically start tracking its value (also known as Stub):
            // start "tracking" sets/gets to this property
            mock.SetupProperty(f => f.Name);

            // alternatively, provide a default value for the stubbed property
            mock.SetupProperty(f => f.Name, "foo");


            // Now you can do:
            IFoo ifoo = mock.Object;
            // Initial value was stored
            Assert.AreEqual("foo", ifoo.Name);

            // New value set which changes the initial value
            ifoo.Name = "bar";
            Assert.AreEqual("bar", ifoo.Name);
        }
    }
}
