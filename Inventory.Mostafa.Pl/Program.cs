using Inventory.Mostafa.Pl.Controllers.Helper;
namespace Inventory.Mostafa.Pl
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDependancies(builder.Configuration);

            var app = builder.Build();
            await DependancyInjection.UseApplicationPipeline(app);

            app.Run();

        }
    }
}
