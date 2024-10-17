using Duende.IdentityServer.Models;
using IdentityModel;

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
					ClientId = "studyAssist_id",
					ClientSecrets = {new Secret("studyAssist_secret".ToSha256()) },
					AllowedGrantTypes = GrantTypes.ClientCredentials,
					AllowedScopes =
					{
						"KnowledgeApi"
					}
				}
			};
		}

		internal static IEnumerable<ApiResource> GetApiResources()
		{
			return new List<ApiResource>
			{
				new ApiResource("KnowledgeApi")
			};
		}

		internal static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId()
			};
		}
	}
}
