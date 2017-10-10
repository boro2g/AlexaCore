using System;
using System.Collections.Generic;
using System.Linq;
using Alexa.NET.Request;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlexaCore
{
    public class PersistentQueue<T> : IResettable
    {
	    private readonly ILambdaLogger _logger;

	    private readonly Session _session;

	    private static List<T> _fauxList;
	    
	    public PersistentQueue(ILambdaLogger logger, Session session, string key)
	    {
		    _fauxList = new List<T>();

			_logger = logger;

			_session = session;

		    Key = "_PersistentQueue_" + key;

			if (!_session.Attributes.ContainsKey(Key))
		    {
				//_logger.LogLine("New PersistentQueue " + _commandQueueKey);

				_session.Attributes[Key] = new List<T>();

			    _fauxList = new List<T>();
			}
		    else if (_session.Attributes.ContainsKey(Key))
		    {
			    //_logger.LogLine("Existing PersistentQueue " + _commandQueueKey);

				var stack = _session.Attributes[Key];

				var parsedArray = JArray.Parse(JsonConvert.SerializeObject(stack));

				_session.Attributes[Key] = parsedArray.ToObject<List<T>>();

			    _fauxList = Items;
		    }
	    }

	    public void Enqueue(T name)
	    {
		    _fauxList.Add(name);

		    Items = _fauxList;
	    }

	    public T Find(Func<T, bool> selector)
	    {
		    return _fauxList.FirstOrDefault(selector);
	    }

	    public void Update(T newItem, Func<T, bool> selector, bool addIfMissing = false)
	    {
		    int index = 0;

		    bool match = false;

		    foreach (var item in _fauxList)
		    {
			    if (selector(item))
			    {
				    match = true;

					break;
			    }

			    index++;
		    }

		    if (match)
		    {
			    _fauxList[index] = newItem;
		    }
		    else
		    {
			    if (addIfMissing)
			    {
				    _fauxList.Add(newItem);
			    }
		    }

		    Items = _fauxList;
	    }

	    public T LastItem()
	    {
			return _fauxList.LastOrDefault();
	    }

	    public IEnumerable<T> Entries()
	    {
		    return _fauxList;
	    }

	    public void Debug(string prefix = "")
	    {
		    _logger.LogLine(
			    $"{prefix} {Key} {JsonConvert.SerializeObject(_session.Attributes[Key])} ------ {JsonConvert.SerializeObject(_fauxList)}");
	    }

	    public string Key { get; }

	    private List<T> Items
	    {
		    get => _session.Attributes[Key] as List<T>;
		    set => _session.Attributes[Key] = value;
	    }

	    public void Reset()
	    {
			_logger.LogLine("Resetting " + Key);

		    _fauxList = new List<T>();

			_session.Attributes[Key] = new List<T>();
		}
    }
}
