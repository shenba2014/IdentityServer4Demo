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

			requestByCredential(disco);
			requestByTestUser(disco);
		}

		static async void requestByCredential(DiscoveryResponse disco)
		{
			Console.WriteLine("access resource by client credential");
			var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
			var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
			if (tokenResponse.IsError)
			{
				Console.WriteLine(tokenResponse.Error);
				return;
			}
			requestApi(tokenResponse);
		}

		static async void requestByTestUser(DiscoveryResponse disco)
		{
			Console.WriteLine("access resource by test user");
			var tokenClient = new TokenClient(disco.TokenEndpoint, "testUser", "secret");
			var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("junwen", "password", "api1");
			if (tokenResponse.IsError)
			{
				Console.WriteLine(tokenResponse.Error);
				return;
			}
			requestApi(tokenResponse);
		}

		static async void requestApi(TokenResponse tokenResponse)
		{
			Console.WriteLine(tokenResponse.Json);
			using (var client = new HttpClient())
			{
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
}
