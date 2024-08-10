using Microsoft.AspNetCore.Mvc;
using MinimalApiAot.Models;
using MinimalApiAot.Repos;

namespace MinimalApiAot.RouteExtensions
{
    public static class RestuarantV1Routes
    {
        public static void MapRestuarantV1Routes(this WebApplication app)
        {
            // setup the versioned api and base route with version
            var restuarantV1 = app.NewVersionedApi();
            var restuarantV1Group = restuarantV1.MapGroup("/restuarant/v{version:apiVersion}")
                .HasApiVersion(1.0)
                .HasDeprecatedApiVersion(1.0);

            // map endpoints
            restuarantV1Group.MapGet("/", async(IRestuarantRepo restuarantRepo) =>
            {
                List<Restuarant> restuarants = await restuarantRepo.GetAllRestuarants();
                return restuarants;
            });

            restuarantV1Group.MapGet("/find", async (IRestuarantRepo restuarantRepo, [FromQuery] string name, [FromQuery] string cuisine) =>
            {
                List<Restuarant> restuarants = await restuarantRepo.FindRestuarants(name, cuisine);
                return restuarants;
            });

            restuarantV1Group.MapGet("/{id}", async (IRestuarantRepo restuarantRepo, string id) =>
            {
                Restuarant restuarant = await restuarantRepo.GetRestuarant(id);
                return restuarant;
            });
        }
    }
}
