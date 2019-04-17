using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Threading;
using airplane;
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
		 * 4 - Находится в обслуживании.
		 * 5 - Загрузка пассажиров.
		 * 6 - Ожидание взлета.
		 * 7 - Взлет.
		 * 8 - Полет и деструкция.
		 * 10 - Только что создан, в полете к аэропорту.
		 * 11 - Готовится к посадке.
		 * 12 - Посадка.
		 * 13 - Разгрузка пассажиров.
		 * 14 - Обслуживание.
		 * 15 - Ожидание Follow Me в гараж.
		 * 16 - Прикреплен к Follow Me в гараж.
		 */
		// Пассажиры
		List<IPassenger> Passengers { get; set; }
		int PassengersMax { get; set; }
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
		public int PassengersMax { get; set; }
		public StandardRoute Route { get; set; }
		public List<IBaggage> Baggage { get; set; }
		public double Fuel { get; set; }
		public double FuelMax { get; set; }
		public bool Defrosted { get; set; }
		public List<IFood> Food { get; set; }
		public bool NeedsMaintenance { get; set; }

		private static readonly Fsm fsm = new Fsm();
		private static readonly AirplaneHttpClient AirplaneClient = new AirplaneHttpClient();
		private static readonly PassengerHttpClient PassengerClient = new PassengerHttpClient();
		private static readonly PassengerBusHttpClient PassengerBusClient = new PassengerBusHttpClient();
		private static readonly BaggageTruckHttpClient BaggageTruckClient = new BaggageTruckHttpClient();
		private static readonly RefuellerHttpClient RefuellerClient = new RefuellerHttpClient();
		private static readonly FollowMeHttpClient FollowMeClient = new FollowMeHttpClient();
		private static readonly GroundControlHttpClient GroundControlClient = new GroundControlHttpClient();
		private static readonly MaintenanceServiceHttpClient MaintenanceServiceClient = new MaintenanceServiceHttpClient();
		private static readonly DeicingHttpClient DeicingClient = new DeicingHttpClient();
		private static readonly CateringHttpClient CateringClient = new CateringHttpClient();
		private static readonly ScheduleHttpClient ScheduleClient = new ScheduleHttpClient();

		private readonly Random _r = new Random();
		private const int WaitingTime = 1000;

		public Airplane() { }
		public Airplane(int id)
		{
			Id = id;
			Status = 0;
		}

		[JsonConstructor]
		public Airplane(int iD, int status, List<IPassenger> passengers, int passengersMax, StandardRoute route, List<IBaggage> baggage, double fuel, double fuelMax, bool defrosted, List<IFood> food) : this(iD)
		{
			Id = iD;

			if (status != 0 && status != 10)
			{
				Status = 0;
			}
			else
			{
				Status = status;
			}

			if (passengers == null && status == 10)
			{
				// TODO: вызов генерации пассажиров (к Даниной системе)
			}
			else
			{
				Passengers = passengers;
			}

			PassengersMax = passengersMax == 0 ? _r.Next(50, 101) : passengersMax;

			Route = route;

			if (baggage == null && status == 10)
			{
				// TODO: вызов генерации багажа
			}
			else
			{
				Baggage = baggage;
			}

			if ((int)fuel == 0)
			{
				Fuel = _r.Next(50, 101);
			}
			else
			{
				Fuel = fuel;
			}

			if ((int)fuelMax == 0)
			{
				FuelMax = _r.Next(75, 101);
				if (FuelMax < Fuel) FuelMax = Fuel;
			}
			else
			{
				FuelMax = fuelMax;
			}

			Defrosted = defrosted;

			if (food == null && status == 10)
			{
				// TODO: вызов генерации еды
			}
			else
			{
				Food = food;
			}
		}

		public void Start()
		{
			fsm.SetState(WaitingForOrder);
			fsm.Update();
		}

		private void UpdateStatus(int status)
		{
			Status = status;
			AirplaneClient.UpdateStatus(Id, status);
		}

		public void ChangeStatus(int status)
		{
			Status = status;
		}

		// Status 0
		private void WaitingForOrder()
		{
			while (Status != 1)
			{
				Console.WriteLine("{0} waiting for order. Status 0.", Id);
				Thread.Sleep(WaitingTime);
			}
			
			UpdateStatus(1);
			fsm.SetState(PrepareForTheFlight);
			fsm.Update();
		}

		// Status 1
		private void PrepareForTheFlight()
		{
			RecalculateMaintenance();

			// Вызов к наземному обслуживанию, что нужно самолету

			// Ждем прикрепления к Follow Me
			while (Status != 2)
			{
				Thread.Sleep(WaitingTime);
			}

			UpdateStatus(2);
			fsm.SetState(WithFollowMeToFlight);
			fsm.Update();
		}

		private void RecalculateMaintenance()
		{
			NeedsMaintenance = FuelMax * 0.5 < Fuel || !Defrosted;
		}

		// Status 2
		private void WithFollowMeToFlight()
		{
			while (Status != 4)
			{
				Thread.Sleep(WaitingTime);
			}

			UpdateStatus(4);
			fsm.SetState(AtMaintenance);
			fsm.Update();
		}

		// Status 4
		private void AtMaintenance()
		{
			while (Status != 5)
			{
				Thread.Sleep(WaitingTime);
			}

			UpdateStatus(5);
			fsm.SetState(BoardingPassengers);
			fsm.Update();
		}

		// Status 5
		private void BoardingPassengers()
		{
			while (Status != 6)
			{
				Thread.Sleep(WaitingTime);
			}

			UpdateStatus(6);
			fsm.SetState(MovingToFlight);
			fsm.Update();
		}

		// Status 6
		private string _locationCode;
		private void MovingToFlight()
		{
			var loop = true;
			while (loop)
			{
				// Запрос на разрешение передвижения

				var response = GroundControlClient.PermissionOnFlight(Id);
				response.Wait();
				var content = response.Result.Content.ReadAsStringAsync();
				content.Wait();
				var result = JsonConvert.DeserializeObject<GroundControlPermissionResponse>(content.Result);
				_locationCode = result.locationCode;

				switch (result.permission)
				{
					case "Obtained":
						loop = false;
						UpdateStatus(7);
						fsm.SetState(Takeoff);
						fsm.Update();
						break;
					case "Queued":
						loop = false;
						while (Status != 7)
						{
							Thread.Sleep(WaitingTime);
						}
						UpdateStatus(7);
						fsm.SetState(Takeoff);
						fsm.Update();
						continue;
					case "Denied":
						Thread.Sleep(WaitingTime);
						break;
					default:
						Thread.Sleep(WaitingTime);
						break;
				}
			}
		}

		// Status 7
		private void Takeoff()
		{
			// Имитация передвижения до ВПП и взлет

			Thread.Sleep(WaitingTime * 10);

			UpdateStatus(7);
			fsm.SetState(Flight);
			fsm.Update();
		}

		private void Flight()
		{
			// Сообщение Ground Control, что полоса свободна
			_ = GroundControlClient.FreeTheSpace(Id, _locationCode);

			PassengerClient.NotifyPassengers(Route.reisNumber);
			AirplaneClient.Remove(Id);
		}

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
