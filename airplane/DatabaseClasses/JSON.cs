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

		public List<Airplane> Airplanes { get; set; }

		public List<Airplane> ReadAll()
		{
			using (StreamReader r = new StreamReader(Path))
			{
				string json = r.ReadToEnd();
				List<Airplane> Airplanes = JsonConvert.DeserializeObject<List<Airplane>>(json);
				return Airplanes;
			}
		}

		public List<Airplane> ReadById(int id)
		{
			return Airplanes != null ? Airplanes.Where(x => x.ID == id).ToList() : new List<Airplane>();
		}

		public void CreateAirplane(Airplane airplane)
		{
			Airplanes.Add(airplane);

			var serialized = JsonConvert.SerializeObject(Airplanes);
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
					Airplanes = JsonConvert.DeserializeObject<List<Airplane>>(json);
					if (Airplanes == null)
					{
						Airplanes = new List<Airplane>();
					}
				}
			}
			else
			{
				using (StreamWriter sw = File.AppendText(Path))
				{
					sw.Write("");
					Airplanes = new List<Airplane>();
				}
			}
		}
	}
}
