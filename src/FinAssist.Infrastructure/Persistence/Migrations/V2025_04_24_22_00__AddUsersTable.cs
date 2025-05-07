using FluentMigrator;

namespace FinAssist.Infrastructure.Persistence.Migrations;

[Migration(202504242200)]
public class AddUsersTable : Migration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().Nullable()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable().Unique()
            .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
            .WithColumn("Hash").AsString().NotNullable()
            .WithColumn("Salt").AsString().NotNullable()
            .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
            .WithColumn("LockoutEnd").AsDateTime().Nullable()
            .WithColumn("AccessFailedCount").AsInt32().Nullable();
        
        Create.Table("UserSessions")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().Nullable()
            .WithColumn("UserId").AsGuid().NotNullable()
            .WithColumn("ExpiresAt").AsDateTime().NotNullable()
            .WithColumn("UserAgent").AsString(1024).NotNullable()
            .WithColumn("IpAddress").AsString(50).Nullable()
            .WithColumn("RefreshTokenHash").AsString().NotNullable().Unique()
            .WithColumn("RefreshTokenExpiresAt").AsDateTime().NotNullable();

        Create.ForeignKey("FK_UserSessions_Users")
            .FromTable("UserSessions").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("UserSessions");
        Delete.Table("Users");
    }
}