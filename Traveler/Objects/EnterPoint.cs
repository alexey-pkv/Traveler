namespace Traveler.Objects;


public readonly struct EnterPoint<I, ND, CD>(Node<I, ND, CD> node, double offset = 0.0, object? metaData = null)
	where I : IComparable<I>
{
	#region Properties
	
	public Node<I, ND, CD> Node { get; } = node;
	public double Offset { get; } = offset;
	public object? MetaData { get; init; } = metaData;

	#endregion
}