using Traveler.Objects;


namespace Traveler.PathFinding;


public struct SearchCursor<I, ND, CD> where I : IComparable<I>
{
	#region Private Data Members

	private readonly List<SearchHead<I, ND, CD>>	m_all = new(1024);
	private readonly Dictionary<I, int>				m_map = new();
	private readonly PriorityQueue<SearchHead<I, ND, CD>, double> m_next = new();
	

	#endregion


	#region Constructor

	public SearchCursor()
	{
		
	}

	#endregion


	#region Public Methods

	public bool Has(I id) => m_map.ContainsKey(id);
	public bool HasNext() => m_next.Count > 0;

	public SearchHead<I, ND, CD> Next()
	{
		var item = m_next.Peek();
		
		m_next.Dequeue();

		return item;
	}

	public void Add(SearchHead<I, ND, CD> item)
	{
		item.Index = m_all.Count;
		
		m_all.Add(item);
		m_map[item.Head] = item.Index;
		m_next.Enqueue(item, item.TotalBestCaseDistance);
	}

	#endregion
}