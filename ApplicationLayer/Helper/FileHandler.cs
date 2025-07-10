
using Microsoft.AspNetCore.Http;


namespace ApplicationLayer.Helper
{
    public static class FileHandler
    {
        public static async Task<string?> SaveFileAsync(string FolderName, IFormFile? File)
        {

            if (File is null || File.Length == 0)
            {
                return null;
            }
            
            var FileName = FileNameConstructor(File.FileName);
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FolderName);

            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);

            FilePath = Path.Combine(FilePath, FileName);



            using (var FS = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
            {
                await File.CopyToAsync(FS);
            }

            return $"{FolderName}/{FileName}";
        }

        public static bool DeleteFile(string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                return false;
            }

            var FullFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FilePath);

            if (File.Exists(FullFilePath))
            {
                try
                {
                    string TempFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{Guid.NewGuid()}_{Path.GetFileName(FilePath)}");
                    File.Move(FullFilePath, TempFilePath);
                    File.Delete(TempFilePath);
                    return true;
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error deleting File: {ex.Message}");
                    return false;
                }
            }

            else { return false; }
        }

        private static string FileNameConstructor(string FileName)
            => $"{DateTime.UtcNow.ToString("ddMMyy")}-{Guid.NewGuid()}-{FileName}";
    }
}
