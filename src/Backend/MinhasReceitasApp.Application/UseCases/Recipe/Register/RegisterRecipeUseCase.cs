using AutoMapper;
using MinhasReceitasApp.Communication.Requests;
using MinhasReceitasApp.Communication.Responses;
using MinhasReceitasApp.Domain.Extensions;
using MinhasReceitasApp.Domain.Repositories;
using MinhasReceitasApp.Domain.Repositories.Recipe;
using MinhasReceitasApp.Domain.Services.LoggedUser;
using MinhasReceitasApp.Domain.Services.Storage;
using MinhasReceitasApp.Exceptions.ExceptionsBase;
using MinhasReceitasApp.Application.Extensions;

namespace MinhasReceitasApp.Application.UseCases.Recipe.Register;

public class RegisterRecipeUseCase : IRegisterRecipeUseCase
{
    private readonly IRecipeWriteOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly IBlobStorageService _blobStorageService;
    public RegisterRecipeUseCase(
        IRecipeWriteOnlyRepository repository,
        ILoggedUser loggedUser,
        IUnityOfWork unityOfWork,
        IMapper mapper,
        IBlobStorageService blobStorageService)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
        _blobStorageService = blobStorageService;
    }
    public async Task<ResponseRegisteredRecipeJson> Execute(RequestRegisterRecipeFormData request)
    {
        Validate(request);
        var loggedUser = await _loggedUser.User();

        var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
        recipe.UserId = loggedUser.Id;

        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for (var index = 0; index < instructions.Count; index++)
            instructions[index].Step = index + 1;

        recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

        if (request.Image is not null)
        {
            var fileStream = request.Image.OpenReadStream();
            (var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

            if (isValidImage.IsFalse())
            {
                throw new ErrorOnValidationException(["only png, jpg and jpeg images are accepted."]);
            }

            recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}";

            await _blobStorageService.Upload(loggedUser, fileStream, recipe.ImageIdentifier);
        }

        await _repository.Add(recipe);

        await _unityOfWork.Commit();

        return _mapper.Map<ResponseRegisteredRecipeJson>(recipe);
    }
    private static void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);
        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}
