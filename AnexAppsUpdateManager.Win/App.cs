using System.Collections.Generic;

namespace AnexAppsUpdateManager.Win
{
    public class App
    {
        public string Name { get; set; }
    }
    public class RootObject
    {
        public List<App> Apps { get; set; }
        public string Status { get; set; }
        public string AppsVersion { get; set; }
        public string LauncherVersion { get; set; }
    }
}
