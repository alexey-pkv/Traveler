namespace Traveler.Objects;


public class SearchStep<I, ND, CD> where I : IComparable<I>
{
	#region Properties
	
	public required Node<I, ND, CD> Node { get; init; }
	public required SearchStep<I, ND, CD>? From { get; init; }
	public required EnterPoint<I, ND, CD> Root { get; init; }
	public required double DistanceSoFar { get; init; }
	public required double DistanceBestCase { get; init; }


	public bool IsEntryPoint => From is null;
	public I ID => Node.ID;
	
	#endregion
	
	
	#region Methods

	public SearchStep<I, ND, CD> Next(Connection<I, ND, CD> to, double distanceBestCase)
	{
		return new SearchStep<I, ND, CD>
		{
			Node = to.Other(Node),
			From = this,
			Root = Root,
			DistanceBestCase = distanceBestCase,
			DistanceSoFar = DistanceSoFar + to.Distance,
		};
	}
	
	
	public static SearchStep<I, ND, CD> Create(EnterPoint<I, ND, CD> root, double distanceBestCase)
	{
		return new SearchStep<I, ND, CD>
		{
			Node = root.Node,
			From = null,
			Root = root,
			DistanceBestCase = distanceBestCase,
			DistanceSoFar = root.Offset,
		};
	}
	
	#endregion
}