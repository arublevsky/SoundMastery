using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Moq;
using SoundMastery.Application.Authorization;
using SoundMastery.Application.Common;
using SoundMastery.Application.Identity;
using SoundMastery.Application.Profile;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;
using SoundMastery.Tests.Application.Builders;
using SoundMastery.Tests.Common;
using SoundMastery.Tests.DataAccess.Builders;
using Xunit;

namespace SoundMastery.Tests.Application.Authorization
{
    public class UserAuthorizationServiceTests
    {
        [Fact]
        public async Task when_password_sign_in_failed_it_should_not_login()
        {
            // Arrange
            var identityManager = new Mock<IIdentityManager>();

            identityManager.Setup(x => x.PasswordSignInAsync(
                    It.Is<string>(u => u == "test@username"),
                    It.Is<string>(p => p == "test@password")))
                .ReturnsAsync(() => SignInResult.Failed);

            var sut = new UserAuthorizationServiceBuilder()
                .With(identityManager.Object)
                .Build();

            // Act
            var result = await sut.Login(new LoginUserModel
            {
                Username = "test@username",
                Password = "test@password"
            });

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task when_password_sign_in_succeeded_it_should_return_token_result()
        {
            // Arrange
            var identityManager = new Mock<IIdentityManager>();
            var userService = new Mock<IUserService>();
            var configuration = new Mock<ISystemConfigurationService>();
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var featureCollection = new Mock<IFeatureCollection>();
            var cookiesFeature = new Mock<IResponseCookiesFeature>();
            var cookies = new DummyResponseCookies();

            // date and time
            var now = 12.January(2020).At(10, 10, 10).ToUniversalTime();
            dateTimeProvider.Setup(x => x.GetUtcNow()).Returns(now);

            // sign-in
            identityManager.Setup(x => x.PasswordSignInAsync(
                    It.Is<string>(u => u == "test@username"),
                    It.Is<string>(p => p == "test@password")))
                .Returns(() => Task.FromResult(SignInResult.Success));

            // user repository
            User user = new UserBuilder().Build();
            userService.Setup(x => x.FindByNameAsync(It.Is<string>(u => u == "test@username"))).ReturnsAsync(user);
            userService.Setup(x => x.GetOrAddRefreshToken(It.Is<User>(u => u == user))).ReturnsAsync("refresh_token");

            // JWT configuration
            var accessTokenLifeTime = 10;
            var refreshTokenLifeTime = 100;

            configuration.Setup(x => x.GetSetting<int>(It.Is<string>(u => u == "Jwt:RefreshTokenExpirationInMinutes")))
                .Returns(refreshTokenLifeTime);

            configuration.Setup(x => x.GetSetting<int>(It.Is<string>(u => u == "Jwt:AccessTokenExpirationInMinutes")))
                .Returns(accessTokenLifeTime);

            configuration.Setup(x => x.GetSetting<string>(It.Is<string>(u => u == "Jwt:Key"))).Returns(Guid.NewGuid().ToString());
            configuration.Setup(x => x.GetSetting<string>(It.Is<string>(u => u == "Jwt:Issuer"))).Returns("jwt_issuer");

            // http context and cookies
            cookiesFeature.Setup(cf => cf.Cookies).Returns(() => cookies);
            featureCollection.Setup(fc => fc.Get<IResponseCookiesFeature>()).Returns(() => cookiesFeature.Object);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext(featureCollection.Object));

            var sut = new UserAuthorizationServiceBuilder()
                .With(identityManager.Object)
                .With(userService.Object)
                .With(httpContextAccessor.Object)
                .With(dateTimeProvider.Object)
                .With(configuration.Object)
                .Build();

            // Act
            var result = await sut.Login(new LoginUserModel
            {
                Username = "test@username",
                Password = "test@password"
            });

            // Assert
            result.Should().NotBeNull();
            result!.Token.Should().NotBeNullOrWhiteSpace();
            result.ExpiresInMilliseconds.Should().Be(TimeSpan.FromMinutes(10).TotalMilliseconds);

            cookies.GetCookie("RefreshTokenCookieKey").Should().BeEquivalentTo(new
            {
                Value = @"{""Username"":""admin@admin.com"",""RefreshToken"":""refresh_token""}",
                Options = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = now + TimeSpan.FromMinutes(100),
                }
            });
        }
    }
}