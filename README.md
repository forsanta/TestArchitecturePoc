# POC - Tests d'architecture C#

## Objectif du POC

Ce projet démontre comment **tester automatiquement les règles d'architecture** d'une application .NET avec **NetArchTest**.

L'idée est simple : transformer des conventions d'architecture (Clean Architecture / architecture en couches) en tests unitaires, afin d'éviter les régressions de dépendances entre projets.

## Ce que fait ce POC

Le POC met en place 5 projets :

- `TestArchitecturePoc.Domain` : cœur métier (entités)
- `TestArchitecturePoc.Application` : cas d'usage/services applicatifs
- `TestArchitecturePoc.Infrastructure` : implémentations techniques
- `TestArchitecturePoc.Api` : exposition HTTP (ASP.NET Core)
- `TestArchitecturePoc.ArchitectureTests` : tests d'architecture

Un mini flux fonctionnel est inclus pour rendre l'exemple concret :

- une entité `Order` dans `Domain`
- un service `OrderService` dans `Application`
- un repository en mémoire `InMemoryOrderRepository` dans `Infrastructure`
- un endpoint `GET /api/orders` dans `Api`

## Pourquoi c'est utile

Sans tests d'architecture, il est facile d'introduire des dépendances interdites au fil du temps (par exemple `Application` qui dépend de `Infrastructure`).

Ce POC montre comment :

- détecter ces écarts tôt (en local et en CI),
- rendre les règles explicites,
- sécuriser l'évolution de la structure du code.

## Règles d'architecture vérifiées

Les tests du projet `ArchitectureTests` valident les règles suivantes :

1. **Domain** ne dépend d'aucune autre couche.
2. **Application** ne dépend pas de `Infrastructure` ni `Api`.
3. **Infrastructure** ne dépend pas de `Api`.
4. Les contrôleurs de l'API respectent la convention de nommage `*Controller`.

## Exécution

Depuis la racine du dossier :

```powershell
dotnet test .\TestArchitecturePoc.slnx
```

## Résultat attendu

Si les règles sont respectées :

- tous les tests du projet `TestArchitecturePoc.ArchitectureTests` passent.

Si une dépendance interdite est introduite :

- un test échoue en listant les types en cause.

Cela fournit un garde-fou clair pour maintenir la qualité de l'architecture dans la durée.
