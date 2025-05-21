using Microsoft.AspNetCore.Http;

namespace MinhasReceitasApp.Communication.Requests;

public class RequestRegisterRecipeFormData : RequestRecipeJson
{
    public IFormFile? Image { get; set; }
}
