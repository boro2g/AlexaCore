using System.Collections.Generic;
using Alexa.NET.Request;
using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    class IntentWithResponseTests
    {
        [Test]
        public void WhenIntentExpectsResponse_CorrectOutputIsReturned()
        {
            var slots = new Dictionary<string, Slot>
            {
                ["TestSlot"] = new Slot { Value = "TestSlotValue", Name = "TestSlot" }
            };

            new TestFunctionTestRunner()
                .RunInitialFunction("QuestionNeedingResponseIntent", slots: slots)
                .VerifyOutputSpeechValue("Slot value: TestSlotValue. Are you sure?")
                .RunAgain("YesIntent")
                .VerifyOutputSpeechValue("Yes response")
                //note - the NoIntent climbs the chain of previous commands past the YesIntent - it does this by using: `override CommandDefinition SelectLastCommand`
                .RunAgain("NoIntent")
                .VerifyOutputSpeechValue("No response");
        }

        [Test]
        public void WhenIntentExpectsResponse_ButInvalidResponseIsGiven()
        {
            var slots = new Dictionary<string, Slot>
            {
                ["TestSlot"] = new Slot { Value = "TestSlotValue", Name = "TestSlot" }
            };

            new TestFunctionTestRunner()
                .RunInitialFunction("QuestionNeedingResponseIntent", slots: slots)
                .RunAgain("AnotherResponseIntent")
                .VerifyOutputSpeechValue("I wasn't expecting that answer - how about no or yes");
        }

        [Test]
        public void WhenIntentExpectsResponse_ExternalDataIsReturned()
        {
            var slots = new Dictionary<string, Slot>
            {
                ["TestSlot"] = new Slot { Value = "TestSlotValue", Name = "TestSlot" }
            };

            new TestFunctionTestRunner()
                .RunInitialFunction("QuestionNeedingResponseIntent", slots: slots)
                .RunAgain("ExternalResponseIntent")
                .VerifyOutputSpeechValue("LaunchIntent");
        }
    }
}
