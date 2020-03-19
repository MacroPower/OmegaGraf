using Flurl;
using Flurl.Http;
using OmegaGraf.Compose.Config.Grafana;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using NLog;
using System.Threading;
using Polly;

namespace OmegaGraf.Compose
{
    public class Grafana : IDisposable
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private Token token;
        private readonly string uri;

        public Grafana(string uri)
        {
            this.uri = uri;

            try
            {
                Policy
                    .Handle<Exception>(ex => {
                        logger.Warn(ex, "Error fetching Grafana token, retrying...");
                        return true;
                    })
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(5))
                    .Execute(() => this.token = CreateToken().Result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error fetching Grafana token");
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                RemoveToken().Wait();
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Unable to remove the Grafana token." + 
                                "This may cause errors on future deployments.");
            }
        }

        public Task<Token> CreateToken()
        {
            try
            {
                return this.uri
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
            catch (Exception ex)
            {
                logger.Error(ex, "Could not create a Grafana token");
                throw;
            }
        }

        public Task<IEnumerable<Token>> ListTokens()
        {
            try
            {
                return this.uri
                    .AppendPathSegments("api", "auth", "keys")
                    .WithOAuthBearerToken(this.token.Key)
                    .GetJsonAsync<IEnumerable<Token>>();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Could not list Grafana tokens");
                throw;
            }
        }

        public async Task<System.Net.Http.HttpResponseMessage> RemoveToken()
        {
            var tokens = await ListTokens();

            var toDelete = tokens.FirstOrDefault(t => t.Name == this.token.Name);

            if (toDelete == null)
                return null;

            try
            {
                return await this.uri
                    .AppendPathSegments("api", "auth", "keys", toDelete.ID)
                    .WithOAuthBearerToken(this.token.Key)
                    .DeleteAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Could not remove Grafana token");
                throw;
            }
        }

        public async Task<System.Net.Http.HttpResponseMessage> AddDataSource(DataSource dataSource)
        {
            try
            {
                return await this.uri
                       .AppendPathSegments("api", "datasources")
                       .WithOAuthBearerToken(this.token.Key)
                       .PostJsonAsync(dataSource);
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpStatus.ToString() == "Conflict")
                {
                    logger.Warn(ex, "Seems like you already have this datasource, skipping...");

                    return ex.Call.Response;
                }

                logger.Error(ex);

                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public async Task<System.Net.Http.HttpResponseMessage> AddDashboard(Dashboard dashboard)
        {
            try
            {
                return await this.uri
                       .AppendPathSegments("api", "dashboards", "db")
                       .WithOAuthBearerToken(this.token.Key)
                       .PostJsonAsync(dashboard);
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpStatus.ToString() == "PreconditionFailed")
                {
                    logger.Warn(ex, "Seems like you already have this dashboard, skipping...");

                    return ex.Call.Response;
                }

                logger.Error(ex);

                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
    }
}