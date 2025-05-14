using FinAssist.Domain.Notification.Providers;
using FluentMigrator;

namespace FinAssist.Infrastructure.Persistence.Migrations;

[Migration(202504251000)]
public class AddNotificationProviderToUsers : Migration
{
    public override void Up()
    {
        Alter.Table("Users")
            .AddColumn("NotificationProvider")
            .AsString(50)
            .NotNullable()
            .WithDefaultValue(NotificationProviderType.Email.ToString());
    }

    public override void Down()
    {
        Delete.Column("NotificationProvider").FromTable("Users");
    }
}
