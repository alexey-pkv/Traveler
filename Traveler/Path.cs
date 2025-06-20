using System.Collections;
using Traveler.Objects;


namespace Traveler;


public class Path<I, ND, CD>(double distance = 0.0) : IEnumerable 
	where I : IComparable<I>
{
	#region Static Data Members

	public readonly static Path<I, ND, CD> NOT_FOUND = new (double.PositiveInfinity);

	#endregion
	
	
	#region Properties
	
	public int Count => Steps.Count;
	public bool IsFound => !double.IsPositiveInfinity(Distance);
	public double Distance { get; set; } = distance;
	public List<Step<I, ND, CD>> Steps { get; } = [];
	public EnterPoint<I, ND, CD> From { get; set; }
	public EnterPoint<I, ND, CD> To { get; set; }
	
	#endregion
	
	
	#region Enumerator 
	
	public IEnumerator<Step<I, ND, CD>> GetEnumerator()
	{
		return Steps.GetEnumerator();
	}
	
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
	
	#endregion
}