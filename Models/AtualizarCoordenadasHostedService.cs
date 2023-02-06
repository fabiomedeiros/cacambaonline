using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cacambaonline.Models
{
    public class AtualizarCoordenadasHostedService : IHostedService,IDisposable
    {
        private Timer? _timer = null;
        public Task StartAsync(CancellationToken cancellationToken) 
        {
            //throw new NotImplementedException();
            _timer = new Timer(AtualizarCoordenadas, null, 0, 100000);
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public void Dispose() 
        {
            throw new NotImplementedException();
        }

        private void AtualizarCoordenadas(object state)
        { 

        }
    }
}
