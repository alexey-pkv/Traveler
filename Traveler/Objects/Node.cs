using Traveler.Exceptions;


namespace Traveler.Objects;


public class Node<I, ND, PD>(I id, ND? data)
	where I : struct, IComparable<I>
{
	#region Properties
	
	public I ID { get; } = id;
	public ND? Data { get; } = data;
	public bool IsShortcut { get; set; }
	public Shortcut<I, ND, PD> Shortcut { get; set; }
	public Dictionary<I, Connection<I, ND, PD>> Neighbors { get; } = new();

	public bool HasShortcut => Shortcut.Has;
	
	#endregion
	
	
	#region Methods
	
	public void Connect(Connection<I, ND, PD> connection)
	{
		if (connection.Has(this))
		{
			throw new InvalidConnectionException(
				"The given connection is not connected to the current node. " + 
				$"Got {this} and {connection}.");
		}
		else
		{
			Neighbors[connection.Other(this).ID] = connection;
		}
	}
	
	public bool HasNeighbor(Node<I, ND, PD> to) => Neighbors.ContainsKey(to.ID);
	
	public override string ToString() => $"<Node: {ID}>";

	#endregion
}