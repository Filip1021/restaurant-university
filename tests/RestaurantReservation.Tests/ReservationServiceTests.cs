using RestaurantReservation.BL.Exceptions;
using RestaurantReservation.BL.Services;
using RestaurantReservation.DAL.Entities;
using RestaurantReservation.DAL.Interfaces;
using RestaurantReservation.BL.Options;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace RestaurantReservation.Tests;

public class ReservationServiceTests
{
    [Fact]
    public async Task ReserveAsync_WhenValid_ShouldReturnResult()
    {
        // Arrange
        var slotRepo = new Mock<IReservationSlotRepository>();
        var userRepo = new Mock<IUserRepository>();

        var ev = new ReservationSlotEntity
        {
            Id = "event1",
            Name = "Concert",
            Location = "Sofia",
            SlotTimeUtc = DateTime.UtcNow.AddDays(5),
            Price = 10m,
            AvailableSeats = 10,
            IsActive = true
        };

        var user = new UserEntity
        {
            Id = "user1",
            FullName = "Test User",
            Email = "test@test.com",
            ReservationsMade = 0
        };

        slotRepo.Setup(x => x.GetByIdAsync("event1", It.IsAny<CancellationToken>())).ReturnsAsync(ev);
        userRepo.Setup(x => x.GetByIdAsync("user1", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        slotRepo.Setup(x => x.UpdateAsync(It.IsAny<ReservationSlotEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        userRepo.Setup(x => x.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var options = new Mock<IOptionsMonitor<ReservationOptions>>();
        options.Setup(o => o.CurrentValue).Returns(new ReservationOptions
        {
            MaxSeatsPerUser = 5,
            ReservationFeePercent = 0m,
            AllowReservationAfterStart = false
        });

        var service = new ReservationService(slotRepo.Object, userRepo.Object, options.Object);

        // Act
        var result = await service.ReserveAsync("user1", "event1", 2, CancellationToken.None);

        // Assert
        Assert.Equal("event1", result.SlotId);
        Assert.Equal("user1", result.UserId);
        Assert.Equal(2, result.Quantity);
        Assert.Equal(20m, result.TotalPrice);
        Assert.Equal(8, result.RemainingSeats);

        slotRepo.Verify(x => x.UpdateAsync(It.IsAny<ReservationSlotEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        userRepo.Verify(x => x.UpdateAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReserveAsync_WhenNotEnoughSeats_ShouldThrow()
    {
        // Arrange
        var slotRepo = new Mock<IReservationSlotRepository>();
        var userRepo = new Mock<IUserRepository>();

        var ev = new ReservationSlotEntity
        {
            Id = "event1",
            Name = "Concert",
            Location = "Sofia",
            SlotTimeUtc = DateTime.UtcNow.AddDays(5),
            Price = 10m,
            AvailableSeats = 1,
            IsActive = true
        };

        var user = new UserEntity
        {
            Id = "user1",
            FullName = "Test User",
            Email = "test@test.com",
            ReservationsMade = 0
        };

        slotRepo.Setup(x => x.GetByIdAsync("event1", It.IsAny<CancellationToken>())).ReturnsAsync(ev);
        userRepo.Setup(x => x.GetByIdAsync("user1", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var options = new Mock<IOptionsMonitor<ReservationOptions>>();
        options.Setup(o => o.CurrentValue).Returns(new ReservationOptions
        {
            MaxSeatsPerUser = 5,
            ReservationFeePercent = 0m,
            AllowReservationAfterStart = false
        });

        var service = new ReservationService(slotRepo.Object, userRepo.Object, options.Object);

        // Act + Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.ReserveAsync("user1", "event1", 2, CancellationToken.None));
    }
}
