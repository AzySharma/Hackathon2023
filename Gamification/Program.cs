using Gamification.Config;
using Gamification.Repositories;
using Gamification.IoC;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Cosmos.Fluent;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IDbContext, DBContext>();
builder.Services.AddSingleton<IRewardRepository, RewardRepository>();

//TBD -- move it to Registrator
builder.Services.AddOptions<CosmosDbConfig>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("cosmos").Bind(settings);
                });


builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IOptions<CosmosDbConfig>>().Value;

    return new CosmosClientBuilder(config.Endpoint, config.Key)
        .WithCustomSerializer(new CosmosSerialiser())
        .WithConnectionModeDirect(TimeSpan.FromMinutes(20))
        .WithBulkExecution(true)
        .Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
