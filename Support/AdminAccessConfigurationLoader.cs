using Newtonsoft.Json;

namespace PlaywrightDemo.Support;

public static class AdminAccessConfigurationLoader
{
    private const string FileName = "AdminAccessValues.json";

    public static ProcessConfiguration LoadProcessConfiguration(string configurationName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configurationName);
        var configuration = LoadConfiguration();

        if (!configuration.Processes.TryGetValue(configurationName, out var processConfiguration) || processConfiguration is null)
        {
            throw new KeyNotFoundException($"Process configuration '{configurationName}' was not found in {FileName}.");
        }

        Validate(processConfiguration, configurationName);
        return processConfiguration;
    }

    public static IReadOnlyCollection<UserGroupConfiguration> LoadUserGroups()
    {
        var configuration = LoadConfiguration();

        if (configuration.UserGroups.Count == 0 || configuration.UserGroups.Values.Any(userGroup => userGroup is null || string.IsNullOrWhiteSpace(userGroup.Name)))
        {
            throw new InvalidOperationException($"'{FileName}' must contain at least one UserGroups entry with a userGroupName.");
        }

        return configuration.UserGroups.Values;
    }

    private static AdminAccessConfiguration LoadConfiguration()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Resources", FileName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Configuration file was not found: {path}", path);
        }

        return JsonConvert.DeserializeObject<AdminAccessConfiguration>(File.ReadAllText(path))
            ?? throw new InvalidOperationException($"Configuration file '{path}' is empty or invalid.");
    }

    private static void Validate(ProcessConfiguration configuration, string configurationName)
    {
        if (string.IsNullOrWhiteSpace(configuration.ProcessCreation?.Name) ||
            string.IsNullOrWhiteSpace(configuration.ProcessCreation.DisplayName) ||
            configuration.ProcessCreation.Steps is null ||
            configuration.ProcessCreation.Steps.Count == 0)
        {
            throw new InvalidOperationException($"Process configuration '{configurationName}' requires a process name, display name, and at least one process step.");
        }

        if (configuration.LineCreation is null ||
            string.IsNullOrWhiteSpace(configuration.LineCreation.Name) ||
            string.IsNullOrWhiteSpace(configuration.LineCreation.DisplayName) ||
            configuration.LineCreation.Workstations is null ||
            configuration.LineCreation.Workstations.Count == 0)
        {
            throw new InvalidOperationException($"Process configuration '{configurationName}' requires a line and at least one workstation.");
        }

        if (configuration.ProcessCreation.Steps.Any(step => step is null || string.IsNullOrWhiteSpace(step.Name)) ||
            configuration.LineCreation.Workstations.Any(workstation => workstation is null || string.IsNullOrWhiteSpace(workstation.Id) || string.IsNullOrWhiteSpace(workstation.Name)))
        {
            throw new InvalidOperationException($"Process configuration '{configurationName}' contains a process step or workstation with a missing required value.");
        }
    }
}
