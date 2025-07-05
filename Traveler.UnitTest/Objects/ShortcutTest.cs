using Traveler.Objects;


namespace Traveler.UnitTest.Objects;


[TestFixture]
public class ShortcutTests
{
	#region Sanity
	
	[Test]
	public void Constructor_WithProperties_PropertiesSetCorrectly()
	{
		var toNode = new Node<int, int, int>(1, 10);
		var viaNode = new Node<int, int, int>(2, 20);
		var shortcut = new Shortcut<int, int, int>
		{
			To = toNode,
			Via = viaNode,
			Distance = 15.5
		};
		
		Assert.AreEqual(toNode, shortcut.To);
		Assert.AreEqual(viaNode, shortcut.Via);
		Assert.AreEqual(15.5, shortcut.Distance);
		Assert.IsTrue(shortcut.Has);
	}
	
	#endregion
	
	#region Has Property Tests
	
	[Test]
	public void Has_NullTo_ReturnsFalse()
	{
		var shortcut = new Shortcut<int, int, int>();
		
		Assert.IsFalse(shortcut.Has);
	}
	
	[Test]
	public void Has_WithTo_ReturnsTrue()
	{
		var toNode = new Node<int, int, int>(1, 1);
		var shortcut = new Shortcut<int, int, int> { To = toNode };
		
		Assert.IsTrue(shortcut.Has);
	}
	
	#endregion
}