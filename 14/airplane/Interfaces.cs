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
		string frm { get; set; }
		string to { get; set; }
		int timeStart { get; set; }
		int timeStop { get; set; }
		int? count { get; set; }
		int reisNumber { get; set; }
		int? plain { get; set; }
		int? registrtionTime { get; set; }
		int? boardingTime { get; set; }
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
		public StandardRoute() { }
		public string frm { get; set; }
		public string to { get; set; }
		public int timeStart { get; set; }
		public int timeStop { get; set; }
		public int? count { get; set; }
		public int reisNumber { get; set; }
		public int? plain { get; set; }
		public int? registrtionTime { get; set; }
		public int? boardingTime { get; set; }
	}
}
