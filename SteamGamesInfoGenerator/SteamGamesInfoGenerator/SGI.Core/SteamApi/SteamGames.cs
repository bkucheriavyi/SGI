namespace SGI.Core.SteamApi
{
    public class SteamGames
    {
        public AppList AppList { get; set; }
        
    }

    public class AppList
    {
        public Apps[] Apps { get; set; }
    }

    public class Apps
    {
        public int AppId { get; set; }
        public string Name { get; set; }
    }
}
