namespace DistributedAPI.CommonTools;

public class ConfigurationMissingException : Exception
{
    public ConfigurationMissingException(string sectionName) 
        : base($"Missing configuration section \"{sectionName}\"")
    {
    }
}