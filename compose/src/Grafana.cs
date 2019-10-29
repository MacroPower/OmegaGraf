using Flurl;
using Flurl.Http;
using OmegaGraf.Compose.Config.Grafana;
using System.Threading.Tasks;

namespace OmegaGraf.Compose
{
    public class Grafana
    {
        private Token token;

        public Grafana()
        {
            this.token = GetToken().Result;
        }

        public Task<Token> GetToken()
        {
            return "http://localhost:3000"
                   .AppendPathSegments("api", "auth", "keys")
                   .WithBasicAuth("admin", "admin")
                   .PostJsonAsync(
                       new
                       {
                           role = "Admin",
                           name = "OG-ApiKey"
                       }
                   ).ReceiveJson<Token>();
        }

        public Task AddDataSource(DataSource dataSource)
        {
            return "http://localhost:3000"
                   .AppendPathSegments("api", "datasources")
                   .WithOAuthBearerToken(this.token.Key)
                   .PostJsonAsync(dataSource);
        }
    }
}