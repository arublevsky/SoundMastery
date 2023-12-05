using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SoundMastery.Domain.Core;
using SoundMastery.Domain.Identity;
using SoundMastery.Tests.DataAccess.Builders;
using Xunit;
using ConfigurationBuilder = SoundMastery.Tests.DataAccess.Builders.ConfigurationBuilder;

namespace SoundMastery.Tests.DataAccess.Services.Core;

public class CoursesRepositoryTests : IAsyncLifetime
{
    private readonly DockerContainer _container = DatabaseTestContainerBuilder.Build();

    public Task InitializeAsync() => _container.StartAsync();

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();

    [Fact]
    public async Task it_should_create_course_with_all_hierarchy()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();

        var sut = new GenericRepositoryBuilder<Course>().With(configuration).Build();

        // Act
        var course = await sut.Create(new Course
        {
            Name = Guid.NewGuid().ToString(),
            ProductId = product.Id,
            Cost = 100.00001m,
            TeacherId = teacher.Id
        });

        // Assert
        var createdCourse = await sut.Get(course.Id);

        createdCourse.Name.Should().Be(course.Name);
        createdCourse.Cost.Should().Be((decimal)100.00001);
        createdCourse.Teacher.UserName.Should().Be(teacher.UserName);
        createdCourse.Product.Name.Should().Be(product.Name);
    }

    private async Task<(User, Product)> GetDataForTheCourse(IConfiguration configuration)
    {
        var productsRepository = new GenericRepositoryBuilder<Product>().With(configuration).Build();
        var participantsRepository = new GenericRepositoryBuilder<CourseParticipant>().With(configuration).Build();
        var lessonsAttendeeRepository = new GenericRepositoryBuilder<CourseLessonAttendee>().With(configuration).Build();
        var lessonsRepository = new GenericRepositoryBuilder<CourseLesson>().With(configuration).Build();
        var lessonsRepository = new GenericRepositoryBuilder<CourseLessonHomeAssignment>().With(configuration).Build();
        var lessonsHomeAssignmentMaterialRepository = new GenericRepositoryBuilder<CourseLessonHomeAssignmentMaterial>().With(configuration).Build();
        var usersRepository = new UserRepositoryBuilder().With(configuration).Build();

        var teacher = await usersRepository.Create(new UserBuilder().WithUsername("course@teacher").Build());
        var product = await productsRepository.Create(new Product { Name = Guid.NewGuid().ToString() });
        var product = await productsRepository.Create(new Product { Name = Guid.NewGuid().ToString() });

        return (teacher, product)
    }
}