using FluentAssertions;
using Moq;
using Selah.Domain.Data.Models.TodoItem;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Repository.Interfaces;
using Xunit;

namespace Selah.Application.UnitTests.Infrastructure.RepositoryTests;

public class TodoItemRepositoryUnitTests
{
    private readonly Mock<IBaseRepository> _baseRepository;
    private readonly ITodoItemRepository _todoItemRepository;

    public TodoItemRepositoryUnitTests()
    {
        _baseRepository = new Mock<IBaseRepository>();

        _baseRepository.Setup(x =>
                x.GetFirstOrDefaultAsync<TodoItem>(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(new TodoItem());

        _baseRepository.Setup(x =>
                x.AddAsync<int>(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(1);

        _baseRepository.Setup(x =>
                x.UpdateAsync(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(true);

        _baseRepository.Setup(x =>
                x.DeleteAsync(It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync(true);

        _todoItemRepository = new TodoItemRepository(_baseRepository.Object);
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