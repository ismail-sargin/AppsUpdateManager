using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using Formatting = Newtonsoft.Json.Formatting;
namespace AnexAppsUpdateManager.Win
{
    public partial class FrmLauncher : DevExpress.XtraEditors.XtraForm
    {
        private readonly string _launcherSettingJsonPath = @"";
        private readonly string _launcherDownloadVersionPath = "DownloadVersion";
        private readonly string _launcherRunName = "test.exe";
        private readonly string _launcherSettingsJsonFileName = "settings.json";
        private readonly string _launcherCurrentVersionPath = "LocalVersion";
        private readonly string _appsVersionUrl = "https://test.com/text.txt";
        private readonly string _appsDownloadBaseUrl = "https://apps.com/test/update";
        private readonly string _appsTargetToCopyFolder = @"C:\Users\Desktop\Apps";
        private readonly string _appsRunPathFileName = @"Example.exe";
        public FrmLauncher()
        {
            InitializeComponent();
            this.Opacity = 0;
            this.Size = new Size(1, 1);
            Load += FrmLauncher_Load;
        }
        private async void FrmLauncher_Load(object sender, EventArgs e)
        {
            CheckVersionForLauncher();
            await CheckVersionsForApps();
        }
        private void CheckVersionForLauncher()
        {
            string launcherCurrentVersionPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), _launcherCurrentVersionPath);
            string downloadVersionPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), _launcherDownloadVersionPath);
            string launcherRunPath = Path.Combine(launcherCurrentVersionPath, _launcherRunName);
            string localVersion = ReadLocalVersion(launcherCurrentVersionPath);
            string downloadVersion = ReadLocalVersion(downloadVersionPath);
            if (localVersion != downloadVersion)
            {
                CopyFiles(downloadVersionPath, launcherCurrentVersionPath);
                DeleteDirectory(launcherCurrentVersionPath);
                toastNotificationsManager1.ShowNotification(toastNotificationsManager1.Notifications[1]);
                RunLauncherExecutable(launcherRunPath);
            }
            else
            {
                //toastNotificationsManager1.ShowNotification(toastNotificationsManager1.Notifications[0]);
            }
        }
        private async Task CheckVersionsForApps()
        {
            var setting = ReadSettings();
            var appsVersionUrl = await GetContentFromUrl(_appsVersionUrl);
            string appsDownloadUrl = $" {_appsDownloadBaseUrl}/{appsVersionUrl}.zip";
            string appsTargetToCopyFolder = _appsTargetToCopyFolder;
            string appsZipFilePath = Path.Combine(appsTargetToCopyFolder, $"{appsVersionUrl}.zip");
            string extractAppsTargetFolderPath = Path.Combine(appsTargetToCopyFolder, appsVersionUrl);
            string appsRunFilePath = Path.Combine(extractAppsTargetFolderPath, _appsRunPathFileName);
            if (appsVersionUrl == setting.AppsVersion)
            {
                this.Hide();
                RunExecutable(appsRunFilePath);
            }
            else
            {
                this.Hide();
                FluentSplashScreenOptions op = SetSplashOptions();
                SplashScreenManager.ShowFluentSplashScreen(
                    op,
                    parentForm: this,
                    useFadeIn: true,
                    useFadeOut: true
                );
                await Task.Delay(5000);
                await DownloadFileAsync(appsDownloadUrl, appsTargetToCopyFolder);
                ExtractZipFile(appsZipFilePath, extractAppsTargetFolderPath);
                SplashScreenManager.CloseForm();
                RunExecutable(appsRunFilePath);
                UpdateSettings(appsVersionUrl, setting);
                File.Delete(appsZipFilePath);
            }
        }
        private  FluentSplashScreenOptions SetSplashOptions()
        {
            FluentSplashScreenOptions op = new FluentSplashScreenOptions();
            op.Opacity = 130;
            op.OpacityColor = Color.FromArgb(64, 64, 65);
            op.Title = "New Version Loading Please Wait..." + Environment.NewLine;
            op.AppearanceTitle.ForeColor = Color.FromArgb(246, 237, 227);
            op.AppearanceTitle.Font = new Font("Oswald", 10, FontStyle.Regular);
            op.AppearanceTitle.FontSizeDelta = 10;
            op.Subtitle = Environment.NewLine + "The application will be launched automatically when the installation is complete ";
            op.AppearanceSubtitle.Font = new Font("Lato", 10, FontStyle.Regular);
            op.AppearanceSubtitle.ForeColor = Color.FromArgb(246, 237, 227);
            op.RightFooter = $"Version 2.1.23.122";
            op.AppearanceRightFooter.ForeColor = Color.FromArgb(211, 202, 226);
            op.LoadingIndicatorType = FluentLoadingIndicatorType.Ring;
            op.LeftFooter = "Copyright ©  2024 Anex" + Environment.NewLine + "All Rights reserved.";
            op.AppearanceLeftFooter.ForeColor = Color.FromArgb(211, 202, 226);
            return op;
        }
        void UpdateSettings(string newVersion, RootObject root)
        {
            root.AppsVersion = newVersion;
            string updatedJson = JsonConvert.SerializeObject(root, Formatting.Indented);
            File.WriteAllText(_launcherSettingJsonPath, updatedJson);
        }
        private void ExtractZipFile(string zipFilePath, string extractFolderPath)
        {
            
            if (!Directory.Exists(extractFolderPath))
            {
                Directory.CreateDirectory(extractFolderPath);
            }
            else
            {
                string existingFilePath = Path.Combine(extractFolderPath);
                if (Directory.Exists(existingFilePath))
                {
                    Directory.Delete(existingFilePath, true);
                }
            }
            ZipFile.ExtractToDirectory(zipFilePath, extractFolderPath);
        }
        private void RunExecutable(string exeFilePath)
        {
            if (File.Exists(exeFilePath))
            {
                toastNotificationsManager1.ShowNotification(toastNotificationsManager1.Notifications[2]);
                Process.Start(exeFilePath);
                this.Close();
            }
            else
            {
                XtraMessageBox.Show("Belirtilen exe dosyası bulunamadı.");
            }
        }
        private async Task DownloadFileAsync(string url, string targetPath)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                byte[] fileBytes = await httpClient.GetByteArrayAsync(url);
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                string fileName = Path.GetFileName(url);
                string filePath = Path.Combine(targetPath, fileName);
                File.WriteAllBytes(filePath, fileBytes);
            }
        }
        static async Task<string> GetContentFromUrl(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync(url);
            }
        }
        private RootObject ReadSettings()
        {
            var launcherVersion = new RootObject();
            using (StreamReader r = new StreamReader(_launcherSettingsJsonFileName))
            {
                string json = r.ReadToEnd();
                launcherVersion = JsonConvert.DeserializeObject<RootObject>(json);
            }
            return launcherVersion;
        }
        static string ReadLocalVersion(string folderPath)
        {
            string configPath = Path.Combine(folderPath, "Debug", "app.config");
            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = configPath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            return config.AppSettings.Settings["CurrentVersion"].Value;
        }
        static void CopyFiles(string sourcePath, string destinationPath)
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
        }
        static void DeleteDirectory(string targetDir)
        {
            string debugPath = Path.Combine(targetDir, "bin", "Debug");
            if (Directory.Exists(debugPath))
            {
                foreach (string file in Directory.GetFiles(debugPath, "*", SearchOption.AllDirectories))
                    File.Delete(file);
                Directory.Delete(debugPath, true);
            }
        }
        static void RunLauncherExecutable(string exePath)
        {
            try
            {
                Process.Start(exePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateManager.Launcher.exe başlatılırken bir hata oluştu: {ex.Message}");
            }
        }

    }
}
