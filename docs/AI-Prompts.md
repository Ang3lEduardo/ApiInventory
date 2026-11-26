# AI Prompts

- Suggest me an architecture for a simple .NET 8 API project to handle a simple inventory with Product, Category and InventoryMovement, applying SOLID principles, TDD, Clean Code and CQRS, using EF for queries and Dapper for commands, Swagger, OAuth2 authentication, Docker and SQL Server.
- Is having a handler and a repository per operation excessive? Does CQRS require this structure?
- I am creating the class GetInventoryMovementByProductIdQueryHandler, for the method I want to sent back a DTO, to keep following the CQRS and SOLID structure, where should I write the the conversion to dto?
- How should I handle a read model when the real model (InventoryMovement) does not include all the information and the result is a combination of Product, Category and InventoryMovement?
- Should the SqlConnectionFactory class also be in the Persistence folder?
- Where should I create the fake classes for the tests?
- How should I approach OAuth2 authentication for a simple technical test when the requirement is ambiguous and no specific identity provider has been specified?

# AI Architecture proposal

API
 │
 ├── Application
 │    ├── Commands
 │    ├── Queries
 │    ├── DTOs
 │    └── Interfaces
 │
 ├── Domain
 │    ├── Entities
 │    └── Business Rules
 │
 └── Infrastructure
      ├── EF Core
      ├── Dapper
      ├── SQL Server
      └── Authentication

Me dio el panorama general y me pareció buena elección, aunque la autenticación para este proyecto no se tuvo que realizar en la capa de infraestructura, puesto que solo se implementó OAuth2. Así mismo agregué la parte de tests y algunos servicios como dependencias para solo inyectarlos en el program.cs y tener un código mas limpio.

# Preguntas - Toma de decisiones

1. Describe una decisión técnica reciente que hayas tomado con información incompleta. ¿Qué te faltaba y cómo decidiste avanzar?
     Tuve que definir yo los posibles escenarios faltantes que no eran happy path para poder exponerlos ante un nuevo PM o también una vez faltaban las definiciones en cuanto interacción de interfaz de parte de customer support, por lo que mejor creé diagramas para explicarles y poder llegar a una definición.

2. Cuando dos enfoques técnicos parecen igual de válidos, ¿qué criterios usas para elegir uno?
     Elijo el enfoque que deja el código mas eficiente, adaptable y mantenible (en ese orden de importancia) para futuros desarrollos.

3. Cuenta un caso en el que hayas tenido que revertir o cambiar una decisión técnica propia. ¿Qué aprendiste de esa experiencia?
     En un ocasión empecé a implementar un componente con poca experiencia en un sistema, no conocía bien la base de datos ni hasta donde podía modificar la arquitectura, lo que llevó a problemas de performance, aprendí que es importante conocer bien las bases de código, las reglas de negocio y exponer todas las dudas antes de desarrollar algo.