using Traveler.Objects;


namespace Traveler.UnitTest.Lib;


public static class Create
{
	public static Node<int, int, int> Node(int id, int? data = null)
	{
		return new Node<int, int, int>(id, data ?? (id * 10));
	}
	
	public static Connection<int, int, int> Connection(
		Node<int, int, int> node_1, 
		Node<int, int, int> node_2,
		double distance = 0.0,
		bool connect_nodes = true)
	{
		var connection = new Connection<int, int, int>(node_1, node_2) { Distance = distance };
		
		if (connect_nodes)
		{
			node_1.Connect(connection);
			node_2.Connect(connection);
		}
		
		return connection;
	}
	
	public static Shortcut<int, int, int> Shortcut(
		Node<int, int, int> to, 
		Node<int, int, int> via,
		int distance)
	{
		return new Shortcut<int, int, int>
		{
			To = to,
			Via = via,
			Distance = distance
		};
	}
}