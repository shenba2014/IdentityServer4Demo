using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace DemoIdentityServer
{
	public class Config
	{
		public static IEnumerable<ApiResource> GetApiResource()
		{
			return new List<ApiResource>
			{
				new ApiResource("api1", "My API")
			};
		}

		public static IEnumerable<Client> GetClients()
		{
			return new List<Client>
			{
				new Client
				{
					ClientId = "client",
					AllowedGrantTypes = GrantTypes.ClientCredentials,
					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},
					AllowedScopes = {"api1"}
				},
				new Client
				{
					ClientId = "testUser",
					AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
					ClientSecrets =
					{
						new Secret("secret".Sha256())
					},
					AllowedScopes = {"api1"}
				},
				new Client {
					ClientId = "mvc",
					ClientName = "mvc client",
					AllowedGrantTypes = GrantTypes.Implicit,
					AllowedScopes = new List<string>
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile
					},
					RedirectUris = new List<string> {"http://localhost:61140/signin-oidc"},
					PostLogoutRedirectUris = new List<string> { "http://localhost:61140" }
				}
			};
		}

		public static List<TestUser> GetUsers()
		{
			return new List<TestUser>
			{
				new TestUser
				{
					SubjectId = "1",
					Username = "junwen",
					Password = "password"
				},
				new TestUser
				{
					SubjectId = "2",
					Username = "bob",
					Password = "password"
				}
			};
		}

		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
			};
		}
	}
}
