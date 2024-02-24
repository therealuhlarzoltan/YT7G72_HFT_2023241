using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.WpfClient.Logic
{
    class NotifyService
    {
        private HubConnection conn;

        public NotifyService(string url)
        {
            conn = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            conn.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await conn.StartAsync();
            };
        }

        public void AddHandler<T>(string methodname, Action<T> value)
        {
            conn.On<T>(methodname, value);
        }

        public async void Init()
        {
            await conn.StartAsync();
        }

    }
}
