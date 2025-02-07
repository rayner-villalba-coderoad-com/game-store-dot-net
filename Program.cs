using Gamestore.Api.Entities;

const string GetGameEndpointName = "GetGame";

List <Game> games = new() 
{
  new Game()
  {
    Id = 1,
    Name = "Street Fighter II",
    Genre = "Fighting",
    Price = 19.99M,
    ReleaseDate = new DateTime(1991, 2, 1),
    ImageUri = "https://placehold.co/100"
  },
  new Game()
  {
    Id = 2,
    Name = "Final Fantasy XIV",
    Genre = "Roleplaying",
    Price = 59.99M,
    ReleaseDate = new DateTime(2010, 10, 1),
    ImageUri = "https://placehold.co/100"
  },
  new Game()
  {
    Id = 3,
    Name = "FIFA 23",
    Genre = "Sports",
    Price = 69.99M,
    ReleaseDate = new DateTime(2022, 9, 27),
    ImageUri = "https://placehold.co/100"
  }   
}; 

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/games", () => games);

app.MapGet("/games/{id}", (int id) => 
{
  Game? game = games.Find(game => game.Id == id);

  if (game is null)
  {
    return Results.NotFound(); 
  }

  return Results.Ok(game);
})
.WithName(GetGameEndpointName);

app.MapPost("/games", (Game game) => 
{
  game.Id = games.Max(game => game.Id) + 1;
  games.Add(game);

  return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
});


app.MapPut("/games/{id}", (int id, Game updatedGame) => 
{
  Game? existingGame = games.Find(game => game.Id == id);

  if (existingGame is null)
  {
    return Results.NotFound(); 
  }

  existingGame.Name = updatedGame.Name;
  existingGame.Genre = updatedGame.Genre;
  existingGame.Price = updatedGame.Price;
  existingGame.ReleaseDate = updatedGame.ReleaseDate;
  existingGame.ImageUri = updatedGame.ImageUri;

  return Results.NoContent();
});


app.MapDelete("/games/{id}", (int id) => 
{
  Game? game = games.Find(game => game.Id == id);

  if (game is not null)
  {
    games.Remove(game); 
  }

  return Results.NoContent(); 
});

app.Run();
