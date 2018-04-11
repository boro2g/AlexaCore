using System.Collections.Generic;
using Alexa.NET.Request;
using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    class SlotIntentTests
    {
        [Test]
        public void WhenSlotsAreUsed_CorrectValueIsParsed()
        {
            var slots = new Dictionary<string, Slot>
            {
                ["TestSlot"] = new Slot {Value = "TestSlotValue", Name = "TestSlot"}
            };

            new TestFunctionTestRunner()
                .RunInitialFunction("SlotIntent", slots: slots)
                .VerifyOutputSpeechValue("Slot value: TestSlotValue");
        }

        [Test]
        public void WhenSlotIsMissing_ValidErrorIsRaised()
        {
            var response = new TestFunctionTestRunner()
                .RunInitialFunction("SlotIntent")
                .GetOutputSpeechValue();

            Assert.That(response.Contains("Expected slots are: TestSlot"), Is.True);
        }

        [Test]
        public void WhenSingleSlotIsUsed_CorrectValueIsUsed()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction("SlotIntent",
                    slots: TestFunctionTestRunner.BuildSlots(new Slot {Name = "TestSlot", Value = "TestSlotValue"}))
                .VerifyOutputSpeechValue("Slot value: TestSlotValue");
        }
    }
}