using CER.Json.DocumentObjectModel;
using CER.Json.Stream;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace SerializationTest
{
	[TestClass]
	public class DocumentObjectModelTest
	{
		public DocumentObjectModelTest()
		{
			_utf8 = new UTF8Encoding(false, true);
			_cp1252 = Encoding.GetEncoding(1252, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
		}

		readonly Encoding _utf8;
		readonly Encoding _cp1252;

		[TestMethod]
		public void TestArrayEmptyWhitespace()
		{
			JsonArray elem = new JsonArray
			{
				EmptyWhitespace = new Whitespace(" ")
			};

			string json = GetString(_utf8, elem);
			Assert.AreEqual("[ ]", json);
		}

		[TestMethod]
		public void TestObjectEmptyWhitespace()
		{
			JsonObject elem = new JsonObject
			{
				EmptyWhitespace = new Whitespace(" ")
			};

			string json = GetString(_utf8, elem);
			Assert.AreEqual("{ }", json);
		}

		[TestMethod]
		public void TestLeadingWhitespace()
		{
			JsonNumber elem = new JsonNumber("0")
			{
				Leading = new Whitespace(" ")
			};

			string json = GetString(_utf8, elem);
			Assert.AreEqual(" 0", json);
		}

		[TestMethod]
		public void TestTrailingWhitespace()
		{
			JsonNumber elem = new JsonNumber("0")
			{
				Trailing = new Whitespace(" ")
			};

			string json = GetString(_utf8, elem);
			Assert.AreEqual("0 ", json);
		}

		[TestMethod]
		public void TestZero()
		{
			JsonNumber elem = new JsonNumber("0");
			Assert.AreEqual(0, elem.Value);
		}

		[TestMethod]
		public void TestNegativeZero()
		{
			JsonNumber elem = new JsonNumber("-0");
			Assert.AreEqual(0, elem.Value);
		}

		[TestMethod]
		public void TestGoogolPlex()
		{
			JsonNumber elem = new JsonNumber("1E10000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
			_ = Assert.ThrowsException<OverflowException>(delegate ()
			{
				_ = elem.Value;
			});
		}

		[TestMethod]
		public void TestNull()
		{
			JsonNull elem = new JsonNull();
			string json = GetString(_utf8, elem);
			Assert.AreEqual("null", json);
		}

		[TestMethod]
		public void TestTrue()
		{
			JsonBoolean elem = new JsonBoolean(true);
			string json = GetString(_utf8, elem);
			Assert.AreEqual("true", json);
		}

		[TestMethod]
		public void TestFalse()
		{
			JsonBoolean elem = new JsonBoolean(false);
			string json = GetString(_utf8, elem);
			Assert.AreEqual("false", json);
		}

		[TestMethod]
		public void TestInvalidWhitespace()
		{
			_ = Assert.ThrowsException<ArgumentException>(delegate ()
			{
				_ = new Whitespace("a");
			});
		}

		[TestMethod]
		public void TestInvalidNumber()
		{
			_ = Assert.ThrowsException<FormatException>(delegate ()
			{
				_ = new JsonNumber("");
			});
		}

		[TestMethod]
		public void TestNullNumber()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(delegate ()
			{
				_ = new JsonNumber(null);
			});
		}

		[TestMethod]
		public void TestNonUnicodeString()
		{
			_ = Assert.ThrowsException<FormatException>(delegate ()
			{
				_ = new JsonString("\ud800", true);
			});
		}

		[TestMethod]
		public void TestInvalidJsonString()
		{
			_ = Assert.ThrowsException<FormatException>(delegate ()
			{
				_ = new JsonString("\\", true);
			});
		}

		[TestMethod]
		public void TestNullString()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(delegate ()
			{
				_ = new JsonString(null, false);
			});
		}

		[TestMethod]
		public void TestArrayWithNull()
		{
			JsonArray elem = new JsonArray();
			_ = Assert.ThrowsException<ArgumentNullException>(delegate ()
			{
				elem.Add(null);
			});
		}

		[TestMethod]
		public void TestRiceCrackerUTF8()
		{
			JsonString elem = new JsonString("\ud83c\udf58", false);
			string json = GetString(_utf8, elem);
			Assert.AreEqual("\"\ud83c\udf58\"", json);
		}

		[TestMethod]
		public void TestRiceCrackerCP1252()
		{
			JsonString elem = new JsonString("\ud83c\udf58", false);
			string json = GetString(_cp1252, elem);
			Assert.AreEqual("\"\\uD83C\\uDF58\"", json);
		}

		static string GetString(Encoding encoding, JsonElement element)
		{
			Stream mem = new MemoryStream();

			TextWriter writer = new StreamWriter(mem, encoding);
			element.Serialize(writer);
			writer.Flush();

			mem.Position = 0;

			return new StreamReader(mem, encoding).ReadToEnd();
		}
	}
}
