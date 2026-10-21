using FluentAssertions;

using NetArchTest.Rules;

using TestArchitecturePoc.Api.Controllers;
using TestArchitecturePoc.Application.Services;
using TestArchitecturePoc.Domain.Entities;
using TestArchitecturePoc.Infrastructure.Persistence;

namespace TestArchitecturePoc.ArchitectureTests;

/// <summary>
/// Vérifie le respect des règles d'architecture en couches (Domain, Application, Infrastructure, Api)
/// à l'aide de NetArchTest.
/// </summary>
public sealed class ArchitectureRulesTests
{
	// La couche Domain ne doit dépendre d'aucune autre couche (Application, Infrastructure, Api).
	[Fact]
	public void Domain_ShouldNotDependOnOtherLayers()
	{
		var result = Types.InAssembly(typeof(Order).Assembly)
			.ShouldNot()
			.HaveDependencyOnAny(
				"TestArchitecturePoc.Application",
				"TestArchitecturePoc.Infrastructure",
				"TestArchitecturePoc.Api")
			.GetResult();

		result.IsSuccessful.Should().BeTrue(FormatFailingTypes(result));
	}

	// La couche Application ne doit dépendre ni de l'Infrastructure ni de l'Api.
	[Fact]
	public void Application_ShouldNotDependOnInfrastructureOrApi()
	{
		var result = Types.InAssembly(typeof(OrderService).Assembly)
			.ShouldNot()
			.HaveDependencyOnAny("TestArchitecturePoc.Infrastructure", "TestArchitecturePoc.Api")
			.GetResult();

		result.IsSuccessful.Should().BeTrue(FormatFailingTypes(result));
	}

	// La couche Infrastructure ne doit pas dépendre de l'Api.
	[Fact]
	public void Infrastructure_ShouldNotDependOnApi()
	{
		var result = Types.InAssembly(typeof(InMemoryOrderRepository).Assembly)
			.ShouldNot()
			.HaveDependencyOn("TestArchitecturePoc.Api")
			.GetResult();

		result.IsSuccessful.Should().BeTrue(FormatFailingTypes(result));
	}

	// Les types du namespace des contrôleurs doivent respecter la convention de nommage "...Controller".
	[Fact]
	public void Controllers_ShouldEndWithController()
	{
		var result = Types.InAssembly(typeof(OrdersController).Assembly)
			.That()
			.ResideInNamespace("TestArchitecturePoc.Api.Controllers")
			.Should()
			.HaveNameEndingWith("Controller")
			.GetResult();

		result.IsSuccessful.Should().BeTrue(FormatFailingTypes(result));
	}

	// Aucune dépendance circulaire ne doit exister : une couche inférieure ne doit jamais
	// dépendre d'une couche supérieure (sens inverse du flux de dépendance attendu).
	[Theory]
	[InlineData("TestArchitecturePoc.Domain", "TestArchitecturePoc.Application")]
	[InlineData("TestArchitecturePoc.Domain", "TestArchitecturePoc.Infrastructure")]
	[InlineData("TestArchitecturePoc.Domain", "TestArchitecturePoc.Api")]
	[InlineData("TestArchitecturePoc.Application", "TestArchitecturePoc.Infrastructure")]
	[InlineData("TestArchitecturePoc.Application", "TestArchitecturePoc.Api")]
	[InlineData("TestArchitecturePoc.Infrastructure", "TestArchitecturePoc.Api")]
	public void Layers_ShouldNotHaveCircularDependencies(string lowerLayer, string upperLayer)
	{
		var circularResult = Types.InAssembly(GetAssemblyForLayer(lowerLayer))
			.ShouldNot()
			.HaveDependencyOn(upperLayer)
			.GetResult();

		circularResult.IsSuccessful.Should().BeTrue(FormatFailingTypes(circularResult));
	}

	// Les entités du Domain doivent être immuables : aucune propriété ne doit exposer de setter public.
	[Fact]
	public void DomainEntities_ShouldBeImmutable()
	{
		var mutableProperties = typeof(Order).Assembly
			.GetTypes()
			.Where(t => t.Namespace == typeof(Order).Namespace && t.IsClass)
			.SelectMany(t => t.GetProperties())
			.Where(p => p.SetMethod is { IsPublic: true })
			.Select(p => $"{p.DeclaringType!.Name}.{p.Name}")
			.ToList();

		mutableProperties.Should().BeEmpty($"les propriétés suivantes exposent un setter public : {string.Join(", ", mutableProperties)}");
	}

	// Résout l'assembly correspondant à une couche donnée, utilisé pour les tests paramétrés.
	private static System.Reflection.Assembly GetAssemblyForLayer(string layer) => layer switch
	{
		"TestArchitecturePoc.Domain" => typeof(Order).Assembly,
		"TestArchitecturePoc.Application" => typeof(OrderService).Assembly,
		"TestArchitecturePoc.Infrastructure" => typeof(InMemoryOrderRepository).Assembly,
		"TestArchitecturePoc.Api" => typeof(OrdersController).Assembly,
		_ => throw new ArgumentOutOfRangeException(nameof(layer), layer, "Couche inconnue.")
	};

	// Construit un message d'erreur listant les types en échec, utilisé pour enrichir les assertions FluentAssertions.
	private static string FormatFailingTypes(TestResult result)
	{
		return result.IsSuccessful
			? string.Empty
			: $"Types en échec: {string.Join(", ", result.FailingTypeNames)}";
	}
}