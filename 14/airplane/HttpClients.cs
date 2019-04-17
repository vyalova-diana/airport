using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirplaneClasses;
using Newtonsoft.Json;

namespace airplane
{
	class AirplaneHttpClient
	{
		private HttpClient Client { get; }

		public AirplaneHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.AirplanesIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}

		public void UpdateStatus(int id, int status)
		{
			Client.GetAsync($"/planes/update_status/{id}/{status}");
		}

		public void Remove(int id)
		{
			Client.GetAsync($"/planes/remove/{id}");
		}
	}

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

		public async Task<HttpResponseMessage> NotifyPassengers(int flightId)
		{
			var content = new StringContent(JsonConvert.SerializeObject(flightId), Encoding.UTF8, "application/json");
			const string url = "/api/flightpassengers/takeoff";

			HttpResponseMessage responseMessage = null;

			try
			{
				responseMessage = await Client.PostAsync(url, content);
			}
			catch (Exception ex)
			{
				if (responseMessage == null)
				{
					responseMessage = new HttpResponseMessage();
				}
				responseMessage.StatusCode = HttpStatusCode.InternalServerError;
				responseMessage.ReasonPhrase = $"RestHttpClient.PermissionOnFlight failed: {ex}";
			}

			return responseMessage;
		}
	}

	class PassengerBusHttpClient
	{
		private HttpClient Client { get; set; }

		public PassengerBusHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.PassengerBusIp)
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
				BaseAddress = new Uri(Settings.BaggageTruckIp)
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
				BaseAddress = new Uri(Settings.RefuellerIp)
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
				BaseAddress = new Uri(Settings.FollowMeIp)
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
				BaseAddress = new Uri(Settings.GroundControlIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}

		public async Task<HttpResponseMessage> PermissionOnFlight(string from, string to, int id)
		{
			var request = new GroundControlPermissionRequest(from, to, "Air Facility", id.ToString());
			var json = JsonConvert.SerializeObject(request);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			const string url = "/askForPermission";

			HttpResponseMessage responseMessage = null;

			try
			{
				responseMessage = await Client.PostAsync(url, content);
			}
			catch (Exception ex)
			{
				if (responseMessage == null)
				{
					responseMessage = new HttpResponseMessage();
				}
				responseMessage.StatusCode = HttpStatusCode.InternalServerError;
				responseMessage.ReasonPhrase = $"RestHttpClient.PermissionOnFlight failed: {ex}";
			}

			return responseMessage;
		}

		public async Task<HttpResponseMessage> FreeTheSpace(int id, string locationCode)
		{
			var request = new GroundControlPermissionRequest("Air Facility", id.ToString(), locationCode, "Idle");
			var json = JsonConvert.SerializeObject(request);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			const string url = "/updateTFStatus";

			HttpResponseMessage responseMessage = null;

			try
			{
				responseMessage = await Client.PostAsync(url, content);
			}
			catch (Exception ex)
			{
				if (responseMessage == null)
				{
					responseMessage = new HttpResponseMessage();
				}
				responseMessage.StatusCode = HttpStatusCode.InternalServerError;
				responseMessage.ReasonPhrase = $"RestHttpClient.FreeTheSpace failed: {ex}";
			}

			return responseMessage;
		}
	}

	public class GroundControlPermissionRequest
	{
		public string from;
		public string to;
		public string service;
		public string identifier;

		[JsonConstructor]
		public GroundControlPermissionRequest(string from, string to, string service, string identifier)
		{
			this.from = from;
			this.to = to;
			this.service = service;
			this.identifier = identifier;
		}
	}

	public class GroundControlPermissionResponse
	{
		public string permission;
		public string locationCode;

		[JsonConstructor]
		public GroundControlPermissionResponse(string permission, string locationCode)
		{
			this.permission = permission;
			this.locationCode = locationCode;
		}
	}

	public class GroundControlStatusRequest
	{
		public string service;
		public string identifier;
		public string locationCode;
		public string status;

		[JsonConstructor]
		public GroundControlStatusRequest(string service, string identifier, string locationCode, string status)
		{
			this.service = service;
			this.identifier = identifier;
			this.locationCode = locationCode;
			this.status = status;
		}
	}

	public class GroundControlStatusResponse
	{
		public bool error;
		public string reason;

		[JsonConstructor]
		public GroundControlStatusResponse(bool error, string reason)
		{
			this.error = error;
			this.reason = reason;
		}
	}

	class MaintenanceServiceHttpClient
	{
		private HttpClient Client { get; set; }

		public MaintenanceServiceHttpClient()
		{
			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(Settings.MaintenanceServiceIp)
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
				BaseAddress = new Uri(Settings.DeicingIp)
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
				BaseAddress = new Uri(Settings.CateringIp)
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
				BaseAddress = new Uri(Settings.ScheduleIp)
			};
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Client = httpClient;
		}
	}
}
