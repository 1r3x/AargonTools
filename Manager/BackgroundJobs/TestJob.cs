using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            _timer=new Timer(TheJob,null,TimeSpan.Zero, TimeSpan.FromDays(1));
        }


        public async void TheJob(object? state)
        {
            var count = 1;

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(10);
                //var response = await client.GetAsync("https://g14.aargontools.com/api/Test/CreditCards/PreScheduledPaymentProcessing");
                //var response = await client.GetAsync("https://localhost/api/Test/CreditCards/PreScheduledPaymentProcessing");
                //var response = await client.GetAsync("https://localhost:44357/api/Test/CreditCards/PreScheduledPaymentProcessing");
                //if (response.IsSuccessStatusCode)
                //{
                //    var content = await response.Content.ReadAsStringAsync();
                //    // Do something with the content
                //}
            }


            Console.WriteLine("this is the job no"+count);
            count++;
        }



        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("job stopped..................");
        }
    }
}
