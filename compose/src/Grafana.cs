using Flurl;
using Flurl.Http;
using OmegaGraf.Compose.Config.Grafana;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace OmegaGraf.Compose
{
    public class Grafana : IDisposable
    {
        private readonly Token token;

        public Grafana()
        {
            this.token = GetToken().Result;
        }

        public void Dispose()
        {
            RemoveToken().Wait();
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

        public Task<IEnumerable<Token>> ListTokens()
        {
            return "http://localhost:3000"
                   .AppendPathSegments("api", "auth", "keys")
                   .WithOAuthBearerToken(this.token.Key)
                   .GetJsonAsync<IEnumerable<Token>>();
        }

        public async Task<System.Net.Http.HttpResponseMessage> RemoveToken()
        {
            var tokens = await ListTokens();

            var toDelete = tokens.First(t => t.Name == this.token.Name);

            return await "http://localhost:3000"
                   .AppendPathSegments("api", "auth", "keys", toDelete.ID)
                   .WithOAuthBearerToken(this.token.Key)
                   .DeleteAsync();
        }

        public async Task<System.Net.Http.HttpResponseMessage> AddDataSource(DataSource dataSource)
        {
            try
            {
                return await "http://localhost:3000"
                       .AppendPathSegments("api", "datasources")
                       .WithOAuthBearerToken(this.token.Key)
                       .PostJsonAsync(dataSource);
            }
            catch (FlurlHttpException e)
            {
                if (e.Call.HttpStatus.ToString() == "Conflict")
                {
                    return e.Call.Response;
                }

                throw e;
            }
        }

        public async Task<System.Net.Http.HttpResponseMessage> AddDashboard(object jsonModel)
        {
            try
            {
                return await "http://localhost:3000"
                       .AppendPathSegments("api", "dashboards", "db")
                       .WithOAuthBearerToken(this.token.Key)
                       .PostJsonAsync(jsonModel);
            }
            catch (FlurlHttpException e)
            {
                if (e.Call.HttpStatus.ToString() == "PreconditionFailed")
                {
                    return e.Call.Response;
                }

                throw e;
            }
        }
    }
}