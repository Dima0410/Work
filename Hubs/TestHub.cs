using System;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Server.Services;

namespace Server.Hubs
{
    public class TestHub : Hub
    {
        public async Task Obj()
        {
            Object b = sta
        }


        //  Работает
        public async IAsyncEnumerable<int> IntStream([EnumeratorCancellation] CancellationToken cancellation)
        {
            int iteration = 0;

            while (iteration < 10 && !cancellation.IsCancellationRequested)
            {
                yield return iteration;

                iteration++;

                await Task.Delay(1000 * 1, cancellation);
            }
        }




        public override Task OnConnectedAsync()
        {
            Debug.WriteLine($"Connected new client, id: {Context.ConnectionId + DateTime.Today}");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Debug.WriteLine($"Disconnected client, id: {Context.ConnectionId + DateTime.Today}");

            return base.OnDisconnectedAsync(exception);
        }
    }
}
