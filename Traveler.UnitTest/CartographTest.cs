using Traveler.Exceptions;
using Traveler.Objects;

namespace Traveler.UnitTest;


[TestFixture]
public class CartographTests
{
    #region Test Helper Classes
    
    private class TestRuler : IRuler<int, string, string>
    {
        public double Distance(Node<int, string, string> a, Node<int, string, string> b)
        {
            return Math.Abs(a.ID - b.ID);
        }
    }
    
    #endregion


    #region Private Helper Methods
    
    private TestRuler CreateTestRuler()
    {
        return new TestRuler();
    }
    
    private Cartograph<int, string, string> CreateSubject()
    {
        return new Cartograph<int, string, string>(CreateTestRuler());
    }
    
    #endregion


    #region AddNode Tests
    
    [Test]
    public void AddNode_ValidId_NodeCreated()
    {
        var subject = CreateSubject();
        
        var node = subject.AddNode(1, "TestData");
        
        Assert.IsNotNull(node);
        Assert.AreEqual(1, node.ID);
        Assert.AreEqual("TestData", node.Data);
    }
    
    [Test]
    public void AddNode_NoData_NodeCreatedWithDefaultData()
    {
        var subject = CreateSubject();
        
        var node = subject.AddNode(1);
        
        Assert.IsNotNull(node);
        Assert.AreEqual(1, node.ID);
        Assert.IsNull(node.Data);
    }
    
    [Test]
    public void AddNode_DuplicateId_ThrowsException()
    {
        var subject = CreateSubject();
        subject.AddNode(1, "FirstNode");
        
        Assert.Throws<ArgumentException>(() => subject.AddNode(1, "SecondNode"));
    }
    
    #endregion


    #region Connect Tests
    
    [Test]
    public void Connect_ConnectionCreated()
    {
        var subject = CreateSubject();
        var nodeA = subject.AddNode(1, "NodeA");
        var nodeB = subject.AddNode(2, "NodeB");
        
        
        var connection = subject.Connect(nodeA, nodeB, "TestConnection");
        
        
        Assert.IsNotNull(connection);
        Assert.AreEqual("TestConnection", connection.Data);
        Assert.IsTrue(nodeA.HasNeighbor(nodeB));
        Assert.IsTrue(nodeB.HasNeighbor(nodeA));
    }
    
    [Test]
    public void Connect_NoConnectionData_ConnectionCreatedWithDefaultData()
    {
        var subject = CreateSubject();
        var nodeA = subject.AddNode(1, "NodeA");
        var nodeB = subject.AddNode(2, "NodeB");
        
        
        var connection = subject.Connect(nodeA, nodeB);
        
        
        Assert.IsNotNull(connection);
        Assert.IsNull(connection.Data);
        Assert.IsTrue(nodeA.HasNeighbor(nodeB));
        Assert.IsTrue(nodeB.HasNeighbor(nodeA));
    }
    
    [Test]
    public void Connect_AlreadyConnectedNodes_ThrowsInvalidConnectionException()
    {
        var subject = CreateSubject();
        var nodeA = subject.AddNode(1, "NodeA");
        var nodeB = subject.AddNode(2, "NodeB");
        
        
        subject.Connect(nodeA, nodeB);
        
        
        Assert.Throws<InvalidConnectionException>(() => subject.Connect(nodeA, nodeB));
    }
    
    [Test]
    public void Connect_NodeNotAdded_ThrowsArgumentException()
    {
        var subject = CreateSubject();
        var nodeA = subject.AddNode(1, "NodeA");
        var nodeB = new Node<int, string, string>(2, "NodeB");
        
        Assert.Throws<ArgumentException>(() => subject.Connect(nodeA, nodeB));
    }
    
    [Test]
    public void Connect_BothNodesNotAdded_ThrowsArgumentException()
    {
        var subject = CreateSubject();
        var nodeA = new Node<int, string, string>(1, "NodeA"); // Not added to subject
        var nodeB = new Node<int, string, string>(2, "NodeB"); // Not added to subject
        
        Assert.Throws<ArgumentException>(() => subject.Connect(nodeA, nodeB));
    }
    
    [Test]
    public void Connect_DistanceCalculatedByRuler()
    {
        var subject = CreateSubject();
        var nodeA = subject.AddNode(1, "NodeA");
        var nodeB = subject.AddNode(2, "NodeB");
        
        
        var connection = subject.Connect(nodeA, nodeB);
        
        
        Assert.AreEqual(1.0, connection.Distance);
    }
    
    #endregion


    #region Build Tests
    
    [Test]
    public void Build_EmptyCartograph_ReturnsEmptyMap()
    {
        var subject = CreateSubject();
        
        var map = subject.Build();
        
        Assert.IsNotNull(map);
        Assert.IsTrue(map.IsEmpty);
        Assert.AreEqual(0, map.Count);
    }
    
    [Test]
    public void Build_NodesWithoutConnections_ReturnsMapWithNodes()
    {
        var subject = CreateSubject();
        var nodeA = subject.AddNode(1, "NodeA");
        var nodeB = subject.AddNode(2, "NodeB");
        
        var map = subject.Build();
        
        Assert.IsNotNull(map);
        Assert.IsFalse(map.IsEmpty);
        Assert.AreEqual(2, map.Count);
        Assert.IsTrue(map.Has(1));
        Assert.IsTrue(map.Has(2));
    }
    
    [Test]
    public void Build_ConnectedNodes_ReturnsMapWithConnections()
    {
        var subject = CreateSubject();
        var nodeA = subject.AddNode(1, "NodeA");
        var nodeB = subject.AddNode(2, "NodeB");
        
        
        subject.Connect(nodeA, nodeB);
        
        
        var map = subject.Build();
        
        Assert.IsNotNull(map);
        Assert.AreEqual(2, map.Count);
        
        var mapNodeA = map.Require(1);
        var mapNodeB = map.Require(2);
        
        Assert.IsTrue(mapNodeA.HasNeighbor(mapNodeB));
        Assert.IsTrue(mapNodeB.HasNeighbor(mapNodeA));
    }
    
    [Test]
    public void Build_MultipleNodes_ShortcutsCalculated()
    {
        var subject = CreateSubject();
        var nodeA = subject.AddNode(1, "NodeA");
        var nodeB = subject.AddNode(2, "NodeB");
        var nodeC = subject.AddNode(3, "NodeC");
        
        // Create a chain: A -> B -> C
        subject.Connect(nodeA, nodeB);
        subject.Connect(nodeB, nodeC);
        
        var map = subject.Build();
        
        Assert.IsNotNull(map);
        Assert.AreEqual(3, map.Count);
        
        // Verify nodes exist in map
        var mapNodeA = map.Require(1);
        var mapNodeB = map.Require(2);
        var mapNodeC = map.Require(3);
        
        // Verify connections exist
        Assert.IsTrue(mapNodeA.HasNeighbor(mapNodeB));
        Assert.IsTrue(mapNodeB.HasNeighbor(mapNodeA));
        Assert.IsTrue(mapNodeB.HasNeighbor(mapNodeC));
        Assert.IsTrue(mapNodeC.HasNeighbor(mapNodeB));
    }
    
    #endregion
}