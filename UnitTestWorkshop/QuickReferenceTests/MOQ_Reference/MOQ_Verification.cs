using Moq;

namespace QuickReferenceTests.MOQ_Reference
{
    class MOQ_Verification
    {
        private void Example()
        {
            var mock = new Mock<IFoo>();

            mock.Verify(foo => foo.Execute("ping"));

            // Verify with custom error message for failure
            mock.Verify(foo => foo.Execute("ping"), "When doing operation X, the service should be pinged always");

            // Method should never be called
            mock.Verify(foo => foo.Execute("ping"), Times.Never());

            // Called at least once
            mock.Verify(foo => foo.Execute("ping"), Times.AtLeastOnce());

            mock.VerifyGet(foo => foo.Name);

            // Verify setter invocation, regardless of value.
            mock.VerifySet(foo => foo.Name);

            // Verify setter called with specific value
            mock.VerifySet(foo => foo.Name = "foo");

            // Verify setter with an argument matcher
            mock.VerifySet(foo => foo.Value = It.IsInRange(1, 5, Range.Inclusive));
        }
    }
}
