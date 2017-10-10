using System;
using System.Collections.Generic;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.TestUtilities;
using NUnit.Framework;

namespace AlexaCore.Testing
{
    public abstract class AlexaCoreTestRunner<T> where T : class
    {
        private bool _hasRun;

        private readonly AlexaFunction _function;

        public SkillResponse SkillResponse { get; private set; }

        public Session Session
        {
            get
            {
                ValidateHasRun();

                return new Session { Attributes = SkillResponse.SessionAttributes };
            }
        }

        protected AlexaCoreTestRunner()
        {
            SkillResponse = null;

            _hasRun = false;

            _function = BuildFunction();
        }

        public abstract AlexaFunction BuildFunction();
        
        public T RunInitialFunction(string intentName, string newSessionId = "", Session session = null, Context context = null, Dictionary<string, Slot> slots = null) 
        {
            var lambdaContext = new TestLambdaContext
            {
                Logger = new TestLambdaLogger(),
            };

            if (slots == null)
            {
                slots = new Dictionary<string, Slot>();
            }

            AlexaContext.Container.Reset();

            RegisterTypes();

            var response =
                _function.FunctionHandler(
                    new SkillRequest
                    {
                        Session = session ?? new Session { New = true, SessionId = newSessionId },
                        Request = new IntentRequest { Intent = new Intent { Name = intentName, Slots = slots} },
                        Context = context
                    }, lambdaContext);

            SkillResponse = response;

            _hasRun = true;

            return Convert(this);
        }

        public virtual T Convert(AlexaCoreTestRunner<T> alexaCoreTestRunner)
        {
            return alexaCoreTestRunner as T;
        }

        protected virtual void RegisterTypes()
        {

        }
        
        public T RunAgain(string intentName, Dictionary<string, Slot> slots = null)
        {
            ValidateHasRun();

            return RunInitialFunction(intentName, Session.SessionId, Session, slots: slots);
        }

        public T VerifyIntentIsLoaded(string intentName)
        {
            Assert.That(AlexaContext.IntentFactory.RegisteredIntents().Contains(intentName), Is.True, $"AlexaContext should contain intent: {intentName}");

            return Convert(this);
        }

        public T VerifyOutputSpeechExists()
        {
            ValidateHasRun();

            Assert.That(SkillResponse.Response.OutputSpeech, Is.Not.Null);

            return Convert(this);
        }

        public T VerifyOutputSpeechValue(string value)
        {
            var text = GetOutputSpeechValue();

            Assert.That(text, Is.EqualTo(value));

            return Convert(this);
        }

        public T VerifyOutputSpeechValueContains(bool ignoreCase = false, params string[] values)
        {
            var text = GetOutputSpeechValue();

            if (!values.Any())
            {
                values = new string[0];
            }

            foreach (var value in values)
            {
                string valueToCheck = value;

                if (ignoreCase)
                {
                    text = text.ToLower();

                    valueToCheck = value.ToLower();
                }

                Assert.That(text.Contains(valueToCheck), Is.True, $"Output text doesn't contain {value}. Output text is: {text}");
            }

            return Convert(this);
        }

        public T VerifyTextMatchesAnotherIntentResponse(string counterPartIntentName, Dictionary<string, Slot> counterPartIntentSlots = null)
        {
            if (counterPartIntentSlots == null)
            {
                counterPartIntentSlots = new Dictionary<string, Slot>();
            }

            var text = GetOutputSpeechValue();

            var counterPartText = ((PlainTextOutputSpeech)AlexaContext.IntentFactory.GetIntent(counterPartIntentName)
                .GetResponse(counterPartIntentSlots).Response.OutputSpeech).Text;

            Assert.That(text, Is.EqualTo(counterPartText));

            return Convert(this);
        }

        public string GetOutputSpeechValue()
        {
            ValidateHasRun();

            var text = (PlainTextOutputSpeech)SkillResponse.Response.OutputSpeech;

            return text.Text;
        }

        private void ValidateHasRun()
        {
            if (!_hasRun)
            {
                throw new Exception("Need to RunInitialFunction before you can verify the output");
            }
        }

        public T DumpOutputSpeech(Action<string> logger)
        {
            ValidateHasRun();

            logger(SkillResponse.Response.OutputSpeech.ToString());

            return Convert(this);
        }

        public T VerifySessionApplicationParameters(string key, string value)
        {
            var sessionKey = "_PersistentQueue_Parameters";

            List<ApplicationParameter> parameters = GetSessionParameter<ApplicationParameter>(sessionKey);

            var matchingParameter = parameters.FirstOrDefault(a => a.Name == key);

            Assert.That(matchingParameter, Is.Not.Null, $"No key found in '{sessionKey}' with key '{key}'");

            Assert.That(matchingParameter.Value, Is.EqualTo(value), $"Key found in '{sessionKey}' ({key}) has value '{matchingParameter.Value}'. Expected: '{value}'");

            return Convert(this);
        }

        public T VerifySessionCommandQueue(string key)
        {
            var sessionKey = "_PersistentQueue_Commands";

            List<CommandDefinition> parameters = GetSessionParameter<CommandDefinition>(sessionKey);

            var matchingParameter = parameters.FirstOrDefault(a => a.IntentName == key);

            Assert.That(matchingParameter, Is.Not.Null, $"No key found in '{sessionKey}' with key '{key}'");

            return Convert(this);
        }

        public T VerifySessionInputQueue(string value, string[] tags = null)
        {
            var sessionKey = "_PersistentQueue_Inputs";

            List<InputItem> parameters = GetSessionParameter<InputItem>(sessionKey);

            var matchingParameter = parameters.FirstOrDefault(a => a.Value == value);

            Assert.That(matchingParameter, Is.Not.Null, $"No key found in '{sessionKey}' with key '{value}'");

            Assert.That(matchingParameter.Value, Is.EqualTo(value), $"Key found in '{sessionKey}' ({value}) has value '{matchingParameter.Value}'. Expected: '{value}'");

            if (tags != null)
            {
                Assert.That(matchingParameter.Tags.Length, Is.EqualTo(tags.Length), $"Tags length don't match");

                foreach (string tag in tags)
                {
                    Assert.That(matchingParameter.Tags.Contains(tag), Is.True, $"Tags doesn't contain tag: '{tag}");
                }
            }

            return Convert(this);
        }

        protected List<TSessionParam> GetSessionParameter<TSessionParam>(string sessionKey)
        {
            ValidateHasRun();

            List<TSessionParam> parameters = (List<TSessionParam>)SkillResponse.SessionAttributes[sessionKey];

            return parameters;
        }

        public TType Resolve<TType>(string key)
        {
            ValidateHasRun();

            return AlexaContext.Container.Resolve<TType>(key);
        }
    }
}
