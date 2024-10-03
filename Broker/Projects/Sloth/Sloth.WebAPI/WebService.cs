namespace Sloth.WebAPI
{

	public class WebService
	{
		WebApplicationBuilder _builder = WebApplication.CreateBuilder();

		public WebService()
		{

		}

		public void Run()
		{           
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			_builder.Services.AddControllers();
			_builder.Services.AddEndpointsApiExplorer();
			_builder.Services.AddSwaggerGen();
			_builder.Services.AddControllers();
			_builder.Services.AddMvc().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.PropertyNamingPolicy = null;
				options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict;
			});
			var app = _builder.Build();

			//_builder.Services.AddCors((o) =>
			//o.AddPolicy("cors_Policy_pls_work", (p) => p.WithOrigins("*", "localhost", "http://0.0.0.0")
			//));



			
			app.UseSwagger();
			app.UseSwaggerUI();
			


			app.UseAuthorization();
			app.MapControllers();

			app.Run();

		}
	}
}