# ThomasFreudenberg.Microsoft.Extensions.DependencyInjection

This package adds the option to use **named registrations** with **Microsoft.Extensions.DependencyInjection**.

![Build](https://github.com/thoemmi/Extensions.DependencyInjection/workflows/Build/badge.svg)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/ThomasFreudenberg.Microsoft.Extensions.DependencyInjection.svg)](https://www.nuget.org/packages/ThomasFreudenberg.Microsoft.Extensions.DependencyInjection/absoluteLatest)
[![Nuget](https://img.shields.io/nuget/v/ThomasFreudenberg.Microsoft.Extensions.DependencyInjection.svg)](https://www.nuget.org/packages/ThomasFreudenberg.Microsoft.Extensions.DependencyInjection/)

## Registration

You can register services with their desired names:

```cs
services
   .AddNamedTransient<IPlugin, PluginA>("pluginA")
   .AddNamedTransient<IPlugin, PluginB>("pluginB");
```

(It's also possible to call `AddNamedSingleton` or `AddNamedScoped`)

## Getting named instances

Now we have two services implementing `IPlugin` in the service collection. To get a plugin
by its name from the container at runtime, you take an `IServiceProvider` and call the
extension method `GetNamedService`:

```cs
var pluginA = serviceProvider.GetNamedService<IPlugin>("pluginA");
```

## Motivation

In contract to other containers like **AutoFac** or **StructureMap**, **Microsoft.Extensions.DependencyInjection** does not support named registrations.

If you have a interface with different implementations, and you want to decide at runtime, which implementation you
want to retrieve, you may have implemented this pattern (bear with me stretching the old animal analogy):

```cs
public interface IAnimal
{
   string Name { get; }
   void MakeNoise();
}

public class Cat : IAnimal
{
   public string Name => "cat";
   void MakeNoise() => Console.WriteLine("Meow!");
}

public class Dog : IAnimal
{
   public string Name => "dog";
   void MakeNoise() => Console.WriteLine("Woof!");
}
```

Afterwards you registered in the implementations in the service collection:

```cs
services
  .AddTransient<IAnimal, Cat>()
  .AddTransient<IAnimal, Dog>();
```

And to let the dog bark, you request all `IAnimal`s from the container and pick the correct
instance by its name:

```cs
public class AnimalFarm
{
   private IEnumerable<IAnimal> _animals;

   public AnimalFarm(IEnumerable<IAnimal> animals) => _animals = animals;

   public void LetTheDogBark()
   {
      var dog = _animals.FirstOrDefault(animal => animal.Name == "dog");
      dog.MakeNoise();
   }
}
```

With this package, you can register all animals with their name, like

```cs
services
  .AddNamedTransient<IAnimal, Cat>("cat")
  .AddNamedTransient<IAnimal, Dog>("dog");
```

And the `AnimalFarm` needs an instance of `IServiceProvider` to be able to get the desired 
named animal at runtime:

```cs
public class AnimalFarm
{
   private IServiceProvider _serviceProvider;

   public AnimalFarm(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

   public void LetTheDogBark()
   {
      var dog = serviceProvider.GetNamedService<IAnimal>("dog");
      dog.MakeNoise();
   }
}
```
