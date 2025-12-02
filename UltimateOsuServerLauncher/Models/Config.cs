using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using UltimateOsuServerLauncher.Utils;

namespace UltimateOsuServerLauncher.Models
{
    public class Config
    {
        private const string FileName = "LauncherConfig.json";
        
        [JsonProperty("config_version")]
        public string Version;

        [JsonProperty("osu_path")] 
        public string OsuPath;

        [JsonProperty("current_server")] 
        public int ServerIndex;

        [JsonProperty("servers")]
        public Server[] Servers;
        
        public void SetDefault()
        {
            Version = "0.1";
            ServerIndex = 0;
            OsuPath = string.Empty;
            Servers = new[]
            {
                new Server("Bancho", "osu.ppy.sh", "https://osu.ppy.sh", "ppy.sh"),
                new Server("Gatari", "osu.gatari.pw", "https://osu.gatari.pw", "gatari.pw"),
                new Server("MKOsu", "osu.wejust.rest", "https://osu.wejust.rest", "wejust.rest"),
                new Server("Akatsuki", "akatsuki.gg", "https://akatsuki.gg", "akatsuki.gg")
            };
        }

        public static Config FromRemote()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var wc = new WebClient();
            var str = wc.DownloadString("https://raw.githubusercontent.com/Airkek/ultimate-osu-server-launcher/main/default.json");

            return JsonConvert.DeserializeObject<Config>(str);
        }

        public void Save() => File.WriteAllText(FileName, JsonConvert.SerializeObject(this, Formatting.Indented));

        public static Config Read()
        {
            Config cfg;
            
            if (!File.Exists(FileName))
            {
                try
                {
                    cfg = FromRemote();
                }
                catch (Exception e)
                {
                    cfg = new Config();
                    cfg.SetDefault();
                }
                cfg.Save();
            }
            else
            {
                var file = File.ReadAllText(FileName);
                cfg = JsonConvert.DeserializeObject<Config>(file);
            }

            return cfg;
        }
    }
}
