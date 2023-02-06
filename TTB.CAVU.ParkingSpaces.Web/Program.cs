using TTB.CAVU.ParkingSpaces.DataAccess;
using TTB.CAVU.ParkingSpaces.DataAccess.Seeds;
using TTB.CAVU.ParkingSpaces.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(x => x.AddConsole());
builder.Services.AddDataAccess(builder.Configuration).AddBusinessServices().AddControllers();

var app = builder.Build();

var seedManager = app.Services.GetRequiredService<IDatabaseSeedManager>();
seedManager.RunSeeds();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();