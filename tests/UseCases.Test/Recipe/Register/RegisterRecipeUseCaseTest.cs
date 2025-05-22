using System.Reflection.Metadata;
using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using MinhasReceitasApp.Application.UseCases.Recipe.Register;
using MinhasReceitasApp.Domain.Entities;
using MinhasReceitasApp.Exceptions.ExceptionsBase;
using UseCases.Test.Recipe.InlineDatas;

namespace UseCases.Test.Recipe.Register;

public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task Success_Without_Image()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestRegisterRecipeFormDataBuilder.Build();
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().Be(request.Title);
    }

    [Theory]
    [ClassData(typeof(ImageTypesInLineData))]
    public async Task Success_With_Image(IFormFile file)
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestRegisterRecipeFormDataBuilder.Build(file);
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestRegisterRecipeFormDataBuilder.Build();
        request.Title = string.Empty;
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
                e.ErrorMessages.Contains("title cannot be empty."));
    }

    [Fact]
    public async Task Error_Invalid_File()
    {
        (var user, _) = UserBuilder.Build();

        var textFile = FormFileBuilder.Txt();

        var request = RequestRegisterRecipeFormDataBuilder.Build(textFile);

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(request); };

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 &&
                e.ErrorMessages.Contains("only png, jpg and jpeg images are accepted."));
    }

    private static RegisterRecipeUseCase CreateUseCase(
        MinhasReceitasApp.Domain.Entities.User user,
        MinhasReceitasApp.Domain.Entities.Recipe? recipe = null)
    {
        var mapper = MapperBuilder.Build();
        var unityOfWork = UnityOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = RecipeWriteOnlyRepositoryBuilder.Build();
        var BlobStorage = new BlobStorageServiceBuilder()
            .GetFileUrl(user, recipe?.ImageIdentifier).Build();

        return new RegisterRecipeUseCase(repository, loggedUser, unityOfWork, mapper, BlobStorage);

    }
}
