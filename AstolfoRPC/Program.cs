using AstolfoRPC;
using DiscordRPC;
using DiscordRPC.Logging;

DiscordRpcClient client;

Console.WriteLine("astolforpc v1.0, press ctrl+c to exit");

if (!File.Exists("lyrics.json"))
{
    Console.WriteLine("missing lyrics.json");
    Environment.Exit(905);
}

if (!File.Exists("config.json"))
{
    Console.WriteLine("missing config.json");
    Environment.Exit(904);
}

var config = Config.Welcome.FromJson(File.ReadAllText("config.json"));

PresenceStateList states = new PresenceStateList();
var lyrics = Lyrics.Welcome.FromJson(File.ReadAllText("lyrics.json"));
Console.WriteLine("{0} events. loading.", lyrics.Events.Length);

for (int j = 0; j < lyrics.Events.Length; j++)
{
    if (j == lyrics.Events.Length - 1) break;
    if(j == lyrics.Events.Length)
    {
        states.Add(new PresenceState()
        {
            Text = lyrics.Events[j].Segs[0].Utf8,
            Delay = (int)lyrics.Events[j].DDurationMs
        });
    }
    else
    {
        states.Add(new PresenceState()
        {
            Text = lyrics.Events[j].Segs[0].Utf8,
            Delay = (int)(lyrics.Events[j + 1].TStartMs - lyrics.Events[j].TStartMs)
        });
    }
    
}

Console.WriteLine("events loaded");

client = new DiscordRpcClient(config.ApplicationId);
client.Logger = new ConsoleLogger()  { Level = LogLevel.Warning };
client.OnReady += (sender, e) =>
{
    Console.WriteLine("Connected to {0}#{1}", e.User.Username, e.User.Discriminator);
};
client.Initialize();

while (true)
{
    foreach (var state in states)
    {
        client.SetPresence(new RichPresence()
        {
            Details = state.Text,
            State = config.Subtext,
            Assets = new Assets()
            {
                LargeImageKey = config.ImageKey.ToString(),
                LargeImageText = config.ImageText
            }
        });
        Thread.Sleep(state.Delay);
    }
    Thread.Sleep(10000);
}