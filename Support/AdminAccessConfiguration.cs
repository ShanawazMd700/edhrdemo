using Newtonsoft.Json;

namespace PlaywrightDemo.Support;

public sealed class AdminAccessConfiguration
{
    [JsonProperty("Processes")]
    public Dictionary<string, ProcessConfiguration> Processes { get; init; } = new();

    [JsonProperty("UserGroups")]
    public Dictionary<string, UserGroupConfiguration> UserGroups { get; init; } = new();
}

public sealed class ProcessConfiguration
{
    [JsonProperty("ProcessesCreation")]
    public ProcessDefinition ProcessCreation { get; init; } = new();

    [JsonProperty("LineCreation")]
    public LineDefinition LineCreation { get; init; } = new();
}

public sealed class ProcessDefinition
{
    [JsonProperty("processName")]
    public string Name { get; init; } = string.Empty;

    [JsonProperty("processDisplayName")]
    public string DisplayName { get; init; } = string.Empty;

    [JsonProperty("processSteps")]
    public List<ProcessStepDefinition> Steps { get; init; } = new();
}

public sealed class ProcessStepDefinition
{
    [JsonProperty("stepName")]
    public string Name { get; init; } = string.Empty;

    [JsonProperty("stepDisplayName")]
    public string DisplayName { get; init; } = string.Empty;

    [JsonProperty("isOptional")]
    public bool IsOptional { get; init; }

    [JsonProperty("isDhrStep")]
    public bool IsDhrStep { get; init; }

    [JsonProperty("watsProcessCode")]
    public int WatsProcessCode { get; init; }
}

public sealed class LineDefinition
{
    [JsonProperty("lineName")]
    public string Name { get; init; } = string.Empty;

    [JsonProperty("displayName")]
    public string DisplayName { get; init; } = string.Empty;

    [JsonProperty("loginTimeoutInSec")]
    public int LoginTimeoutInSeconds { get; init; }

    [JsonProperty("watsLocation")]
    public string WatsLocation { get; init; } = string.Empty;

    [JsonProperty("workstations")]
    public List<WorkstationDefinition> Workstations { get; init; } = new();
}

public sealed class WorkstationDefinition
{
    [JsonProperty("workstationId")]
    public string Id { get; init; } = string.Empty;

    [JsonProperty("workstationName")]
    public string Name { get; init; } = string.Empty;

    [JsonProperty("processStepNames")]
    public List<string> ProcessStepNames { get; init; } = new();

    [JsonProperty("userGroups")]
    public List<string> UserGroups { get; init; } = new();

    [JsonProperty("assets")]
    public List<AssetDefinition> Assets { get; init; } = new();
}

public sealed class AssetDefinition
{
    [JsonProperty("assetName")]
    public string Name { get; init; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; init; } = string.Empty;

    [JsonProperty("assetDisplayName")]
    public string DisplayName { get; init; } = string.Empty;

    [JsonProperty("serialNumber")]
    public string SerialNumber { get; init; } = string.Empty;

    [JsonProperty("displayAtWorkstation")]
    public bool DisplayAtWorkstation { get; init; }

    [JsonProperty("expirationDate")]
    public string ExpirationDate { get; init; } = string.Empty;
}

public sealed class UserGroupConfiguration
{
    [JsonProperty("userGroupName")]
    public string Name { get; init; } = string.Empty;

    [JsonProperty("users")]
    public List<string> Users { get; init; } = new();
}
