using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ThomasFreudenberg.Microsoft.Extensions.DependencyInjection.Tests
{
    public class TransientWithInstanceRegistrationTests
    {
        [Fact]
        public void can_resolve_transient_class()
        {
            var services = new ServiceCollection();
            services.AddNamedTransient<IPlugin, PluginA>("A", new PluginA());
            services.AddNamedTransient<IPlugin, PluginB>("B", new PluginB());

            using var serviceProvider = services.BuildServiceProvider();
            var pluginA = serviceProvider.GetNamedService<IPlugin>("A");

            pluginA
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PluginA>();
        }

        [Fact]
        public void can_resolve_transient_class_in_scope()
        {
            var services = new ServiceCollection();
            services.AddNamedTransient<IPlugin, PluginA>("A", new PluginA());
            services.AddNamedTransient<IPlugin,PluginB>("B", new PluginB());

            using var scope = services.BuildServiceProvider().CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var pluginB = serviceProvider.GetNamedService<IPlugin>("B");

            pluginB
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PluginB>();
        }

        [Fact]
        public void getting_not_registered_class_returns_null()
        {
            var services = new ServiceCollection();

            using var serviceProvider = services.BuildServiceProvider();
            var pluginA = serviceProvider.GetNamedService<IPlugin>("A");

            pluginA.Should().BeNull();
        }

        [Fact]
        public void can_resolve_required_transient_class()
        {
            var services = new ServiceCollection();
            services.AddNamedTransient<IPlugin, PluginA>("A", new PluginA());
            services.AddNamedTransient<IPlugin, PluginB>("B", new PluginB());

            using var serviceProvider = services.BuildServiceProvider();
            var pluginB = serviceProvider.GetRequiredNamedService<IPlugin>("B");

            pluginB
                .Should()
                .NotBeNull()
                .And
                .BeOfType<PluginB>();
        }

        [Fact]
        public void resolving_required_class_without_any_registrations_throws()
        {
            var services = new ServiceCollection();

            using var serviceProvider = services.BuildServiceProvider();

            serviceProvider
                .Invoking(sp => sp.GetRequiredNamedService<IPlugin>("B"))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void resolving_required_not_existing_class_throws()
        {
            var services = new ServiceCollection();
            services.AddNamedTransient<IPlugin, PluginA>("A", new PluginA());

            using var serviceProvider = services.BuildServiceProvider();

            serviceProvider
                .Invoking(sp => sp.GetRequiredNamedService<IPlugin>("B"))
                .Should()
                .Throw<ArgumentException>();
        }
    }
}
