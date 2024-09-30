using LinkDev.IKEA.BLL.Common.Services.Attachments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Route.IKEA.BLL.Services.Departments;
using Route.IKEA.BLL.Services.Employees;
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
