using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using NLog;
using OmegaGraf.Compose.Config.Grafana;
using Polly;

namespace OmegaGraf.Compose
{
    public class Grafana : IDisposable
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private Token _token;
        private readonly string _uri;

        public Grafana(string uri)
        {
            this._uri = uri;

            try
            {
                _=Policy
                    .Handle<Exception>(ex =>
                    {
                        logger.Warn(ex, "Error fetching Grafana token, retrying...");
                        return true;
                    })
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(5))
                    .Execute(() => this._token = this.CreateToken().Result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error fetching Grafana token");
                throw;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                this.RemoveToken().Wait();
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
                return this._uri
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
                return this._uri
                    .AppendPathSegments("api", "auth", "keys")
                    .WithOAuthBearerToken(this._token.Key)
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
            var tokens = await this.ListTokens();

            var toDelete = tokens.FirstOrDefault(t => t.Name == this._token.Name);

            if (toDelete == null)
            {
                return null;
            }

            try
            {
                return await this._uri
                    .AppendPathSegments("api", "auth", "keys", toDelete.ID)
                    .WithOAuthBearerToken(this._token.Key)
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
                return await this._uri
                    .AppendPathSegments("api", "datasources")
                    .WithOAuthBearerToken(this._token.Key)
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
                return await this._uri
                    .AppendPathSegments("api", "dashboards", "db")
                    .WithOAuthBearerToken(this._token.Key)
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
