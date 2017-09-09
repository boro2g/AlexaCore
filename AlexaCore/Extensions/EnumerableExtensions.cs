using System;
using System.Collections.Generic;
using System.Linq;

namespace AlexaCore.Extensions
{
	public static class EnumerableExtension
	{
		public static T PickRandom<T>(this IEnumerable<T> source)
		{
			return source.PickRandom(1).Single();
		}

		public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
		{
			return source.Shuffle().Take(count);
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.OrderBy(x => Guid.NewGuid());
		}

		public static string JoinStringList(this IEnumerable<string> items, string divider = ", ",
			string lastDivider = " and ")
		{
			if (items == null || !items.Any())
			{
				return "";
			}

			if (items.Count() == 1)
			{
				return String.Join(divider, items);
			}

			return String.Join(divider, items.Take(items.Count() - 1)) + lastDivider + items.Last();
		}
	}
}
