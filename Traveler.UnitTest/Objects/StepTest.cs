using Traveler.Objects;


namespace Traveler.UnitTest.Objects;


[TestFixture]
public class StepTest
{
	#region Private Helper Methods
	
	private Node<int, int, int> CreateTestNode(int id, int data)
	{
		return new Node<int, int, int>(id, data);
	}
	
	private Connection<int, int, int> CreateConnection(
		Node<int, int, int> node1, 
		Node<int, int, int> node2,
		int distance,
		bool connect = true)
	{
		var conn = new Connection<int, int, int>(node1, node2) { Distance = distance };
		
		if (connect)
		{
			node1.Connect(conn);
			node2.Connect(conn);
		}
		
		return conn;
	}
	
	private Shortcut<int, int, int> CreateShortcut(
		Node<int, int, int> to, 
		Node<int, int, int> via,
		int distance)
	{
		return new Shortcut<int, int, int>
		{
			To = to,
			Via = via,
			Distance = distance
		};
	}
	
	#endregion

	
	#region Sanity
	
	[Test]
	public void Sanity_Test()
	{
		var node_1 = CreateTestNode(1, 10);
		var node_2 = CreateTestNode(2, 20);
		var connection_1 = CreateConnection(node_1, node_2, 12);
		
		
		var subject = new Step<int, int, int>(connection_1, node_1);
		
	
		Assert.AreEqual(connection_1, subject.Connection);
		Assert.AreEqual(12, subject.DistanceTraversed);
		Assert.AreEqual(node_1, subject.From);
		Assert.AreEqual(node_2, subject.To);
	}
	
	[Test]
	public void Sanity_FromToSetupCorrectly()
	{
		var node_1 = CreateTestNode(1, 10);
		var node_2 = CreateTestNode(2, 20);
		var connection_1 = CreateConnection(node_1, node_2, 12);
		
		
		var subject_1 = new Step<int, int, int>(connection_1, node_1);
		var subject_2 = new Step<int, int, int>(connection_1, node_2);
		
		
		Assert.AreEqual(node_1, subject_1.From);	
		Assert.AreEqual(node_1, subject_2.To);
	
		Assert.AreEqual(node_2, subject_1.To);
		Assert.AreEqual(node_2, subject_2.From);
	}
	
	#endregion
	
	#region Test Flip
	
	[Test]
	public void Flip_Test()
	{
		var node_1 = CreateTestNode(1, 10);
		var node_2 = CreateTestNode(2, 20);
		var connection_1 = CreateConnection(node_1, node_2, 12);
		
		
		var subject_1 = new Step<int, int, int>(connection_1, node_1);
		var subject_2 = subject_1.Flip();
		
		
		Assert.AreEqual(subject_1.From,	subject_2.To);	
		Assert.AreEqual(subject_1.To,	subject_2.From);
		Assert.AreEqual(connection_1,	subject_2.Connection);
		Assert.AreEqual(12,				subject_2.DistanceTraversed);
	}
	
	#endregion
	
	
	#region Test FromShortcut
	
	[Test]
	public void FromShortcut_Test()
	{
		var node_1 = CreateTestNode(1, 10);
		var node_2 = CreateTestNode(2, 20);
		var node_3 = CreateTestNode(3, 30);
		
		var connection_1 = CreateConnection(node_1, node_2, 12);
		var connection_2 = CreateConnection(node_2, node_3, 12);
		
		var shortcut = CreateShortcut(node_1, node_2, 33);
		
		node_3.Shortcut = shortcut;
		
		
		var subject_1 = Step<int, int, int>.FromShortcut(node_3, false);
		var subject_2 = Step<int, int, int>.FromShortcut(node_3, true);
		
		
		Assert.AreEqual(node_2,			subject_1.To);	
		Assert.AreEqual(node_3,			subject_1.From);
		Assert.AreEqual(connection_2,	subject_1.Connection);
		
		Assert.AreEqual(node_3,			subject_2.To);	
		Assert.AreEqual(node_2,			subject_2.From);
		Assert.AreEqual(connection_2,	subject_2.Connection);
		
		// Distance should not be set
		Assert.AreEqual(12, subject_1.DistanceTraversed);
		Assert.AreEqual(12, subject_2.DistanceTraversed);
	}
	
	#endregion
}