using FluentAssertions;
using NSubstitute;
using Selah.Domain.Data.Models.TodoItem;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Infrastructure.RepositoryTests;

public class TodoItemRepositoryUnitTests
{
    private readonly IBaseRepository _baseRepository;
    private readonly ITodoItemRepository _todoItemRepository;

    public TodoItemRepositoryUnitTests()
    {
        _baseRepository = Substitute.For<IBaseRepository>();

        _baseRepository.GetFirstOrDefaultAsync<TodoItem>(Arg.Any<string>(), Arg.Any<object>())
            .Returns(new TodoItem());

        _baseRepository.AddAsync<int>(Arg.Any<string>(), Arg.Any<object>())
            .Returns(1);

        _baseRepository.UpdateAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(true);

        _baseRepository.DeleteAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(true);

        _todoItemRepository = new TodoItemRepository(_baseRepository);
    }

    [Fact]
    public async Task CreateTodoItem_ShouldReturnNewId()
    {
        var result = await _todoItemRepository.CreateTodoItem(new TodoItem());
        result.Should().Be(1);
    }

    [Fact]
    public async Task DeleteTodo_ShouldReturnTrue()
    {
        var result = await _todoItemRepository.DeleteTodo(1);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetTodoByUserAndId_ShouldReturnSingleTodoItem()
    {
        var result = await _todoItemRepository.GetTodoByUserAndId(1, 1);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetTodoList_ShouldReturnAEmptyList()
    {
        var result = await _todoItemRepository.GetTodoList(1, 25, 1);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateTodo_ShouldReturnTrue()
    {
        var result = await _todoItemRepository.Update(new TodoItem());
        result.Should().BeTrue();
    }
}