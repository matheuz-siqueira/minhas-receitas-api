using MinhasReceitasApp.Communication.Enums;

namespace MinhasReceitasApp.Communication.Requests;

public class RequestRecipeJson
{
    public string Title { get; set; } = string.Empty;
    public CookingTime? CookingTime { get; set; }
    public Difficulity? Difficulity { get; set; }
    public IList<string> Ingredients { get; set; } = [];
    public IList<RequestInstructionsJson> Instructions { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];

}
