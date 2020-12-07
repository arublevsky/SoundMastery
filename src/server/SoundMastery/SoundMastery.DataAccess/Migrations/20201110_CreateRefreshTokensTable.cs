using FluentMigrator;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Migrations
{
    [Migration(20201110)]
    public class CreateRefreshTokensTable : Migration
    {
        public override void Up()
        {
            Create.Table("RefreshTokens")
                .WithColumn(nameof(RefreshToken.Id)).AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn(nameof(RefreshToken.UserId)).AsGuid().NotNullable()
                .WithColumn(nameof(RefreshToken.Token)).AsString(255).NotNullable()
                .WithColumn(nameof(RefreshToken.CreatedAtUtc)).AsDateTime().NotNullable();

            Create.ForeignKey("FK_RefreshTokens_UserId_Users_Id").FromTable("RefreshTokens")
                .ForeignColumn(nameof(RefreshToken.UserId))
                .ToTable("Users")
                .PrimaryColumn(nameof(User.Id));
        }

        public override void Down()
        {
            // do nothing
        }
    }
}
