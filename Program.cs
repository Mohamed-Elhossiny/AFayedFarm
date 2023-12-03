
using AFayedFarm.Model;
using AFayedFarm.Repositories.Clients;
using AFayedFarm.Repositories.Expenses;
using AFayedFarm.Repositories.Supplier;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

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
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
			});
			
			builder.Services.AddScoped<IFarmsRepo, FarmsRepo>();
			builder.Services.AddScoped<IClientRepo, ClientRepo>();
			builder.Services.AddScoped<IExpenseRepo, ExpenseRepo>();


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseCors("AllowAll");
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
