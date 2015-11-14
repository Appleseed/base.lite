using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Appleseed.Base.Data.Repository;
using Appleseed.Base.Data.Repository.Contracts;
using Appleseed.Base.Data.Utility;
using NLog;

namespace Appleseed.Base.Data.Service
{
    public static class RepositoryService
    {

        public static IRepository GetRepository()
        {
            var connectionString = ConfigurationManager.AppSettings["Database_CollectionItemQueue"].ToString();
            var dbConnectionConfig = ConfigurationManager.AppSettings["Database_Connection"].ToString();
            var repository = GetConditionalRepository(connectionString, dbConnectionConfig);
            return repository;
        }

        public static IRepository GetRepository(string connectionString, string databaseType)
        {
            var repository = GetConditionalRepository(connectionString, databaseType);
            return repository;
        }

        private static IRepository GetConditionalRepository(string connectionString, string dbConnectionConfig)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                Logger log = LogManager.GetCurrentClassLogger();

                if (dbConnectionConfig == Constants.MySql)
                {
                    return new MySqlRepository(connectionString, log);
                }
                else if (dbConnectionConfig == Constants.SqlServer)
                {
                    return new SqlRepository(connectionString, log);
                }
                else if (dbConnectionConfig == Constants.MongoDb)
                {
                    return null;
                }
                else if (dbConnectionConfig == Constants.Lucene)
                {
                    if (Directory.Exists(LuceneService.LuceneDir))
                    {
                        Directory.Delete(LuceneService.LuceneDir, true);
                    }

                    return new LuceneRepository();
                }

                else if(dbConnectionConfig == Constants.InMemory)
                {
                    return new InMemoryRepository();
                }
            }

            return null;
        }

        public static List<IRepository> GetAllRepositories(QueueSection queueSection)
        {
            var repositorys = new List<IRepository>();

            foreach (var queue in queueSection.Queue)
            {
                repositorys.Add(GetRepository(queue.ConnectionString, queue.QueueName));
            }

            return repositorys;
        }  
    }
}
