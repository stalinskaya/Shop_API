using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shop.BLL.Filters;
using Shop.BLL.Interfaces;
using Shop.BLL.Services;
using Shop.BLL.Settings;
using Shop.DAL.EF;
using Shop.Models;

namespace Shop.UI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
		
			services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true; // consent required
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddSession(options =>
			{
				// Set a short timeout for easy testing.
				options.IdleTimeout = TimeSpan.FromSeconds(10);
				options.Cookie.HttpOnly = true;
				// Make the session cookie essential
				options.Cookie.IsEssential = true;
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddDbContext<ShopContext>(options =>
			options.UseSqlServer(Configuration.GetConnectionString("ShopContext")));

			services.AddIdentity<Shop.Models.ApplicationUser, IdentityRole>(options =>
			{
				options.User.RequireUniqueEmail = false;
			})
			.AddEntityFrameworkStores<ShopContext>()
			.AddDefaultTokenProviders();

			
			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 4;
			}
			);

			services.AddScoped<DAL.Interfaces.IUnitOfWork, DAL.Repositories.EFUnitOfWork>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IOrderProductService, OrderProductService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IEmailService, EmailService>();
			services.AddScoped<IRoleService, RoleService>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IFileService, FileService>();


			services.AddCors();

			//Jwt Authentication

			var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x => {
				x.RequireHttpsMetadata = false;
				x.SaveToken = false;
				x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				};
			});
		}
	
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
		{

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}


			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCors(builder =>
			
			builder.WithOrigins(Configuration["ApplicationSettings:Client_URL"].ToString())
			.AllowAnyHeader()
			.AllowAnyMethod()
			);

			app.UseAuthentication();
			app.UseCookiePolicy();
			app.UseSession();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
			CreateRoles(serviceProvider).Wait();
		}

		private async Task CreateRoles(IServiceProvider serviceProvider)
		{
			//adding custom roles
			var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			string[] roleNames = { "Admin", "Manager", "Member" };
			IdentityResult roleResult;

			foreach (var roleName in roleNames)
			{
				//creating the roles and seeding them to the database
				var roleExist = await RoleManager.RoleExistsAsync(roleName);
				if (!roleExist)
				{
					roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
				}
			}

			//creating a super user who could maintain the web app
			var poweruser = new ApplicationUser
			{
				UserName = Configuration.GetSection("UserSettings")["UserEmail"],
				Email = Configuration.GetSection("UserSettings")["UserEmail"],
				EmailConfirmed = true
			};

			string UserPassword = Configuration.GetSection("UserSettings")["UserPassword"];
			var _user = await UserManager.FindByEmailAsync(Configuration.GetSection("UserSettings")["UserEmail"]);

			if (_user == null)
			{
				var createPowerUser = await UserManager.CreateAsync(poweruser, UserPassword);
				if (createPowerUser.Succeeded)
				{
					//here we tie the new user to the "Admin" role 
					await UserManager.AddToRoleAsync(poweruser, "Admin");
				}
			}
		}
	}
}
