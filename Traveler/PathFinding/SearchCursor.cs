using Traveler.Objects;


namespace Traveler.PathFinding;


public struct SearchCursor<I, ND, CD> where I : struct, IComparable<I>
{
	#region Private Data Members
	
	private readonly List<SearchHead<I, ND, CD>>					m_all		= new(1024);
	private readonly HashSet<I>										m_visited	= new();
	private readonly Dictionary<I, int>								m_map		= new();
	private readonly PriorityQueue<SearchHead<I, ND, CD>, double>	m_next		= new();
	
	#endregion


	#region Constructor

	public SearchCursor()
	{
		
	}

	#endregion


	#region Public Methods

	public bool Has(I id) => m_map.ContainsKey(id);
	public bool HasNext() => m_next.Count > 0;
	public bool TryGet(I id, out SearchHead<I, ND, CD>? existing)
	{
		existing = null;
		
		if (!m_map.TryGetValue(id, out var result))
		{
			return false;
		}
		
		existing = m_all[result];
		
		return true;
	}
	
	public SearchHead<I, ND, CD> RequireByID(I id)
	{
		if (!TryGet(id, out var result))
		{
			throw new KeyNotFoundException();
		}
		
		return result ?? throw new InvalidOperationException();
	}
	
	public SearchHead<I, ND, CD> RequireByIndex(int index)
	{
		if (index < 0 || index >= m_all.Count)
		{
			throw new ArgumentOutOfRangeException(nameof(index));
		}
		
		return m_all[index];
	}

	public SearchHead<I, ND, CD> Next()
	{
		var item = m_next.Peek();
		
		m_next.Dequeue();

		return item;
	}
	
	public bool IsVisited(I id) => m_visited.Contains(id);
	
	public void Visited(I id)
	{
		m_visited.Add(id);
	}
	
	public void Add(SearchHead<I, ND, CD> item)
	{
		item.Index = m_all.Count;
		
		m_all.Add(item);
		m_map[item.Head] = item.Index;
		m_next.Enqueue(item, item.TotalBestCaseDistance);
		m_visited.Add(item.Head);
	}
	
	public void Clear()
	{
		m_all.Clear();
		m_map.Clear();
		m_next.Clear();
		m_visited.Clear();
	}

	#endregion
}