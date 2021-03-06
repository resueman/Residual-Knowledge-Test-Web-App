using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResidualKnowledgeTestApp.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Server.Services;
using AutoMapper;

namespace ResidualKnowledgeTestApp.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAutoMapper(typeof(Startup));

            // AUTOMAPPER
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // DATABASE
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ProjectContext>(options => options.UseSqlServer(connectionString));

            // REPOSITORIES
            services.AddScoped<ICheckingDisciplineRepository, CheckingDisciplineRepository>();
            services.AddScoped<ICompetenceRepository, CompetenceRepository>();
            services.AddScoped<ICurriculumRepository, CurriculumRepository>();
            services.AddScoped<IDisciplineRepository, DisciplineRepository>();
            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<IMarkCriterionRepository, MarkCriterionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // SERVICES
            services.AddScoped<ICheckingDisciplinesService, CheckingDisciplinesService>();
            services.AddScoped<ICompetenceService, CompetenceService>();
            services.AddScoped<ICurriculumsService, CurriculumsService>();
            services.AddScoped<IDisciplinesService, DisciplinesService>();
            services.AddScoped<IProjectsService, ProjectsService>();
            services.AddScoped<IUserService, UserService>();

            services.AddControllersWithViews();
            
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
