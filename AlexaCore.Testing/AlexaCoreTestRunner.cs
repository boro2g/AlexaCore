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
    public abstract class AlexaCoreTestRunner
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
        
        public AlexaCoreTestRunner RunTest(string intentName, string newSessionId = "", Session session = null, Context context = null)
        {
            var lambdaContext = new TestLambdaContext
            {
                Logger = new TestLambdaLogger(),
            };

            RegisterTypes();

            var response =
                _function.FunctionHandler(
                    new SkillRequest
                    {
                        Session = session ?? new Session { New = true, SessionId = newSessionId },
                        Request = new IntentRequest { Intent = new Intent { Name = intentName } },
                        Context = context
                    }, lambdaContext);

            SkillResponse = response;

            _hasRun = true;

            return this;
        }

        protected virtual void RegisterTypes()
        {

        }

        public AlexaCoreTestRunner RunAgain(string intentName)
        {
            ValidateHasRun();

            return RunTest(intentName, Session.SessionId, Session);
        }

        public AlexaCoreTestRunner VerifyIntentIsLoaded(string intentName)
        {
            Assert.That(AlexaContext.IntentFactory.RegisteredIntents().Contains(intentName), Is.True, $"AlexaContext should contain intent: {intentName}");

            return this;
        }

        public AlexaCoreTestRunner VerifyOutputSpeech()
        {
            ValidateHasRun();

            Assert.That(SkillResponse.Response.OutputSpeech, Is.Not.Null);

            return this;
        }

        private void ValidateHasRun()
        {
            if (!_hasRun)
            {
                throw new Exception("Need to RunTest before you can verify the output");
            }
        }

        public AlexaCoreTestRunner DumpOutputSpeech(Action<string> logger)
        {
            ValidateHasRun();

            logger(SkillResponse.Response.OutputSpeech.ToString());

            return this;
        }

        public AlexaCoreTestRunner VerifySessionApplicationParameters(string key, string value)
        {
            var sessionKey = "_PersistentQueue_Parameters";

            List<ApplicationParameter> parameters = GetSessionParameter<ApplicationParameter>(sessionKey);

            var matchingParameter = parameters.FirstOrDefault(a => a.Name == key);

            Assert.That(matchingParameter, Is.Not.Null, $"No key found in '{sessionKey}' with key '{key}'");

            Assert.That(matchingParameter.Value, Is.EqualTo(value), $"Key found in '{sessionKey}' ({key}) has value '{matchingParameter.Value}'. Expected: '{value}'");

            return this;
        }

        public AlexaCoreTestRunner VerifySessionCommandQueue(string key)
        {
            var sessionKey = "_PersistentQueue_Commands";

            List<CommandDefinition> parameters = GetSessionParameter<CommandDefinition>(sessionKey);

            var matchingParameter = parameters.FirstOrDefault(a => a.IntentName == key);

            Assert.That(matchingParameter, Is.Not.Null, $"No key found in '{sessionKey}' with key '{key}'");

            return this;
        }

        public AlexaCoreTestRunner VerifySessionInputQueue(string value, string[] tags = null)
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

            return this;
        }

        protected List<T> GetSessionParameter<T>(string sessionKey)
        {
            ValidateHasRun();

            List<T> parameters = (List<T>)SkillResponse.SessionAttributes[sessionKey];

            return parameters;
        }
    }
}
