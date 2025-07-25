﻿using Traveler.Objects;
using Traveler.UnitTest.Lib;


namespace Traveler.UnitTest.Objects;


[TestFixture]
public class EnterPointTests
{
	#region Constructor Tests
	
	[Test]
	public void Constructor_DefaultValues_PropertiesSetCorrectly()
	{
		var enterPoint = new EnterPoint<int, string, string>();
		
		Assert.IsNull(enterPoint.Node);
		Assert.AreEqual(0.0, enterPoint.Offset);
		Assert.IsNull(enterPoint.MetaData);
		Assert.AreEqual(0, enterPoint.ID);
	}
	
	[Test]
	public void Constructor_WithNode_PropertiesSetCorrectly()
	{
		var node = Create.Node(1, 2);
		var enterPoint = new EnterPoint<int, int, int>(node);
		
		Assert.AreEqual(node, enterPoint.Node);
		Assert.AreEqual(0.0, enterPoint.Offset);
		Assert.IsNull(enterPoint.MetaData);
		Assert.AreEqual(1, enterPoint.ID);
	}
	
	[Test]
	public void Constructor_WithNodeAndOffset_PropertiesSetCorrectly()
	{
		var node = Create.Node(1, 2);
		var enterPoint = new EnterPoint<int, int, int>(node, 5.5);
		
		Assert.AreEqual(node, enterPoint.Node);
		Assert.AreEqual(5.5, enterPoint.Offset);
		Assert.IsNull(enterPoint.MetaData);
		Assert.AreEqual(1, enterPoint.ID);
	}
	
	[Test]
	public void Constructor_WithAllParameters_PropertiesSetCorrectly()
	{
		var node = Create.Node(1, 2);
		var metaData = 3;
		var enterPoint = new EnterPoint<int, int, int>(node, 10.0, metaData);
		
		Assert.AreEqual(node, enterPoint.Node);
		Assert.AreEqual(10.0, enterPoint.Offset);
		Assert.AreEqual(metaData, enterPoint.MetaData);
		Assert.AreEqual(1, enterPoint.ID);
	}
	
	#endregion


	#region ID Property Tests
	
	[Test]
	public void ID_NullNode_ReturnsDefault()
	{
		var enterPoint = new EnterPoint<int, string, string>();
		
		Assert.AreEqual(0, enterPoint.ID);
	}
	
	[Test]
	public void ID_WithNode_ReturnsNodeID()
	{
		var node = Create.Node(1, 2);
		var enterPoint = new EnterPoint<int, int, int>(node);
		
		Assert.AreEqual(1, enterPoint.ID);
	}
	
	#endregion
}