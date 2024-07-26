/* 
    This class will help use our configuration from appsettings.json and help us access these configurations settings using this class
*/
namespace API;

public class CloudinarySettings
{
    public required string CloudName { get; set; }
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}
