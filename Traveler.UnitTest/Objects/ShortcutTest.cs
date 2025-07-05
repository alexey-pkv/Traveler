using Traveler.Objects;


namespace Traveler.UnitTest.Objects;


[TestFixture]
public class ShortcutTests
{
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