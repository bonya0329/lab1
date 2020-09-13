using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace lab1
{
    public class Startup
    {
        //Delegate
        public delegate Task RequestDelegate(HttpContext context);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private static int cash = 50000;

        private static void PIN_CODE(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await next.Invoke();
                await context.Response.WriteAsync($"You entered PIN code: ****\n");
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync($"Your PIN code: 1234\n");
            });
        }

        private static void Balance(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync($"Balance: {cash}");
            });
        }

        private static void Withdraw(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                cash -= 3000;
                await next.Invoke();
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("You took the money: 3000\n");
                await context.Response.WriteAsync($"\nYou have:{cash} tenge left on your card.");
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Map("/pin", PIN_CODE);
            app.Map("/balance", Balance);
            app.Map("/withdraw", Withdraw);

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Welcome to the virtual cash machine\n");
                await context.Response.WriteAsync("To see your balance go to the /balance\n");
                await context.Response.WriteAsync("To see your pin code go to the /pin\n");
                await context.Response.WriteAsync("To take money go to the /withdraw\n");
            });
        }
    }
}