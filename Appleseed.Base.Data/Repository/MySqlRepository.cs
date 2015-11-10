using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Appleseed.Base.Data.Model;
using Appleseed.Base.Data.Repository.Contracts;
using Appleseed.Base.Data.Utility;
using MySql.Data.MySqlClient;
using NLog;

namespace Appleseed.Base.Data.Repository
{
    public class MySqlRepository : IMySqlRepository, IDisposable
    {
        private MySqlConnection MySqlClient;

        public Logger Log
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public string ConnectionString
        {
            get;
            private set;
        }

        public MySqlRepository(string incomingConnectionString, Logger logger)
        {
            this.MySqlClient = new MySqlConnection(incomingConnectionString);
            this.ConnectionString = incomingConnectionString;
            this.Log = logger;
        }
        public void Insert(BaseCollectionItem baseCollectionItem)
        {
            baseCollectionItem.Id = Guid.NewGuid();
            baseCollectionItem.CreatedDate = DateTime.Now;
            ////var data = baseCollectionItem.Data.SerializeToByteArray();
            this.MySqlClient.Open();
            var query = @"Insert into basecollectionitem (Id, Data, ItemProcessed, CreatedDate)
                        values(@ItemId, @ItemData, @ItemProcessed, @CreatedDate)";
            var command = new MySqlCommand(query, this.MySqlClient);

            command.Parameters.AddWithValue("@ItemId", baseCollectionItem.Id);
            command.Parameters.AddWithValue("@ItemData", baseCollectionItem.Data.SerializeToByteArray());
            command.Parameters.AddWithValue("@ItemProcessed", baseCollectionItem.ItemProcessed);
            command.Parameters.AddWithValue("@CreatedDate", baseCollectionItem.CreatedDate);
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();

            this.MySqlClient.Close();
        }

        public void Update(BaseCollectionItem baseCollectionItem)
        {
            this.MySqlClient.Open();
            var query = @"update basecollectionitem 
                           set ItemProcessed=@ItemProcessed where Id=@Id;";
            var command = new MySqlCommand(query, this.MySqlClient);
            command.Parameters.AddWithValue("@ItemProcessed", baseCollectionItem.ItemProcessed);
            command.Parameters.AddWithValue("@Id", baseCollectionItem.Id);
            command.CommandType = CommandType.Text;
            var affectedRows =command.ExecuteNonQuery();

            this.MySqlClient.Close();
        }

        public void DeleteProcessedItems()
        {
            this.MySqlClient.Open();
            var query = @"Delete From basecollectionItem where ItemProcessed = 'true'";
            var command = new MySqlCommand(query, this.MySqlClient);
            command.ExecuteNonQuery();
            this.MySqlClient.Close();
        }

        public List<BaseCollectionItem> GetAllFromQueue()
        {
            //DONE: implement method to get batch repository items 
            // gets unprocessed items 

            this.MySqlClient.Open();
            Log.Info("Connected to the database");

            ////DONE: Select data from table in MYSQL Successfully and get it back in .NET
            ////TODO: filter if processed or not?
            MySqlCommand myCommand = new MySqlCommand("SELECT * FROM collectionitemqueue", this.MySqlClient);
            MySqlDataReader myReader = myCommand.ExecuteReader();

            Log.Info("Retreiving Items from Queue");
            //DONE: enumerate through data , create collection to return ;

            List<BaseCollectionItem> itemCollection = new List<BaseCollectionItem>();

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
            this.MySqlClient.Close();
            return itemCollection;
        }

        public BaseCollectionItem GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BaseCollectionItem> GetUnProcessedBaseCollectionItems()
        {
            this.MySqlClient.Open();
            Log.Info("Connected to the database");

            ////DONE: Select data from table in MYSQL Successfully and get it back in .NET
            ////TODO: filter if processed or not?
            MySqlCommand myCommand = new MySqlCommand("SELECT * FROM basecollectionitem where ItemProcessed='false'", this.MySqlClient);
            MySqlDataReader myReader = myCommand.ExecuteReader();

            Log.Info("Retreiving Items from Queue");
            //DONE: enumerate through data , create collection to return ;

            List<BaseCollectionItem> itemCollection = new List<BaseCollectionItem>();
            if (myReader.HasRows)
            {
                Log.Info("This Collection has Items");
                while (myReader.Read())
                {
                    Log.Info(myReader.GetInt32(0) + ":" + myReader.GetBoolean(3));

                    //DONE: convert list of tags separated by commas into a List of strings
                    ////List<string> itemTags = new List<string>(myReader.GetString(4).Split(','));

                    var binaryData = (byte[])myReader["Data"];

                    itemCollection.Add(new BaseCollectionItem
                    {
                        TableId = myReader.GetInt32(0),
                        Id = myReader.GetGuid(1),
                        Data = (BaseCollectionItemData)binaryData.DeSerializeToObject(),
                        ItemProcessed = myReader.GetBoolean(3),
                        CreatedDate = myReader.GetDateTime(4)
                    });
                }
            }
            // close out stuff 
            myReader.Close();
            MySqlClient.Close();
            return itemCollection.AsQueryable();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
