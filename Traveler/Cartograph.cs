using Traveler.Exceptions;
using Traveler.Objects;


namespace Traveler;


public class Cartograph<I, ND, CD>(IRuler<I, ND, CD> ruler)
	where I : IComparable<I>
{
	#region Private Data Members

	private readonly Dictionary<I, Node<I, ND, CD>> m_nodes = new();

	#endregion
	
	
	#region Private Methods
	
	private void CalcShortcuts(Map<I, ND, CD> map, CancellationToken ct)
	{
		Queue<Node<I, ND, CD>> queue = new();
		
		foreach (var nodes in map)
		{
			queue.Enqueue(nodes);
		}

		while (queue.Count > 0 && !ct.IsCancellationRequested)
		{
			var next = queue.Dequeue();
			var shortcut = next.Shortcut;
			
			foreach (var neighbor in next.Neighbors.Values)
			{
				var other = neighbor.Other(next);
				var distance = shortcut.Distance + neighbor.Distance;
				
				if (other.IsShortcut)
					continue;
				if (other.HasShortcut && other.Shortcut.Distance <= distance)
					continue;
				
				other.Shortcut = new Shortcut<I, ND, CD>
				{
					To	= shortcut.To,
					Via	= next,
					Distance = distance
				};
				
				queue.Enqueue(other);
			}
		}
	}
	
	#endregion
	
	
	#region Methods
	
	public Node<I, ND, CD> AddNode(I i, ND? data = default)
	{
		var node = new Node<I, ND, CD>(i, data);
		
		m_nodes.Add(i, node);

		return node;
	}

	public Connection<I, ND, CD> Connect(Node<I, ND, CD> a, Node<I, ND, CD> b, CD? data = default)
	{
		if (a.HasNeighbor(b))
			throw new InvalidConnectionException($"Nodes {a} and {b} already connected");
		
		if (!m_nodes.TryGetValue(a.ID, out _))
			throw new ArgumentException("Node {a.ID} was not added");
		if (!m_nodes.TryGetValue(b.ID, out _))
			throw new ArgumentException("Node {a.ID} was not added");
		
		var connection = new Connection<I, ND, CD>(a, b)
		{
			Data = data,
			Distance = ruler.Distance(a, b)
		};

		a.Connect(connection);
		b.Connect(connection);
		
		return connection;
	}
	
	public Map<I, ND, CD> Build(CancellationToken ct = default)
	{
		var map = new Map<I, ND, CD>(m_nodes);
		
		CalcShortcuts(map, ct);
		
		return map;
	}

	#endregion
}