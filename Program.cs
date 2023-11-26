
using AFayedFarm.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AFayedFarm
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<FarmContext>(
				option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
			builder.Services.AddIdentity<ApplicationUser,IdentityRole>(
				 option =>
				 {
					 option.Password.RequireNonAlphanumeric = false;
					 option.Password.RequiredLength = 5;
				 }).AddEntityFrameworkStores<FarmContext>();


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
