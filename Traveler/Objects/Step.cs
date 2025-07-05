namespace Traveler.Objects;


public readonly struct Step<I, ND, CD>(Connection<I, ND, CD> connection, Node<I, ND, CD> from)
	where I : struct, IComparable<I>
{
	#region Properties
	
	public Connection<I, ND, CD> Connection { get; } = connection;
	public double DistanceTraversed { get; } = connection.Distance;
	public Node<I, ND, CD> From { get; } = from;

	public double Distance => Connection.Distance;
	public Node<I, ND, CD> To => Connection.Other(From);
	
	#endregion
	
	
	#region Public Methods
	
	public Step<I, ND, CD> Flip() => new(Connection, To);

	#endregion


	#region Public Static Methods

	public static Step<I, ND, CD> FromShortcut(Node<I, ND, CD> node, bool flip = false)
	{
		var step = new Step<I, ND, CD>(node.Neighbors[node.Shortcut.Via.ID], node);
		
		return flip ? step.Flip() : step;
	}
	
	public static void BuildShortcutPath(List<Step<I, ND, CD>> into, Node<I, ND, CD>? from, bool flip = false)
	{
		if (from == null)
			throw new ArgumentNullException(nameof(from));
		
		int offset = into.Count;
		
		while (!from.IsShortcut)
		{
			if (!from.HasShortcut)
				throw new InvalidDataException("Cannot build shortcut");

			into.Add(FromShortcut(from, flip));
			from = from.Shortcut.Via;
		}

		if (flip)
		{
			into.Reverse(offset, into.Count - offset);
		}
	}
	
	#endregion
}