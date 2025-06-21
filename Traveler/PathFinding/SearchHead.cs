using Traveler.Objects;


namespace Traveler.PathFinding;


public class SearchHead<I, ND, CD> : IComparable<SearchHead<I, ND, CD>> where I : IComparable<I>
{
	#region Public Properties

	public int Index { get; set; } = 0;
	public int From { get; init; } = -1;
	public int StepsCount { get; init; } = 1;
	public required I Head { get; init; }
	public required Connection<I, ND, CD> Via { get; init; }
	public required double DistanceTravelled { get; init; }
	public required double DistanceRemaining { get; init; }
	
	public double TotalBestCaseDistance { get; }
	public Node<I, ND, CD> HeadNode => Via.ByID(Head);
	public Node<I, ND, CD> FromNode => Via.OtherByID(Head);
	
	#endregion
	

	#region Costructor

	public SearchHead()
	{
		TotalBestCaseDistance = DistanceTravelled + DistanceRemaining;
	}

	#endregion


	#region Public Methods

	public SearchHead<I, ND, CD> Next(
		int index,
		I head,
		Connection<I, ND, CD> via,
		double distanceRemaining)
	{
		return new SearchHead<I, ND, CD>
		{
			Index = index,
			Head = head,
			From = Index,
			Via = via,
			StepsCount = StepsCount + 1,
			DistanceTravelled = DistanceTravelled + via.Distance,
			DistanceRemaining = distanceRemaining
		};
	}

	#endregion


	#region Public Static Methods

	public static SearchHead<I, ND, CD> First(EnterPoint<I, ND, CD> ep, double remaining)
	{
		return new SearchHead<I, ND, CD>
		{
			Head = ep.ID,
			Via = default,
			DistanceTravelled = 0,
			DistanceRemaining = remaining
		};
	}

	#endregion
	

	#region IComparable Implementation

	public int CompareTo(SearchHead<I, ND, CD>? other)
	{
		if (other == null)
			throw new ArgumentNullException(nameof(other));
		
		return TotalBestCaseDistance.CompareTo(other.TotalBestCaseDistance);
	}

	#endregion
}