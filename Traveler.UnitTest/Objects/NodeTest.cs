using Traveler.Exceptions;
using Traveler.UnitTest.Lib;


namespace Traveler.UnitTest.Objects;


[TestFixture]
public class NodeTest
{
	#region Test Connect
    
    [Test]
    public void Connect_ConnectionAddedWithCorrectKey()
    {
        var subject_1 = Create.Node(1);
        var subject_2 = Create.Node(2);
        
        Create.Connection(subject_1, subject_2);
        
        
        Assert.AreEqual(1, subject_2.Neighbors.Count);
        Assert.IsTrue(subject_2.Neighbors.ContainsKey(1));
        
        Assert.AreEqual(1, subject_1.Neighbors.Count);
        Assert.IsTrue(subject_1.Neighbors.ContainsKey(2));
    }
    
    [Test]
    public void Connect_ConnectionReAdded_ExceptionThrown()
    {
        var subject_1 = Create.Node(1);
        var subject_2 = Create.Node(2);
        var conn_1 = Create.Connection(subject_1, subject_2, connect_nodes: false);
        
        
        subject_1.Connect(conn_1);
        
        
        Assert.Throws<InvalidConnectionException>(() => subject_1.Connect(conn_1));
    }
    
    [Test]
    public void Connect_ConnectionWithoutNode_ExceptionThrown()
    {
        var subject_1 = Create.Node(1);
        var subject_2 = Create.Node(2);
        var subject_3 = Create.Node(3);
        var conn_1 = Create.Connection(subject_2, subject_3, connect_nodes: false);
        
        Assert.Throws<InvalidConnectionException>(() => subject_1.Connect(conn_1));
    }
	
	#endregion
	
	
	#region Test HasNeighbor
    
    [Test]
    public void HasNeighbor_NeighborAdded_ReturnsTrue()
    {
        var subject_1 = Create.Node(1);
        var subject_2 = Create.Node(2);
        var conn_1 = Create.Connection(subject_1, subject_2, connect_nodes: false);
        
        
        subject_1.Connect(conn_1);
        
        
        Assert.IsTrue(subject_1.HasNeighbor(subject_2));
    }
    
    [Test]
    public void HasNeighbor_NotANeighbor_ReturnsFalse()
    {
        var subject_1 = Create.Node(1);
        var subject_2 = Create.Node(2);
        var subject_3 = Create.Node(3);
        var conn_1 = Create.Connection(subject_1, subject_2, connect_nodes: false);
        
        
        subject_1.Connect(conn_1);
        
        
        Assert.IsFalse(subject_1.HasNeighbor(subject_1));
        Assert.IsFalse(subject_1.HasNeighbor(subject_3));
    }
    
    #endregion
    
    
	#region ToString
    
    [Test]
    public void ToString_ValidStringReturned()
    {
        var subject_1 = Create.Node(1);
        
        Assert.AreEqual("<Node: 1>", subject_1.ToString());
    }
	
	#endregion
}