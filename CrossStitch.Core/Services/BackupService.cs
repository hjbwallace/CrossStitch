using CrossStitch.Core.Interfaces;
using System.IO;

namespace CrossStitch.Core.Services
{
    public class BackupService : IBackupService
    {
        private readonly IDatabaseContextService _contextFactory;
        private readonly ICurrentDateService _dateTimeProvider;

        public BackupService(IDatabaseContextService contextFactory, ICurrentDateService dateTimeProvider)
        {
            _contextFactory = contextFactory;
            _dateTimeProvider = dateTimeProvider;
        }

        public void BackupDatabase()
        {
            using (var ctx = _contextFactory.GetContext().Invoke())
            {
                var databaseFilePath = Directory.GetCurrentDirectory() + ctx.DatabasePath;

                var backupDir = $@"C:\Backup\CrossStitch\";
                var backupPath = $@"{backupDir}CrossStitchDatabase_{_dateTimeProvider.Now().ToString("yyyyMMddhhmmss")}.db";

                Directory.CreateDirectory(backupDir);
                File.Copy(databaseFilePath, backupPath);
            }
        }
    }
}