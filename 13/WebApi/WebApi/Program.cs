using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace WebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:61120/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            while (true)
            {


                
                        string txt1 = Console.ReadLine();
                        string req = "api//" + txt1;
                        var result = client.GetAsync(req).Result;
                        var body = result.Content.ReadAsStringAsync().Result;
                       var messages = JsonConvert.DeserializeObject<long>(body);

                        Console.WriteLine(messages);

                       
                }

            }
        }
    }

