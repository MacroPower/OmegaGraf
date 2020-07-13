using Nancy;

namespace OmegaGraf.Compose.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            this.Get("/ready", _ =>
            {
                return HttpStatusCode.OK;
            });

            this.Get("/swagger-ui", _ =>
            {
                var url = $"{this.Request.Url.BasePath}/api-docs";
                return this.View["swagger", url];
            });

            this.Get("/", _ =>
            {
                return this.View["Index"];
            });

            this.Get("/{all*}", _ =>
            {
                return this.View["Index"];
            });
        }
    }
}
