using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using HXCloud.APIV2.Filters;
using HXCloud.APIV2.MiddleWares;
using HXCloud.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HXCloud.APIV2
{
    public class StartupTest
    {
        public StartupTest(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //���automapper ��Ҫ����automapper������
            var test2 = AppDomain.CurrentDomain.Load("hxcloud.service");
            services.AddAutoMapper(test2/*AppDomain.CurrentDomain.GetAssemblies()*//*Assembly.Load("HXCloud.ViewModel")*/);

            services.AddScoped<TypeActionFilter>();

            services.AddControllers();
            #region ����Զ���ģ����֤
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        //Type = "www.baidu.com",
                        Title = "����Ĳ�������",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "���Errors",
                        Instance = context.HttpContext.Request.Path
                    };
                    problemDetails.Extensions.Add("TraceId", context.HttpContext.TraceIdentifier);
                    return new BadRequestObjectResult(problemDetails);
                };
            });
            #endregion

            //�������ݿ�
            var connection = Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<HXCloudContext>(a => a.UseSqlServer(connection, b => b.MigrationsAssembly("HXCloud.Repository"))
            .UseLoggerFactory(MyLoggerFactory));
        }
        #region ���sql��־�������̨
        public static readonly ILoggerFactory MyLoggerFactory
          = LoggerFactory.Create(builder =>
          {
              builder
             .AddFilter((category, level) =>
                 category == DbLoggerCategory.Database.Command.Name
                 && level == LogLevel.Information)
             .AddConsole();
          });
        #endregion

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //�м��˳�� ExceptionHandler��HSTS��HttpsRedirection��Static Files��Routing ��CORS��Authentication��Authorization��Custom Middlewares��Endpoint
            loggerFactory.AddLog4Net();
            app.UseStaticFiles();
            app.UseRouting();

            //app.UseAuthorization();
            //�Զ����м��
            //app.UseTokenCheck();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //ҵ���߼������ڳ��������ռ�
            Assembly service = Assembly.Load("HXCloud.Service");
            //�ӿڲ����ڳ��������ռ�
            Assembly repository = Assembly.Load("HXCloud.Repository");
            #region
            //�Զ�ע��
            //builder.RegisterAssemblyTypes(service, repository)
            //    .Where(t => t.Name.EndsWith("Service"))
            //    .AsImplementedInterfaces();
            ////ע��ִ�������IRepository�ӿڵ�Repository��ӳ��
            //builder.RegisterGeneric(typeof(Repository<>))
            //    //InstancePerDependency��Ĭ��ģʽ��ÿ�ε��ã���������ʵ��������ÿ�����󶼴���һ���µĶ���
            //    .As(typeof(IRepository<>)).InstancePerDependency();
            #endregion
            builder.RegisterAssemblyTypes(service/*GetAssemblyByName("HXCloud.Service")*/).AsImplementedInterfaces().PropertiesAutowired();
            builder.RegisterAssemblyTypes(repository/*GetAssemblyByName("HXCloud.Repository")*/).AsImplementedInterfaces().PropertiesAutowired();
        }
    }
}
