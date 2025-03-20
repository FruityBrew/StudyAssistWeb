using Radzen;
using StudyAssist.BlazorApp.Components;
using StudyAssist.BlazorApp.Interfaces;
using StudyAssist.BlazorApp.Services;

namespace StudyAssist.BlazorApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddRazorComponents()
				.AddInteractiveServerComponents();
			builder.Services.AddBlazorBootstrap();
            builder.Services.AddRadzenComponents();

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IDtoReceiverService, AppDtoReceivedService>();

            var app = builder.Build();
			
			app.UseAntiforgery();

			app.MapRazorComponents<Components.App>()
				.AddInteractiveServerRenderMode();

			app.UseStaticFiles();
			
			//app.UseRouting();
			//app.MapGet("/", () => "Hello World!");	


			app.Run();
		}
	}
}
