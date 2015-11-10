using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Appleseed.Base.Data.Model;
using Appleseed.Base.Data.Repository.Contracts;
using Appleseed.Base.Data.Utility;
using NLog;

namespace Appleseed.Base.Data.Repository
{
    public class SqlRepository : ISqlRepository, IDisposable
    {
        private readonly SqlConnection SqlClient;

        public SqlRepository(string incomingConnectionString, Logger logger)
        {
            SqlClient = new SqlConnection(incomingConnectionString);
            ConnectionString = incomingConnectionString;
            Log = logger;
        }

        public Logger Log { get; set; }

        /// <summary>
        ///     Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public string ConnectionString { get; private set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Insert(BaseCollectionItem baseCollectionItem)
        {
            baseCollectionItem.Id = Guid.NewGuid();
            baseCollectionItem.CreatedDate = DateTime.Now;
            ////var data = baseCollectionItem.Data.SerializeToByteArray();
            SqlClient.Open();
            var query = @"Insert into dbo.basecollectionitem (Id, Data, ItemProcessed, CreatedDate)
                        values(@ItemId, @ItemData, @ItemProcessed, @CreatedDate)";
            var command = new SqlCommand(query, SqlClient);
            command.Parameters.Add("@ItemId", SqlDbType.UniqueIdentifier).Value = baseCollectionItem.Id;
            command.Parameters.Add("@ItemData", SqlDbType.VarBinary).Value =
                baseCollectionItem.Data.SerializeToByteArray();
            command.Parameters.Add("@ItemProcessed", SqlDbType.Bit).Value = baseCollectionItem.ItemProcessed;
            command.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = baseCollectionItem.CreatedDate;
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();

            SqlClient.Close();
        }

        public void Update(BaseCollectionItem baseCollectionItem)
        {
            baseCollectionItem.Id = Guid.NewGuid();
            baseCollectionItem.CreatedDate = DateTime.Now;
            ////var data = baseCollectionItem.Data.SerializeToByteArray();
            SqlClient.Open();
            var query = @"update dbo.basecollectionitem 
                           set ItemProcessed=@ItemProcessed where Id=@Id;";
            var command = new SqlCommand(query, SqlClient);
            command.Parameters.Add("@ItemProcessed", SqlDbType.Bit).Value = baseCollectionItem.ItemProcessed;
            command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = baseCollectionItem.Id;
            command.CommandType = CommandType.Text;
            var affectedRows = command.ExecuteNonQuery();
            SqlClient.Close();
        }

        public void DeleteProcessedItems()
        {
            SqlClient.Open();
            var query = @"Delete From basecollectionItem where ItemProcessed = 'true'";
            var command = new SqlCommand(query, SqlClient);
            command.ExecuteNonQuery();
            SqlClient.Close();
        }

        public List<BaseCollectionItem> GetAllFromQueue()
        {
            SqlClient.Open();
            Log.Info("Connected to the database");

            ////DONE: Select data from table in MYSQL Successfully and get it back in .NET
            ////TODO: filter if processed or not?
            var myCommand = new SqlCommand("SELECT * FROM dbo.collectionitemqueue", SqlClient);
            var myReader = myCommand.ExecuteReader();

            Log.Info("Retreiving Items from Queue");
            //DONE: enumerate through data , create collection to return ;

            var itemCollection = new List<BaseCollectionItem>();

            while (myReader.Read())
            {
                Log.Info(myReader.GetString(1) + ":" + myReader.GetString(2));

                //DONE: convert list of tags separated by commas into a List of strings
                ////List<string> itemTags = new List<string>(myReader.GetString(4).Split(','));

                var baseCollectionItemData = new BaseCollectionItemData
                {
                    ItemID = myReader.GetInt32(0),
                    ItemTitle = myReader.GetString(1),
                    ItemUrl = myReader.GetString(2),
                    ItemContent_Image = "",
                    ItemContent_Raw = "",
                    ItemDescription = myReader.GetString(3),
                    ItemTags = myReader.GetString(4),
                    ItemProcessedDate = DateTime.Today
                };

                itemCollection.Add(new BaseCollectionItem
                {
                    Id = Guid.NewGuid(),
                    Data = baseCollectionItemData
                }
                    );
            }
            // close out stuff 
            myReader.Close();
            SqlClient.Close();
            return itemCollection;
        }

        public IQueryable<BaseCollectionItem> GetUnProcessedBaseCollectionItems()
        {
            SqlClient.Open();
            Log.Info("Connected to the database");

            ////DONE: Select data from table in MYSQL Successfully and get it back in .NET
            ////TODO: filter if processed or not?
            var myCommand = new SqlCommand("SELECT * FROM dbo.basecollectionitem where ItemProcessed='false'", SqlClient);
            var myReader = myCommand.ExecuteReader();

            Log.Info("Retreiving Items from Queue");
            //DONE: enumerate through data , create collection to return ;

            var itemCollection = new List<BaseCollectionItem>();
            if (myReader.HasRows)
            {
                Log.Info("This Collection has Items");
                while (myReader.Read())
                {
                    Log.Info(myReader.GetInt32(0) + ":" + myReader.GetBoolean(3));

                    //DONE: convert list of tags separated by commas into a List of strings
                    ////List<string> itemTags = new List<string>(myReader.GetString(4).Split(','));

                    var binaryData = (byte[]) myReader["Data"];

                    itemCollection.Add(new BaseCollectionItem
                    {
                        TableId = myReader.GetInt32(0),
                        Id = myReader.GetGuid(1),
                        Data = (BaseCollectionItemData) binaryData.DeSerializeToObject(),
                        ItemProcessed = myReader.GetBoolean(3),
                        CreatedDate = myReader.GetDateTime(4)
                    });
                }
            }
            // close out stuff 
            myReader.Close();
            SqlClient.Close();
            return itemCollection.AsQueryable();
        }

        public BaseCollectionItem GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}