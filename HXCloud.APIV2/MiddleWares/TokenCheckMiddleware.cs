using HXCloud.Common;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HXCloud.APIV2.MiddleWares
{
    public class TokenCheckMiddleware
    {
        private readonly RequestDelegate _next;
        IConfiguration Configuration { get; }
        public TokenCheckMiddleware(RequestDelegate requestDelegate, IConfiguration configuration)
        {
            this._next = requestDelegate;
            Configuration = configuration;
        }
        public Task Invoke(HttpContext context)
        {
            //如果不带认证信息则跳过,在授权中检查是否允许匿名登录
            #region
            //            string authorization = context.Request.Headers["Authorization"];

            //            // If no authorization header found, nothing to process further
            //            if (string.IsNullOrEmpty(authorization))
            //            {
            //                return AuthenticateResult.None;
            //            }

            //            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            //{
            //                token = authorization.Substring("Bearer ".Length).Trim();
            //            }

            //// If no token found, no further work possible
            //            if (string.IsNullOrEmpty(token))
            //            {
            //                return AuthenticateResult.None();
            //            }
            #endregion

            string Tickets = context.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(Tickets))
            {
                return _next.Invoke(context);
            }
            else
            {
                //验证用户登录的票据
                if (Tickets.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = Tickets.Substring("Bearer ".Length).Trim();
                    try
                    {
                        string strUser = DesEncrypt.DecryptString(token);
                        var user = JsonConvert.DeserializeObject<UserMessage>(strUser);
                        var claims = new List<Claim>();
                        //claims.Add(new Claim(ClaimTypes.Name, user.Account));
                        //claims.Add(new Claim(ClaimTypes.Role, user.Roles));
                        claims.Add(new Claim(ClaimTypes.Name, strUser));
                        //claims.Add(new Claim("code", user.Code));
                        var identity = new ClaimsIdentity(claims, "MyClaimsLogin");
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        context.User = principal;
                        return _next.Invoke(context);

                        #region 跳过redis
                        /*
                        ConnectionMultiplexer redis = RedisHelper.Singleton;// ConnectionMultiplexer.Connect(redisconf);
                        IDatabase db = redis.GetDatabase();
                        if (db.StringGet(user.Key).HasValue)
                        {
                            //判断是否被踢掉
                            if (db.StringGet(user.Key) != token)
                            {
                                context.Response.StatusCode = 401;
                                return context.Response.WriteAsync("该用户在其他地方已登录");
                            }
                            else
                            {
                                var config = Configuration.GetSection("time");
                                int hours = int.Parse(config.GetSection("hours").Value);
                                int minutes = int.Parse(config.GetSection("minutes").Value);
                                int seconds = int.Parse(config.GetSection("seconds").Value);
                                db.KeyExpire(user.Key, new TimeSpan(hours, minutes, seconds));
                                var claims = new List<Claim>();
                                //claims.Add(new Claim(ClaimTypes.Name, user.Account));
                                claims.Add(new Claim(ClaimTypes.Name, strUser));
                                //claims.Add(new Claim(ClaimTypes.Email, "xzc02106430@163.com"));
                                //claims.Add(new Claim(ClaimTypes.Role, user.Roles));
                                //claims.Add(new Claim(ClaimTypes.Country, user.Token));
                                claims.Add(new Claim("code", user.Code));
                                var identity = new ClaimsIdentity(claims, "MyClaimsLogin");
                                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                                //context.User = new GenericPrincipal()
                                context.User = principal;
                                return _next.Invoke(context);
                            }
                        }
                        else
                        {
                            //如果redis不可以用
                            context.Response.StatusCode = 401;
                            return context.Response.WriteAsync("该凭据不正确，请重新获取凭据");
                            //context.ErrorResult = new AuthenticationFailureResult("Missing credentials", req);
                        }
                        */
                        #endregion
                    }
                    //catch (RedisConnectionException ex)
                    //{
                    //    //redis无法连接
                    //    context.Response.StatusCode = 401;
                    //    return context.Response.WriteAsync("该授权信息不正确" + ex.Message);
                    //}
                    catch (Exception ex)
                    {
                        var ty = ex.GetType();
                        context.Response.StatusCode = 401;
                        return context.Response.WriteAsync("该授权信息不正确" + ex.Message);
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    return context.Response.WriteAsync("票据格式不正确，请使用正确的票据格式");
                }
            }
        }
    }
}
