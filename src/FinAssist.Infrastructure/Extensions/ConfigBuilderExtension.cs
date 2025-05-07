using Microsoft.Extensions.Configuration;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace FinAssist.Infrastructure.Extensions;

public static class ConfigBuilderExtension
{
    public static async Task LoadSecretsFromVault(this ConfigurationManager configuration, string vaultAddress, string vaultToken)
    {
        var authMethod = new TokenAuthMethodInfo(vaultToken);
        var vaultClientSettings = new VaultClientSettings(vaultAddress, authMethod);
        var vaultClient = new VaultClient(vaultClientSettings);

        var secrets = 
            await vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync<Dictionary<string, object>>("prod");
        
        foreach (var kvp in secrets.Data)
        {
            configuration[kvp.Key] = kvp.Value.ToString();
        }
    }
}