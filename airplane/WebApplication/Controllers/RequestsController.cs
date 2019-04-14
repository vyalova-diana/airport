using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AirplaneClasses;
using DatabaseClasses;

namespace WebApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/planes")]
    public class RequestsController : Controller
    {
		protected static IStorage Storage { get; set; } = new JSON();

		private Airplane CreatePlane(int id)
		{
			var newPlane = new Airplane(id);
			Storage.CreateAirplane(newPlane);
			return newPlane;
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

		[HttpGet]
		[Route("create/{id}")]
		public List<Airplane> Create(int id)
		{
			if (Get(id).Count() == 0)
			{
				return new List<Airplane> { CreatePlane(id) };
			}
			else
			{
				return Get(id);
			}
		}

		[HttpGet]
		[Route("{id}")]
		public List<Airplane> Get(int id)
		{
			if (Storage.Airplanes != null)
			{
				var sequence = Storage.Airplanes.Where(x => x.ID == id);
				/*if (sequence.Count() == 0)
				{
					return null;
				}*/
				return sequence.ToList();
			}
			else
			{
				return new List<Airplane>();
			}
		}
	}
}