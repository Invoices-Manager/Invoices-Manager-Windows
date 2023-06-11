using ProgressBar = System.Windows.Controls.ProgressBar;

namespace InvoicesManager.Core
{
    public class BackUpSystem
    {
        private BackUpView _backUpMgrView;

        public void CheckBackUpCount()
        {
            
        }

        public bool BackUp(string backupFilePath, BackUpView backUpMgr = null)
        {
            return false;
        }

        public bool Restore(string backupFilePath, BackUpView backUpMgr = null)
        {
            return false;
        }

        public bool SaveAs(string backupFilePath, string newPath)
        {
            return false;
        }

        //public async IAsyncEnumerable<BackUpInfoModel> GetBackUps()
        //{
        //    return null;
        //}

        private BackUpInfoModel GetBackUpMetaData(string file)
        {
            return null;
        }

        public static string GetFileSize(string path)
        {
            return "";
        }

        public bool Delete(string backUpPath)
        {
            return false;
        }
    }
}
