using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AirplaneClasses;
using DatabaseClasses;
using Newtonsoft.Json;

namespace WebApplication.Controllers
{
    [Produces("application/json")]
    [Route("planes")]
    public class PlanesController : Controller
    {
		protected static Json Storage { get; set; } = DatabaseClasses.Json.Instance;

		[HttpGet]
		public List<IPlane> Get()
		{
			return Storage.Airplanes;
		}

		[HttpGet]
		[Route("create")]
		public IPlane Create()
		{
			return Storage.CreateAirplane();
		}

		[HttpPost]
		[Route("create")]
		public IPlane CreateWithBody([FromBody]Airplane airplane)
		{
			var serialized = JsonConvert.SerializeObject(airplane);
			return Storage.CreateAirplane(serialized);
		}

		[HttpGet]
		[Route("remove/{id}")]
		public string Remove(int id)
		{
			try
			{
				Storage.RemoveAirplane(id);
				return "Plane with Id = " + id + " successfully removed!";
			}
			catch
			{
				return "Error occured. Try again?";
			}
		}

		[HttpGet]
		[Route("{id}")]
		public IPlane Get(int id)
		{
			if (Storage.Airplanes == null) return null;
			if (Storage.Airplanes.Count == 0) return null;

			var sequence = Storage.Airplanes.Where(x => x.Id == id);

			var list = sequence.ToList();
			return !list.Any() ? null : list.First();
		}

		[HttpPost]
		[Route("update/{id}")]
		public IPlane Update([FromBody]Airplane airplane, int id)
		{
			var serialized = JsonConvert.SerializeObject(airplane);
			try
			{
				return Storage.UpdateAirplane(serialized, id);
			}
			catch
			{
				return null;
			}
		}

		/*[HttpGet]
		public Message[] Get()
		{
			return Storage.ReadAll();
		}

		[HttpGet]
		[Route("{from}")]
		public Message[] Get(string from)
		{
			return Storage.ReadByFrom(from);
		}

		[HttpPost]
		public void Post([FromBody]Message msg)
		{
			Storage.AddMessage(msg);
		}*/
	}
}