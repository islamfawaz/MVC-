using LinkDev.IKEA.BLL.Common.Services.Attachments;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Route.IKEA.BLL.Services.Departments;
using Route.IKEA.BLL.Services.Employees;
using Route.IKEA.DAL.Entities.Identity;
using Route.IKEA.DAL.Presistence.Data;
using Route.IKEA.DAL.Presistence.Repositories.Departments;
using Route.IKEA.DAL.Presistence.Repositories.Employees;
using Route.IKEA.DAL.Presistence.UnitOfWork;
using Route.IKEA.PL.Mapping;

namespace Route.IKEA.PL
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			// Configure the database connection using ApplicationDbContext
			builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
			{
				optionsBuilder.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			// Register Department-related services and repositories
			builder.Services.AddScoped<IDepartmentService, DepartmentService>();

			// Register Employee-related services and repositories
			builder.Services.AddScoped<IEmployeeService, EmployeeService>();

			builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));

			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddTransient<IAttachmentService, AttachmentService>();

			// Configure Identity services
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequiredLength = 5;
				options.Password.RequireDigit = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
				options.Password.RequiredUniqueChars = 1;

				options.User.RequireUniqueEmail = true;

				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);
			})
			.AddEntityFrameworkStores<ApplicationDbContext>();

			// Configure cookie settings for authentication
			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/SignIn";
				options.AccessDeniedPath = "/Home/Error";
				options.ExpireTimeSpan = TimeSpan.FromDays(1);
				options.LogoutPath = "/Account/SignIn";
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Account}/{action=SignIn}/{id?}");

			app.Run();
		}
	}
}
