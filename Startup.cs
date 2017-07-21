using Api_ELearning.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Api_ELearning.Middleware;
using Microsoft.EntityFrameworkCore;
using Api_ELearning.Services;
using Api_ELearning.Repositories;
using Microsoft.AspNetCore.ResponseCompression;
using System.Linq;
using Microsoft.AspNetCore.Http.Features;

namespace Api_ELearning
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer().AddDbContext<ELearningDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ElearningDb"))
            );
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Optimal;
            });
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {  "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/font-woff2",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml" });
            });
            //add singleton, scoped or transient here
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<ICetificateRepository, CetificateRepository>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<IResultQuestionRepository, ResultQuestionRepository>();
            services.AddTransient<ICoursesRepository, CoursesRepository>();
            services.AddTransient<IRateRepository, RateCoursesRepository>();
            services.AddTransient<IExamRepository, ExamRepository>();
            services.AddTransient<ILearnRepository, LearnRepository>();
            services.AddTransient<IExamDetailsRepository, ExamDetailsRepository>();
            services.AddTransient<ICourseDetailsRepository, CourseDetailsRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ILessonRepository, LessonRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.Configure<FormOptions>(x => x.KeyLengthLimit= 1048576);
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, ELearningDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(builder =>
                        builder
                        .WithOrigins("http://localhost:60000", "http://localhost:5001")  //<-- OP's origin
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        );
            }
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/ELearning_Logs_{Date}.txt");
            
            app.UseResponseCompression().UseStaticFiles();

            app.UseTokenMiddlewareExtensions();

            app.UseMvc();

            InitDb.Init(db);
        }
    }
}
