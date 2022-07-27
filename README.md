# LkPkg.EntityFrameworkCore

Provides the UnitOfWork and Repository patterns for Microsoft.EntityFrameworkCore.


## Installation

LkPkg.EntityFrameworkCore is available on Nuget.

```
Install-Package LkPkg.EntityFrameworkCore
```

## Usage

The following code demonstrates basic usage of UnitOfWork and Repository Pattern

```C#
//Get your connection string
var connectionString = builder.Configuration.GetConnectionString("DbConnection"); 

//Configure your database, use any provider here
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(connectionString));

//Register DbContext here
builder.Services.AddScoped<DbContext, ProductContext>();

builder.Services.AddUnitOfWork();
```

After that, your can use UnitOfWork as follow

```C#
//Inject the unitOfWork (This is .NET 6 Minimal API)
 app.MapPost("v1/catelog", async (CatelogModel model, IUnitOfWork unitOfWork) => {

                //Get the repository from UnitOfWork
                var catelogRepository = unitOfWork.Repository<Catelog>();

                //Insert new entity
                var createdCatelog = await catelogRepository.InsertAsync(new Catelog()
                {
                    Name = model.Name,
                    ParentId = model.ParentId
                });

                //Save changes
                await unitOfWork.SaveChangesAsync();

                return Results.Ok(createdCatelog); 
 });
```
