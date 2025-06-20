namespace Traveler.Objects;


public readonly struct Step<I, ND, CD>(Connection<I, ND, CD> connection, Node<I, ND, CD> from)
	where I : IComparable<I>
{
	#region Properties
	
	public Connection<I, ND, CD> Connection { get; init; } = connection;
	public double DistanceTraversed { get; init; } = connection.Distance;
	public Node<I, ND, CD> From { get; init; } = from;

	public double Distance => Connection.Distance;
	public Node<I, ND, CD> To => Connection.Other(From);
	
	#endregion
}