using System;
using System.Collections.Generic;
using System.Text;
using AirplaneClasses;

namespace DatabaseClasses
{
	public interface IStorage
	{
		List<Airplane> Airplanes { get; set; }
		void CreateAirplane(Airplane airplane);
	}
}
