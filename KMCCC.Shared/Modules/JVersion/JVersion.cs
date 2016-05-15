namespace KMCCC.Modules.JVersion
{
    #region

    using System;
    using System.Collections.Generic;
    using LitJson;
    using System.Text;
    using Tools;
    #endregion

    /// <summary>
    ///     用来Json的实体类
    /// </summary>
    public class JVersion
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("time")]
		public DateTime Time { get; set; }

		[JsonPropertyName("releaseTime")]
		public DateTime ReleaseTime { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("minecraftArguments")]
		public string MinecraftArguments { get; set; }

		[JsonPropertyName("minimumLauncherVersion")]
		public int MinimumLauncherVersion { get; set; }

		[JsonPropertyName("libraries")]
		public List<JLibrary> Libraries { get; set; }

		[JsonPropertyName("mainClass")]
		public string MainClass { get; set; }

		[JsonPropertyName("assets")]
		public string Assets { get; set; }

		[JsonPropertyName("inheritsFrom")]
		public string InheritsVersion { get; set; }

		[JsonPropertyName("jar")]
		public string JarId { get; set; }

        [JsonPropertyName("downloads")]
        public JDownload Downloads { get; set; }
	}

	public class JLibrary
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

        [JsonPropertyName("downloads")]
        public LibrariesDownloadInfo Downloads { get; set; }

        [JsonPropertyName("natives")]
		public Dictionary<string, string> Natives { get; set; }

		[JsonPropertyName("rules")]
		public List<JRule> Rules { get; set; }

		[JsonPropertyName("extract")]
		public JExtract Extract { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        public JDownloadInfo GetDownloadInfo()
        {
            if (Downloads == null)
                Downloads = new LibrariesDownloadInfo();
            JDownloadInfo info;
            if (Natives != null)
            {
                if (Downloads.Classifiers == null)
                    Downloads.Classifiers = new Dictionary<string, JDownloadInfo>();
                if (!Downloads.Classifiers.ContainsKey(Natives["windows"]))
                    Downloads.Classifiers.Add(Natives["windows"], info = new JDownloadInfo());
                else
                    info = Downloads.Classifiers[Natives["windows"]];
            }
            else if (Downloads.Artifact == null)
                Downloads.Artifact = info = new JDownloadInfo();
            else
                info = Downloads.Artifact;
            if (string.IsNullOrWhiteSpace(info.Path))
            {
                info.Path = FormatName();
                if (info.Path == null)
                    return null;
            }
            info.forgeUrl = Url;
            return info;
        }

        public string FormatName()
        {
            string[] s = Name.Split(':');
            if (s.Length < 3)
                return null;
            StringBuilder sb = new StringBuilder(s[0].Replace('.', '\\')).Append('\\').Append(s[1]).Append('\\').Append(s[2]).Append('\\').Append('1').Append('-').Append(s[2]);
            if (Natives != null)
                sb.Append('-').Append(FormatArch());
            return sb.Append(".jar").ToString();
        }

        public string FormatArch()
        {
            return Natives["windows"].Replace("${arch}", SystemTools.GetArch());
        }
	}

	public class JRule
	{
		[JsonPropertyName("action")]
		public string Action { get; set; }

		[JsonPropertyName("os")]
		public JOperatingSystem OS { get; set; }
	}

	public class JOperatingSystem
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}

	public class JExtract
	{
		[JsonPropertyName("exclude")]
		public List<string> Exculde { get; set; }
	}

    public class JDownload
    {
        [JsonPropertyName("client")]
        public JDownloadInfo Client { get; set; }

        [JsonPropertyName("server")]
        public JDownloadInfo Server { get; set; }
    }

    public class JDownloadInfo
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("sha1")]
        public string SHA1{ get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        public string forgeUrl { get; set; }
    }

    public class JAssetIndex : JDownloadInfo
    {
        [JsonPropertyName("totalSize")]
        public long TotalSize { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("known")]
        public bool Known { get; set; }
    }

    public class LibrariesDownloadInfo
    {
        [JsonPropertyName("classifiers")]
        public Dictionary<string, JDownloadInfo> Classifiers;

        [JsonPropertyName("artifact")]
        public JDownloadInfo Artifact { get; set; }
    }

}