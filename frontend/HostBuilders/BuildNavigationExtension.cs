using Lastik.HostBuilders.Navigation;
using Microsoft.Extensions.Hosting;

namespace Lastik.HostBuilders
{
    public static class BuildNavigationExtension
    {
        public static IHostBuilder BuildNavigation(this IHostBuilder builder)=> 
            builder.BuildBodyNavigation().BuildModalNavigation();
    }
}
