using Azure.Identity;

namespace AzureConfigurations
{
    internal static class Credential
    {
        public static readonly DefaultAzureCredentialOptions Options =
            new()
            {
                ExcludeAzureDeveloperCliCredential = true,
                ExcludeAzurePowerShellCredential = true,
                ExcludeEnvironmentCredential = true,
                ExcludeInteractiveBrowserCredential = true,
                ExcludeSharedTokenCacheCredential = true,
                ExcludeVisualStudioCodeCredential = true,
                ExcludeVisualStudioCredential = true,
                ExcludeWorkloadIdentityCredential = true,

                // Allow
                ExcludeManagedIdentityCredential = false,
                ExcludeAzureCliCredential = false,
            };
    }
}
