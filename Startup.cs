using FineUICore.QuickStart;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FineUICore.EmptyProject.RazorForms
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession();

            // 添加本地化服务 - 这是解决错误的关键
            services.AddLocalization(); 

            // FineUI 服务
            services.AddFineUI(Configuration);

            services.AddRazorPages();

            services.AddDbContext<MovieContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SQLServer")));

            // 配置请求参数限制
            services.Configure<FormOptions>(x =>
            {
                x.ValueCountLimit = 1024;   // 请求参数的个数限制（默认值：1024）
                x.ValueLengthLimit = 4194304;   // 单个请求参数值的长度限制（默认值：4194304 = 1024 * 1024 * 4）
            });
            
            // FineUI 服务
            services.AddFineUI(Configuration);

            services.AddRazorPages().AddMvcOptions(options =>
            {
                // 自定义JSON模型绑定（添加到最开始的位置）
                options.ModelBinderProviders.Insert(0, new FineUICore.JsonModelBinderProvider());

                // 自定义RazorForms过滤器（仅在启用EnableRazorForms时有效）
                options.Filters.Insert(0, new FineUICore.RazorFormsFilter());

            }).AddNewtonsoftJson().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();   
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            // FineUI 中间件（确保 UseFineUI 位于 UseEndpoints 的前面）
            app.UseFineUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
