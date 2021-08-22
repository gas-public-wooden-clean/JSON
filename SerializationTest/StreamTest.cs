using CER.Json.Stream;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace SerializationTest
{
	[TestClass]
	public class StreamTest
	{
		public StreamTest() => _strictUTF8 = new UTF8Encoding(false, true);

		readonly Encoding _strictUTF8;

		[TestMethod]
		public void TestEmpty()
		{
			JsonReader json = GetReader(_strictUTF8, "");
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				_ = json.Read();
			});
			_ = Assert.ThrowsException<InvalidOperationException>(delegate ()
			{
				_ = json.Read();
			});
		}

		[TestMethod]
		public void TestNumber()
		{
			JsonReader json = GetReader(_strictUTF8, "0");

			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.Number, json.CurrentToken);
			_ = Assert.ThrowsException<InvalidOperationException>(delegate ()
			{
				_ = json.BooleanValue;
			});
			_ = Assert.ThrowsException<InvalidOperationException>(delegate ()
			{
				_ = json.StringValue;
			});
			_ = Assert.ThrowsException<InvalidOperationException>(delegate ()
			{
				_ = json.Whitespace;
			});
			Assert.AreEqual("0", json.NumberValue);
			Assert.IsFalse(json.Read());
		}

		[TestMethod]
		public void TestString()
		{
			JsonReader json = GetReader(_strictUTF8, "\"\"");

			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.String, json.CurrentToken);
			_ = Assert.ThrowsException<InvalidOperationException>(delegate ()
			{
				_ = json.NumberValue;
			});
			Assert.AreEqual("", json.StringValue);
			Assert.IsFalse(json.Read());
		}

		[TestMethod]
		public void TestNull()
		{
			JsonReader json = GetReader(_strictUTF8, "null");

			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.Null, json.CurrentToken);
			Assert.IsFalse(json.Read());
		}

		[TestMethod]
		public void TestNullWrongCaps()
		{
			JsonReader json = GetReader(_strictUTF8, "Null");

			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				_ = json.Read();
			});
		}

		[TestMethod]
		public void TestFalse()
		{
			JsonReader json = GetReader(_strictUTF8, "false");

			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.Boolean, json.CurrentToken);
			Assert.AreEqual(false, json.BooleanValue);
			Assert.IsFalse(json.Read());
		}

		[TestMethod]
		public void TestTrue()
		{
			JsonReader json = GetReader(_strictUTF8, "true");

			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.Boolean, json.CurrentToken);
			Assert.AreEqual(true, json.BooleanValue);
			Assert.IsFalse(json.Read());
		}

		[TestMethod]
		public void TestNumberInArray()
		{
			JsonReader json = GetReader(_strictUTF8, "[0]");

			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.BeginArray, json.CurrentToken);
			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.Number, json.CurrentToken);
			Assert.AreEqual("0", json.NumberValue);
			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.EndArray, json.CurrentToken);
			Assert.IsFalse(json.Read());
		}

		[TestMethod]
		public void TestTrailingCommaArray()
		{
			JsonReader json = GetReader(_strictUTF8, "[0,]");

			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.BeginArray, json.CurrentToken);
			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.Number, json.CurrentToken);
			Assert.AreEqual("0", json.NumberValue);
			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.ListSeparator, json.CurrentToken);
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				_ = json.Read();
			});
		}

		[TestMethod]
		public void TestTrailingCommaObject()
		{
			JsonReader json = GetReader(_strictUTF8, "{\"\":0,}");

			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.BeginObject, json.CurrentToken);
			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.String, json.CurrentToken);
			Assert.AreEqual("", json.StringValue);
			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.KeyValueSeparator, json.CurrentToken);
			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.Number, json.CurrentToken);
			Assert.AreEqual("0", json.NumberValue);
			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.ListSeparator, json.CurrentToken);
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				_ = json.Read();
			});
		}

		[TestMethod]
		public void TestObjectNonStringKey()
		{
			JsonReader json = GetReader(_strictUTF8, "{0:0}");

			Assert.IsTrue(json.Read());
			Assert.AreEqual(TokenType.BeginObject, json.CurrentToken);
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				_ = json.Read();
			});
		}

		static JsonReader GetReader(Encoding encoding, string json)
		{
			Stream mem = new MemoryStream(encoding.GetBytes(json));

			TextReader reader = new StreamReader(mem, encoding);
			return new JsonReader(reader);
		}
	}
}
