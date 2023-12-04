# Entity Framework

To learn about Entity Framework, review the summary of the notes I made during my learning, and review LearningEF, a simple implementation of it. 

## Notes

### What is it and why do I want it

Entity Framework Core (EF Core) is an open-source, lightweight, extensible, and cross-platform version of Entity Framework, Microsoft's Object-Relational Mapping (ORM) tool for .NET. It serves as a bridge between your C# code and the database, allowing you to work with data in the form of domain-specific objects and properties, without needing to write SQL queries directly. Here are some key aspects of EF Core and reasons why it's important in C# .NET projects:

1. **Object-Relational Mapping (ORM)**: EF Core automates the mapping between your C# classes (entities) and database tables. This simplifies data access in your application, as you can perform CRUD operations (Create, Read, Update, Delete) directly on objects.

1. **Database Agnostic**: EF Core supports multiple database providers like SQL Server, MySQL, SQLite, PostgreSQL, and others. This allows for more flexibility in choosing or switching your database provider.

1. **LINQ Support**: It integrates with Language Integrated Query (LINQ), enabling you to write database queries using C# syntax. LINQ queries are then translated into SQL queries by EF Core.

1. **Migrations**: EF Core provides a way to automatically manage schema changes to your database through migrations. This means you can evolve your database schema along with your application's data model.
    - *Applying migrations*: 
        - EF Core uses a feature called Migrations to manage changes to the database schema. 
        - When you modify your entities (e.g., adding, removing, or changing properties), you need to create a new migration. 
        - Migrations are essentially code files that describe how to bring your database schema from one version to another. 
        - Applying a migration to your database will alter the database schema to match your new entity models.
    - *Data preservation*: when you apply a migration, EF Core attempts to preserve existing data. For example:
        - Adding a column: EF Core adds it to the database without affecting existing data.
        - Removing a column: EF Core removes it and its data from the database.
        - Rename a column/table: EF Core will attempt to preserve the data in the new structure.
        - Complex changes (like splitting a table, merging tables, changing relationships): you may need to write custom migration code. EF Core generates a baseline migration script, but it might not cover complex data transformations or preserve data as you want. In such cases, you need to modify the migration script manually to handle data appropriately.
        - Data loss risk: some changes can lead to data loss (e.g., dropping columns, tables, or changing data types). Always review migration scripts carefully and test them in a development or staging environment before applying them to your production database. Also, before applying any migration to a production database it's crucial to back up your data. 

1. **Performance**: While EF Core may not always match the performance of raw SQL queries, it has been optimized for most common scenarios, and performance improvements are a continuous focus in its development.

1. **Code-First Approach**: EF Core allows for a code-first approach, where you define your database schema in code rather than creating the database first. 

### So can I just write C# code with Entity Framework and the database setup will automatically follow?

Yes, you can indeed write C# code using Entity Framework (EF) and then connect it to a database to automatically create and set up your database schema. This is one of the key features of EF, particularly in the "Code-First" approach. Here's how it works:

1. **Define Your Models**: First, you define your data models in C#. These models are plain C# classes (POCOs - Plain Old CLR Objects) that represent the tables and relationships you want in your database.

2. **DbContext Class**: You then create a `DbContext` class. This class acts as a session with the database, allowing you to query and save data. It includes `DbSet<TEntity>` properties that correspond to the tables in the database.

3. **Database Connection**: You configure EF with a connection string to specify which database server and database EF should target. This can be done in various ways, such as in a configuration file.

4. **Migrations**: Once your models and `DbContext` are set up, you can use EF Migrations to generate the database schema. Migrations are a way to incrementally update the database schema to keep it in sync with your model classes. When you add, remove, or change your models, you create a new migration, which EF uses to adjust the database schema accordingly.

5. **Database Creation and Update**: When you run your application, EF checks the database. If it doesn't exist or if there's a mismatch between the model and the existing schema, EF can automatically create or update the database based on your model definitions and migrations.

6. **CRUD Operations**: With the database set up, you can use EF to perform CRUD (Create, Read, Update, Delete) operations. EF allows you to work with your C# objects, and it translates these operations into the appropriate SQL commands for the database.

7. **Support for Multiple Databases**: Entity Framework supports various databases such as Microsoft SQL Server, MySQL, PostgreSQL, SQLite, and more. The flexibility to switch between different databases with minimal changes to your code is a significant advantage.

In summary, Entity Framework's Code-First approach lets you focus on your application's C# code and models, and it handles the complexities of generating and managing the database schema, making the development process more streamlined and efficient.

## Assignment

Creat the simplest implementation of Entity Framework that you can think of, connect it to Azure SQL and play around with it. 

Steps I took:

Follow [this](https://learn.microsoft.com/en-us/azure/azure-sql/database/azure-sql-dotnet-entity-framework-core-quickstart?view=azuresql&tabs=dotnet-cli%2Cservice-connector%2Cportal) quickstart;
- The connection string gave me issues, instead I used the following:
    ```bash
    "AZURE_SQL_CONNECTIONSTRING": "Server=tcp:<SQL server name>.database.windows.net,1433;Initial Catalog=<SQL db name>;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default;"
    ```
-   For the first migrations run:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```
-   I did not go ahead and deploy to Azure App Service as my learning is about EF Core, and not 
    so much about Azure App Service.
- Key Components of this sample:
    - **App Startup Program.cs**:
        - Initializes the web application.
        - Configures services like Swagger for API documentation and `DbContext` for database access.
        - Determines the database connection string based on the environment (Development or Production).
        - Defines HTTP request handling, like enabling Swagger UI in development and setting up HTTPS redirection.
        - Defines endpoints each entity (e.g. Person, Address) to retrieve or get records. 
    - **Database Context `PersonDbContext`**:
        - Inherits from `DbContext`, a class provided by Entity Framework Core (EF Core) for interacting with the database.
        - `DbContext` represents a session with the database, allowing for querying and saving data.
        - `PersonDbContext` is configured with the SQL Server database connection in the startup class. We're doing that explicitly in the constructor:
            ```csharp
            public PersonDbContext(DbContextOptions<PersonDbContext> options)
            : base(options)
            {
            }
            ```
            here a `DbContextOptions` object is created in `Program.cs` that is then passed to the `DbContext` constructor. This allows a `DbContext` configured for dependency injection to also be constructed explicitly. Read more on all the options for configuration in the [docs](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/).
        - **Entity Set `DbSet<Person>`**:
            - Defined in `PersonDbContext`.
            - Represents a collection of Person entities that you can query and save.
            - EF Core uses this to map Person objects to the corresponding database table.
    - **Entity Model Classes**:
        - **Person**:
            - Represents the data model for a person with properties like `Id`, `FirstName`, and `LastName`.
            - These properties map to columns in the corresponding database table.
            - Each Person object in your application is a representation of a row in the Person table in the database, with the class properties mapping to the respective columns in the table.
- Add a few database items by running the app locally. I find it quite slow;
- Update the sample with an Address table. Make sure to create a new 
migration and update the database afterwards: 
    ```bash
    dotnet ef migrations add AddAddress
    dotnet ef database update
    ``` 
    This adds the following:
    - **Entity Set `DbSet<Address>`**:
        - Defined in `PersonDbContext`.
        - Represents a collection of Address entities that you can query and save.
        - EF Core uses this to map Address objects to the corresponding database table.
    - **Entity Model Classes**:
        - **Address**:
            - Has a many:1 relationship with Person. One person can have multiple addresses (home address, work address, post address).

    Note that EF Core automatially  identifies `PersonId` as the foreign key because `Person` is added as navigation property to the Address Class. If you would have called it `PersonId2`, it would not have automatically recognized it and you'd need to add to your code. 
    
    Add to `PersonDbContext` using FluentAPI:
    ```charsp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Person)
            .WithMany(p => p.Addresses)
            .HasForeignKey(a => a.PersonId2); // Explicitly specifying the foreign key
    }
    ```
    Or add to `Address` class using Data Annotations:
    ```csharp
    public class Address
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }

        public int? PersonId2 { get; set; } // Custom foreign key name

        [ForeignKey("PersonId2")]
        public Person Person { get; set; }
    }
    ```
