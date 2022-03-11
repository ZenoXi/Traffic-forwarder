using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LocalUI
{
    class Options
    {
        // Whitelist
        public List<string> Whitelist { get; private set; }
        public bool WhitelistEnabled { get; set; }

        // Blacklist
        public List<string> Blacklist { get; private set; }
        public bool BlacklistEnabled { get; set; }

        // Max connections
        public int MaxConnections { get; set; }
        public bool DisconnectExcess { get; set; }

        // Saved input
        public string LastLocalPort { get; set; }
        public string LastRemoteIp { get; set; }
        public string LastRemotePort { get; set; }

        private string whitelistFilename = "whitelist";
        private string blacklistFilename = "blacklist";
        private string optionsFilename = "options";

        public Options()
        {
            // Default values
            Whitelist = new List<string>();
            Blacklist = new List<string>();
            WhitelistEnabled = false;
            BlacklistEnabled = false;
            MaxConnections = 99999;
            DisconnectExcess = true;
            LastLocalPort = "44444";
            LastRemoteIp = "127.0.0.1";
            LastRemotePort = "55555";

            // Read from file
            try
            {
                var whitelistLines = File.ReadLines(whitelistFilename);
                foreach (var line in whitelistLines)
                {
                    if (IpValid(line))
                    {
                        Whitelist.Add(line);
                    }
                }
                var blacklistLines = File.ReadLines(blacklistFilename);
                foreach (var line in blacklistLines)
                {
                    if (IpValid(line))
                    {
                        Blacklist.Add(line);
                    }
                }
                var optionLines = File.ReadLines(optionsFilename);
                foreach (var line in optionLines)
                {
                    ParseOptionLine(line);
                }
            }
            catch (Exception) { }
        }

        ~Options()
        {
            Save();
        }

        public void Save()
        {
            File.WriteAllLines(whitelistFilename, Whitelist);
            File.WriteAllLines(blacklistFilename, Blacklist);

            List<string> options = new List<string>();
            options.Add($"whitelistEnabled={WhitelistEnabled}");
            options.Add($"blacklistEnabled={BlacklistEnabled}");
            options.Add($"maxConnections={MaxConnections}");
            options.Add($"disconnectExcess={DisconnectExcess}");
            options.Add($"lastLocalPort={LastLocalPort}");
            options.Add($"lastRemoteIp={LastRemoteIp}");
            options.Add($"lastRemotePort={LastRemotePort}");
            File.WriteAllLines(optionsFilename, options);
        }

        public bool IpValid(string ip)
        {
            var parts = ip.Split('.');
            if (parts.Length != 4) return false;

            foreach (var part in parts)
            {
                if (int.TryParse(part, out int result))
                {
                    if (result < 0 || result > 255)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void ParseOptionLine(string line)
        {
            var parts = line.Split('=');
            if (parts.Length != 2) return;
            parts[1] = parts[1].ToLower();

            switch (parts[0])
            {
                case "whitelistEnabled":
                {
                    if (parts[1] == "true")
                    {
                        WhitelistEnabled = true;
                    }
                    else if (parts[1] == "false")
                    {
                        WhitelistEnabled = false;
                    }
                    break;
                }
                case "blacklistEnabled":
                {
                    if (parts[1] == "true")
                    {
                        BlacklistEnabled = true;
                    }
                    else if (parts[1] == "false")
                    {
                        BlacklistEnabled = false;
                    }
                    break;
                }
                case "maxConnections":
                {
                    if (int.TryParse(parts[1], out int result))
                    {
                        if (result >= 0 && result <= 99999)
                        {
                            MaxConnections = result;
                        }
                    }
                    break;
                }
                case "disconnectExcess":
                {
                    if (parts[1] == "true")
                    {
                        DisconnectExcess = true;
                    }
                    else if (parts[1] == "false")
                    {
                        DisconnectExcess = false;
                    }
                    break;
                }
                case "lastLocalPort":
                {
                    LastLocalPort = parts[1];
                    break;
                }
                case "lastRemoteIp":
                {
                    LastRemoteIp = parts[1];
                    break;
                }
                case "lastRemotePort":
                {
                    LastRemotePort = parts[1];
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }
}
