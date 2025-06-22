using Traveler.Base;
using Traveler.Objects;


namespace Traveler.PathFinding;


public class Finder<I, ND, CD>(INodeMap<I, ND, CD> m_map) where I : IComparable<I>
{
	#region Private Data Members
	
	private INavigator<I, ND, CD>	m_navigator = null;
	private CancellationToken		m_token		= CancellationToken.None;
	
	private SearchCursor<I, ND, CD> m_source	= new();
	private SearchCursor<I, ND, CD> m_target	= new();
	
	private double	m_minDistance	= double.PositiveInfinity;
	private I		m_foundNode		= default;
	private bool	m_isFound		= false;
	
	#endregion
	
	
	#region Private Methods

	private Path<I, ND, CD> BuildFoundPath()
	{
		Path<I, ND, CD> path = new();
		
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

	private void SetupCursor(
		SearchCursor<I, ND, CD> cursor,
		List<EnterPoint<I, ND, CD>> start,
		List<EnterPoint<I, ND, CD>> end)
	{
		var index = 0;
		
		foreach (var ep in start)
		{
			var remaining = PredictDistance(m_navigator, ep.Node, end);
			var sp = SearchHead<I, ND, CD>.First(index++, ep, remaining);
			
			if (double.IsPositiveInfinity(remaining) || 
			    (!double.IsPositiveInfinity(m_minDistance) && m_minDistance <= remaining))
			{
				continue;
			}	
			
			cursor.Add(sp);
		}
	}
	
	private void ProcessNext(
		SearchCursor<I, ND, CD> cursor,
		SearchCursor<I, ND, CD> other,
		List<EnterPoint<I, ND, CD>> targets)
	{
		var next = cursor.Next();
		
		if (!double.IsPositiveInfinity(m_minDistance) && next.TotalBestCaseDistance >= m_minDistance)
			return;
		
		var node = m_map.Require(next.Head);

		foreach (var kvp in node.Neighbors)
		{
			var conn = kvp.Value;
			var to = conn.Other(node);
			
			if (cursor.IsVisited(to.ID))
				continue;
			
			if (other.TryGet(to.ID, out var existing))
			{
				var pathDistance = existing.DistanceTravelled + next.DistanceTravelled + conn.Distance;
				
				cursor.Visited(to.ID);
				
				if (pathDistance < m_minDistance)
				{
					m_minDistance	= pathDistance;
					m_foundNode		= to.ID;
					m_isFound		= true;
				}
				
				continue;
			}
			
			var distance = PredictDistance(m_navigator, to, targets);
			
			if (double.IsPositiveInfinity(distance))
				continue;
			
			cursor.Add(next.Next(
				via: conn,
				distanceRemaining: distance
			));
		}
	}
	
	private Path<I, ND, CD> TryFind(
		List<EnterPoint<I, ND, CD>> start,
		List<EnterPoint<I, ND, CD>> end)
	{
		SetupCursor(m_source, start, end);
		SetupCursor(m_target, end, start);
		
		while (m_source.HasNext() && m_target.HasNext())
		{
			m_token.ThrowIfCancellationRequested();
			
			ProcessNext(m_source, m_target, end);
			ProcessNext(m_target, m_source, start);
		}
		
		var path = m_isFound ? BuildFoundPath() : Path<I, ND, CD>.NOT_FOUND;
		
		Clear();
		
		return path;
	}
	
	#endregion
	
	
	#region Public Methods
	
	public void Clear()
	{
		m_minDistance = double.PositiveInfinity;
		m_navigator = null;
		m_token = CancellationToken.None;
		
		m_source.Clear();
		m_target.Clear();
	}
	
	public Finder<I, ND, CD> With(INavigator<I, ND, CD> navigator)
	{
		m_navigator = navigator;
		return this;
	}
	
	public Finder<I, ND, CD> With(CancellationToken token)
	{
		m_token = token;
		return this;
	}
	
	public Finder<I, ND, CD> WithAtMost(double minDistance)
	{
		m_minDistance = minDistance;
		return this;
	}
	
	public Path<I, ND, CD> Find(
		List<EnterPoint<I, ND, CD>> start,
		List<EnterPoint<I, ND, CD>> end)
	{
		try
		{
			return TryFind(start, end);
		}
		finally
		{
			Clear();
		}
	}
	
	#endregion
}