using System;
using System.Collections.Generic;
using System.Net;
using AirplaneClasses.Interfaces;
using Newtonsoft.Json;

namespace AirplaneClasses
{
	interface IPlane
	{
		int ID { get; }
		// Пассажиры
		List<IPassenger> Passengers { get; set; }
		// Багаж
		List<IBaggage> Baggage { get; set; }
		// Топливо
		double Fuel { get; set; }
		// Емкость топливного бака
		double Fuel_max { get; set; }
		// Обработан ли антиобледенителем
		bool Defrosted { get; set; }
		// Еда
		List<IFood> Food { get; set; }
		// Время отправки
		DateTime Departure { get; set; }
		// Время прибытия
		DateTime Arrival { get; set; }
	}
	public class Airplane : IPlane
	{
		public Airplane(int id)
		{
			ID = id;
		}
		public Airplane(int id, double fuel_max)
		{
			ID = id;
			Fuel_max = fuel_max;
		}

		[JsonConstructor]
		public Airplane(int iD, List<IPassenger> passengers, List<IBaggage> baggage, double fuel, double fuel_max, bool defrosted, List<IFood> food, DateTime departure, DateTime arrival) : this(iD)
		{
			Passengers = passengers;
			Baggage = baggage;
			Fuel = fuel;
			Fuel_max = fuel_max;
			Defrosted = defrosted;
			Food = food;
			Departure = departure;
			Arrival = arrival;
		}

		public int ID { get; }
		// Пассажиры
		public List<IPassenger> Passengers { get; set; }
		// Багаж
		public List<IBaggage> Baggage { get; set; }
		// Топливо
		public double Fuel { get; set; }
		// Емкость топливного бака
		public double Fuel_max { get; set; }
		// Обработан ли антиобледенителем
		public bool Defrosted { get; set; }
		// Еда
		public List<IFood> Food { get; set; }
		// Время отправки
		public DateTime Departure { get; set; }
		// Время прибытия
		public DateTime Arrival { get; set; }

		// Метод получения пассажиров
		private void GetPassengers(List<IPassenger> Passengers)
		{

		}

		// Метод отдачи пассажиров
		private void SendPassengers(IPAddress ip)
		{

		}

		// Метод получения багажа
		private void GetBaggage(List<IBaggage> Baggage)
		{

		}

		// Метод отдачи багажа
		private void SendBaggage(IPAddress ip)
		{

		}

		// Метод получения топлива
		private void GetFuel(double fuel)
		{

		}

		// Симуляция полета
		private void Fly()
		{

		}

		// Симуляция движения с помощью Follow Me
		private void Move(int time)
		{

		}

		// Запрос разрешения на передвижение по аэропорту
		private int RequestPermissionToMove(IPAddress ip)
		{
			return 0;
		}

		// Метод получения еды
		private void GetFood(IFood Food)
		{

		}
		// Метод получения еды
		private void GetFood(List<IFood> Food)
		{

		}

		// Кушац
		private void Eat()
		{

		}

		// Отдать остатки еды
		private void ReturnLeftovers(IPAddress ip)
		{

		}

		// Метод получения расписания
		private void GetSchedule(DateTime departure, DateTime arrival)
		{

		}

		public Airplane Clone()
		{
			return (Airplane)MemberwiseClone();
		}
	}
}
