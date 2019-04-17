using AirplaneClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DatabaseClasses
{
	public class Json
	{
		public List<Airplane> Airplanes { get; set; }

		private const string Path = "..\\storage.json";
		private const string IdFilePath = "..\\current-id.txt";

		// Витин .Instance()
		private static readonly object SingleLock = new object();
		private static Json _instance;

		public static Json Instance
		{ 
			get
			{
				lock (SingleLock)
				{
					return _instance ?? (_instance = new Json());
				}
			}
		}

		private Json()
		{
			if (File.Exists(Path))
			{
				ReadAll();
			}
			else
			{
				using (var sw = File.AppendText(Path))
				{
					sw.Write("");
					Airplanes = new List<Airplane>();
				}
			}
		}

		private void ReadAll()
		{
			using (var r = new StreamReader(Path))
			{
				var json = r.ReadToEnd();
				Airplanes = JsonConvert.DeserializeObject<List<Airplane>>(json);
			}
		}

		private void AddAirplaneToDB(Airplane airplane)
		{
			Airplanes.Add(airplane);

			var serialized = JsonConvert.SerializeObject(Airplanes/*, Formatting.Indented*/);
			using (var w = new StreamWriter(Path, false))
			{
				w.Write(serialized);
			}
		}

		public Airplane CreateAirplane()
		{
			int id;

			using (var r = new StreamReader(IdFilePath))
			{
				id = Convert.ToInt32(r.ReadToEnd()) + 1;
			}
			using (var sw = new StreamWriter(IdFilePath, false))
			{
				sw.Write(id);
			}

			var airplane = new Airplane(id);

			new Thread(() =>
			{
				AddAirplaneToDB(airplane);
			}).Start();

			return airplane;
		}

		public Airplane CreateAirplane(string json)
		{
			int id;

			using (var r = new StreamReader(IdFilePath))
			{
				id = Convert.ToInt32(r.ReadToEnd()) + 1;
			}
			using (var sw = new StreamWriter(IdFilePath, false))
			{
				sw.Write(id);
			}

			var jObject = JObject.Parse(json);
			jObject["Id"] = id;

			var airplane = JsonConvert.DeserializeObject<Airplane>(jObject.ToString());

			new Thread(() =>
			{
				AddAirplaneToDB(airplane);
			}).Start();

			return airplane;
		}

		public void RemoveAirplane(int id)
		{
			Airplanes.RemoveAll(x => x.Id == id);
			var serialized = JsonConvert.SerializeObject(Airplanes/*, Formatting.Indented*/);
			using (var w = new StreamWriter(Path, false))
			{
				w.Write(serialized);
			}
		}

		public Airplane UpdateAirplane(string json, int id)
		{
			var jObject = JObject.Parse(json);
			jObject["Id"] = id;

			var newAirplane = JsonConvert.DeserializeObject<Airplane>(jObject.ToString());

			new Thread(() =>
			{
				RemoveAirplane(id);
				AddAirplaneToDB(newAirplane);
				SortDB();
			}).Start();

			return newAirplane;
		}

		private void SortDB()
		{
			Airplanes.Sort(Comparison);

			var serialized = JsonConvert.SerializeObject(Airplanes/*, Formatting.Indented*/);
			using (var w = new StreamWriter(Path, false))
			{
				w.Write(serialized);
			}
		}

		private static int Comparison(Airplane x, Airplane y)
		{
			if (x.Id > y.Id) return 1;
			if (x.Id == y.Id) return 0;
			return -1;
		}

		public int UpdateStatus(int id, int status)
		{
			try
			{
				Airplanes.First(x => x.Id == id).ChangeStatus(status);
				return 1;
			}
			catch
			{
				return 0;
			}
		}

		public int IsFollowing(int id)
		{
			var status = Airplanes.First(x => x.Id == id).Status;
			if (status == 2 || status == 16) return 1;
			return 0;
		}
	}
}
