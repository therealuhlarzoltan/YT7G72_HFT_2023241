using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Logic;
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
            services.AddTransient<UniversityDatabaseContext>();
            services.AddTransient<IRepository<Student>, StudentRepository>();
            services.AddTransient<IRepository<Teacher>, TeacherRepository>();
            services.AddTransient<IRepository<Curriculum>, CurriculumRepository>();
            services.AddTransient<IRepository<Subject>, SubjectRepository>();
            services.AddTransient<IRepository<Course>, CourseRepository>();
            services.AddTransient<IRepository<Grade>, GradeRepository>();
            services.AddTransient<IPersonLogic, PersonLogic>();
            services.AddTransient<IGradeLogic, GradeLogic>();
            services.AddTransient<IEducationLogic, EducationLogic>();
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                .Get<IExceptionHandlerPathFeature>()
                .Error;
                var response = new { error = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));
        }
    }
}
