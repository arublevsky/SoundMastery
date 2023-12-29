namespace SoundMastery.Application;

public static class Constants
{
    public static class Roles
    {
        public const string Teacher = "teacher";
        public const string Student = "student";
    }

    public static class FileStorage
    {
        public const string AzureConnectionStringName = "AzureStorageAccount";
        public const string AzureBlobContainerName = "soundmastery";
        public const string LocalUploadsDirectory = "uploads";
    }
}