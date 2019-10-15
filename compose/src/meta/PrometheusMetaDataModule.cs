using Nancy;

namespace OmegaGraf.Compose.MetaData
{
    public class PrometheusModule : NancyModule
    {
        public PrometheusModule() : base("/prom")
        {
            Get(
                "/{id}",
                args =>
                {
                    return Nancy.HttpStatusCode.OK;
                }, null, "Info"
            );

            Post(
                "/",
                args =>
                {
                    // bind and execute runner

                    return Nancy.HttpStatusCode.OK;
                }, null, "Deploy"
            );
        }
    }
}