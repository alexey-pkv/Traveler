using Traveler.Exceptions;
using Traveler.Objects;


namespace Traveler.UnitTest.Objects;


[TestFixture]
public class NodeTest
{
	#region Private Methods
	
	private Node<int, object, object> CreateNode(int id)
	{
		return new Node<int, object, object>(id, null);
	}
	
	private Connection<int, object, object> CreateConnection(
		Node<int, object, object> node1, 
		Node<int, object, object> node2)
	{
		return new Connection<int, object, object>(node1, node2);
	}
	
	#endregion
	
	
	#region Test Connect
    
    [Test]
    public void Connect_ConnectionAddedWithCorrectKey()
    {
        var subject_1 = CreateNode(1);
        var subject_2 = CreateNode(2);
        var conn_1 = CreateConnection(subject_1, subject_2);
        
        
        subject_1.Connect(conn_1);
        subject_2.Connect(conn_1);
        
        
        Assert.AreEqual(1, subject_2.Neighbors.Count);
        Assert.IsTrue(subject_2.Neighbors.ContainsKey(1));
        
        Assert.AreEqual(1, subject_1.Neighbors.Count);
        Assert.IsTrue(subject_1.Neighbors.ContainsKey(2));
    }
    
    [Test]
    public void Connect_ConnectionReAdded_ExceptionThrown()
    {
        var subject_1 = CreateNode(1);
        var subject_2 = CreateNode(2);
        var conn_1 = CreateConnection(subject_1, subject_2);
        
        
        subject_1.Connect(conn_1);
        
        
        Assert.Throws<InvalidConnectionException>(() => subject_1.Connect(conn_1));
    }
    
    [Test]
    public void Connect_ConnectionWithoutNode_ExceptionThrown()
    {
        var subject_1 = CreateNode(1);
        var subject_2 = CreateNode(2);
        var subject_3 = CreateNode(3);
        var conn_1 = CreateConnection(subject_2, subject_3);
        
        Assert.Throws<InvalidConnectionException>(() => subject_1.Connect(conn_1));
    }
	
	#endregion
	
	
	#region Test HasNeighbor
    
    [Test]
    public void HasNeighbor_NeighborAdded_ReturnsTrue()
    {
        var subject_1 = CreateNode(1);
        var subject_2 = CreateNode(2);
        var conn_1 = CreateConnection(subject_1, subject_2);
        
        
        subject_1.Connect(conn_1);
        
        
        Assert.IsTrue(subject_1.HasNeighbor(subject_2));
    }
    
    [Test]
    public void HasNeighbor_NotANeighbor_ReturnsFalse()
    {
        var subject_1 = CreateNode(1);
        var subject_2 = CreateNode(2);
        var subject_3 = CreateNode(3);
        var conn_1 = CreateConnection(subject_1, subject_2);
        
        
        subject_1.Connect(conn_1);
        
        
        Assert.IsFalse(subject_1.HasNeighbor(subject_1));
        Assert.IsFalse(subject_1.HasNeighbor(subject_3));
    }
    
    #endregion
    
    
	#region ToString
    
    [Test]
    public void ToString_ValidStringReturned()
    {
        var subject_1 = CreateNode(1);
        
        Assert.AreEqual("<Node: 1>", subject_1.ToString());
    }
	
	#endregion
}