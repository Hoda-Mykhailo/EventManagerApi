using EventManagerApi.Models;
using EventManagerApi.Models.DTO;
using EventManagerApi.Repositories;
using EventManagerApi.Services;
using Moq;
using Xunit;
using FluentAssertions;

namespace EventManagerApi.Tests;

public class EventServiceTests
{
    private readonly Mock<IEventRepository> _eventRepoMock;
    private readonly EventService _eventService;

    public EventServiceTests()
    {
        _eventRepoMock = new Mock<IEventRepository>();
        _eventService = new EventService(_eventRepoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedEvent()
    {
        var dto = new EventDto
        {
            Title = "Conference",
            Description = "Tech event",
            Date = DateTime.UtcNow
        };

        var ev = new Event
        {
            Id = 1,
            Title = dto.Title,
            Description = dto.Description,
            Date = dto.Date,
            UserId = 5
        };

        _eventRepoMock.Setup(r => r.AddAsync(It.IsAny<Event>())).ReturnsAsync(ev);

        var result = await _eventService.CreateAsync(dto, 5);

        result.Should().NotBeNull();
        result.Title.Should().Be("Conference");
        result.UserId.Should().Be(5);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_IfUserNotOwner()
    {
        var dto = new EventDto { Title = "Updated", Description = "Changed", Date = DateTime.UtcNow };
        var existing = new Event { Id = 1, Title = "Old", Description = "Old", UserId = 99 };

        _eventRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        Func<Task> act = async () => await _eventService.UpdateAsync(1, dto, 5);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepository_IfUserIsOwner()
    {
        var existing = new Event { Id = 1, Title = "Old", UserId = 5 };
        _eventRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        await _eventService.DeleteAsync(1, 5);

        _eventRepoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
    }
}
