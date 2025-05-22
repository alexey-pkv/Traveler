namespace Traveler.Objects;


public class Node<I, ND, PD>(I id, ND data)
	where I : IComparable<I>
{
	#region Properties
	
	public I ID { get; } = id;
	public ND Data { get; } = data;
	public Dictionary<I, Connection<I, ND, PD>> Neighbors { get; } = new();
	
	#endregion
	
	
	#region Methods
	
	public void Connect(Connection<I, ND, PD> connection)
	{
		
	}
	
	#endregion
}