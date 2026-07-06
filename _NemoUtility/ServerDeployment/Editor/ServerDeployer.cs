using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace NemoUtility.ServerDeployment
{
    public class ServerDeployer : EditorWindow
    {
        private const string CONFIG_FILENAME = "server_deploy_config.json";

        [Serializable]
        public class DeployConfig
        {
            public string keyPath;
            public string localBuildPath;
            public string localZipPath;
            public string remoteUser;
            public string remoteHost;
            public string remoteDest;
            public string dockerImageName;
            public string remoteZipName;
            public string remoteFolderName;
            public string remoteExecutable;
        }

        private DeployConfig config = new DeployConfig();
        private string statusMessage = "Hazır";
        private float progress = 0f;
        private string logOutput = "";
        private bool isDeploying = false;
        private Vector2 scrollPos;
        private bool showSettings = true;

        [MenuItem("NemoUtility/Server/Deployment Panel")]
        public static void ShowWindow()
        {
            GetWindow<ServerDeployer>("Server Deployer");
        }

        private void OnEnable()
        {
            LoadConfig();
        }

        private void OnGUI()
        {
            GUILayout.Label("Sunucu Dağıtım Paneli", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            showSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showSettings, "Sunucu Ayarları");
            if (showSettings)
            {
                EditorGUI.indentLevel++;
                config.remoteHost = EditorGUILayout.TextField("Sunucu IP (Host)", config.remoteHost);
                config.remoteUser = EditorGUILayout.TextField("Kullanıcı (User)", config.remoteUser);
                config.keyPath = EditorGUILayout.TextField("SSH Key Yolu", config.keyPath);

                EditorGUILayout.Space();
                config.localBuildPath = EditorGUILayout.TextField("Build Klasör Yolu", config.localBuildPath);
                if (GUILayout.Button("Build Klasörü Seç", GUILayout.Width(150)))
                {
                    string path = EditorUtility.OpenFolderPanel("Server Build Klasörü", "", "");
                    if (!string.IsNullOrEmpty(path)) config.localBuildPath = path;
                }

                config.localZipPath = EditorGUILayout.TextField("Yerel Zip Yolu", config.localZipPath);
                config.remoteDest = EditorGUILayout.TextField("Sunucu Hedef Klasör", config.remoteDest);

                EditorGUILayout.Space();
                config.dockerImageName = EditorGUILayout.TextField("Docker Imaj Adı", config.dockerImageName);
                config.remoteZipName = EditorGUILayout.TextField("Sunucu Zip Adı", config.remoteZipName);
                config.remoteFolderName = EditorGUILayout.TextField("Sunucu Klasör Adı", config.remoteFolderName);
                config.remoteExecutable = EditorGUILayout.TextField("Çalıştırılabilir Dosya", config.remoteExecutable);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (EditorGUI.EndChangeCheck())
            {
                SaveConfig();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space();

            // --- DURUM ---
            GUILayout.Label("Dağıtım Durumu", EditorStyles.boldLabel);
            Rect r = EditorGUILayout.GetControlRect(false, 25);
            EditorGUI.ProgressBar(r, progress, statusMessage);
            EditorGUILayout.Space();

            GUILayout.Label("İşlem Logları:", EditorStyles.miniLabel);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));
            EditorGUILayout.TextArea(logOutput, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            GUI.enabled = !isDeploying;
            if (GUILayout.Button("Dağıtımı Başlat (Deploy)", GUILayout.Height(40)))
            {
                StartDeploy();
            }
            GUI.enabled = true;
        }

        private void LoadConfig()
        {
            string configPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, CONFIG_FILENAME);
            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                config = JsonUtility.FromJson<DeployConfig>(json);
            }
            else
            {
                statusMessage = "Config dosyası bulunamadı!";
            }
        }

        private void SaveConfig()
        {
            string configPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, CONFIG_FILENAME);
            string json = JsonUtility.ToJson(config, true);
            File.WriteAllText(configPath, json);
        }

        // ==============================================================
        // Deploy akışı — AutoLoadBuildServer.py ile BİREBİR AYNI mantık
        // ==============================================================
        private async void StartDeploy()
        {
            isDeploying = true;
            progress = 0f;
            logOutput = "";
            statusMessage = "İşlem başlatılıyor...";
            Repaint();

            try
            {
                // 0. Zip — Build klasörünü ziplerken klasör adını koruyoruz
                //    Python'daki zip'te "HarryPotter_LinuxServer/" klasörü zip'in içinde.
                //    PowerShell Compress-Archive ile aynısını yapıyoruz.
                statusMessage = "Build klasörü zipleniyor...";
                progress = 0.05f;
                Repaint();

                if (!await ZipBuild())
                {
                    statusMessage = "❌ Zipleme Başarısız!";
                    isDeploying = false;
                    Repaint();
                    return;
                }

                // 1. SCP — Birebir Python'daki gibi
                //    scp -i "KEY" "LOCAL_FILE" user@host:/home/ubuntu/
                statusMessage = "Dosya yükleniyor (SCP)...";
                progress = 0.2f;
                Repaint();

                string zipFullPath = config.localZipPath;
                if (!Path.IsPathRooted(zipFullPath))
                    zipFullPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, zipFullPath);

                string scpArgs = $"-i \"{config.keyPath}\" \"{zipFullPath}\" {config.remoteUser}@{config.remoteHost}:{config.remoteDest}";

                logOutput += $"\n[SCP] {zipFullPath} -> {config.remoteDest}";
                bool scpOk = await RunCommandAsync("scp", scpArgs);

                if (!scpOk)
                {
                    statusMessage = "❌ SCP Hatası!";
                    isDeploying = false;
                    Repaint();
                    return;
                }

                // 2. SSH — Komutları sırayla çalıştır (Windows escaping sorunlarını önler)
                statusMessage = "Sunucu işlemleri yürütülüyor (SSH)...";
                progress = 0.5f;
                Repaint();

                string dest = config.remoteDest;
                if (!dest.EndsWith("/")) dest += "/";

                string sshBase = $"-i \"{config.keyPath}\" {config.remoteUser}@{config.remoteHost}";

                // 2a. Unzip
                logOutput += "\n[SSH] Unzip başlatılıyor...";
                statusMessage = "Unzip yapılıyor...";
                Repaint();
                if (!await RunCommandAsync("ssh", $"{sshBase} \"unzip -o {dest}{config.remoteZipName} || true\""))
                { statusMessage = "❌ Unzip Hatası!"; isDeploying = false; Repaint(); return; }

                // 2b. Chmod
                logOutput += "\n[SSH] Chmod ayarlanıyor...";
                if (!await RunCommandAsync("ssh", $"{sshBase} \"chmod +x {dest}{config.remoteFolderName}/{config.remoteExecutable}\""))
                { statusMessage = "❌ Chmod Hatası!"; isDeploying = false; Repaint(); return; }

                // 2c. Docker build
                logOutput += "\n[SSH] Docker build başlatılıyor...";
                statusMessage = "Docker image oluşturuluyor...";
                progress = 0.7f;
                Repaint();
                bool sshOk = await RunCommandAsync("ssh", $"{sshBase} \"cd {dest}{config.remoteFolderName} && docker build -t {config.dockerImageName} .\"");

                if (sshOk)
                {
                    progress = 1f;
                    statusMessage = "✅ Başarıyla Tamamlandı!";
                }
                else
                {
                    statusMessage = "❌ SSH Hatası!";
                }
            }
            catch (Exception e)
            {
                statusMessage = "❌ Beklenmedik Hata!";
                logOutput += $"\nHata: {e.Message}";
            }

            isDeploying = false;
            Repaint();
        }

        private async Task<bool> ZipBuild()
        {
            if (string.IsNullOrEmpty(config.localBuildPath) || !Directory.Exists(config.localBuildPath))
            {
                logOutput += "\n[Error] Build klasörü bulunamadı!";
                return false;
            }

            // Eski zipi sil
            string zipFullPath = config.localZipPath;
            if (!Path.IsPathRooted(zipFullPath))
                zipFullPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, zipFullPath);

            if (File.Exists(zipFullPath)) File.Delete(zipFullPath);

            // PowerShell ile build KLASÖRÜNÜ (adıyla birlikte) ziple
            // Python'daki zip'te "HarryPotter_LinuxServer/" kök klasör olarak var
            // Compress-Archive -Path 'C:/.../HarryPotter_LinuxServer' bunu otomatik yapar
            string psCmd = $"Compress-Archive -Path '{config.localBuildPath}' -DestinationPath '{zipFullPath}' -Force";
            logOutput += $"\n[ZIP] {config.localBuildPath} -> {zipFullPath}";
            return await RunCommandAsync("powershell", $"-Command \"{psCmd}\"");
        }

        private Task<bool> RunCommandAsync(string fileName, string args)
        {
            var tcs = new TaskCompletionSource<bool>();

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            Process process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    lock (this) { logOutput += $"\n[OUT] {e.Data}"; }
                }
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    lock (this) { logOutput += $"\n[ERR] {e.Data}"; }
                }
            };

            process.Exited += (sender, e) =>
            {
                tcs.SetResult(process.ExitCode == 0);
                process.Dispose();
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;
        }

        private void Update()
        {
            if (isDeploying) Repaint();
        }
    }
}
