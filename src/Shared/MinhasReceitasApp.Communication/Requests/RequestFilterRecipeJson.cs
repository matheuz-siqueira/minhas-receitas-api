using MinhasReceitasApp.Communication.Enums;

namespace MinhasReceitasApp.Communication.Requests;

public class RequestFilterRecipeJson
{
    public string? RecipeTitle_Ingredient { get; set; }
    public IList<CookingTime> CookingTimes { get; set; } = [];
    public IList<Difficulty> Difficulties { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}
