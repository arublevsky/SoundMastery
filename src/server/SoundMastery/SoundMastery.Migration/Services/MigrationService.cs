using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Domain.Identity;
using SoundMastery.Migration.Common;

namespace SoundMastery.Migration.Services
{
    internal class MigrationService : IMigrationService
    {
        private readonly IMigrationRunner _migrationRunner;
        private readonly IUserStore<User> _userStore;

        public MigrationService(IMigrationRunner migrationRunner, IUserStore<User> userStore)
        {
            _migrationRunner = migrationRunner;
            _userStore = userStore;
        }

        public Task MigrateUp()
        {
            _migrationRunner.MigrateUp();
            return Task.CompletedTask;
        }

        public Task ApplySeeds()
        {
            return Task.WhenAll(SeedData.Users.Select(user => _userStore.CreateAsync(user, CancellationToken.None)));
        }
    }
}
