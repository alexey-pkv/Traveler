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
}