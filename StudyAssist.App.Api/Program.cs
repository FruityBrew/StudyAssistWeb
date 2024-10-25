namespace StudyAssist.App.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddHttpClient();

			var app = builder.Build();


			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
