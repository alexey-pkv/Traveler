namespace Traveler.Exceptions;


public class TravelerException(string message, Exception? inner = null) : Exception(message, inner);