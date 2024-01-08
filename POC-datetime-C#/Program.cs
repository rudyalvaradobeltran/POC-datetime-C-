using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

var timeInfos = new TimeInfo[] {
    new(1, "Local", TimeZoneInfo.Local.DisplayName, DateTime.Now.ToString()),
    new(2, "Pacific SA Standard Time", TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time").DisplayName, TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time")).ToString()),
    new(3, "Eastern Standard Time", TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").DisplayName, TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToString())
};

var timeInfosAPI = app.MapGroup("/timeInfos");
timeInfosAPI.MapGet("/", () => timeInfos);

app.Run();

public record TimeInfo(int Id, string Comment, string CurrentTimeZone, string CurrentDateTime);

[JsonSerializable(typeof(TimeInfo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
