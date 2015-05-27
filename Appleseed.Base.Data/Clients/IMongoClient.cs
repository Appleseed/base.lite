using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoDB;

namespace GA.Data.Clients
{
	public interface IMongoClient
	{
		MongoDatabase GetDatabase(string name);
	}
}

