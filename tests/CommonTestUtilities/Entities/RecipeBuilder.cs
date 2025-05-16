using Bogus;
using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class RecipeBuilder
{
    public static IList<Recipe> Collection(User user, uint count = 2)
    {
        var list = new List<Recipe>();

        if (count == 0)
            count = 1;
        var recipeId = 1;
        for (int i = 0; i < count; i++)
        {
            var fakerRecipe = Build(user);
            fakerRecipe.Id = recipeId++;

            list.Add(fakerRecipe);
        }

        return list;
    }
    public static Recipe Build(User user)
    {
        return new Faker<Recipe>()
            .RuleFor(r => r.Id, () => 1)
            .RuleFor(r => r.Title, f => f.Lorem.Word())
            .RuleFor(r => r.CookingTime, f => f.PickRandom<CookingTime>())
            .RuleFor(r => r.Difficulty, f => f.PickRandom<Difficulty>())
            .RuleFor(r => r.Ingredients, f => f.Make(1, () => new Ingredient
            {
                Id = 1,
                Item = f.Commerce.ProductName()
            }))
            .RuleFor(r => r.Instructions, f => f.Make(1, () => new Instruction
            {
                Id = 1,
                Step = 1,
                Text = f.Lorem.Paragraph()
            }))
            .RuleFor(r => r.DishTypes, f => f.Make(1, () => new MinhasReceitasApp.Domain.Entities.DishType
            {
                Id = 1,
                Type = f.PickRandom<MinhasReceitasApp.Domain.Enums.DishType>()
            }))
            .RuleFor(r => r.UserId, _ => user.Id);

    }
}
