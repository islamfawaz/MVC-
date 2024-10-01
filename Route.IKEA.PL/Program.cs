using LinkDev.IKEA.BLL.Common.Services.Attachments;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
            #region Services
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure the database connection using ApplicationDbContext
            builder.Services.AddDbContext<ApplicationDbContext>((optionsBuilder) =>
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Register Department-related services and repositories
           // builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();

            // Register Employee-related services and repositories
           // builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();


            builder.Services.AddAutoMapper(M=>M.AddProfile(new MappingProfile()));

            builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
            builder.Services.AddTransient<IAttachmentService, AttachmentService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>((option) =>
            {
                option.Password.RequiredLength = 5;
                option.Password.RequireDigit = true;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = true;
                option.Password.RequiredUniqueChars = 1;

                option.User.RequireUniqueEmail = true;

                option.Lockout.AllowedForNewUsers = true;
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromDays(5);
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

             ///         builder.Services.AddScoped<UserManager<ApplicationUser>>();
			///builder.Services.AddScoped<SignInManager<ApplicationUser>>();
   /// builder.Services.AddScoped<RoleManager<IdentityRole>>();






			#endregion

			var app = builder.Build();

            #region Configure Kestrel Middlewares
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Employee}/{action=Index}/{id?}"); // This can be changed to Department or Employee as needed

            #endregion

            app.Run();
        }
    }
}
