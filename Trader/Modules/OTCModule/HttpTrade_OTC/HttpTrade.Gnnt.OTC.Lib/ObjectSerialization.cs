using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace HttpTrade.Gnnt.OTC.Lib
{
	internal class ObjectSerialization
	{
		public static byte[] SerializeMemory(object pObj)
		{
			if (pObj == null)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, pObj);
				memoryStream.Position = 0L;
			}
			catch (SerializationException)
			{
				throw;
			}
			byte[] array = new byte[memoryStream.Length];
			memoryStream.Read(array, 0, array.Length);
			memoryStream.Close();
			return array;
		}
		public static object DeserializeMemory(byte[] pBytes)
		{
			object result = null;
			if (pBytes == null)
			{
				return result;
			}
			MemoryStream memoryStream = new MemoryStream(pBytes);
			memoryStream.Position = 0L;
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				result = binaryFormatter.Deserialize(memoryStream);
			}
			catch (SerializationException)
			{
				throw;
			}
			memoryStream.Close();
			return result;
		}
		public static void SerializeFile(string filename, object pObj)
		{
			FileStream fileStream = new FileStream(filename, FileMode.Create);
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(fileStream, pObj);
			}
			catch (SerializationException)
			{
				throw;
			}
			finally
			{
				fileStream.Close();
			}
		}
		public static object DeserializeFile(string filename)
		{
			if (!File.Exists(filename))
			{
				return null;
			}
			FileStream fileStream = new FileStream(filename, FileMode.Open);
			object result = null;
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				result = binaryFormatter.Deserialize(fileStream);
			}
			catch (SerializationException)
			{
				throw;
			}
			finally
			{
				fileStream.Close();
			}
			return result;
		}
		public static object DeserializeFile(FileStream fs)
		{
			if (fs == null)
			{
				return null;
			}
			object result = null;
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				result = binaryFormatter.Deserialize(fs);
			}
			catch (SerializationException)
			{
				throw;
			}
			finally
			{
				fs.Close();
			}
			return result;
		}
		public static void SerializeFile(FileStream fs, object pObj)
		{
			if (fs == null)
			{
				return;
			}
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(fs, pObj);
			}
			catch (SerializationException)
			{
				throw;
			}
			finally
			{
				fs.Close();
			}
		}
	}
}
