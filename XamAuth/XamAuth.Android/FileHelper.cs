namespace XamAuth.Droid
{
    public static class FileHelper
    {
        public static string GetLocalStoragePath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }
    }
}