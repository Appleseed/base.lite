using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using Appleseed.Base.Data.Model;

namespace Appleseed.Base.Data.Utility
{
    public static class Serializer
    {
        public static byte[] SerializeToByteArray(this BaseCollectionItemData item)
        {
            if (item == null)
                return null;
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                return ms.ToArray();
            }
        }

        public static object DeSerializeToObject(this byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            BaseCollectionItemData data = new BaseCollectionItemData();
            BinaryFormatter formatter = new BinaryFormatter();

            using (var ms = new MemoryStream(bytes))
            {
                data = (BaseCollectionItemData)formatter.Deserialize(ms);
                return data;
            }            
        }

        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
}