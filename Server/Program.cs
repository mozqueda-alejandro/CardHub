using System.Collections.Concurrent;
using CardHub.Games.Common;
using CardHub.Games.Une;
using CardHub.Games.Une.Card;
using CardHub.Games.Une.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
var settings = new UneRules
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