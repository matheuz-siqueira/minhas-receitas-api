using MinhasReceitasApp.Domain.Enums;

namespace MinhasReceitasApp.Domain.Dtos;

public record FilterRecipesDto
{
    public string? RecipeTitle_Ingredients { get; init; }
    public IList<CookingTime> CookingTimes { get; init; } = [];
    public IList<Difficulty> Difficulties { get; init; } = [];
    public IList<DishType> DishTypes { get; init; } = [];

}
