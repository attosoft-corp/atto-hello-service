using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RawRabbit;
using System.Threading.Tasks;

namespace Hello.Service
{
    public class ReplayClass
    {
        private readonly ILogger<ReplayClass> _logger;

        public ReplayClass(IBusClient client, ILogger<ReplayClass> logger, IConfiguration configuration)
        {
            var exchangeSend = configuration["info:exchangeSend"];
            var queueSend = configuration["info:queueSend"];

            var exchangeGet = configuration["info:exchangeGet"];
            var queueGet = configuration["info:queueGet"];

            client.RespondAsync<ValueRequest, ValueResponse>(async request => await SendValuesThoughRpcAsync(request),
                 x => x.UseRespondConfiguration(r =>
                 {
                     r.FromDeclaredQueue(dq => dq.WithName(queueSend));
                     r.OnDeclaredExchange(de => de.WithName(exchangeSend));
                 })).GetAwaiter().GetResult();



             client.RespondAsync<ValueRequest, ValueResponse>(async request => await GetValuesThoughRpcAsync(request),
               x => x.UseRespondConfiguration(r =>
               {
                   r.FromDeclaredQueue(dq => dq.WithName(queueGet));
                   r.OnDeclaredExchange(de => de.WithName(exchangeGet));
               })).GetAwaiter().GetResult();


            _logger = logger;
        }


        private async  Task<ValueResponse> SendValuesThoughRpcAsync(ValueRequest request)
        {
            _logger.LogInformation($"from Send {request.Value}");
            return await Task.FromResult(new ValueResponse { Value = $"Send{request.Value}" });
        }

        private async  Task<ValueResponse> GetValuesThoughRpcAsync(ValueRequest request)
        {
            _logger.LogInformation($"from Get {request.Value}");
            return await Task.FromResult(new ValueResponse { Value = $"Get{request.Value}" });
        }
    }


    public class ValueResponse
    {
        public string Value { get; set; }
    }

    public class ValueRequest
    {
        public int Value { get; set; }
    }
}
