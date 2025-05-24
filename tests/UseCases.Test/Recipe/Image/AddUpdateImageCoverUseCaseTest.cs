using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MinhasReceitasApp.Application.UseCases.Image;
using MinhasReceitasApp.Exceptions.ExceptionsBase;
using UseCases.Test.Recipe.InlineDatas;

namespace UseCases.Test.Recipe.Image;

public class AddUpdateImageCoverUseCaseTest
{
    [Theory]
    [ClassData(typeof(ImageTypesInLineData))]
    public async Task Success(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var useCase = CreateUseCase(user, recipe);
        Func<Task> act = async () => await useCase.Execute(recipe.Id, file);
        await act.Should().NotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(ImageTypesInLineData))]
    public async Task Error_Recipe_NotFound(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(1, file);

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(e => e.Message.Equals("Recipe not found."));
    }

    [Fact]
    public async Task Error_File_Is_Txt()
    {
        (var user, _) = UserBuilder.Build();

        var recipe = RecipeBuilder.Build(user);

        var useCase = CreateUseCase(user, recipe);
        var file = FormFileBuilder.Txt();

        Func<Task> act = async () => await useCase.Execute(recipe.Id, file);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
                e.ErrorMessages.Contains("only png, jpg and jpeg images are accepted."));
    }


    private static AddUpdateImageCoverUseCase CreateUseCase(
        MinhasReceitasApp.Domain.Entities.User user,
        MinhasReceitasApp.Domain.Entities.Recipe? recipe = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var unityOfWork = UnityOfWorkBuilder.Build();
        var repository = new RecipeUpdateOnlyRepositoryBuilder().GetById(user, recipe).Build();
        var blobStorage = new BlobStorageServiceBuilder().GetFileUrl(user, recipe?.ImageIdentifier).Build();

        return new AddUpdateImageCoverUseCase(loggedUser, repository, unityOfWork, blobStorage);
    }
}
