using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    class ContainerTests
    {
        [Test]
        public void WhenFunctionRegistersType_ItCanBeResolvedOffTheContext()
        {
            var resolvedType = new TestFunctionTestRunner()
                .RunInitialFunction("LaunchIntent")
                .Resolve<ITestDataStore>("globalItem");

            Assert.That(resolvedType.Name, Is.EqualTo("Function"));
        }

        [Test]
        public void WhenFunctionRegistersTypeWithoutKey_ItCanBeResolvedOffTheContext()
        {
            var resolvedType = new TestFunctionTestRunner()
                .RunInitialFunction("LaunchIntent")
                .Resolve<TestDependency>();

            Assert.That(resolvedType.GetData(), Is.EqualTo("concrete"));
        }

        [Test]
        public void WhenFunctionRegistersTypeWithoutKeyAndInterface_ItCanBeResolvedOffTheContext()
        {
            var resolvedType = new TestFunctionTestRunner()
                .RunInitialFunction("LaunchIntent")
                .Resolve<ITestDependency>();

            Assert.That(resolvedType.GetData(), Is.EqualTo("interface"));
        }

        [Test]
        public void WhenFunctionPreRegistersType_ItDoesntGetOverwrittenAndItCanBeResolvedOffTheContext()
        {
            var overwrittenDataStore = new TestFunctionTestRunner()
                .PerformRegisterTypes(true)
                .RunInitialFunction("LaunchIntent")
                .Resolve<ITestDataStore>("globalItem");

            Assert.That(overwrittenDataStore.Name, Is.EqualTo("override"));
        }

        [Test]
        public void ConstructorInjectionIntoIntent_CanBeResolved()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction("ConstructorInjectionIntent")
                .VerifyOutputSpeechValue("bob");
        }
    }
}