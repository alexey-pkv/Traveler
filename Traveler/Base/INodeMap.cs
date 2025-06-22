using Traveler.Objects;


namespace Traveler.Base;


public interface INodeMap<I, ND, CD> where I : struct, IComparable<I>
{
	public int Count { get; }
	public bool IsEmpty { get; }
	public bool Has(I id);
	
	public Node<I, ND, CD>? Get(I id);
	public Node<I, ND, CD> Require(I id);

	public bool TryGet(I id, out Node<I, ND, CD>? node);
}