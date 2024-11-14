// using System.Collections.Concurrent;

// var builder = WebApplication.CreateBuilder(args);
//
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// builder.Services.AddSignalR(hubOptions =>
// {
//     if (builder.Environment.IsDevelopment())
//     {
//         hubOptions.EnableDetailedErrors = true;
//     }
// });
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("DevelopmentPolicy", policyBuilder =>
//     {
//         policyBuilder.AllowAnyMethod()
//             .AllowAnyHeader()
//             .WithOrigins("http://localhost:3000")
//             .AllowCredentials();
//     });
//     options.AddPolicy("ProductionPolicy", policyBuilder =>
//     {
//         policyBuilder.AllowAnyMethod()
//             .AllowAnyHeader()
//             .WithOrigins("https://playcardhub.vercel.app", "https://playcardhub.com")
//             .AllowCredentials();
//     });
// });
//
//
// #region DI
//
// builder.Services.AddSingleton<IDictionary<string, UneGame>>(_ => new ConcurrentDictionary<string, UneGame>());
//
// #endregion
//
//
// var app = builder.Build();
//
// app.UseHttpsRedirection();
//
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
//     app.UseCors("DevelopmentPolicy");
// }
// else
// {
//     app.UseCors("ProductionPolicy");
// }
//
// app.UseAuthorization();
// app.MapControllers();
//
// app.MapHub<UneHub>("/unehub", options =>
// {
//     options.AllowStatefulReconnects = true;
// });
//
// app.Run();

using CardHub.Games.Une;
using CardHub.Games.Une.Entities;

var rubi = new UnePlayer("Rubi");
var lyssie = new UnePlayer("Lyssie");
var alex = new UnePlayer("Alex");

var players = new[] { rubi, lyssie, alex };
var game = new UneGame(players);

Console.WriteLine("Une Game!");
await game.StartGame();
Console.WriteLine("Your cards: ");
var myState = await game.GetStatePlayer("Alex");
myState.Hand.ForEach(Console.WriteLine);

var input = Console.ReadLine();
if (input is null) return;
var cardId = int.Parse(input);


