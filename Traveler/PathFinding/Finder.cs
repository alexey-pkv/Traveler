using System.Data;
using Traveler.Base;
using Traveler.Objects;


namespace Traveler.PathFinding;


public class Finder<I, ND, CD>(INodeMap<I, ND, CD> m_map) where I : struct, IComparable<I>
{
	#region Private Data Members
	
	private INavigator<I, ND, CD>?	m_navigator = null;
	private CancellationToken		m_token		= CancellationToken.None;
	
	private SearchCursor<I, ND, CD> m_source	= new();
	private SearchCursor<I, ND, CD> m_target	= new();
	
	private List<EnterPoint<I, ND, CD>>? m_start	= null;
	private List<EnterPoint<I, ND, CD>>? m_end		= null;
	
	private double	m_minDistance	= double.PositiveInfinity;
	private I		m_foundID		= default;
	private bool	m_isFound		= false;
	
	#endregion
	
	
	#region Private Methods

	private Path<I, ND, CD> BuildFoundPath()
	{
		if (!m_isFound) return Path<I, ND, CD>.NOT_FOUND;
		
		var a = m_source.RequireByID(m_foundID);
		var b = m_target.RequireByID(m_foundID);
		var n = a;
		
		Path<I, ND, CD> path = new(
			distance: a.DistanceTravelled + b.DistanceTravelled, 
			capacity: a.StepsCount + b.StepsCount)
		{
			From	= m_start![a.EntryPointIndex],
			To		= m_end![b.EntryPointIndex]
		};

		for (int i = 0; i < a.StepsCount; i++)
		{
			path.Steps.Add(
				new Step<I, ND, CD>(
					connection: n.Via,
					from: n.FromNode
				));
			
			n = m_source.RequireByIndex(n.From);
		}
		
		path.Steps.Reverse(0, path.Steps.Count - 1);
		n = b;
		
		for (int i = 0; i < b.StepsCount; i++)
		{
			path.Steps.Add(
				new Step<I, ND, CD>(
					connection: n.Via,
					from: n.HeadNode
				));
			
			n = m_source.RequireByIndex(n.From);
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
			if (ep.Node == null)
				throw new NullReferenceException("Node in an entry point must be set!");
			
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
		if (m_navigator == null)
			throw new NoNullAllowedException("Navigator must be set!");
		
		var index = 0;
		
		foreach (var ep in start)
		{
			if (ep.Node == null)
				throw new NullReferenceException("Node in an entry point must be set!");
			
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
				var pathDistance = existing!.DistanceTravelled + next.DistanceTravelled + conn.Distance;
				
				cursor.Visited(to.ID);
				
				if (pathDistance < m_minDistance)
				{
					m_minDistance	= pathDistance;
					m_foundID		= to.ID;
					m_isFound		= true;
				}
				
				continue;
			}
			
			var distance = PredictDistance(m_navigator!, to, targets);
			
			if (double.IsPositiveInfinity(distance))
				continue;
			
			cursor.Add(next.Next(
				via: conn,
				distanceRemaining: distance
			));
		}
	}
	
	private Path<I, ND, CD> TryFind()
	{
		if (m_start == null || m_end == null)
			throw new NoNullAllowedException();
		
		SetupCursor(m_source, m_start, m_end);
		SetupCursor(m_target, m_end, m_start);
		
		while (m_source.HasNext() && m_target.HasNext())
		{
			m_token.ThrowIfCancellationRequested();
			
			ProcessNext(m_source, m_target, m_end);
			ProcessNext(m_target, m_source, m_start);
		}
		
		return BuildFoundPath();
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
	
	public Finder<I, ND, CD> From(List<EnterPoint<I, ND, CD>> ep)
	{
		m_start = ep;
		return this;
	}
	
	public Finder<I, ND, CD> To(List<EnterPoint<I, ND, CD>> ep)
	{
		m_end = ep;
		return this;
	}
	
	
	public Path<I, ND, CD> Find()
	{
		try
		{
			return TryFind();
		}
		finally
		{
			Clear();
		}
	}
	
	#endregion
}