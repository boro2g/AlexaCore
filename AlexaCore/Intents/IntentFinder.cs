﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AlexaCore.Intents
{
    public static class IntentFinder
    {
	    public static IEnumerable<AlexaIntent> FindIntents(IEnumerable<Assembly> sourceAssemblies, Func<AlexaIntent, bool> exclusionFilter = null)
	    {
		    var intents = new List<AlexaIntent>();

		    foreach (var assembly in sourceAssemblies)
		    {
			    var intentBases = assembly.DefinedTypes
					.Where(type => typeof(AlexaIntent).GetTypeInfo().IsAssignableFrom(type.AsType()) && !type.IsAbstract);

			    intents.AddRange(intentBases.Select(intent => Activator.CreateInstance(intent.UnderlyingSystemType) as AlexaIntent));
		    }

		    if (exclusionFilter != null)
		    {
			    intents = intents.Where(exclusionFilter).ToList();
		    }

		    return intents;
	    }

        public static IEnumerable<Type> FindIntentTypes(IEnumerable<Assembly> sourceAssemblies, Func<Type, bool> exclusionFilter = null)
        {
            var intents = new List<Type>();

            foreach (var assembly in sourceAssemblies)
            {
                var intentBases = assembly.DefinedTypes
                    .Where(type => typeof(AlexaIntent).GetTypeInfo().IsAssignableFrom(type.AsType()) && !type.IsAbstract);

                intents.AddRange(intentBases.Select(intent => intent));
            }

            if (exclusionFilter != null)
            {
                intents = intents.Where(exclusionFilter).ToList();
            }

            return intents;
        }
    }
}
