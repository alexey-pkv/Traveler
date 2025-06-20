using System.Collections;
using Traveler.Exceptions;
using Traveler.Objects;


namespace Traveler;


public class Map<I, ND, CD> : IEnumerable 
	where I : IComparable<I>
{
	#region Data Members
	
	private readonly Dictionary<I, Node<I, ND, CD>> m_nodes = new();
	
	#endregion


	#region Properties
	
	public int Count => m_nodes.Count;
	public bool IsEmpty => m_nodes.Count == 0;

	#endregion


	#region Private Methods

	private Path<I, ND, CD> FindShortcutPath(
		List<EnterPoint<I, ND, CD>> start,
		List<EnterPoint<I, ND, CD>> end,
		INavigator<I, ND, CD> navigator)
	{
		var path = Path<I, ND, CD>.NOT_FOUND;

		foreach (var sp in start)
		{
			foreach (var ep in end)
			{
				var offset = sp.Offset + ep.Offset;
				var distance = GetShortcutDistance(sp.Node, ep.Node, navigator);
				
				if (double.IsPositiveInfinity(distance) || distance + offset > path.Distance)
					continue;
				
				path.Distance = distance + offset;
				path.From = sp;
				path.To = ep;
			}
		}

		return path;
	}

	private void BuildShortcutPath(Path<I, ND, CD> path)
	{
		if (!path.IsFound)
			throw new ArgumentException("Path not found");
		
		Step<I, ND, CD>.BuildShortcutPath(path.Steps, path.From.Node);
		Step<I, ND, CD>.BuildShortcutPath(path.Steps, path.To.Node, true);
	}

	#endregion
	
	
	#region Methods
	
	public bool Has(I id) => m_nodes.ContainsKey(id);
	public Node<I, ND, CD>? Get(I id) => m_nodes.GetValueOrDefault(id);

	public Node<I, ND, CD> Require(I id)
	{
		return m_nodes.GetValueOrDefault(id) ?? throw new TravelerException($"Node {id} not found");
	}

	public bool TryGet(I id, out Node<I, ND, CD>? node) => m_nodes.TryGetValue(id, out node);
	
	public Node<I, ND, CD> Add(I i, ND? data = default)
	{
		Node<I, ND, CD> node = new(i, data);

		if (Has(i))
			throw new ArgumentException($"Node {i} has already been added");
		
		m_nodes[node.ID] = node;
		
		return node;
	}
	
	public Connection<I, ND, CD> Connect(Node<I, ND, CD> a, Node<I, ND, CD> b, double distance, CD? data = default)
	{
		if (a.HasNeighbor(b))
			throw new InvalidConnectionException($"Nodes {a} and {b} already connected");
		
		var connection = new Connection<I, ND, CD>(a, b)
		{
			Data = data,
			Distance = distance
		};

		a.Connect(connection);
		b.Connect(connection);
		
		return connection;
	}
	
	#endregion
	
	
	#region Public Path Find Methods

	public double GetShortcutDistance(Node<I, ND, CD> from, Node<I, ND, CD> to, INavigator<I, ND, CD>? navigator = null)
	{
		if (!from.HasShortcut || !to.HasShortcut)
			return double.PositiveInfinity;
		
		var dist = from.Shortcut.Distance + to.Shortcut.Distance;
		
		if (navigator != null && from.Shortcut.To != to.Shortcut.To)
		{
			dist += navigator.ShortcutDistance(from.Shortcut.To, to.Shortcut.To);
		}
		
		return dist;
	}

	public Path<I, ND, CD> FindShortcut(
		List<EnterPoint<I, ND, CD>> start,
		List<EnterPoint<I, ND, CD>> end,
		INavigator<I, ND, CD> navigator)
	{
		var path = FindShortcutPath(start, end, navigator);

		if (path.IsFound)
		{
			BuildShortcutPath(path);
		}
		
		return path;
	}

	public Path<I, ND, CD> Find(
		List<EnterPoint<I, ND, CD>> start,
		List<EnterPoint<I, ND, CD>> end,
		INavigator<I, ND, CD> navigator,
		bool allowShortcuts = true,
		CancellationToken token = default)
	{
		Path<I, ND, CD>? shortcut = allowShortcuts ? FindShortcutPath(start, end, navigator) : null;
		double bestDistance = shortcut?.Distance ?? double.PositiveInfinity;
		
		
		
		return null;
	}
	
	#endregion
	
	
	#region Enumerator 
	
	public IEnumerator<Node<I, ND, CD>> GetEnumerator()
	{
		return m_nodes.Values.GetEnumerator();
	}
	
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
	
	#endregion
}