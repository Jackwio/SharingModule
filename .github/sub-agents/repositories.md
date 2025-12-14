# Repository Best Practices & Conventions

> This document offers best practices for implementing Repository classes in your modules and applications based on Domain-Driven-Design principles.

## Repository Interfaces

* **Do** define repository interfaces in the **domain layer**.
* **Do** define a repository interface (like `IIdentityUserRepository`) and create its corresponding implementations for **each aggregate root**.
  * **Do** always use the default repository interface from the application code.
  * **Do** use generic repository interfaces (like `IRepository<IdentityUser, Guid>`) from the application code.
  * **Do not** use `IQueryable<TEntity>` features in the application code (domain, application... layers).

## Repository Methods

* **Do** define all repository methods as **asynchronous**.
* **Do** add an optional `bool includeDetails = false` parameter (default value is `false`) for every repository method which returns a **list of entities**. Example:

````C#
Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
    string normalizedRoleName, 
    bool includeDetails = false,
    CancellationToken cancellationToken = default
);
````