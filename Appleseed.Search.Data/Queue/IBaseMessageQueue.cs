using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace GA.Data
{
	/// <summary>
	/// I base queue. A basic queue implementiation powered by a repository instead of an array. 
	/// Using a repository, the the data can be stored in EntityFramework, RavenDB, MongoDB, InMemory, XML
	/// Ref: http://www.functionx.com/csharp/queues/introduction.htm
	/// It will use transactions if the repository itself doesn't support it 
	/// Ref: https://msdn.microsoft.com/en-us/library/vstudio/bb738523%28v=vs.100%29.aspx
	/// It won't be an Azure Queue or a Azure Service Bus but it should be easily refactorable to that
	/// Ref: https://msdn.microsoft.com/en-us/magazine/jj159884.aspx
	/// Good example of tool usig CloudQueue service
	/// Ref: http://www.asp.net/aspnet/overview/developing-apps-with-windows-azure/building-real-world-cloud-apps-with-windows-azure/queue-centric-work-pattern
	/// Good example of a Perl Queue processor in Digg called Muuttaa
	/// Ref: https://github.com/activeingredient/muuttaa/blob/master/Muuttaa/Process.php
	/// Another good example is called SSMQ - written in PHP 
	/// Ref: http://stackoverflow.com/questions/530211/creating-a-blocking-queuet-in-net
	/// Good example of creating a blocking queue for multi threading 
	/// </summary>
	public interface IBaseMessageQueue<T>
	{
		/*Goals 
		1. Create a simple Message queue in .NET backed by SQL or MongoDB or RavenDB 
			1.1. Choose a simple SQL wrapper that will make it easy to retreive, update, and insert collection items
				- Peta Poco - seems like best candidate - has good documentation - one file 
					- may be over kill 
				- Dapper - seems way faster - has multiple insert / batch insert from collection - i like this - one file
					- may be over kill 
				- ServiceStack.OrmLite - fast - may be the best tool for the job because it is simple 
					- may be the best for the job 

			Why: in the case that a message queue may take up too much memory / man power to maintain
			Why: we have legacy cases in which SQL is being used. 
			Why: SQL based queues are easier to manage and easy to insert into 
			Why: eventually in MongoDB or RavenDB because both are really good DBs also 
		*/

		//int Count { get; }
		//bool IsEmpty { get; }
		void Enqueue(T item);
		void Enqueue(List<T> items);
		T Dequeue();
		T Peek();
		//bool Contains(T item);

		//void Clear();
		IEnumerator<T> GetEnumerator();


	}

}
