using Nancy;

namespace OmegaGraf.Compose.MetaData
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/ready", _ =>
            {
                return HttpStatusCode.OK;
            });

            Get("/swagger-ui", _ =>
            {
                var url = $"{Request.Url.BasePath}/api-docs";
                return View["swagger", url];
            });

            Get("/", _ =>
            {
                return View["Index"];
            });

            Get("/(.*)", _ =>
            {
                return View["Index"];
            });
        }
    }
}