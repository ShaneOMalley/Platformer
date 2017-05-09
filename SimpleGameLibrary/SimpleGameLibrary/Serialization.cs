using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleGameLibrary
{
    public static class Serialization
    {
        /* Serialize using filename */
        public static void Serialize<T>(string fileName, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(fileName, settings))
            {
                serializer.Serialize(writer, data);
            }
        }

        /* Serialize using stream */
        public static void Serialize<T>(Stream stream, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, data);
            }
        }

        /* Deserializes using filename */
        public static T Deserialize<T>(string fileName)
        {
            T data;

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            Console.WriteLine(fileName == null ? "filename is null" : "filename is not null");

            using (XmlReader reader = XmlReader.Create(fileName))
            {
                data = (T)serializer.Deserialize(reader);
            }

            return data;
        }

        /* Deserialize using stream */
        public static T Deserialize<T>(Stream stream)
        {
            T data;

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (XmlReader reader = XmlReader.Create(stream))
            {
                data = (T)serializer.Deserialize(reader);
            }

            return data;
        }
    }
}
