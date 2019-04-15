namespace AirplaneClasses.Interfaces
{
	public interface IPassenger
	{

	}
	public interface IBaggage
	{

	}
	public interface IFood
	{

	}
	public interface IRoute
	{
		string From { get; set; }
		string To { get; set; }
		int TimeStart { get; set; }
		int TimeStop { get; set; }
		int? Count { get; set; }
		int FlightNumber { get; set; }
		int? PlaneId { get; set; }
		int? RegistrationTime { get; set; }
		int? BoardingTime { get; set; }
	}

	public class Passenger : IPassenger
	{

	}

	public class Baggage : IBaggage
	{

	}

	public class Food : IFood
	{

	}

	public class StandardRoute : IRoute
	{
		public string From { get; set; }
		public string To { get; set; }
		public int TimeStart { get; set; }
		public int TimeStop { get; set; }
		public int? Count { get; set; }
		public int FlightNumber { get; set; }
		public int? PlaneId { get; set; }
		public int? RegistrationTime { get; set; }
		public int? BoardingTime { get; set; }

		public StandardRoute() { }
	}
}
