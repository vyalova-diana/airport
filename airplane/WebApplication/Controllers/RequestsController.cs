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
    [Route("api/plane")]
    public class RequestsController : Controller
    {
		protected IStorage Storage { get; } = new JSON();

		private List<Airplane> PlanePool { get; set; }

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
		[HttpPost]
		[Route("create/{id}")]
		public Airplane Post(int id)
		{
			return CreatePlane(id);
		}

		[HttpGet]
		[Route("{id}")]
		public Airplane Get(int id)
		{
			var plane = PlanePool.Where(x => x.ID == id).First();
			if (plane == null)
			{
				return CreatePlane(id);
			}
			else
			{
				return plane;
			}
		}

		private Airplane CreatePlane(int id)
		{
			var newPlane = new Airplane(id);
			PlanePool.Add(newPlane.Clone());
			return newPlane;
		}
	}
}