using System.Collections.Concurrent;
using System.Text;
using CardHub.Application;
using CardHub.Application.Games.Shared;
using CardHub.Application.Games.Une;
using CardHub.Application.Games.Une.Hub;
using CardHub.Application.Identity;
using CardHub.Domain.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

// SignalR Auth
// https://learn.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-9.0#built-in-jwt-authentication
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.UseSecurityTokenValidators = true;
        options.RequireHttpsMetadata = false;

        var audience = builder.Configuration["Jwt:Audience"] ?? throw new NullReferenceException();
        var issuer = builder.Configuration["Jwt:Issuer"] ?? throw new NullReferenceException();
        var secretKey = builder.Configuration["Jwt:Secret"] ?? throw new NullReferenceException();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidAudience = audience,
            ValidIssuer = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/api/v1/unehub"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

// IdentityModelEventSource.ShowPII = true;
// IdentityModelEventSource.LogCompleteSecurityArtifact = true;


builder.Services.AddSignalR(hubOptions =>
{
    if (builder.Environment.IsDevelopment())
    {
        hubOptions.EnableDetailedErrors = true;
    }
});

var developmentCors = "http://localhost:3000";
var productionCors = new[] { "https://playcardhub.vercel.app", "https://playcardhub.com" };
builder.Services.AddCors(options =>
{
    options.AddPolicy(nameof(developmentCors), policyBuilder =>
    {
        policyBuilder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(developmentCors)
            .AllowCredentials()
            .AllowAnyHeader();
    });
    options.AddPolicy(nameof(productionCors), policyBuilder =>
    {
        policyBuilder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(productionCors)
            .AllowCredentials();
    });
});

// builder.Services.AddAntiforgery()

// System
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Identity
builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
builder.Services.AddSingleton<TokenProvider>();

// Clients
builder.Services.AddSingleton<IDictionary<int, Guid>>(new ConcurrentDictionary<int, Guid>());
builder.Services.AddSingleton<IDictionary<Guid, Room>>(new ConcurrentDictionary<Guid, Room>());
builder.Services.AddSingleton<IDictionary<Guid, IClient>>(new ConcurrentDictionary<Guid, IClient>());
builder.Services.AddSingleton<IClientManager, ClientManager>();

// Game
builder.Services.AddSingleton<IDictionary<Guid, IGame>>(new ConcurrentDictionary<Guid, IGame>());
builder.Services.AddSingleton(typeof(IGameManager<>), typeof(GameManager<>));


// builder.Services.AddSingleton<IDictionary<int, UneCard>>(new Dictionary<int, UneCard>());
// builder.Services.AddSingleton<IEqualityComparer<UneCard>, UneCardEqualityComparer>();
// builder.Services.AddSingleton<UneCardBuilder>();
// builder.Services.AddSingleton<UneDeckFactory>();
// builder.Services.AddTransient<UneGameFactory>();
// builder.Services.AddTransient<UneGame>();
// builder.Services.AddSingleton<Func<UneGame>>(x => () => x.GetService<UneGame>()!);



var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(app.Environment.IsDevelopment() ? nameof(developmentCors) : nameof(productionCors));

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapTokenEndpoints();

app.MapHub<UneHub>("api/v1/unehub", options => { options.AllowStatefulReconnects = false; });

/*

using var scope = app.Services.CreateScope(); // Create a scoped DI context
var uneBuilder = scope.ServiceProvider.GetRequiredService<UneCardBuilder>();
var uneFactory = scope.ServiceProvider.GetRequiredService<UneGameFactory>();

uneBuilder.SetColors([UneColor.Blue, UneColor.Red]).SetNumber(0).AddCards();
uneBuilder.SetColors([UneColor.Blue, UneColor.Red]).SetNumberRange(1, 10).AddCards(2);
uneBuilder.SetDrawAmount(2).AddCards(2);
uneBuilder.SetAction(UneAction.Reverse).AddCards(2);
uneBuilder.SetAction(UneAction.Skip).AddCards(2);
uneBuilder.SetWildColor().AddCards(4);
uneBuilder.SetWildColor().SetDrawAmount(4).AddCards(4);

// No Mercy Cards
uneBuilder.SetAction(UneAction.DiscardColor).AddCards();
uneBuilder.SetAction(UneAction.SkipAll).AddCards();
uneBuilder.SetAction(UneAction.ReverseDraw).SetDrawAmount(4).AddCards();
uneBuilder.SetWildColor().SetDrawAmount(6).AddCards();
uneBuilder.SetWildColor().SetDrawAmount(10).AddCards();
uneBuilder.SetAction(UneAction.ColorRoulette).AddCards();

var uneCards = uneBuilder.Build();
// var cardSet = scope.ServiceProvider.GetRequiredService<IDictionary<int, UneCard>>();

var rubi = new UnePlayer("Rubi");
var lyssie = new UnePlayer("Lyssie");
var alex = new UnePlayer("Alex");

var players = new List<UnePlayer> { rubi, lyssie, alex };
var rules = new UneRules
{
    ForcePlay = false,
    FreeDraw = false,
    JumpIn = false,
    Mercy = false,
    SevensSwap = false,
    UneStackRule = UneStackRule.None,
    ZerosPass = false
};
var game = uneFactory.Create(players, rules);
Console.WriteLine("Une Game created");

await game.Start();
Console.WriteLine("Une Game started");

var moveMade = true; // hack to print initial state
while (false)
{
    var stateGB = await game.GetStateGB();
    var currentPlayer = stateGB.CurrentPlayer;
    var statePlayer = await game.GetStatePlayer(currentPlayer);

    if (moveMade)
    {
        Console.WriteLine($"\nCurrent Player: {currentPlayer}");
        statePlayer.Hand.ForEach(Console.WriteLine);
        Console.WriteLine($"Current Card: {stateGB.DiscardPile.Last()}");
    }

    Console.Write("IN:");

    var input = Console.ReadLine();
    if (input is null) continue;

    var cardId = int.Parse(input);
    moveMade = await game.PlayCard(currentPlayer, cardId);

    Console.WriteLine("Card" + uneCards.Find(card => card.Id == cardId) + " -> " + moveMade);
}

*/

app.Run();