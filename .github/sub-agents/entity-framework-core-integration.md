# Entity Framework Core Integration Best Practices

> This document offers best practices for implementing Entity Framework Core integration in your modules and applications.

## General

- **Do not** rely on lazy loading on the application development.
- **Do not** enable lazy loading for the `DbContext`.

## Table Prefix and Schema

- **Do** add static `TablePrefix` and `Schema` **properties** to the `DbContext` class. Set default value from a constant. Example:

````C#
public static string TablePrefix { get; set; } = AbpIdentityConsts.DefaultDbTablePrefix;
public static string Schema { get; set; } = AbpIdentityConsts.DefaultDbSchema;
````

  - **Do** always use a short `TablePrefix` value for a module to create **unique table names** in a shared database. `Abp` table prefix is reserved for ABP core modules.
  - **Do** set `Schema` to `null` as default.

## Model Mapping

- **Do** explicitly **configure all entities** by overriding the `OnModelCreating` method of the `DbContext`. Example:

````C#
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);
    builder.ConfigureIdentity();
}
````

- **Do not** configure model directly in the  `OnModelCreating` method. Instead, create an **extension method** for `ModelBuilder`. Use Configure*ModuleName* as the method name. Example:

````C#
public static class IdentityDbContextModelBuilderExtensions
{
    public static void ConfigureIdentity([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<IdentityUser>(b =>
        {
            b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users", AbpIdentityDbProperties.DbSchema);
            b.ConfigureByConvention();
            //code omitted for brevity
        });

        builder.Entity<IdentityUserClaim>(b =>
        {
            b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UserClaims", AbpIdentityDbProperties.DbSchema);
            b.ConfigureByConvention();
            //code omitted for brevity
        });
        
        //code omitted for brevity
    }
}
````

* **Do** call `b.ConfigureByConvention();` for each entity mapping (as shown above).