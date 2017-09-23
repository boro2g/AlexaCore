using System;
using System.Collections.Generic;
using System.Net.Http;
using Alexa.NET.Request;
using AlexaCore.Content;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AlexaCore.Tests.Content
{
    [TestFixture]
    class ContentServiceTests
    {
        [Test]
        public void WhenContentIsRequested_ValidTextIsReturned()
        {
            var queue = new PersistentQueue<ApplicationParameter>(null,
                new Session {Attributes = new Dictionary<string, object>()}, "key");

            var contentService = new TestContentService(queue, "userId",
                new MockHttpClient(new HttpResponseMessage
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(new IntentContent
                        {
                            Content = "Welcome to the house of fun"
                        }))
                }));

            var result = contentService.LoadAndFormatContent("HelpIntent", "default text");

            Console.WriteLine(result);

            //todo - need some tests around cookies and persistance
            //todo - need to actually test it working
        }
    }
}
