using Duende.IdentityServer.Models;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StudyAssist.Identity
{
	internal static class IdentityConfiguration
	{
		internal static IEnumerable<Client> GetClients()
		{
			return new List<Client>
			{
				new Client()
				{
					ClientName = "FruityBrew",
					ClientId = "fruityBrew_id",
					ClientSecrets = {new Secret("fruityBrew_secret".ToSha256()) },
					AllowedGrantTypes = GrantTypes.ClientCredentials,
				
					AllowedScopes =
					{
						"Author"
					},
				},
				new Client()
				{
					ClientName = "SomeUser",
					ClientId = "someUser_id",
					ClientSecrets = {new Secret("someUser_secret".ToSha256()) },
					AllowedGrantTypes = GrantTypes.ClientCredentials,

					AllowedScopes =
					{
						"User"
					},
				},
				new Client()
				{
					ClientName = "studyAssist",
					ClientId = "studyAssist_id",
					ClientSecrets = {new Secret("studyAssist_secret".ToSha256()) },
					AllowedGrantTypes = GrantTypes.ClientCredentials,

					AllowedScopes =
					{
						"Author", "User"
					},
				}
			};
		}

		internal static IEnumerable<ApiResource> GetApiResources()
		{
			return new List<ApiResource>
			{
				new ApiResource("KnowledgeApi.Read", new List<string>{"Author", "User"}),
				new ApiResource("KnowledgeApi.Write", new List<string>{"Author"}),

			};
		}

		internal static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
			};
		}

		internal static IEnumerable<ApiScope> GetApiScopes()
		{
			return new List<ApiScope> { new ApiScope("Author"), new ApiScope("User") };
		}
	}
}
