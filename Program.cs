using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EscuelaWeb.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EscuelaWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Creo el web server lo guardo en una variable host , no lo ejecuto, por que quiero primero asegurarme de que mi base de datos
            //me funcione en memoria.
            var host = CreateWebHostBuilder(args).Build();
            //variable Scope le guardamos una lista de servicios luego de crearlo. Este scope se nos puede quedar vivo en memoria y para 
            //evitarlo utilizamos el using y le creamos una zona donde ese objeto pueda vivir.
            using (var scope = host.Services.CreateScope())
            {
                //Capturo mi "lista" de servicios.
                var services = scope.ServiceProvider;
                try
                {
                    //var context => le administro el servicio que cree en Sturtup.cs EscuelaContext
                    var context = services.GetRequiredService<EscuelaContext>();
                    //Me aseguro de que la base de datos haya sido creada y ya este en memoria.
                    context.Database.EnsureCreated();
                }catch(Exception ex) //Si no pudo acceder a la base de datos : capturo el error
                { 
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex , "An Error Ocurred Creating the Data Base");
                }
                host.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
