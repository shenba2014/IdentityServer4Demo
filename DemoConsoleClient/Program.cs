using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace DemoConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
			requestsApi();
			Console.ReadLine();
		}

		static async void requestsApi()
		{
			var disco = await DiscoveryClient.GetAsync("http://localhost:61138");
			if (disco.IsError)
			{
				Console.WriteLine(disco.Error);
				return;
			}

			var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
			var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

			if (tokenResponse.IsError)
			{
				Console.WriteLine(tokenResponse.Error);
				return;
			}

			Console.WriteLine(tokenResponse.Json);

			var client = new HttpClient();
			client.SetBearerToken(tokenResponse.AccessToken);

			var response = await client.GetAsync("http://localhost:61139/identity");
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine(response.StatusCode);
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				Console.WriteLine(JArray.Parse(content));
			}
		}
    }
}
