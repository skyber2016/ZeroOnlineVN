using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AutoBackupCloud
{
    public class Worker : BackgroundService
    {
        private DriveService Service { get; set; }
        private readonly AppSettings AppSetting;
        private readonly ILoggerManager Logger;
        public Worker(IOptions<AppSettings> setting, ILoggerManager logger)
        {
            this.Logger = logger;
            this.AppSetting = setting.Value;
            XmlDocument log4netConfig = new XmlDocument();

            using (var fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"))
            {
                log4netConfig.Load(fs);

                var repo = LogManager.CreateRepository(
                        Assembly.GetEntryAssembly(),
                        typeof(log4net.Repository.Hierarchy.Hierarchy));

                XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    this.Service = this.GetService();
                    Logger.Info($"Worker running at: {DateTimeOffset.Now}");
                    var newPath = this.BackupFile();
                    Logger.Info("newPath: " + newPath);
                    if(newPath == null)
                    {
                        throw new Exception("Khong tim thay thu muc");
                    }
                    var zipPath = this.CreateZip(newPath);
                    Logger.Info("new zip: " + zipPath);
                    this.UploadDrive(zipPath, new FileInfo(zipPath).Name);
                    this.RemoveFolder(newPath);
                    this.RemoveFile(zipPath);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
                await Task.Delay(this.AppSetting.Timer, stoppingToken);
            }
        }
        private string[] Scopes = { DriveService.Scope.DriveFile, DriveService.Scope.Drive, DriveService.Scope.DriveReadonly  };
        private string ApplicationName = "Drive API .NET Quickstart";
        private void RemoveFile(string path)
        {
            if (File.Exists(path))
            {
                Logger.Info("Remove file: " + path);
                File.Delete(path);
            }
        }
        private void RemoveFolder(string path)
        {
            if (Directory.Exists(path))
            {
                Logger.Info("Remove folder: " + path);
                Directory.Delete(path, true);
            }
        }
        private DriveService GetService()
        {
            if(this.Service == null)
            {
                UserCredential credential;
                Logger.Info(AppDomain.CurrentDomain.BaseDirectory);
                using (var stream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Logger.Info("Credential file saved to: " + credPath);
                }

                // Create Drive API service.
                this.Service =  new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
            return this.Service;
            
        }

        private string BackupFile()
        {
            try
            {
                Logger.Info("Begin backup file");
                var path = this.AppSetting.PathSql;
                if (!Directory.Exists(path))
                {
                    return null;
                }
                var newPath = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                Directory.CreateDirectory(newPath);
                foreach (var file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    var combine = Path.Combine(newPath, Path.GetFileName(file));
                    File.Copy(file, combine, true);
                }
                return newPath;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
            finally
            {
                Logger.Info("End backup file");
            }

        }

        private string CreateZip(string path)
        {
            try
            {
                string zipPath = path + ".zip";
                Logger.Info("Begin Create ZIP");
                ZipFile.CreateFromDirectory(path, zipPath, CompressionLevel.Optimal, false);
                return zipPath;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
            finally
            {
                Logger.Info("End Create ZIP");
            }
            
        }

        private void UploadDrive(string path, string fileName)
        {
            try
            {
                Logger.Info("Begin upload drive");
                this.CleanDrive();
                var driveService = this.Service;
                var folderId = "root";
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    // Tên file sẽ lưu trên Google Drive
                    Name = fileName,

                    // Thư mục chưa file
                    Parents = new List<string>
                {
                    folderId
                }
                };

                FilesResource.CreateMediaUpload request;
                using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
                {
                    request = driveService.Files.Create(fileMetadata, stream, "application/zip");

                    // Cấu hình thông tin lấy về là ID
                    request.Fields = "id";
                    // thực hiện Upload
                    request.Upload();
                }

                // Trả về thông tin file đã được upload lên Google Drive
                var file = request.ResponseBody;
                Logger.Info(file.Id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
            finally
            {
                Logger.Info("End upload drive");
            }


        }


        private void CleanDrive()
        {
            try
            {
                Logger.Info("Begin clean drive");
                FilesResource.ListRequest listRequest = this.Service.Files.List();
                listRequest.PageSize = 10;
                listRequest.OrderBy = "modifiedTime";
                listRequest.Fields = "nextPageToken, files(id, name, modifiedTime)";

                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                    .Files.Where(x => {
                        if (!x.Name.EndsWith(".zip"))
                        {
                            return false;
                        }
                        var time = (DateTime.Now - x.ModifiedTime.Value).TotalDays > this.AppSetting.Days;
                        return time;
                    })
                    .ToList()
                    ;
                if (files.Any())
                {
                    Logger.Info($"Clear {files.Count} files");
                }
                foreach (var item in files)
                {
                    var deleteRequest = this.Service.Files.Delete(item.Id);
                    deleteRequest.Execute();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
            finally
            {
                Logger.Info("End clean file");

            }

        }
    }
}
