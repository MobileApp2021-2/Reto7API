using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using TicTacToeAPI.Logica.Dominio.Configurtacion.Implementacion;
using TicTacToeAPI.Logica.LogicaDeNegocio.Hubs.Implementacion;
using TicTacToeAPI.Logica.LogicaDeNegocio.Implementacion;
using TicTacToeAPI.Logica.LogicaDeNegocio.Interfaz;
using TicTacToeAPI.Persistencia.UnidadDeTrabajo.Implementacion;
using TicTacToeAPI.Persistencia.UnidadDeTrabajo.Interfaz;

namespace TicTacToeAPI
{
    public class Startup
    {
        public readonly MongoSettings _mongoSettings;
        readonly string misOrigenes = "misOrigenes";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _mongoSettings = Configuration.GetSection("BoardDatabaseSettings").Get<MongoSettings>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ResolvverInyeccionUnidadDeTrabajo(services);
            ConfiguracionLogicaNegocio(services);

            services.AddControllers(); //.AddJsonOptions(x =>
                   // x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.SetIsOriginAllowed(_ => true)
                     .WithMethods("GET", "POST")
                     .AllowAnyHeader()
                     .AllowCredentials());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TicTacToeAPI", Version = "v1" });
            });

            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicTacToeAPI v1"));
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TicTacToeHub>("/tictactoe");
            });
        }

        private void ResolvverInyeccionUnidadDeTrabajo(IServiceCollection services)
        {
            services.AddSingleton<IUnitOfwork>(option => new UnitOfWork(_mongoSettings));
        }

        private void ConfiguracionLogicaNegocio(IServiceCollection services)
        {
            services.AddTransient<IBoardLogic, BoardLogic>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
