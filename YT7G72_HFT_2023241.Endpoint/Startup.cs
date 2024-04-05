using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Logic.Implementations;
using YT7G72_HFT_2023241.Logic.Interfaces;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Endpoint
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<UniversityDatabaseContext>();
            services.AddTransient<IRepository<Student>, StudentRepository>();
            services.AddTransient<IRepository<Teacher>, TeacherRepository>();
            services.AddTransient<IRepository<Curriculum>, CurriculumRepository>();
            services.AddTransient<IRepository<Subject>, SubjectRepository>();
            services.AddTransient<IRepository<Course>, CourseRepository>();
            services.AddTransient<IRepository<Grade>, GradeRepository>();
            services.AddTransient<IPersonLogic, PersonLogic>();
            services.AddTransient<IGradeLogic, GradeLogic>();
            services.AddTransient<IEducationLogic, EducationLogic>();
            services.AddTransient<ICurriculumLogic, CurriculumLogic>();
            services.AddControllers();
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NeptunDbApp.Endpoint", Version = "v1" });
            });
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI();
                app.UseSwagger();
                app.UseSwagger(c =>
                {
                    c.SerializeAsV2 = true;
                });

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Neptun API V1");
                });
                
            }

            app.UseDeveloperExceptionPage();

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                .Get<IExceptionHandlerPathFeature>()
                .Error;
                var response = new { Msg = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:12466")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("NEPTUN API v1");
                });
                endpoints.MapHub<SignalRHub>("/hub");
            });
        }
    }
}
