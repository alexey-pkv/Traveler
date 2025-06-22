using Traveler.Objects;


namespace Traveler;


public interface INavigator<I, ND, CD> 
	where I : struct, IComparable<I>
{
	#region Methods
	
	public double ShortcutDistance(Node<I, ND, CD> from, Node<I, ND, CD> to);
	public double PredictDistance(Node<I, ND, CD> from, IEnumerable<Node<I, ND, CD>> to);
	
	public double PredictDistance(Node<I, ND, CD> from, Node<I, ND, CD> to) => PredictDistance(from, [to]);
	
	#endregion
}