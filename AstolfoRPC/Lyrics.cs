namespace Lyrics
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Welcome
    {
        [JsonProperty("wireMagic")]
        public string WireMagic { get; set; }

        [JsonProperty("pens")]
        public Pen[] Pens { get; set; }

        [JsonProperty("wsWinStyles")]
        public Pen[] WsWinStyles { get; set; }

        [JsonProperty("wpWinPositions")]
        public Pen[] WpWinPositions { get; set; }

        [JsonProperty("events")]
        public Event[] Events { get; set; }
    }

    public partial class Event
    {
        [JsonProperty("tStartMs")]
        public long TStartMs { get; set; }

        [JsonProperty("dDurationMs")]
        public long DDurationMs { get; set; }

        [JsonProperty("segs")]
        public Seg[] Segs { get; set; }
    }

    public partial class Seg
    {
        [JsonProperty("utf8")]
        public string Utf8 { get; set; }
    }

    public partial class Pen
    {
    }

    public partial class Welcome
    {
        public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, Lyrics.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, Lyrics.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}