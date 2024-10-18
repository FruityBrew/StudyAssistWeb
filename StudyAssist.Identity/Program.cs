using Microsoft.Extensions.DependencyInjection;

namespace StudyAssist.Identity
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddIdentityServer(options =>
				{
					options.KeyManagement.Enabled = false;
				})
				.AddInMemoryClients(IdentityConfiguration.GetClients())
				.AddInMemoryApiResources(IdentityConfiguration.GetApiResources())
				.AddInMemoryIdentityResources(IdentityConfiguration.GetIdentityResources())
				.AddDeveloperSigningCredential();

			builder.Services.AddControllers();

			var app = builder.Build();
			app.UseRouting();

			app.UseIdentityServer();

			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapDefaultControllerRoute();
			//});

			app.Run();
		}
	}
}
