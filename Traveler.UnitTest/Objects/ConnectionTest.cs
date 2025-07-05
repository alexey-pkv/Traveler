using Traveler.Objects;
using Traveler.UnitTest.Lib;


namespace Traveler.UnitTest.Objects;


[TestFixture]
public class ConnectionTest
{
	#region Test Constructor
	
	[Test]
	public void Test_Sanity()
	{
		var node_1 = Create.Node(1);
		var node_2 = Create.Node(2);
		
		
		var conn = new Connection<int, int, int>(node_1, node_2)
		{
			Distance = 23,
			Data = 12
		};
		
		
		Assert.AreEqual(node_1, conn.From);
		Assert.AreEqual(node_2, conn.To);
		Assert.AreEqual(23, conn.Distance);
		Assert.AreEqual(12, conn.Data);
	}
	
	[Test]
	public void Test_Constructor_NodesOrderedByID()
	{
		var node_1 = Create.Node(1);
		var node_2 = Create.Node(2);
		
		
		var conn_12 = new Connection<int, int, int>(node_1, node_2);
		var conn_21 = new Connection<int, int, int>(node_2, node_1);
		
		
		Assert.AreEqual(conn_21.From, conn_12.From);
		Assert.AreEqual(conn_21.To, conn_12.To);
	}
	
	#endregion
	
	
	#region Test Has
	
	
	[Test]
	public void Test_Has()
	{
		var node_1 = Create.Node(1);
		var node_2 = Create.Node(2);
		var node_3 = Create.Node(3);
		
		
		var conn = new Connection<int, int, int>(node_1, node_2);
		
		
		Assert.IsTrue(conn.Has(1));
		Assert.IsTrue(conn.Has(2));
		Assert.IsFalse(conn.Has(3));
		
		Assert.IsTrue(conn.Has(node_1));
		Assert.IsTrue(conn.Has(node_2));
		Assert.IsFalse(conn.Has(node_3));
	}
	
	#endregion
	
	
	#region Test Other
	
	
	[Test]
	public void Test_Other()
	{
		var node_1 = Create.Node(1);
		var node_2 = Create.Node(2);
		
		
		var conn = new Connection<int, int, int>(node_1, node_2);
		
		
		Assert.AreSame(node_2, conn.Other(node_1));
		Assert.AreSame(node_1, conn.Other(node_2));
	}
	
	#endregion
	
	
	#region Test OtherByID
	
	
	[Test]
	public void Test_OtherByID()
	{
		var node_1 = Create.Node(1);
		var node_2 = Create.Node(2);
		
		
		var conn = new Connection<int, int, int>(node_1, node_2);
		
		
		Assert.AreSame(node_2, conn.OtherByID(1));
		Assert.AreSame(node_1, conn.OtherByID(2));
	}
	
	#endregion
	
	
	#region Test ByID
	
	
	[Test]
	public void Test_ByID()
	{
		var node_1 = Create.Node(1);
		var node_2 = Create.Node(2);
		
		
		var conn = new Connection<int, int, int>(node_1, node_2);


		Assert.AreSame(node_1, conn.ByID(1));
		Assert.AreSame(node_2, conn.ByID(2));
	}
	
	
	[Test]
	public void Test_ByID_InvalidID_ExceptionThrown()
	{
		var node_1 = Create.Node(1);
		var node_2 = Create.Node(2);
		
		
		var conn = new Connection<int, int, int>(node_1, node_2);


		Assert.Throws<ArgumentException>(() => conn.ByID(3));
	}
	
	#endregion
}