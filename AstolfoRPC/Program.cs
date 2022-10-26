using AstolfoRPC;
using DiscordRPC;
using Lyrics;
using DiscordRPC.Logging;

DiscordRpcClient client;

Console.WriteLine("i have too much time on my hands");

if (!File.Exists("lyrics.json"))
{
    Console.WriteLine("missing lyrics.json");
    Environment.Exit(5);
}

PresenceStateList states = new PresenceStateList();
var lyrics = Lyrics.Welcome.FromJson(File.ReadAllText("lyrics.json"));
for (int j = 0; j < lyrics.Events.Length; j++)
{
    if (j == 69) break;
    Console.WriteLine(j);
    Console.WriteLine(lyrics.Events[j].Segs[0].Utf8);
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

client = new DiscordRpcClient("515926861797392394");
client.Logger = new ConsoleLogger()  { Level = LogLevel.Warning };
client.OnReady += (sender, e) =>
{
    Console.WriteLine("Connected to {0}#{1}", e.User.Username, e.User.Discriminator);
};
client.Initialize();

client.SetPresence(new RichPresence()
{
    Details = "busy being your favorite girl",
    State = "i have too much time on my hands",
    Assets = new Assets()
    {
        LargeImageKey = "38",
        LargeImageText = "lol"
    }
});

while (true)
{
    foreach (var state in states)
    {
        client.SetPresence(new RichPresence()
        {
            Details = state.Text,
            State = "AstolfoRPC v1",
            Assets = new Assets()
            {
                LargeImageKey = "38",
                LargeImageText = "song: i wanna be a girl - mafumafu"
            }
        });
        Thread.Sleep(state.Delay);
    }
    Thread.Sleep(10000);
}