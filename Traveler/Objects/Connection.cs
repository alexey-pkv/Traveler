namespace Traveler.Objects;


public readonly struct Connection<I, ND, CD> where I : IComparable<I>
{
	#region Public Properties
	
	public Node<I, ND, CD>	From		{ get; }
	public Node<I, ND, CD>	To			{ get; }
	public CD?				Data		{ get; init; }
	public double			Distance	{ get; init; }
	
	#endregion
	
	
	#region Constructor
	
	public Connection(Node<I, ND, CD> from, Node<I, ND, CD> to)
	{
		if (from.ID.CompareTo(to.ID) <= 0)
		{
			From = from;
			To = to;
		}
		else
		{
			From = to;
			To = from;
		}
	}
	
	#endregion
	
	
	#region Methods
	
	public bool Has(Node<I, ND, CD> node) => node == From || node == To;
	public bool Has(I id) => From.ID.Equals(id) || To.ID.Equals(id);
	public Node<I, ND, CD> Other(Node<I, ND, CD> other) => From == other ? To : From;
	public override string ToString() => $"<Connection: {From} -> {To}>";
	
	#endregion
}