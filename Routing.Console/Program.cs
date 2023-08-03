using Newtonsoft.Json;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Globalization;
using Yandex.API;

var apiKeyOption = new Option<string>(
    aliases: new string[] { "--apikey", "-a" }, description: "Api key")
{
    IsRequired = true,
};

var waypointOption = new Option<(decimal lat, decimal lon)[]>(
    aliases: new string[] { "--waypoints", "-w" }, description: "Waypoints for route in format: {latitude},{longitude};{latitude},{longitude}", parseArgument: (arg) =>
{
    if (arg == null || !arg.Tokens.Any())
        throw new ArgumentNullException(nameof(arg));
    var token = arg.Tokens.Single().Value;
    var coords = token.Split(";");
    var result = new List<(decimal lat, decimal lon)>();
    foreach (var coord in coords)
    {
        var latlon = coord.Split(',');
        var lat = decimal.Parse(latlon[0], CultureInfo.InvariantCulture);
        var lon = decimal.Parse(latlon[1], CultureInfo.InvariantCulture);
        result.Add((lat, lon));
    }
    return result.ToArray();
})
{
    IsRequired = true,
};

var modeOption = new Option<RoutingMode>(
    aliases: new string[] { "--mode", "-m" }, description: "Routing mode", parseArgument: (arg) =>
    {
        if (arg == null || !arg.Tokens.Any())
            throw new ArgumentNullException(nameof(arg));
        var token = arg.Tokens.Single().Value;
        return (RoutingMode)Enum.Parse(typeof(RoutingMode), token);
    })
{
    IsRequired = false,
};
RootCommand rootCommand = new(description: "Route with Yandex.")
{
    apiKeyOption,
    waypointOption,
    modeOption
};
rootCommand.SetHandler(async (apiKeyOption,
    waypointOption,
    modeOption) =>
{
    var result = await new RoutingClient(apiKeyOption).Route(new RoutingRequest(waypointOption) { 
        Mode=modeOption
    });
    Console.WriteLine(JsonConvert.SerializeObject(result));
}, apiKeyOption,
    waypointOption,
    modeOption);
try
{
    await rootCommand.InvokeAsync(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}