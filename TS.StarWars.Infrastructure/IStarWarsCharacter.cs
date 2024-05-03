namespace TS.StarWars.Infrastructure;

public interface IStarWarsCharacter
{
    string BirthYear { get; }

    string EyeColor { get; }

    string Gender { get; }

    string HairColor { get; }

    string Height { get; }

    string Homeworld { get; }

    string Mass { get; }

    string Name { get; }

    string SkinColor { get; }

    string[] Starships { get; }
    string[] Vehicles { get; }
    string[] Films { get; }
}
