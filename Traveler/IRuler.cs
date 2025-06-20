using Traveler.Objects;


namespace Traveler;


public interface IRuler<I, ND, CD> 
	where I : IComparable<I>
{
	#region Methods
	
	public double Distance(Node<I, ND, CD> a, Node<I, ND, CD> b);
	
	#endregion
}