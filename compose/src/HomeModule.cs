using Nancy;

namespace OmegaGraf.Compose.MetaData
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", _ =>
            {
                return HttpStatusCode.OK;
            });

            Get("/swagger-ui", _ =>
            {
                var url = $"{Request.Url.BasePath}/api-docs";
                return View["swagger", url];
            });
        }
    }
}