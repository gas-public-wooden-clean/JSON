using CER.Json.Stream;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace SerializationTest
{
	[TestClass]
	public class StreamTest
	{
		static readonly Encoding _strictUTF8 = new UTF8Encoding(false, true);

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

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_1_true_without_comma.json"/>
		[TestMethod]
		public void TestArrayOneTrueWithoutComma()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[1 true]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_colon_instead_of_comma.json"/>
		[TestMethod]
		public void TestArrayColonInsteadOfComma()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[\"\": 1]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_comma_after_close.json"/>
		[TestMethod]
		public void TestArrayCommaAfterClose()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[\"\"],");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_comma_and_number.json"/>
		[TestMethod]
		public void TestArrayCommanAndNumber()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[,1]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_double_comma.json"/>
		[TestMethod]
		public void TestArrayDoubleComma()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[1,,2]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_double_extra_comma.json"/>
		[TestMethod]
		public void TestArrayDoubleExtraComma()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[\"x\",,]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_extra_close.json"/>
		[TestMethod]
		public void TestArrayExtraClose()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[\"x\"]]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_extra_comma.json"/>
		[TestMethod]
		public void TestArrayExtraComma()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[\"\",]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_incomplete.json"/>
		[TestMethod]
		public void TestArrayIncomplete()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[\"x\"");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_incomplete_invalid_value.json"/>
		[TestMethod]
		public void TestArrayIncompleteInvalidValue()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[\"x");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_inner_array_no_comma.json"/>
		[TestMethod]
		public void TestArrayInnerArrayNoComma()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[3[4]]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_items_separated_by_semicolon.json"/>
		[TestMethod]
		public void TestArrayItemsSeparatedBySemicolon()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[1:2]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_just_comma.json"/>
		[TestMethod]
		public void TestArrayJustComma()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[,]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_just_minus.json"/>
		[TestMethod]
		public void TestArrayJustMinus()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[-]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_missing_value.json"/>
		[TestMethod]
		public void TestArrayMissingValue()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[   , \"\"]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_newlines_unclosed.json"/>
		[TestMethod]
		public void TestArrayNewlinesUnclosed()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument(string.Format(CultureInfo.InvariantCulture, "[\"a\",{0}4{0},1,", Environment.NewLine));
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_number_and_comma.json"/>
		[TestMethod]
		public void TestArrayNumberAndComma()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[1,]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_number_and_several_commas.json"/>
		[TestMethod]
		public void TestArrayNumberAndSeveralCommas()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[1,,]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_spaces_vertical_tab_formfeed.json"/>
		[TestMethod]
		public void TestArraySpacesVerticalTabFormfeed()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[\"\va\"\f]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_star_inside.json"/>
		[TestMethod]
		public void TestArrayStarInside()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[*]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_unclosed.json"/>
		[TestMethod]
		public void TestArrayUnclosed()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[\"\"");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_unclosed_trailing_comma.json"/>
		[TestMethod]
		public void TestArrayUnclosedTrailingComma()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[1,");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_unclosed_with_new_lines.json"/>
		[TestMethod]
		public void TestArrayUnclosedWithNewLines()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument(string.Format(CultureInfo.InvariantCulture, "[1,{0}1{0},1", Environment.NewLine));
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_array_unclosed_with_object_inside.json"/>
		[TestMethod]
		public void TestArrayUnclosedWithObjectInside()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[{}");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_incomplete_false.json"/>
		[TestMethod]
		public void TestIncompleteFalse()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[fals]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_incomplete_null.json"/>
		[TestMethod]
		public void TestIncompleteNull()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[nul]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_incomplete_true.json"/>
		[TestMethod]
		public void TestIncompleteTrue()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[tru]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_multidigit_number_then_00.json"/>
		[TestMethod]
		public void TestMultidigitNumberThenZeroZero()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("123\0");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_number_%2B%2B.json"/>
		[TestMethod]
		public void TestNumberPlusPlus()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[++1234]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_number_%2B1.json"/>
		[TestMethod]
		public void TestNumberPlusOne()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[+1]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_number_%2BInf.json"/>
		[TestMethod]
		public void TestNumberPlusInf()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[+Inf]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_number_-01.json"/>
		[TestMethod]
		public void TestNumberMinusZeroOne()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[-01]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_number_-1.0..json"/>
		[TestMethod]
		public void TestNumberMinusOnePointZeroPoint()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[-1.0.]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_number_-2..json"/>
		[TestMethod]
		public void TestNumberMinusTwoPoint()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[-2.]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_number_-NaN.json"/>
		[TestMethod]
		public void TestNumberMinusNotANumber()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[-NaN]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_number_.-1.json"/>
		[TestMethod]
		public void TestNumberPointMinusOne()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[.-1]");
			});
		}

		/// <see cref="https://github.com/nst/JSONTestSuite/blob/master/test_parsing/n_number_.2e-3.json"/>
		[TestMethod]
		public void TestNumberPointTwoExponentMinusThree()
		{
			_ = Assert.ThrowsException<InvalidJsonException>(delegate ()
			{
				ReadDocument("[.2e-3]");
			});
		}

		static JsonReader GetReader(Encoding encoding, string json)
		{
			Stream mem = new MemoryStream(encoding.GetBytes(json));

			TextReader reader = new StreamReader(mem, encoding);
			return new JsonReader(reader);
		}

		static void ReadDocument(string json)
		{
			Stream mem = new MemoryStream(_strictUTF8.GetBytes(json));
			JsonReader jsonReader = new JsonReader(new StreamReader(mem, _strictUTF8));
			while (jsonReader.Read())
			{
				// Nothing else.
			}
		}
	}
}
