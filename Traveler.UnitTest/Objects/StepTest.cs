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
	
	#region Test BuildShortcutPath
	
	[Test]
	public void BuildShortcutPath_FlipFalse_Test()
	{
	    var node_1 = CreateTestNode(1, 10);
	    var node_2 = CreateTestNode(2, 20);
	    var node_3 = CreateTestNode(3, 30);
	    var node_4 = CreateTestNode(4, 40);
	    
	    // Set up connections
	    var connection_1 = CreateConnection(node_1, node_2, 12);
	    var connection_2 = CreateConnection(node_2, node_3, 23);
	    var connection_3 = CreateConnection(node_3, node_4, 34);
	    
	    // Set up shortcuts chain: node_4 -> node_3 -> node_2 -> node_1 (shortcut)
	    node_1.IsShortcut = true;
	    
	    node_2.Shortcut = CreateShortcut(node_1, node_1, 10);
	    node_3.Shortcut = CreateShortcut(node_1, node_2, 20);
	    node_4.Shortcut = CreateShortcut(node_1, node_3, 30);
	    
	    var steps = new List<Step<int, int, int>>();
	    
	    
	    Step<int, int, int>.BuildShortcutPath(steps, node_4, false);
	    
	    
	    Assert.AreEqual(3, steps.Count);
	    
	    // First step: node_4 -> node_3
	    Assert.AreEqual(node_4, steps[0].From);
	    Assert.AreEqual(node_3, steps[0].To);
	    Assert.AreEqual(connection_3, steps[0].Connection);
	    
	    // Second step: node_3 -> node_2  
	    Assert.AreEqual(node_3, steps[1].From);
	    Assert.AreEqual(node_2, steps[1].To);
	    Assert.AreEqual(connection_2, steps[1].Connection);
	    
	    // Third step: node_2 -> node_1
	    Assert.AreEqual(node_2, steps[2].From);
	    Assert.AreEqual(node_1, steps[2].To);
	    Assert.AreEqual(connection_1, steps[2].Connection);
	}

	[Test]
	public void BuildShortcutPath_FlipTrue_Test()
	{
	    var node_1 = CreateTestNode(1, 10);
	    var node_2 = CreateTestNode(2, 20);
	    var node_3 = CreateTestNode(3, 30);
	    var node_4 = CreateTestNode(4, 40);
	    
	    // Set up connections
	    var connection_1 = CreateConnection(node_1, node_2, 12);
	    var connection_2 = CreateConnection(node_2, node_3, 23);
	    var connection_3 = CreateConnection(node_3, node_4, 34);
	    
	    // Set up shortcuts chain: node_4 -> node_3 -> node_2 -> node_1 (shortcut)
	    node_1.IsShortcut = true;
	    
	    node_2.Shortcut = CreateShortcut(node_1, node_1, 10);
	    node_3.Shortcut = CreateShortcut(node_1, node_2, 20);
	    node_4.Shortcut = CreateShortcut(node_1, node_3, 30);
	    
	    var steps = new List<Step<int, int, int>>();
	    
	    
	    Step<int, int, int>.BuildShortcutPath(steps, node_4, true);
	    
	    
	    Assert.AreEqual(3, steps.Count);
	    
	    // First step: node_1 -> node_2
	    Assert.AreEqual(node_1, steps[0].From);
	    Assert.AreEqual(node_2, steps[0].To);
	    Assert.AreEqual(connection_1, steps[0].Connection);
	    
	    // Second step: node_2 -> node_3
	    Assert.AreEqual(node_2, steps[1].From);
	    Assert.AreEqual(node_3, steps[1].To);
	    Assert.AreEqual(connection_2, steps[1].Connection);
	    
	    // Third step: node_3 -> node_4
	    Assert.AreEqual(node_3, steps[2].From);
	    Assert.AreEqual(node_4, steps[2].To);
	    Assert.AreEqual(connection_3, steps[2].Connection);
	}

	#endregion
}