# Appleseed Base : An open source .NET enterprise communications engine. 


# For MySQL and Sql Database you have to initialize databases first.

#SQL Server

1. Create a database named "ga"
2. Go to folder named "script" in the solution.
3. Run the scripts into visual studio management studio from the file "SqlServer_Script.sql"

#MySQL

1. Go to folder named "script" in the solution.
2. Run the scripts from the file "mySql_Script.sql"

# The application can be run in two forms.

1. Application can be run for all the Queues described in the app config file (snippet of the app.config is given below)
2. Application can be run for a single Queue.

# If you want to run for a single Queue add the queueName in the program.cs file.
# If you want to run all the Queues you have to omit queueName from the program.cs .

#App.config snippet . Only the needed configuration are shown here. Add the below section before the ending </configuration> tag
  
  <queue>
    <queue name="SqlServerQueue" connectionString="Server=.\SQLEXPRESS; Database=ga;Uid=sa;Pwd=sa123;" queueName="SqlServer" type="SQL" />
    <queue name="MySqlQueue" connectionString="Server=localhost;Database=ga;Uid=sa;Pwd=sa123;" queueName="MySql" type="MySql" />    
    <queue name="InMemoryQueue" connectionString="" queueName="InMemory" type="InMemory" />
    <queue name="LuceneQueue" connectionString="" queueName="Lucene" type="Lucene" />
  </queue>
  
</configuration>
