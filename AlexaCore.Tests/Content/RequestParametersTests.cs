using AlexaCore.Content;
using NUnit.Framework;

namespace AlexaCore.Tests.Content
{
    [TestFixture]
    class RequestParametersTests
    {
        [Test]
        public void RequestParameters_ToQueryStringOk()
        {
            var requestParameters = new RequestParameters(new[]
                {new RequestParameter("a", "1"), new RequestParameter("b", "2"),});

            Assert.That(requestParameters.ToQueryString("?"), Is.EqualTo("?a=1&b=2"));
        }

        [Test]
        public void RequestParametersGetEncoded_ToQueryStringOk()
        {
            var requestParameters = new RequestParameters(new[]
                {new RequestParameter("a", "1 2 3"), new RequestParameter("b", "4 5"),});

            Assert.That(requestParameters.ToQueryString("?"), Is.EqualTo("?a=1+2+3&b=4+5"));
        }
    }
}
