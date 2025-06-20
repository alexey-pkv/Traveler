using Traveler.Objects;


namespace Traveler;


public class Cartograph<I, ND, CD>(IRuler<I, ND, CD> ruler)
	where I : IComparable<I>
{
	#region Properties

	public Map<I, ND, CD> Map { get; } = new();
	
	#endregion
	
	
	#region Private Methods
	
	private void CalcShortcuts()
	{
		Queue<Node<I, ND, CD>> queue = new();

		foreach (var nodes in Map)
		{
			queue.Enqueue(nodes);
		}

		while (queue.Count > 0)
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
		return Map.Add(i, data);
	}

	public Connection<I, ND, CD> Connect(Node<I, ND, CD> a, Node<I, ND, CD> b, CD? data = default)
	{
		var connection = Map.Connect(
			a, b,
			ruler.Distance(a, b),
			data);
		
		return connection;
	}
	
	public Map<I, ND, CD> Build()
	{
		CalcShortcuts();
		
		return Map;
	}

	#endregion
}