using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using AirplaneClasses;
using AirplaneClasses.Interfaces;
using Newtonsoft.Json;

namespace airplane
{
	class Program
	{
		static void Main(string[] args)
		{
			// Console, testing requests and etc.

			while (true)
			{
				Console.Clear();
				Console.WriteLine("1. Все самолеты");
				Console.WriteLine("2. Создать самолет с помощью структуры");
				Console.WriteLine("3. Обновить самолет с помощью структуры");
				Console.WriteLine("4. Тест для Сани");

				var key = Convert.ToInt32(Console.ReadLine());

				switch (key)
				{
					case 1:
						ShowAirplanes();
						break;
					case 2:
						CreateAirplane();
						break;
					case 3:
						UpdateAirplane();
						break;
					case 4:
						FreeTheSpace(123, "кек");
						break;
					default:
						continue;
				}
			}
		}

		private static void ShowAirplanes()
		{
			Console.Clear();

			var client = new HttpClient
			{
				BaseAddress = new Uri(Settings.AirplanesIp)
			};
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var result = client.GetAsync("/planes").Result;
			var body = result.Content.ReadAsStringAsync().Result;

			Console.WriteLine(body != "" ? body : "No airplanes in DB!");

			Console.ReadLine();
		}

		private static void CreateAirplane()
		{
			var creation = true;

			var status = 0;
			var passengers = new List<IPassenger>();
			var baggage = new List<IBaggage>();
			var fuel = 0.0;
			var fuelMax = 0.0;
			var defrosted = false;
			var food = new List<IFood>();
			var route = new StandardRoute();

			while (creation)
			{
				Console.Clear();
				Console.WriteLine(
					$"1. Status (" + status + ")\n" +
					$"2. Add Passenger (" + JsonConvert.SerializeObject(passengers) + ")\n" +
					$"3. Change Route (" + JsonConvert.SerializeObject(route) + ")\n" +
					$"4. Add Baggage (" + JsonConvert.SerializeObject(baggage) + ")\n" +
					$"5. Fuel (" + fuel + ")\n" +
					$"6. FuelMax (" + fuelMax + ")\n" +
					$"7. Defrosted (" + defrosted + ")\n" +
					$"8. Add Food (" + JsonConvert.SerializeObject(food) + ")\n\n" +
					$"9. Create!\n" +
					$"0. Create with JSON!"
				);

				var key = Convert.ToInt32(Console.ReadLine());

				Console.Clear();

				switch (key)
				{
					case 1:
						throw new NotImplementedException();
						//Пассажир
						/*Console.WriteLine(
							$"1. Passengers (" + JsonConvert.SerializeObject(passengers) + ")\n" +
							$"2. Route" + JsonConvert.SerializeObject(route) + ")\n" +
							$"3. Baggage (" + JsonConvert.SerializeObject(baggage) + ")\n" +
							$"4. Fuel (" + fuel + ")\n" +
							$"5. FuelMax (" + fuelMax + ")\n" +
							$"6. Defrosted (" + defrosted.ToString() + ")\n" +
							$"7. Food (" + JsonConvert.SerializeObject(food) + ")\n" +
							$"0. Create!"
						);*/
						break;
					case 2:
						throw new NotImplementedException();
						break;
					case 3:
						throw new NotImplementedException();
						break;
					case 4:
						throw new NotImplementedException();
						break;
					case 5:
						throw new NotImplementedException();
						break;
					case 6:
						throw new NotImplementedException();
						break;
					case 7:
						throw new NotImplementedException();
						break;
					case 9:
						creation = false;
						throw new NotImplementedException();
						break;
					case 0:
						creation = false;
						Console.WriteLine("Paste Json here:");
						var airplane = JsonConvert.DeserializeObject<Airplane>(Console.ReadLine());
						var serializedAirplane = JsonConvert.SerializeObject(airplane);

						Console.WriteLine("Final airplane:\n" + serializedAirplane);

						CreateAirplaneWithPost(serializedAirplane);
						break;
					default:
						continue;
				}
			}
			/*
			 * {
			 * "Id":3,
			 * "Passengers":null,
			 * "Baggage":null,
			 * "Fuel":0.0,
			 * "FuelMax":0.0,
			 * "Defrosted":false,
			 * "Food":null,
			 * "Route":null
			 * }
			 */
		}

		private static void CreateAirplaneWithPost(string airplane)
		{
			var client = new HttpClient
			{
				BaseAddress = new Uri(Settings.AirplanesIp)
			};
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var content = new StringContent(airplane, Encoding.UTF8, "application/json");

			client.PostAsync("/planes/create", content);

			Console.WriteLine("Post-request sent.");
			Console.ReadLine();
		}


		private static void UpdateAirplane()
		{
			Console.Clear();
			Console.Write("Id самолета: ");
			var id = Convert.ToInt32(Console.ReadLine());

			Console.WriteLine("Paste Json here:");
			var airplane = JsonConvert.DeserializeObject<Airplane>(Console.ReadLine());
			var serializedAirplane = JsonConvert.SerializeObject(airplane);

			UpdateAirplaneWithPost(serializedAirplane, id);
		}

		private static void UpdateAirplaneWithPost(string airplane, int id)
		{
			var client = new HttpClient
			{
				BaseAddress = new Uri(Settings.AirplanesIp)
			};
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var content = new StringContent(airplane, Encoding.UTF8, "application/json");

			var url = "/planes/update/" + id;
			client.PostAsync(url, content);

			Console.WriteLine("Post-request sent.");
			Console.ReadLine();
		}

		private static void FreeTheSpace(int id, string locationCode)
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.GroundControlIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			var client = httpClient;

			var request = new GroundControlStatusRequest("Air Facility", id.ToString(), locationCode, "Idle");
			var json = JsonConvert.SerializeObject(request);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			const string url = "/updateTFStatus";

			var response = client.PostAsync(url, content);
			response.Wait();
			var cont = response.Result.Content.ReadAsStringAsync();
			cont.Wait();
			var result = cont.Result;
			Console.WriteLine(json);
			Console.WriteLine(result);
			Console.ReadLine();
		}
	}
}
