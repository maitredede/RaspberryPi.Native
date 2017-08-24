using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.DisplaySpyServer
{
    public static class DisplaySpyServerDependencyInjectionExtensions
    {
        public static IServiceCollection AddDispmanSpy(this IServiceCollection services)
        {
            return services
                .AddSingleton<BcmHost>()
                .AddSingleton<IScreenCapture>(svc =>
                {
                    try
                    {
                        BcmHost host = svc.GetRequiredService<BcmHost>();
                        return new DispmanCapture(host);
                    }
                    catch (DllNotFoundException)
                    {
                        return new FakeCapture();
                    }
                })
                .AddSingleton<DispmanCapture>()
                .AddSingleton<ImageGrabberService>()
                .AddSingleton<IHostedService>(svc => svc.GetRequiredService<ImageGrabberService>())
                .AddTransient<IScreenSpyImageProvider, SpyImageProvider>()
                ;
        }
    }
}
