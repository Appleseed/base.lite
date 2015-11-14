# Appleseed Base : An open source .NET enterprise communications engine. 


# For MySQL and Sql Database you have to initializa databases first.

#SQL Server

1. Create a database named "ga"
2. Go to folder named "script" in the solution.
3. Run the scripts into visual studio management studio from the file "SqlServer_Script.sql"

#MySQL

1. Go to folder named "script" in the solution.
2. Run the scripts from the file "mySql_Script.sql"

# The application can be run in two forms.

1. Application can be run for all the repositories described in the app config file (snippet of the app.config is given below)
2. Application can be run for a single repository.

#App.config snippet . Only the needed configuration are shown here.

# If you want to run for a single repository add the queueName in the appSettings section for the key "SingleQueueName".
# If you want to run all the repositories you have to omit the "SingleQueueName" key from the appSettings section. Just remove/comment out it and the application will start running for all the repositories one by one.

<configuration>
  <configSections>
    <sectionGroup name="common">
      <section name="logging" type="Common.Logging.ConfigurationSectionHandler, Common.Logging" />
    </sectionGroup>
    <section name="nlog" type="NLog.Config.ConfigSectionHandler, NLog" />
   <section name="queue" type="Appleseed.Base.Data.Utility.QueueSection, Appleseed.Base.Data" />
  </configSections>
  <appSettings>
    <add key="SingleQueueName" value ="SqlServer"/>
    <add key="Database_CollectionItemQueue" value="Server=localhost;Database=ga;Uid=sa;Pwd=sa123;" />
    <add key="Database_Connection" value="Lucene" />
    <!--<add key="Database_Connection" value="SqlServer"/>-->
    <add key="Database_CollectionItemRepository" value="mongo://localhost:27017" />
    <add key="Service_ScraperIO_Key" value="waPkNsJ7085HkmGyb8hmMRnDBJBTfy9A" />
    <add key="Service_ScraperIO_Secret" value="DUgQxtFy4S8AMGfbMuRd7p6T0wgnm0ar" />
  </appSettings>
  <nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" autoReload="true" throwExceptions="false">
    <extensions>
      <add assembly="NLog.SignalR" />
    </extensions>
    <targets async="true">
      <target name="logfile" xsi:type="File" fileName="logfile.txt" />
      <target xsi:type="SignalR" name="signalr" uri="http://localhost:8081" hubName="LoggingHub" methodName="Log" layout="${message}" />
    </targets>
    <rules>
      <logger name="*" minlevel="Info" writeTo="logfile" />
      <logger name="*" minlevel="Info" writeTo="signalr" />
    </rules>
  </nlog>
  <common>
    <logging>
      <factoryAdapter type="Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter, Common.Logging">
        <arg key="level" value="INFO" />
        <arg key="showLogName" value="true" />
        <arg key="showDateTime" value="true" />
        <arg key="dateTimeFormat" value="yyyy/MM/dd HH:mm:ss:fff" />
      </factoryAdapter>
    </logging>
  </common>
  
  <queue>
    <!--<queue name="SqlServerQueue" connectionString="Server=.\SQLEXPRESS; Database=ga;Uid=sa;Pwd=sa123;" queueName="SqlServer" type="SQL" />
    <queue name="MySqlQueue" connectionString="Server=localhost;Database=ga;Uid=sa;Pwd=sa123;" queueName="MySql" type="MySql" />-->
    <queue name="SqlServerQueue" connectionString="Server=SAFKAT\SQLEXPRESS; Database=ga;Uid=safkat;Pwd=1234567890;" queueName="SqlServer" type="SQL" />
    <queue name="InMemoryQueue" connectionString="" queueName="InMemory" type="InMemory" />
    <queue name="LuceneQueue" connectionString="" queueName="Lucene" type="Lucene" />
  </queue>
  
</configuration>
