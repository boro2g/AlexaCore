using AlexaCore.Extensions;
using NUnit.Framework;

namespace AlexaCore.Tests.Extensions
{
	[TestFixture]
    class EnumerableExtensionsTests
    {
	    [Test]
	    public void IfListIsEmpty_EmptyResult()
	    {
		    var emptyList = new string[0];

		    var result = emptyList.JoinStringList();

		    Assert.That(result, Is.EqualTo(""));
	    }

		[Test]
		public void IfListContainsOneItem_ThatsAllYouGet()
		{
			var emptyList = new[] { "item 1" };

			var result = emptyList.JoinStringList();

			Assert.That(result, Is.EqualTo("item 1"));
		}

	    [Test]
	    public void IfListContainsTwoItems_TheLastDividerSplitsThem()
	    {
		    var emptyList = new[] { "item 1" , "item 2"};

		    var result = emptyList.JoinStringList();

		    Assert.That(result, Is.EqualTo("item 1 and item 2"));

		    result = emptyList.JoinStringList(lastDivider:" custom ");

		    Assert.That(result, Is.EqualTo("item 1 custom item 2"));
		}

	    [Test]
	    public void IfListContainsMoreThanTwoItems_BothDividersAreUsed()
	    {
		    var emptyList = new[] { "item 1", "item 2", "item 3" };

		    var result = emptyList.JoinStringList();

		    Assert.That(result, Is.EqualTo("item 1, item 2 and item 3"));

		    result = emptyList.JoinStringList("| ", " custom ");

		    Assert.That(result, Is.EqualTo("item 1| item 2 custom item 3"));
	    }
	}
}
