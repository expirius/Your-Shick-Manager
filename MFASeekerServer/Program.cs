using MFASeekerServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB configuring // ¡≈« »—œŒÀ‹«Œ¬¿Õ»ﬂ DEPENDENCY INJECTION. TRY FLUENT API
builder.Services.AddDbContext<SeekerDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")); // SQLite
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SeekerDbContext>();
    dbContext.Database.Migrate();
    //DbInitializer.Initialize(dbContext);
}

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
