using AirplaneClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DatabaseClasses
{
	public class JSON : IStorage
	{
		private readonly string Path = "..\\storage.json";

		private List<Airplane> airplanes = null;

		public List<Airplane> ReadAll()
		{
			using (StreamReader r = new StreamReader(Path))
			{
				string json = r.ReadToEnd();
				List<Airplane> airplanes = JsonConvert.DeserializeObject<List<Airplane>>(json);
				return airplanes;
			}
		}

		public List<Airplane> ReadById(int id)
		{
			return airplanes != null ? airplanes.Where(x => x.ID == id).ToList() : new List<Airplane>();
		}

		public void CreateAirplane(Airplane airplane)
		{
			airplanes.Add(airplane);

			var serialized = JsonConvert.SerializeObject(airplanes);
			using (StreamWriter w = new StreamWriter(Path, false))
			{
				w.Write(serialized);
			}
		}

		public JSON()
		{
			if (File.Exists(Path))
			{
				using (StreamReader r = new StreamReader(Path))
				{
					string json = r.ReadToEnd();
					airplanes = JsonConvert.DeserializeObject<List<Airplane>>(json);
				}
			}
			else
			{
				using (StreamWriter sw = File.AppendText(Path))
				{
					sw.Write("");
				}
			}
		}
	}
}
