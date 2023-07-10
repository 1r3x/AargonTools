using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AargonTools.Manager.BackgroundJobs
{
    public class TestJob:BackgroundService
    {
        private Timer _timer;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Job started ...................");
            _timer=new Timer(TheJob,null,TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public void TheJob(object? state)
        {
            var count = 1;
            Console.WriteLine("this is the job no"+count);
            count++;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("job stopped..................");
        }
    }
}
