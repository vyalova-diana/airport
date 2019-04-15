using System;
using System.Collections.Generic;
using System.Net;
using AirplaneClasses.Interfaces;
using Newtonsoft.Json;

namespace AirplaneClasses
{
	public interface IPlane
	{
		int Id { get; }

		int Status { get; set; }
		/*
		 * 0 - Только что создан. Ожидает приказов в гараже аэропорта.
		 * 1 - Ожидает Follow Me.
		 * 2 - Прикреплен к Follow Me.
		 * 3 - Загрузка пассажиров.
		 * 4 - Ожидает обслуживания.
		 * 5 - Находится в обслуживании.
		 * 6 - Загрузка пассажиров.
		 * 7 - Ожидание взлета.
		 * 8 - Взлет.
		 * 9 - Полет и деструкция.
		 * 10 - Только что создан, в полете к аэропорту.
		 * 11 - Готовится к посадке.
		 * 12 - Посадка.
		 * 13 - Разгрузка пассажиров.
		 * 14 - Обслуживание?
		 * 15 - Ожидание Follow Me в гараж.
		 * 16 - Прикреплен к Follow Me в гараж.
		 */
		// Пассажиры
		List<IPassenger> Passengers { get; set; }
		// Багаж
		List<IBaggage> Baggage { get; set; }
		// Топливо
		double Fuel { get; set; }
		// Емкость топливного бака
		double FuelMax { get; set; }
		// Обработан ли антиобледенителем
		bool Defrosted { get; set; }
		// Еда
		List<IFood> Food { get; set; }
		// Рейс, маршрут
		StandardRoute Route { get; set; }
	}
	public class Airplane : IPlane
	{
		public int Id { get; }
		public int Status { get; set; }
		public List<IPassenger> Passengers { get; set; }
		public StandardRoute Route { get; set; }
		public List<IBaggage> Baggage { get; set; }
		public double Fuel { get; set; }
		public double FuelMax { get; set; }
		public bool Defrosted { get; set; }
		public List<IFood> Food { get; set; }

		public Airplane() { }
		public Airplane(int id)
		{
			Id = id;
		}

		[JsonConstructor]
		public Airplane(int iD, int status, List<IPassenger> passengers, StandardRoute route, List<IBaggage> baggage, double fuel, double fuelMax, bool defrosted, List<IFood> food) : this(iD)
		{
			Id = iD;
			Status = status;
			Passengers = passengers;
			Route = route;
			Baggage = baggage;
			Fuel = fuel;
			FuelMax = fuelMax;
			Defrosted = defrosted;
			Food = food;
		}

		/*
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
		*/
	}
}
