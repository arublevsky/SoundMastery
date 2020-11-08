using FluentMigrator;
using SoundMastery.DataAccess.Services;

namespace SoundMastery.DataAccess.Migrations
{
    [Migration(20201029)]
    public class InitialMigration : Migration
    {
        private readonly DatabaseEngineAccessor _accessor;

        public InitialMigration(DatabaseEngineAccessor accessor)
        {
            _accessor = accessor;
        }

        public override void Up()
        {
            if (_accessor() == DatabaseEngine.Postgres)
            {
                Execute.Sql(@"CREATE EXTENSION IF NOT EXISTS ""uuid-ossp""");
            }

            Create.Table("Users")
                .WithColumn("Id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("UserName").AsString(255).NotNullable()
                .WithColumn("NormalizedUserName").AsString(255).NotNullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("NormalizedEmail").AsString(255).NotNullable()
                .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
                .WithColumn("PasswordHash").AsString(2000).NotNullable()
                .WithColumn("SecurityStamp").AsString(2000).Nullable()
                .WithColumn("ConcurrencyStamp").AsString(2000).Nullable()
                .WithColumn("PhoneNumber").AsString(255).Nullable()
                .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
                .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
                .WithColumn("LockoutEnd").AsDateTimeOffset().Nullable()
                .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
                .WithColumn("AccessFailedCount").AsByte().NotNullable()
                .WithColumn("FirstName").AsString(255).NotNullable()
                .WithColumn("LastName").AsString(255).NotNullable();

            Create.Table("IdentityUserClaims")
                .WithColumn("Id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("ClaimType").AsString(255).NotNullable()
                .WithColumn("ClaimValue").AsString(2000).NotNullable();

            Create.ForeignKey("FK_IdentityUserClaims_UserId_Users_Id").FromTable("IdentityUserClaims")
                .ForeignColumn("UserId").ToTable("Users").PrimaryColumn("Id");

            Create.Table("Roles")
                .WithColumn("Id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("NormalizedName").AsString(255).NotNullable()
                .WithColumn("ConcurrencyStamp").AsString(2000).NotNullable();

            Create.Table("IdentityUserRole")
                .WithColumn("UserId").AsGuid().ForeignKey("Users", "Id").NotNullable()
                .WithColumn("RoleId").AsGuid().ForeignKey("Roles", "Id").NotNullable();

            Create.PrimaryKey("PK_IdentityUserRole").OnTable("IdentityUserRole")
                .Columns("UserId", "RoleId");
        }

        public override void Down()
        {
            // do nothing
        }
    }
}
