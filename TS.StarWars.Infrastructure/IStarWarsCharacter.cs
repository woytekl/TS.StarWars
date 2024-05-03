namespace TS.StarWars.Infrastructure;

public interface IStarWarsCharacter
{
    string Birth_year { get; set; }

    DateTimeOffset Created { get; set; }

    DateTimeOffset Edited { get; set; }

    string Eye_color { get; set; }

    string Gender { get; set; }

    string Hair_color { get; set; }

    string Height { get; set; }

    string Homeworld { get; set; }

    string Mass { get; set; }

    string Name { get; set; }

    string Skin_color { get; set; }

    string[] Starships { get; set; }
    string[] Vehicles { get; set; }
    string[] Films { get; set; }
}
