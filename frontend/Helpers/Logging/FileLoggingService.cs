using System.IO;
using System.Text;

namespace Lastik.Helpers.Logging
{
    public class FileLoggingService : ILoggingService
    {
        private const int LockTimeoutMilliseconds = 3000;

        private readonly string _directory;
        private readonly ReaderWriterLock _readerWriterLock;
        
        public FileLoggingService(string directory)
        {
            _directory = directory;
            _readerWriterLock = new ReaderWriterLock();

            CheckDirectory(_directory);
        }
        
        public void Log(string message)
        {
            var path = GetFilePath();
            var entry = GetLogEntry(message);

            Task.Run(() =>
            {
                WriteLogEntry(entry, path);
            });
        }

        public void Log(Exception exception, string message = null)
        {
            if (exception is null)
                return;

            var description = GetExceptionDescription(exception, message);

            Log(description);
        }

        private string GetFilePath()
        {
            var fileName = $"log_{DateTime.Today:yyyy.MM.dd}.log";
            return Path.Combine(_directory, fileName);
        }

        private void CheckDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        private void WriteLogEntry(string entry, string path)
        {
            _readerWriterLock.AcquireWriterLock(LockTimeoutMilliseconds);

            using (var stream = File.AppendText(path))
            {
                try
                {
                    stream.WriteLine(entry);
                    stream.WriteLine();
                }
                catch (Exception ex)
                {
                    
                }
                finally
                {
                    _readerWriterLock.ReleaseWriterLock();
                }
            }
        }

        private string GetLogEntry(string message)
        {
            return $"{DateTime.Now:HH:mm:ss} - {message}";
        }

        private string GetExceptionDescription(Exception exception, string message)
        {
            var sb = new StringBuilder();

            if (!(message is null))
                sb.Append($"{message}: ");

            sb.Append(exception);

            return sb.ToString();
        }
    }
}
