namespace Traveler.Objects;


public readonly struct Shortcut<I, ND, CD>
	where I : IComparable<I>
{
	#region Properties
	
	public Node<I, ND, CD> To { get; init; }
	public Node<I, ND, CD> Via { get; init; }
	public double Distance { get; init; }
	public bool Has => To != null;

	#endregion
}