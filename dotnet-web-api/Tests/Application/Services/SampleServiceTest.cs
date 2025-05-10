using Application.Services;
using Core.Lib.EventHandler.Interfaces;
using Database;
using Microsoft.EntityFrameworkCore;
using Moq;
using StarterApp.Domain.Model;
using static Application.Constants.AppConstants;

namespace Tests.Application.Services
{
    public class SampleServiceTest
    {
        private readonly Mock<AppDbContext> _dbContextMock;
        private readonly Mock<IEventHandler> _eventHandlerMock;
        private readonly SampleService _sampleService;

        public SampleServiceTest()
        {
            _dbContextMock = new Mock<AppDbContext>();
            _eventHandlerMock = new Mock<IEventHandler>();
            _sampleService = new SampleService(_dbContextMock.Object, _eventHandlerMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllSamples()
        {
            // Arrange
            var samples = new List<SampleEntity>
                {
                    new SampleEntity { Id = 1, Name = "Sample 1" },
                    new SampleEntity { Id = 2, Name = "Sample 2" }
                };
            var cancellationToken = CancellationToken.None;
            _dbContextMock.Setup(db => db.Samples.ToListAsync(cancellationToken)).ReturnsAsync(samples);

            // Act
            var result = await _sampleService.GetAllAsync(cancellationToken);

            // Assert
            Assert.Equal(samples, result);
        }

        [Fact]
        public async Task GetAsync_WithValidId_ShouldReturnSample()
        {
            // Arrange
            var sample = new SampleEntity { Id = 1, Name = "Sample 1" };
            var id = 1;
            var cancellationToken = CancellationToken.None;
            _dbContextMock.Setup(db => db.Samples.FirstOrDefaultAsync(e => e.Id == id, cancellationToken)).ReturnsAsync(sample);

            // Act
            var result = await _sampleService.GetAsync(id, cancellationToken);

            // Assert
            Assert.Equal(sample, result);
        }

        [Fact]
        public async Task GetAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var id = 1;
            var cancellationToken = CancellationToken.None;
            _dbContextMock.Setup(db => db.Samples.FirstOrDefaultAsync(e => e.Id == id, cancellationToken)).ReturnsAsync((SampleEntity)null);

            // Act
            var result = await _sampleService.GetAsync(id, cancellationToken);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddSampleAndPublishEvent()
        {
            // Arrange
            var sample = new SampleEntity { Id = 1, Name = "Sample 1" };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _sampleService.CreateAsync(sample, cancellationToken);

            // Assert
            _dbContextMock.Verify(db => db.Samples.AddAsync(sample, cancellationToken), Times.Once);
            _dbContextMock.Verify(db => db.SaveChangesAsync(cancellationToken), Times.Once);
            _eventHandlerMock.Verify(eh => eh.PublishEvent(It.IsAny<object>(), ProcessName.Sample, ActionName.Insert), Times.Once);
            Assert.Equal(sample, result);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldRemoveSampleAndPublishEvent()
        {
            // Arrange
            var sample = new SampleEntity { Id = 1, Name = "Sample 1" };
            var id = 1;
            var cancellationToken = CancellationToken.None;
            _dbContextMock.Setup(db => db.Samples.FindAsync(id)).ReturnsAsync(sample);

            // Act
            var result = await _sampleService.DeleteAsync(id, cancellationToken);

            // Assert
            _dbContextMock.Verify(db => db.Samples.Remove(sample), Times.Once);
            _dbContextMock.Verify(db => db.SaveChangesAsync(cancellationToken), Times.Once);
            _eventHandlerMock.Verify(eh => eh.PublishEvent(It.IsAny<object>(), ProcessName.Sample, ActionName.Delete), Times.Once);
            Assert.Equal(sample, result);
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var id = 1;
            var cancellationToken = CancellationToken.None;
            _dbContextMock.Setup(db => db.Samples.FindAsync(id)).ReturnsAsync((SampleEntity)null);

            // Act and Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _sampleService.DeleteAsync(id, cancellationToken));
        }
    }
}