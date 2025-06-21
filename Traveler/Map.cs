using System.Collections;
using Traveler.Exceptions;
using Traveler.Objects;
using Traveler.PathFinding;


namespace Traveler;


public class Map<I, ND, CD>(Dictionary<I, Node<I, ND, CD>> source) : IEnumerable 
	where I : IComparable<I>
{
	#region Data Members
	
	private readonly Dictionary<I, Node<I, ND, CD>> m_nodes = new(source);
	
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

	public bool TryGet(I id, out Node<I, ND, CD>? node) => m_nodes.TryGetValue(id, out node);
	
	public Node<I, ND, CD> Add(I i, ND? data = default)
	{
		Node<I, ND, CD> node = new(i, data);

		if (Has(i))
			throw new ArgumentException($"Node {i} has already been added");
		
		m_nodes[node.ID] = node;
		
		return node;
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

	private double PredictDistance(
		INavigator<I, ND, CD> navigator,
		Node<I, ND, CD> from, 
		List<EnterPoint<I, ND, CD>> to)
	{
		var min = double.PositiveInfinity;
		
		foreach (var ep in to)
		{
			var curr = navigator.PredictDistance(from, ep.Node);

			if (double.IsPositiveInfinity(curr))
				continue;
			
			curr += ep.Offset;

			if (curr < min)
			{
				min = curr;
			}
		}

		return min;
	}

	private SearchCursor<I, ND, CD> CreateCursor(
		INavigator<I, ND, CD> navigator,
		List<EnterPoint<I, ND, CD>> start,
		List<EnterPoint<I, ND, CD>> end)
	{
		var cursor = new SearchCursor<I, ND, CD>();
		
		foreach (var ep in start)
		{
			var remaining = PredictDistance(navigator, ep.Node, end);
			
			if (double.IsPositiveInfinity(remaining))
				continue;
			
			cursor.Add(SearchHead<I, ND, CD>.First(ep, remaining));
		}

		return cursor;
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
		
		var startCursor = CreateCursor(navigator, start, end);
		var endCursor = CreateCursor(navigator, end, start);

		while (
			!token.IsCancellationRequested &&
			startCursor.HasNext() &&
			endCursor.HasNext())
		{
			
		}
		
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