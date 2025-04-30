namespace MinhasReceitasApp.Domain.Entities;

public class Instruction
{
    public int Step { get; set; }
    public string Text { get; set; } = string.Empty;
    public long RecipeId { get; set; }
}
