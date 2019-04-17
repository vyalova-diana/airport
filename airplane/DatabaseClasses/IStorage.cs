using System;
using System.Collections.Generic;
using System.Text;
using AirplaneClasses;

namespace DatabaseClasses
{
	// Unused
	public interface IStorage
	{
		List<Airplane> Airplanes { get; set; }
		Airplane CreateAirplane();
	}
}
