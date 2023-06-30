using CER.Json.DocumentObjectModel;
using CER.Json.Stream;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace UI
{
	public partial class EditJson : Form
	{
		public EditJson()
		{
			InitializeComponent();

			_ = _typeValue.Items.Add(_arrayType);
			_ = _typeValue.Items.Add(_boolType);
			_ = _typeValue.Items.Add(_nullType);
			_ = _typeValue.Items.Add(_numberType);
			_ = _typeValue.Items.Add(_objectType);
			_ = _typeValue.Items.Add(_stringType);

			_detected = _utf8;

			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			_cp1252 = Encoding.GetEncoding(1252, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);

			LoadElement(new JsonNull());
		}

		string _path;
		bool _updating;
		Encoding _detected;
		readonly string _arrayType = Strings.Array;
		readonly string _boolType = Strings.Boolean;
		readonly string _nullType = Strings.Null;
		readonly string _numberType = Strings.Number;
		readonly string _objectType = Strings.Object;
		readonly string _stringType = Strings.String;
		readonly Encoding _ascii = Encoding.GetEncoding(20127, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
		/// <summary>
		/// UTF-8 with no byte order mark.
		/// </summary>
		readonly Encoding _utf8 = new UTF8Encoding(false, true);
		/// <summary>
		/// UTF-8 with a byte order mark.
		/// </summary>
		readonly Encoding _utf8bom = new UTF8Encoding(true, true);
		/// <summary>
		/// Little endian UTF-16 with a byte order mark.
		/// </summary>
		readonly Encoding _utf16le = new UnicodeEncoding(false, true, true);
		/// <summary>
		/// Big endian UTF-16 with a byte order mark.
		/// </summary>
		readonly Encoding _utf16be = new UnicodeEncoding(true, true, true);
		/// <summary>
		/// Little endian UTF-32 with a byte order mark.
		/// </summary>
		readonly Encoding _utf32le = new UTF32Encoding(false, true, true);
		/// <summary>
		/// Big endian UTF-32 with a byte order mark.
		/// </summary>
		readonly Encoding _utf32be = new UTF32Encoding(true, true, true);
		/// <summary>
		/// Code page 1252.
		/// </summary>
		readonly Encoding _cp1252;

		static bool BytesStartWith(byte[] left, int leftLength, byte[] right)
		{
			if (leftLength < right.Length)
			{
				return false;
			}

			for (int i = 0; i < right.Length; i++)
			{
				if (left[i] != right[i])
				{
					return false;
				}
			}

			return true;
		}

		static void SetText(TreeNode item)
		{
			JsonElement element;
			string name;
			if (item.Tag is JsonObjectPair pair)
			{
				element = pair.Value;
				name = $"\"{pair.Key.Value}\": ";
			}
			else
			{
				element = (JsonElement)item.Tag;
				name = string.Empty;
			}

			if (element is JsonArray)
			{
				name += Strings.Array;
			}
			else if (element is JsonBoolean boolValue)
			{
				name += boolValue.Value.ToString();
			}
			else if (element is JsonNull)
			{
				name += Strings.Null;
			}
			else if (element is JsonNumber numberValue)
			{
				name += numberValue.Json;
			}
			else if (element is JsonObject)
			{
				name += Strings.Object;
			}
			else if (element is JsonString stringValue)
			{
				name += $"\"{stringValue.Json}\"";
			}

			item.Text = name;
		}

		static bool TryEncoding(Stream stream, Encoding encoding)
		{
			using (TextReader reader = new StreamReader(stream, encoding, false, 4 * 1024, true))
			{
				char[] buffer = new char[2 * 1024];
				try
				{
					while (reader.Read(buffer, 0, buffer.Length) > 0) { }
				}
				catch (DecoderFallbackException)
				{
					return false;
				}
				finally
				{
					_ = stream.Seek(0, SeekOrigin.Begin);
				}
			}

			return true;
		}

		void LoadElement(JsonElement element)
		{
			_navigation.Nodes.Clear();
			TreeNode root = new TreeNode
			{
				Tag = element
			};
			SetText(root);
			_ = _navigation.Nodes.Add(root);

			LoadChildren(root, element);

			_navigation.SelectedNode = root;
		}

		bool SaveElement(string fileName)
		{
			UpdateDOM();

			Encoding encoding = GetSelectedEncoding();

			TextWriter writer;
			try
			{
				writer = new StreamWriter(fileName, false, encoding);
			}
			catch (SystemException ex)
			{
				if (ex is not UnauthorizedAccessException and
					not IOException and
					not SecurityException)
				{
					throw;
				}

				string message = string.Format(CultureInfo.CurrentCulture, Strings.SaveFailed, Environment.NewLine, fileName);
				ShowWarning(message, Strings.Save);
				return false;
			}
			using (writer)
			{
				try
				{
					((JsonElement)_navigation.Nodes[0].Tag).Serialize(writer);
				}
				catch (IOException)
				{

					string message = string.Format(CultureInfo.CurrentCulture, Strings.WritingFailed, Environment.NewLine, fileName);
					ShowWarning(message, Strings.Save);
				}
			}

			return true;
		}

		void SaveAs()
		{
			if (_saveDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			if (SaveElement(_saveDialog.FileName))
			{
				_path = _saveDialog.FileName;
			}
		}

		void UpdateSelectionDisplay(TreeNode node)
		{
			_booleanControl.Visible = false;
			_nullControl.Visible = false;
			_numberControl.Visible = false;
			_containerControl.Visible = false;
			_stringControl.Visible = false;

			_add.Enabled = false;

			bool inContainer = node.Parent != null;
			_insertBefore.Enabled = inContainer;
			_insertAfter.Enabled = inContainer;
			_delete.Enabled = inContainer;

			JsonElement element;
			if (node.Tag is JsonObjectPair pairValue)
			{
				_keyValueLayout.ColumnStyles[0].Width = 50;
				_keyControl.Value = pairValue.Key;
				_keyControl.Enabled = true;
				element = pairValue.Value;
			}
			else
			{
				_keyValueLayout.ColumnStyles[0].Width = 0;
				_keyControl.Enabled = false;
				element = (JsonElement)node.Tag;
			}

			if (node.Parent != null)
			{
				_insertBefore.Enabled = true;
			}

			_updating = true;
			if (element is JsonArray)
			{
				_typeValue.SelectedItem = _arrayType;

				_containerControl.Value = element;
				_containerControl.Visible = true;

				_add.Enabled = true;
			}
			else if (element is JsonBoolean booleanValue)
			{
				_typeValue.SelectedItem = _boolType;

				_booleanControl.Value = booleanValue;
				_booleanControl.Visible = true;
			}
			else if (element is JsonNull nullValue)
			{
				_typeValue.SelectedItem = _nullType;

				_nullControl.Value = nullValue;
				_nullControl.Visible = true;
			}
			else if (element is JsonNumber numberValue)
			{
				_typeValue.SelectedItem = _numberType;

				_numberControl.Value = numberValue;
				_numberControl.Visible = true;
			}
			else if (element is JsonObject)
			{
				_typeValue.SelectedItem = _objectType;

				_containerControl.Value = element;
				_containerControl.Visible = true;

				_add.Enabled = true;
			}
			else if (element is JsonString stringValue)
			{
				_typeValue.SelectedItem = _stringType;

				_stringControl.Value = stringValue;
				_stringControl.Visible = true;
			}
			_updating = false;
		}

		void LoadChildren(TreeNode parent, JsonElement parentValue)
		{
			if (parentValue is JsonArray childArray)
			{
				PopulateArray(parent, childArray);
			}
			else if (parentValue is JsonObject childObject)
			{
				PopulateObject(parent, childObject);
			}
		}

		void PopulateArray(TreeNode parent, JsonArray arrayValue)
		{
			foreach (JsonElement element in arrayValue)
			{
				TreeNode child = new TreeNode
				{
					Tag = element
				};
				SetText(child);
				_ = parent.Nodes.Add(child);
				LoadChildren(child, element);
			}
		}

		void PopulateObject(TreeNode parent, JsonObject objectValue)
		{
			foreach (JsonObjectPair pair in objectValue)
			{
				TreeNode child = new TreeNode
				{
					Tag = pair
				};
				SetText(child);
				_ = parent.Nodes.Add(child);
				LoadChildren(child, pair.Value);
			}
		}

		void Insert(TreeNode parent, int index)
		{
			TreeNode child = new TreeNode();

			JsonElement parentElement = GetValue(parent);

			if (parentElement is JsonArray arrayValue)
			{
				JsonElement toInsert = new JsonNull();
				arrayValue.Insert(index, toInsert);
				child.Tag = toInsert;
			}
			else if (parentElement is JsonObject objectValue)
			{
				JsonObjectPair toInsert = new JsonObjectPair(new JsonString(), new JsonNull());
				objectValue.Insert(index, toInsert);
				child.Tag = toInsert;
			}
			else
			{
				Debug.Fail("Parents must either be arrays or objects.");
			}

			SetText(child);
			parent.Nodes.Insert(index, child);
			_navigation.SelectedNode = child;
		}

		static JsonElement GetValue(TreeNode node)
		{
			if (node.Tag is JsonObjectPair pair)
			{
				return pair.Value;
			}
			else if (node.Tag is JsonElement elem)
			{
				return elem;
			}
			else
			{
				Debug.Fail("Tags must be either pairs or elements.");
				return new JsonNull();
			}
		}

		static void SetValue(TreeNode node, JsonElement value)
		{
			if (node.Tag is JsonObjectPair pair)
			{
				int index = node.Parent.Nodes.IndexOf(node);
				JsonObject container = (JsonObject)GetValue(node.Parent);
				container[index] = new JsonObjectPair(pair.Key, value);
				node.Tag = container[index];
			}
			else
			{
				if (node.Parent is not null)
				{
					int index = node.Parent.Nodes.IndexOf(node);
					JsonArray container = (JsonArray)GetValue(node.Parent);
					container[index] = value;
				}
				node.Tag = value;
			}
		}

		void UpdateDOM()
		{
			TreeNode selected = _navigation.SelectedNode;
			if (selected is null)
			{
				return;
			}

			JsonElement element;
			if (_arrayType.Equals(_typeValue.SelectedItem) ||
				_objectType.Equals(_typeValue.SelectedItem))
			{
				element = _containerControl.Value;
			}
			else if (_boolType.Equals(_typeValue.SelectedItem))
			{
				element = _booleanControl.Value;
			}
			else if (_nullType.Equals(_typeValue.SelectedItem))
			{
				element = _nullControl.Value;
			}
			else if (_numberType.Equals(_typeValue.SelectedItem))
			{
				element = _numberControl.Value;
			}
			else if (_stringType.Equals(_typeValue.SelectedItem))
			{
				element = _stringControl.Value;
			}
			else
			{
				Debug.Fail($"Unrecognized type {_typeValue.SelectedItem}.");
				return;
			}

			SetValue(selected, element);
			SetText(selected);
		}

		void SelectEncoding(ToolStripItem selected)
		{
			IEnumerable<ToolStripMenuItem> options = new ToolStripMenuItem[]
			{
				_autoOption,
				_asciiOption,
				_utf8Option,
				_utf8bomOption,
				_utf16leOption,
				_utf16beOption,
				_utf32leOption,
				_utf32beOption,
			};
			foreach (ToolStripMenuItem encodingOption in options)
			{
				encodingOption.Checked = encodingOption == selected;
			}
		}

		Encoding GetSelectedEncoding()
		{
			if (_asciiOption.Checked)
			{
				return _ascii;
			}
			else if (_utf8Option.Checked)
			{
				return _utf8;
			}
			else if (_utf8bomOption.Checked)
			{
				return _utf8bom;
			}
			else if (_utf16leOption.Checked)
			{
				return _utf16le;
			}
			else if (_utf16beOption.Checked)
			{
				return _utf16be;
			}
			else if (_utf32leOption.Checked)
			{
				return _utf32le;
			}
			else if (_utf32beOption.Checked)
			{
				return _utf32be;
			}
			else if (_cp1252Option.Checked)
			{
				return _cp1252;
			}
			else
			{
				return _detected;
			}
		}

		Encoding DetectEncoding(Stream stream)
		{
			// Check for byte order marks.
			byte[] buffer = new byte[4];
			int numRead = stream.Read(buffer, 0, buffer.Length);
			_ = stream.Seek(0, SeekOrigin.Begin);
			if (BytesStartWith(buffer, numRead, _utf32le.GetPreamble()))
			{
				return _utf32le;
			}
			else if (BytesStartWith(buffer, numRead, _utf32be.GetPreamble()))
			{
				return _utf32be;
			}
			else if (BytesStartWith(buffer, numRead, _utf16le.GetPreamble()))
			{
				return _utf16le;
			}
			else if (BytesStartWith(buffer, numRead, _utf16be.GetPreamble()))
			{
				return _utf16be;
			}
			else if (BytesStartWith(buffer, numRead, _utf8bom.GetPreamble()))
			{
				return _utf8bom;
			}

			if (TryEncoding(stream, _ascii))
			{
				// Prefer ASCII over UTF-8 because it's more compatible and it doesn't limit us: we
				// can just use JSON to escape characters that aren't directly representable as
				// ASCII.
				return _ascii;
			}
			else if (TryEncoding(stream, _utf8))
			{
				return _utf8;
			}
			else
			{
				return _cp1252;
			}
		}

		void ShowWarning(string message, string caption)
		{
			MessageBoxOptions globalized;
			if (RightToLeft == RightToLeft.Yes)
			{
				globalized = MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
			}
			else
			{
				globalized = default;
			}

			_ = MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, globalized);
		}

		void BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			UpdateDOM();
		}

		void AfterSelect(object sender, TreeViewEventArgs e)
		{
			UpdateSelectionDisplay(e.Node);
		}

		void NewClick(object sender, EventArgs e)
		{
			LoadElement(new JsonNull());
		}

		void OpenClick(object sender, EventArgs e)
		{
			if (_openDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			JsonElement element;

			using (Stream file = _openDialog.OpenFile())
			{
				if (_autoOption.Checked)
				{
					_detected = DetectEncoding(file);
				}
				else
				{
					_detected = GetSelectedEncoding();
				}

				StreamReader reader;
				try
				{
					reader = new StreamReader(file, _detected);
				}
				catch (IOException)
				{
					string message = string.Format(CultureInfo.CurrentCulture, Strings.OpenFailed, Environment.NewLine, _openDialog.FileName);
					ShowWarning(message, Strings.Open);
					return;
				}
				using (reader)
				{
					JsonReader json = new JsonReader(reader);
					try
					{
						element = JsonElement.Deserialize(json);
					}
					catch (Exception ex)
					{
						if (ex is not DecoderFallbackException and
							not InvalidJsonException)
						{
							throw;
						}
						string message = $"{_openDialog.FileName}{Environment.NewLine}{ex.Message}";
						ShowWarning(message, Strings.Open);
						return;
					}
				}

				if (_detected == _ascii)
				{
					_autoOption.Text = Strings.AutoAscii;
				}
				else if (_detected == _utf8)
				{
					_autoOption.Text = Strings.AutoUtf8;
				}
				else if (_detected == _cp1252)
				{
					_autoOption.Text = Strings.AutoCP1252;
				}
				else if (_detected == _utf8bom)
				{
					_autoOption.Text = Strings.AutoUtf8Bom;
				}
				else if (_detected == _utf16le)
				{
					_autoOption.Text = Strings.AutoUtf16LEBom;
				}
				else if (_detected ==_utf16be)
				{
					_autoOption.Text = Strings.AutoUtf16BEBom;
				}
				else if (_detected == _utf32le)
				{
					_autoOption.Text = Strings.AutoUtf32LEBom;
				}
				else if (_detected == _utf32be)
				{
					_autoOption.Text = Strings.AutoUtf32BEBom;
				}
				else
				{
					Debug.Fail("Unsupported encoding detected.");
				}
			}
			LoadElement(element);

			if (!_openDialog.ReadOnlyChecked)
			{
				_path = _openDialog.FileName;
			}
		}

		void SaveClick(object sender, EventArgs e)
		{
			if (_path is null)
			{
				SaveAs();
			}
			else
			{
				_ = SaveElement(_path);
			}
		}

		void SaveAsClick(object sender, EventArgs e)
		{
			SaveAs();
		}

		void ExitClick(object sender, EventArgs e)
		{
			Close();
		}

		void SelectedTypeChanged(object sender, EventArgs e)
		{
			if (_updating)
			{
				return;
			}

			JsonElement newValue;
			if (_arrayType.Equals(_typeValue.SelectedItem))
			{
				newValue = new JsonArray();
			}
			else if (_boolType.Equals(_typeValue.SelectedItem))
			{
				newValue = new JsonBoolean(false);
			}
			else if (_nullType.Equals(_typeValue.SelectedItem))
			{
				newValue = new JsonNull();
			}
			else if (_numberType.Equals(_typeValue.SelectedItem))
			{
				newValue = new JsonNumber(0);
			}
			else if (_objectType.Equals(_typeValue.SelectedItem))
			{
				newValue = new JsonObject();
			}
			else if (_stringType.Equals(_typeValue.SelectedItem))
			{
				newValue = new JsonString();
			}
			else
			{
				Debug.Fail($"Unrecognized type {_typeValue.SelectedItem}.");
				return;
			}

			SetValue(_navigation.SelectedNode, newValue);
			SetText(_navigation.SelectedNode);
			_navigation.SelectedNode.Nodes.Clear();

			UpdateSelectionDisplay(_navigation.SelectedNode);
		}

		void AddClick(object sender, EventArgs e)
		{
			Insert(_navigation.SelectedNode, _navigation.SelectedNode.Nodes.Count);
		}

		void InsertBeforeClick(object sender, EventArgs e)
		{
			TreeNode parent = _navigation.SelectedNode.Parent;
			if (parent is null)
			{
				return;
			}

			int selectedIndex = parent.Nodes.IndexOf(_navigation.SelectedNode);

			Insert(parent, selectedIndex);
		}

		void InsertAfterClick(object sender, EventArgs e)
		{
			TreeNode parent = _navigation.SelectedNode.Parent;
			if (parent is null)
			{
				return;
			}

			int selectedIndex = parent.Nodes.IndexOf(_navigation.SelectedNode);

			Insert(parent, selectedIndex + 1);
		}

		void DeleteClick(object sender, EventArgs e)
		{
			TreeNode parent = _navigation.SelectedNode.Parent;
			if (parent is null)
			{
				return;
			}

			int selectedIndex = parent.Nodes.IndexOf(_navigation.SelectedNode);

			if (parent.Tag is JsonArray arrayValue)
			{
				arrayValue.RemoveAt(selectedIndex);
			}
			else if (parent.Tag is JsonObject objectValue)
			{
				objectValue.RemoveAt(selectedIndex);
			}
			else
			{
				Debug.Fail("Parent nodes must be either arrays or objects.");
			}

			parent.Nodes.Remove(_navigation.SelectedNode);
			_navigation.SelectedNode = parent;
		}

		void CollapseClick(object sender, EventArgs e)
		{
			_navigation.SelectedNode.Collapse(false);
		}

		void ExpandClick(object sender, EventArgs e)
		{
			_navigation.SelectedNode.ExpandAll();
		}

		void EncodingDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			SelectEncoding(e.ClickedItem);
		}

		void UpdateValue(JsonElement newValue)
		{
			SetValue(_navigation.SelectedNode, newValue);
			SetText(_navigation.SelectedNode);
		}

		void BooleanChanged(object sender, EventArgs e)
		{
			UpdateValue(_booleanControl.Value);
		}

		void StringChanged(object sender, EventArgs e)
		{
			UpdateValue(_stringControl.Value);
		}

		void NumberChanged(object sender, EventArgs e)
		{
			UpdateValue(_numberControl.Value);
		}

		void KeyChanged(object sender, EventArgs e)
		{
			TreeNode selected = _navigation.SelectedNode;
			int index = selected.Parent.Nodes.IndexOf(selected);
			JsonObjectPair pair = (JsonObjectPair)selected.Tag;
			JsonObject objectContainer = (JsonObject)selected.Parent.Tag;
			objectContainer[index] = new JsonObjectPair(_keyControl.Value, pair.Value);
			selected.Tag = objectContainer[index];
			SetText(selected);
		}
	}
}
