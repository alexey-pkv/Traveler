namespace Traveler.Objects;


public struct Shortcut<I, ND, CD>
	where I : IComparable<I>
{
	#region Properties
	
	public Node<I, ND, CD> To { get; set; }
	public Node<I, ND, CD> Via { get; set; }
	public double Distance { get; set; }
	public bool Has => To != null;

	#endregion
}