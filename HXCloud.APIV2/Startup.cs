using System;
using System.Reflection;
using System.Text;
using Autofac;
using AutoMapper;
using HXCloud.APIV2.Filters;
using HXCloud.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HXCloud.APIV2
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //添加automapper 需要载入automapper配置类
            var test2 = AppDomain.CurrentDomain.Load("hxcloud.service");
            services.AddAutoMapper(test2/*AppDomain.CurrentDomain.GetAssemblies()*//*Assembly.Load("HXCloud.ViewModel")*/);

            services.AddScoped<TypeActionFilter>();

            services.AddControllers();
            #region 添加自定义模型认证
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        //Type = "www.baidu.com",
                        Title = "输入的参数有误",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "详见Errors",
                        Instance = context.HttpContext.Request.Path
                    };
                    problemDetails.Extensions.Add("TraceId", context.HttpContext.TraceIdentifier);
                    return new BadRequestObjectResult(problemDetails);
                };
            });
            #endregion

            #region 添加jwt认证
            services.Configure<JwtConfig>(Configuration.GetSection("JWT"));
            JwtConfig config = new JwtConfig();
            Configuration.GetSection("JWT").Bind(config);//读取appsetting.json配置文件，并将配置文件绑定到类上
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    //ValidateLifetime = true,
                    ValidateLifetime = false,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config.Issuer,
                    ValidAudience = config.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.IssuerSigningKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion
            //添加授权策略
            //services.AddAuthorization(opt=> {
            //    //opt.AddPolicy("")
            //});
            //配置数据库
            var connection = Configuration.GetConnectionString("SqlServer");
            services.AddDbContext<HXCloudContext>(a => a.UseSqlServer(connection, b => b.MigrationsAssembly("HXCloud.Repository"))
            .UseLoggerFactory(MyLoggerFactory));
            //设置上传文件大小
            services.Configure<FormOptions>(opt =>
            {
                opt.ValueCountLimit = int.MaxValue;
                opt.MultipartBodyLengthLimit = 300_000_000; //int.MaxValue;
                opt.MultipartHeadersLengthLimit = int.MaxValue;
            });
        }
        #region 添加sql日志输出控制台
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
            //中间件顺序 ExceptionHandler、HSTS、HttpsRedirection、Static Files、Routing 、CORS、Authentication、Authorization、Custom Middlewares、Endpoint
            loggerFactory.AddLog4Net();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //自定义中间件
            //app.UseTokenCheck();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //业务逻辑层所在程序集命名空间
            Assembly service = Assembly.Load("HXCloud.Service");
            //接口层所在程序集命名空间
            Assembly repository = Assembly.Load("HXCloud.Repository");
            #region
            //自动注入
            //builder.RegisterAssemblyTypes(service, repository)
            //    .Where(t => t.Name.EndsWith("Service"))
            //    .AsImplementedInterfaces();
            ////注册仓储，所有IRepository接口到Repository的映射
            //builder.RegisterGeneric(typeof(Repository<>))
            //    //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //    .As(typeof(IRepository<>)).InstancePerDependency();
            #endregion
            builder.RegisterAssemblyTypes(service/*GetAssemblyByName("HXCloud.Service")*/).AsImplementedInterfaces().PropertiesAutowired();
            builder.RegisterAssemblyTypes(repository/*GetAssemblyByName("HXCloud.Repository")*/).AsImplementedInterfaces().PropertiesAutowired();
        }
    }
}
