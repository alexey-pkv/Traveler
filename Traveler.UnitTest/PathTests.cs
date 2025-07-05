using Traveler.Objects;


namespace Traveler.UnitTest;


[TestFixture]
public class PathTests
{
	[Test]
	public void EmptyPath_SanityCheck()
	{
		var path = new Path<int, string, string>();
		
		Assert.AreEqual(0, path.Count);
		Assert.AreEqual(double.PositiveInfinity, path.Distance);
		Assert.IsFalse(path.IsFound);
		Assert.IsEmpty(path.Steps);
		Assert.AreEqual(0, path.From.ID);
		Assert.AreEqual(0, path.To.ID);
	}
	
	[Test]
	public void DistanceSet_DistanceUsed()
	{
		var path = new Path<int, string, string>(distance: 23.4);
		
		Assert.AreEqual(23.4, path.Distance);
	}
	
	[Test]
	public void CapacitySet_CapacityUsed()
	{
		var path = new Path<int, string, string>(capacity: 23);
		
		Assert.AreEqual(23, path.Steps.Capacity);
	}
	
	[Test]
	public void IsFound_NotFound_ReturnFalse()
	{
		var path = new Path<int, string, string>(distance: double.PositiveInfinity);
		
		Assert.IsFalse(path.IsFound);
	}
	
	[Test]
	public void IsFound_Found_ReturnTrue()
	{
		Assert.IsTrue((new Path<int, string, string>(distance: 123.2)).IsFound);
		Assert.IsTrue((new Path<int, string, string>(distance: 0.0)).IsFound);
	}
		[Test]
	public void Enumeration_EmptySet()
	{
		var path = new Path<int, string, string>();
		var steps = new List<Step<int, string, string>>();
		
		foreach (var step in path)
		{
			steps.Add(step);
		}
		
		Assert.IsEmpty(steps);
	}
	
	[Test]
	public void Enumeration_DataExists_DataEnumerated()
	{
		var nodeA = new Node<int, string, string>(1, "NodeA");
		var nodeB = new Node<int, string, string>(2, "NodeB");
		var nodeC = new Node<int, string, string>(3, "NodeC");
		
		var connectionAB = new Connection<int, string, string>(nodeA, nodeB) { Distance = 10.0 };
		var connectionBC = new Connection<int, string, string>(nodeB, nodeC) { Distance = 15.0 };
		
		var stepAB = new Step<int, string, string>(connectionAB, nodeA);
		var stepBC = new Step<int, string, string>(connectionBC, nodeB);
		
		var path = new Path<int, string, string>(distance: 25.0);
		
		path.Steps.Add(stepAB);
		path.Steps.Add(stepBC);
		
		var enumeratedSteps = new List<Step<int, string, string>>();
		
		foreach (var step in path)
		{
			enumeratedSteps.Add(step);
		}
		
		Assert.AreEqual(2, enumeratedSteps.Count);
		Assert.AreEqual(stepAB, enumeratedSteps[0]);
		Assert.AreEqual(stepBC, enumeratedSteps[1]);
	}
}