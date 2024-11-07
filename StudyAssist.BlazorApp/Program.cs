using Radzen;
using StudyAssist.BlazorApp.Components;

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
