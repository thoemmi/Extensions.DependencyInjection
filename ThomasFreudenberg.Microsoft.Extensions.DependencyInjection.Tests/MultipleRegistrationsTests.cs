using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ThomasFreudenberg.Microsoft.Extensions.DependencyInjection.Tests
{
    public class MultipleRegistrationsTests
    {
        [Fact]
        public void can_resolve_multiple_scoped_classes()
        {
            var services = new ServiceCollection();
            services.AddNamedScoped<IPlugin, PluginA>("C", new PluginA());
            services.AddNamedScoped<IPlugin, PluginB>("C", new PluginB());

            using var serviceProvider = services.BuildServiceProvider();
            var plugins = serviceProvider.GetNamedServices<IPlugin>("C").ToList();

            plugins
                .Should().NotBeNull()
                .And.HaveCount(2)
                .And.OnlyHaveUniqueItems(t => t.GetType())
                .And.Contain(t => t.GetType() == typeof(PluginA))
                .And.Contain(t => t.GetType() == typeof(PluginB));
        }

        [Fact]
        public void returns_empty_enumeration_if_none_found()
        {
            var services = new ServiceCollection();

            using var serviceProvider = services.BuildServiceProvider();
            var plugins = serviceProvider.GetNamedServices<IPlugin>("C").ToList();

            plugins
                .Should().NotBeNull()
                .And.BeEmpty();
        }

    }
}