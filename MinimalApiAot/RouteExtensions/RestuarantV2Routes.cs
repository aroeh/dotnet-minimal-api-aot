namespace MinimalApiAot.RouteExtensions
{
    public static class RestuarantV2Routes
    {
        public static void MapRestuarantV2Routes(this WebApplication app)
        {
            // setup the versioned api and base route with version
            var restuarantV2 = app.NewVersionedApi();
            var restuarantV2Group = restuarantV2.MapGroup("/restuarant/v{version:apiVersion}").HasApiVersion(2.0);

            // map endpoints for the group

            // Get All Restuarants
            restuarantV2Group.MapGet("/", RestuarantV2Methods.GetAll).CacheOutput();


            // Find Restuarants using matching criteria from query strings
            restuarantV2Group.MapPost("/find", RestuarantV2Methods.Find);


            // Get a Restuarant using the provided id
            restuarantV2Group.MapGet("/{id}", RestuarantV2Methods.GetById);

            // Inserts a new restuarant
            restuarantV2Group.MapPost("/", RestuarantV2Methods.AddRestuarant);

            // Updates an existing restuarant
            restuarantV2Group.MapPut("/", RestuarantV2Methods.UpdateRestuarant);
        }
    }
}
