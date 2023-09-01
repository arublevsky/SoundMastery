using System;
using Moq;
using SoundMastery.Domain.Services;

namespace SoundMastery.Tests.Application.Authorization;

public class SystemConfigurationServiceBuilder
{
    public ISystemConfigurationService BuildWithJwtConfigured()
    {
        var configuration = new Mock<ISystemConfigurationService>();

        var accessTokenLifeTime = 10;
        var refreshTokenLifeTime = 100;

        configuration.Setup(x => x.GetSetting<int>(It.Is<string>(u => u == "Jwt:RefreshTokenExpirationInMinutes")))
            .Returns(refreshTokenLifeTime);

        configuration.Setup(x => x.GetSetting<int>(It.Is<string>(u => u == "Jwt:AccessTokenExpirationInMinutes")))
            .Returns(accessTokenLifeTime);

        configuration.Setup(x => x.GetSetting<string>(It.Is<string>(u => u == "Jwt:Key"))).Returns(Guid.NewGuid().ToString());
        configuration.Setup(x => x.GetSetting<string>(It.Is<string>(u => u == "Jwt:Issuer"))).Returns("jwt_issuer");

        return configuration.Object;
    }
}