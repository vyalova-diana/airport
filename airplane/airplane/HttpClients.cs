using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace airplane
{
	class PassengerHttpClient 
	{
		private HttpClient Client { get; }

		public PassengerHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.PassengersIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}
	}

	class PassengerBusHttpClient
	{
		private HttpClient Client { get; set; }

		public PassengerBusHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.PassengersIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}
	}

	class BaggageTruckHttpClient
	{
		private HttpClient Client { get; set; }

		public BaggageTruckHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.PassengersIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}
	}

	class RefuellerHttpClient
	{
		private HttpClient Client { get; set; }

		public RefuellerHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.PassengersIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}
	}

	class FollowMeHttpClient
	{
		private HttpClient Client { get; set; }

		public FollowMeHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.PassengersIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}
	}

	class GroundControlHttpClient
	{
		private HttpClient Client { get; set; }

		public GroundControlHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.PassengersIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}
	}

	class HandlingServiceHttpClient
	{
		private HttpClient Client { get; set; }

		public HandlingServiceHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.PassengersIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}
	}

	class DeicingHttpClient
	{
		private HttpClient Client { get; set; }

		public DeicingHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.PassengersIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}
	}

	class CateringHttpClient
	{
		private HttpClient Client { get; set; }

		public CateringHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.PassengersIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}
	}

	class ScheduleHttpClient
	{
		private HttpClient Client { get; set; }

		public ScheduleHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.PassengersIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}
	}
}
