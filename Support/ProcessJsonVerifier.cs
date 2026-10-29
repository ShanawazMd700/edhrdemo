using Newtonsoft.Json;
using NUnit.Framework;

namespace PlaywrightDemo.Support;

public static class ProcessJsonVerifier
{
    public static void Verify(string downloadedFilePath, ProcessDefinition expected)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(downloadedFilePath);
        ArgumentNullException.ThrowIfNull(expected);

        if (!File.Exists(downloadedFilePath))
        {
            throw new FileNotFoundException($"Downloaded process JSON was not found: {downloadedFilePath}", downloadedFilePath);
        }

        var actual = JsonConvert.DeserializeObject<DownloadedProcessDefinition>(File.ReadAllText(downloadedFilePath))
            ?? throw new InvalidOperationException($"Downloaded process JSON '{downloadedFilePath}' is empty or invalid.");

        Assert.Multiple(() =>
        {
            Assert.That(actual.ProcessName, Is.EqualTo(expected.Name), "processName");
            Assert.That(actual.ProcessDisplayName, Is.EqualTo(expected.DisplayName), "processDisplayName");
            Assert.That(actual.DisplayName, Is.EqualTo(expected.DisplayName), "displayName");
            Assert.That(actual.Version, Is.EqualTo("2"), "version");
            Assert.That(actual.ProcessSteps, Has.Count.EqualTo(expected.Steps.Count), "processSteps count");
        });

        for (var index = 0; index < expected.Steps.Count && index < actual.ProcessSteps.Count; index++)
        {
            var expectedStep = expected.Steps[index];
            var actualStep = actual.ProcessSteps[index];

            Assert.Multiple(() =>
            {
                Assert.That(actualStep.Name, Is.EqualTo(expectedStep.Name), $"processSteps[{index}].stepName");
                Assert.That(actualStep.DisplayName, Is.EqualTo(expectedStep.DisplayName), $"processSteps[{index}].stepDisplayName");
                Assert.That(actualStep.IsOptional, Is.EqualTo(expectedStep.IsOptional), $"processSteps[{index}].isOptional");
                Assert.That(actualStep.IsDhrStep, Is.EqualTo(expectedStep.IsDhrStep), $"processSteps[{index}].isDhrStep");
                Assert.That(actualStep.WatsProcessCode, Is.EqualTo(expectedStep.WatsProcessCode), $"processSteps[{index}].watsProcessCode");
            });
        }
    }
}
