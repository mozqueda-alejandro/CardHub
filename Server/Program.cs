using System.Collections.Concurrent;
using CardHub.Games.Common;
using CardHub.Games.Une;
using CardHub.Games.Une.Card;
using CardHub.Games.Une.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSignalR(hubOptions =>
{
    if (builder.Environment.IsDevelopment())
    {
        hubOptions.EnableDetailedErrors = true;
    }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", policyBuilder =>
    {
        policyBuilder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:3000")
            .AllowCredentials();
    });
    options.AddPolicy("ProductionPolicy", policyBuilder =>
    {
        policyBuilder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("https://playcardhub.vercel.app", "https://playcardhub.com")
            .AllowCredentials();
    });
});


#region DI

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSingleton(typeof(BaseHub<>));
builder.Services.AddSingleton(typeof(BaseHubFactory<>));
builder.Services.AddSingleton<IDictionary<string, IGame>>(new ConcurrentDictionary<string, IGame>());
builder.Services.AddSingleton<IDictionary<int, UneCard>>(new Dictionary<int, UneCard>());
builder.Services.AddSingleton<IEqualityComparer<UneCard>, UneCardEqualityComparer>();
builder.Services.AddSingleton<UneCardBuilder>();
builder.Services.AddSingleton<UneDeckFactory>();
builder.Services.AddTransient<UneGameFactory>();
builder.Services.AddTransient<UneGame>();
builder.Services.AddSingleton<Func<UneGame>>(x => () => x.GetService<UneGame>()!);

#endregion


var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevelopmentPolicy");
}
else
{
    app.UseCors("ProductionPolicy");
}

app.UseAuthorization();
app.MapControllers();

app.MapHub<UneHub>("/unehub", options =>
{
    options.AllowStatefulReconnects = false;
});


using var scope = app.Services.CreateScope(); // Create a scoped DI context
var uneBuilder = scope.ServiceProvider.GetRequiredService<UneCardBuilder>();
var uneFactory = scope.ServiceProvider.GetRequiredService<UneGameFactory>();

uneBuilder.Colors([UneColor.Blue, UneColor.Red]).Number(0).AddCards();
uneBuilder.Colors([UneColor.Blue, UneColor.Red]).NumberRange(1, 10).AddCards(2);
// uneBuilder.DrawAmount(2).AddCards(2);
// uneBuilder.Action(UneAction.Reverse).AddCards(2);
// uneBuilder.Action(UneAction.Skip).AddCards(2);
// uneBuilder.WildColor().AddCards();
// uneBuilder.WildColor().DrawAmount(4).AddCards();
//
// // No Mercy Cards
// uneBuilder.Action(UneAction.DiscardColor).AddCards();
// uneBuilder.Action(UneAction.SkipAll).AddCards();
// uneBuilder.Action(UneAction.ReverseDraw).DrawAmount(4).AddCards();
// uneBuilder.WildColor().DrawAmount(6).AddCards();
// uneBuilder.WildColor().DrawAmount(10).AddCards();
// uneBuilder.Action(UneAction.ColorRoulette).AddCards();

var uneCards = uneBuilder.Build();
// var cardSet = scope.ServiceProvider.GetRequiredService<IDictionary<int, UneCard>>();

var rubi = new UnePlayer("Rubi");
var lyssie = new UnePlayer("Lyssie");
var alex = new UnePlayer("Alex");

var players = new List<UnePlayer> { rubi, lyssie, alex };
var settings = new UneSettings
{
    ForcePlay = false,
    FreeDraw = false,
    JumpIn = false,
    Mercy = false,
    SevensSwap = false,
    UneStackRule = UneStackRule.None,
    ZerosPass = false
};
var game = uneFactory.Create(players, settings);
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



app.Run();