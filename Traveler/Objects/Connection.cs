namespace Traveler.Objects;


public class Connection<I, ND, CD> where I : IComparable<I>
{
	#region Public Properties
	
	public Node<I, ND, CD>	From	{ get; }
	public Node<I, ND, CD>	To		{ get; }
	public CD				Data	{ get; }
	
	#endregion
	
	
	#region Constructor
	
	public Connection(Node<I, ND, CD> from, Node<I, ND, CD> to, CD data)
	{
		if (from.ID.CompareTo(to.ID) < 0)
		{
			From = from;
			To = to;
		}
		else
		{
			From = to;
			To = from;
		}
		
		Data = data;
	}
	
	#endregion
	
	
	#region Methods
	
	public Node<I, ND, CD> Other(Node<I, ND, CD> other) => From == other ? To : From;
	
	#endregion
}