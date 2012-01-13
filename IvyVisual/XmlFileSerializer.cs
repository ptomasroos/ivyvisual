using System;
using System.IO;
using System.Xml.Serialization;

namespace IvyVisual
{
    public static class XmlFileSerializer<T>
    {
        private static readonly XmlSerializer InstanceSerializer = new XmlSerializer(typeof(T));

        public static T Deserialize(string path)
        {
            FileInfo file = new FileInfo(path);

            if (!file.Exists)
            {
                throw new FileNotFoundException("Can't deserialize a file that doesnt exist");
            }

            T instance;

            using (FileStream fileStream = file.OpenRead())
            {
                instance = (T)InstanceSerializer.Deserialize(fileStream);
                fileStream.Close();
            }

            return instance;
        }

        public static void Serialize(string path, T instance)
        {
            FileInfo file = new FileInfo(path);

            if (file.Exists)
            {
                file.Delete();
            }

            using (FileStream fileStream = File.Create(file.FullName))
            {
                InstanceSerializer.Serialize(fileStream, instance);
                fileStream.Close();
            }
        }
    }
}