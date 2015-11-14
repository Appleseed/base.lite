using System.Collections.Generic;
using System.Configuration;

namespace Appleseed.Base.Data.Utility
{
    public class QueueSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(QueueCollection), AddItemName = "queue")]
        public QueueCollection Queue
        {
            get { return (QueueCollection)this[""]; }
        }
    }

    public class QueueCollection : ConfigurationElementCollection, IEnumerable<QueueConfigurationElement>
    {
        private readonly List<QueueConfigurationElement> elements;

        public QueueCollection()
        {
            this.elements = new List<QueueConfigurationElement>();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var element = new QueueConfigurationElement();
            this.elements.Add(element);

            return element;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((QueueConfigurationElement)element).Name;
        }

        public new IEnumerator<QueueConfigurationElement> GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }
    }
    public class QueueConfigurationElement : ConfigurationElement
    {
        ////public string Name { get; set; }
        ////public string QueueName { get; set; }
        ////public string ConfigurationString { get; set; }
        ////public string Type { get; set; }


        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return (string)this["connectionString"]; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
        }

        [ConfigurationProperty("queueName", IsKey = true, IsRequired = true)]
        public string QueueName
        {   
            get { return (string)this["queueName"]; }
        }

    }
}