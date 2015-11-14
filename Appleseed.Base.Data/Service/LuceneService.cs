using System;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Store;

namespace Appleseed.Base.Data.Service
{
    public static class LuceneService
    {
        public static readonly string LuceneDir =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lucene_index");

        private static FSDirectory _directoryTemp;

        public static FSDirectory Directory
        {
            get
            {
                if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(LuceneDir));
                if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                var lockFilePath = Path.Combine(LuceneDir, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }
    }
}