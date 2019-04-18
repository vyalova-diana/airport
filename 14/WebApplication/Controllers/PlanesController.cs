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
		public List<Airplane> Get()
		{
			return Storage.Airplanes;
		}

		[HttpGet]
		[Route("create")]
		public Airplane Create()
		{
			return Storage.CreateAirplane();
		}

		[HttpPost]
		[Route("create")]
		public Airplane CreateWithBody([FromBody]Airplane airplane)
		{
			var serialized = JsonConvert.SerializeObject(airplane);
			return Storage.CreateAirplane(serialized);
		}

		[HttpGet]
		[Route("remove/{id}")]
		public int Remove(int id)
		{
			try
			{
				Storage.RemoveAirplane(id);
				return 1;
			}
			catch
			{
				return 0;
			}
		}

		[HttpGet]
		[Route("{id}")]
		public Airplane Get(int id)
		{
			if (Storage.Airplanes == null) return null;
			if (Storage.Airplanes.Count == 0) return null;

			var sequence = Storage.Airplanes.Where(x => x.Id == id);

			var list = sequence.ToList();
			return !list.Any() ? null : list.First();
		}

		[HttpPost]
		[Route("update/{id}")]
		public Airplane Update([FromBody]Airplane airplane, int id)
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

		[HttpGet]
		[Route("update_status/{id}/{status}")]
		public int UpdateStatus(int id, int status)
		{
			try
			{
				return Storage.UpdateStatus(id, status);
			}
			catch
			{
				return 0;
			}
		}

		[HttpGet]
		[Route("is_following/{id}")]
		public int IsFollowing(int id)
		{
			return Storage.IsFollowing(id);
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