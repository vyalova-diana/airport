using System;
using System.Collections.Generic;
using System.Net;
using airplane.Interfaces;

namespace airplane
{
	class Airplane
	{
		// Пассажиры
		private List<IPassenger> Passengers { get; set; }
		// Багаж
		private List<IBaggage> Baggage { get; set; }
		// Топливо
		private double Fuel { get; set; }
		// Емкость топливного бака
		private double Fuel_max { get; set; }
		// Обработан ли антиобледенителем
		private bool Defrosted { get; set; }
		// Еда
		private List<IFood> Food { get; set; }
		// Время отправки
		private DateTime Departure { get; set; }
		// Время прибытия
		private DateTime Arrival { get; set; }

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
	}
}
