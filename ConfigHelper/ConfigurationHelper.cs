using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace ConfigHelper
{
    public class ConfigurationHelper<T> where T : class, new()
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            MaxDepth = 128,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

        private readonly string ConfigFullPath;
        private readonly string ConfigDirectory;
        private FileSystemWatcher watcher;

        private readonly FileStream FileStream;

        private static readonly SemaphoreSlim Semaphore = new(1);

        private readonly ILogger<ConfigurationHelper<T>> Logger;

        public delegate void ConfigurationChanged();
        public event ConfigurationChanged OnConfigurationChanged;

        public T Config { get; private set; }

        public ConfigurationHelper(string ConfigPath, ILoggerFactory logger)
        {
            Logger = logger?.CreateLogger<ConfigurationHelper<T>>();

            if (string.IsNullOrEmpty(ConfigPath))
            {
                throw new Exception($"{nameof(ConfigPath)} can not be null or empty.");
            }

            ConfigFullPath = Path.GetFullPath(ConfigPath);
            ConfigDirectory = Path.GetDirectoryName(ConfigPath);

            FileStream = File.Open(ConfigFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            Logger?.LogInformation($"The config file will be => {ConfigFullPath}");

            Load();

            OnConfigurationChanged += Load;

            FileWatch();
        }

        private void FileWatch()
        {
            var fileName = Path.GetFileName(ConfigFullPath);
            // Create a new FileSystemWatcher and set its properties.
            watcher = new FileSystemWatcher()
            {
                // Set the watch to look in that folder
                Path = ConfigDirectory,
                // Watch for changes (LastWrite)
                NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite,
                // Only watch config file.
                Filter = fileName,
                // Begin watching.
                EnableRaisingEvents = true,
                // ??
                InternalBufferSize = 4096 * 4,
            };

            // Add event handlers.
            watcher.Created += ChangeDetected;
            watcher.Changed += ChangeDetected;

            watcher.Error += (object sender, ErrorEventArgs e) =>
            {
                Logger.LogCritical("Error Detecting Config Changes", e.ToString());
            };

            watcher.EnableRaisingEvents = true;
        }

        private void ChangeDetected(object source, FileSystemEventArgs e)
        {
            OnConfigurationChanged();
        }

        public void Load()
        {
            if (FileStream.Length == 0)
            {
                Save();
            }

            Semaphore.Wait();

            FileStream.Position = 0;
            Config = JsonSerializer.Deserialize<T>(FileStream, JsonSerializerOptions);

            Semaphore.Release();

            Logger?.LogInformation("Configuration Read");
        }

        public void Save()
        {
            if (Config is null)
            {
                Config = new T();
            }

            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }

            Semaphore.Wait();

            FileStream.Position = 0;
            JsonSerializer.Serialize(FileStream, Config, JsonSerializerOptions);
            FileStream.Flush();
            FileStream.SetLength(FileStream.Position);

            Semaphore.Release();

            Logger?.LogInformation("Configuration Written");
        }

        public void OverwriteCurrent(T newSettings = null)
        {
            if (Config is null)
            {
                Config = new T();
            }

            Config = newSettings;
        }
    }
}
