
using AFayedFarm.Helper;
using AFayedFarm.Model;
using AFayedFarm.Repositories.Auth;
using AFayedFarm.Repositories.Clients;
using AFayedFarm.Repositories.Employee;
using AFayedFarm.Repositories.Expenses;
using AFayedFarm.Repositories.FinancialSafe;
using AFayedFarm.Repositories.Fridges;
using AFayedFarm.Repositories.Products;
using AFayedFarm.Repositories.Store;
using AFayedFarm.Repositories.Supplier;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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

			builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

			builder.Services.AddDbContext<FarmContext>(
				option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
				 option =>
				 {
					 option.Password.RequireNonAlphanumeric = false;
					 option.Password.RequiredLength = 5;
				 }).AddEntityFrameworkStores<FarmContext>();
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
			});

			builder.Services.AddAuthentication(option =>
			{
				option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(o =>
			{
				o.RequireHttpsMetadata = false;
				o.SaveToken = true;
				o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration["JWT:Issuer"],
					ValidateAudience = true,
					ValidAudience = builder.Configuration["JWT:Audiance"],
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
				};
			});


			builder.Services.AddScoped<IFarmsRepo, FarmsRepo>();
			builder.Services.AddScoped<IClientRepo, ClientRepo>();
			builder.Services.AddScoped<IExpenseRepo, ExpenseRepo>();
			builder.Services.AddScoped<IStoreRepo, StoreRepo>();
			builder.Services.AddScoped<IProductRepo, ProductRepo>();
			builder.Services.AddScoped<ISafeRepo, SafeRepo>();
			builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
			builder.Services.AddScoped<IFridgeRepo, FridgeRepo>();
			builder.Services.AddScoped<IAuthService, AuthService>();

			OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
			{
				Reference = new OpenApiReference()
				{
					Id = "Bearer",
					Type = ReferenceType.SecurityScheme
				}
			};
			OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
			{
				{securityScheme, new string[] { }},
			};
			builder.Services.AddSwaggerGen(swagger =>
			{
				// To Enable authorization using Swagger (JWT)    
				swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Bearer",
					BearerFormat = "JWT",
					Scheme = "bearer",
					Description = "Specify the authorization token.",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
				});
				swagger.AddSecurityRequirement(securityRequirements);

			});


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			//if (app.Environment.IsDevelopment())
			//{
			app.UseSwagger();
			app.UseSwaggerUI();
			//}

			app.UseCors("AllowAll");
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
