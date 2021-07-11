using CER.JSON.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;

using Array = CER.JSON.DocumentObjectModel.Array;
using Boolean = CER.JSON.DocumentObjectModel.Boolean;
using Object = CER.JSON.DocumentObjectModel.Object;
using String = CER.JSON.DocumentObjectModel.String;

namespace UI
{
	public partial class EditJSON : Form
	{
		public EditJSON()
		{
			InitializeComponent();

			_ = _typeValue.Items.Add(_arrayType);
			_ = _typeValue.Items.Add(_boolType);
			_ = _typeValue.Items.Add(_nullType);
			_ = _typeValue.Items.Add(_numberType);
			_ = _typeValue.Items.Add(_objectType);
			_ = _typeValue.Items.Add(_stringType);

			_detected = _utf8;

			LoadElement(new Null());
		}

		string _path;
		bool _updating;
		Encoding _detected;
		const string _arrayType = "Array";
		const string _boolType = "Boolean";
		const string _nullType = "Null";
		const string _numberType = "Number";
		const string _objectType = "Object";
		const string _stringType = "String";
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
		readonly Encoding _cp1252 = Encoding.GetEncoding(1252, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);

		void LoadElement(Element element)
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
				if (!(ex is UnauthorizedAccessException) &&
					!(ex is IOException) &&
					!(ex is SecurityException))
				{
					throw;
				}

				string message = string.Format("{1}{0}Check the file name and try again.", Environment.NewLine, fileName);
				_ = MessageBox.Show(message, "Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			using (writer)
			{
				try
				{
					((Element)_navigation.Nodes[0].Tag).Serialize(writer);
				}
				catch (IOException)
				{

					string message = string.Format("{1}{0}Writing failed.", Environment.NewLine, fileName);
					_ = MessageBox.Show(message, "Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

			Element element;
			if (node.Tag is ObjectPair pairValue)
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
				element = (Element)node.Tag;
			}

			if (node.Parent != null)
			{
				_insertBefore.Enabled = true;
			}

			_updating = true;
			if (element is Array)
			{
				_typeValue.SelectedItem = _arrayType;

				_containerControl.Value = element;
				_containerControl.Visible = true;

				_add.Enabled = true;
			}
			else if (element is Boolean booleanValue)
			{
				_typeValue.SelectedItem = _boolType;

				_booleanControl.Value = booleanValue;
				_booleanControl.Visible = true;
			}
			else if (element is Null nullValue)
			{
				_typeValue.SelectedItem = _nullType;

				_nullControl.Value = nullValue;
				_nullControl.Visible = true;
			}
			else if (element is Number numberValue)
			{
				_typeValue.SelectedItem = _numberType;

				_numberControl.Value = numberValue;
				_numberControl.Visible = true;
			}
			else if (element is Object)
			{
				_typeValue.SelectedItem = _objectType;

				_containerControl.Value = element;
				_containerControl.Visible = true;

				_add.Enabled = true;
			}
			else if (element is String stringValue)
			{
				_typeValue.SelectedItem = _stringType;

				_stringControl.Value = stringValue;
				_stringControl.Visible = true;
			}
			_updating = false;
		}

		void LoadChildren(TreeNode parent, Element parentValue)
		{
			if (parentValue is Array childArray)
			{
				PopulateArray(parent, childArray);
			}
			else if (parentValue is Object childObject)
			{
				PopulateObject(parent, childObject);
			}
		}

		void PopulateArray(TreeNode parent, Array arrayValue)
		{
			foreach (Element element in arrayValue.Values)
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

		void PopulateObject(TreeNode parent, Object objectValue)
		{
			foreach (ObjectPair pair in objectValue.Values)
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

		void SetText(TreeNode item)
		{
			Element element;
			string name;
			if (item.Tag is ObjectPair pair)
			{
				element = pair.Value;
				name = string.Format("\"{0}\": ", pair.Key.Value);
			}
			else
			{
				element = (Element)item.Tag;
				name = string.Empty;
			}

			if (element is Array)
			{
				name += "Array";
			}
			else if (element is Boolean boolValue)
			{
				name += boolValue.Value.ToString();
			}
			else if (element is Null)
			{
				name += "Null";
			}
			else if (element is Number numberValue)
			{
				name += numberValue.JSON;
			}
			else if (element is Object)
			{
				name += "Object";
			}
			else if (element is String stringValue)
			{
				name += "\"" + stringValue.JSON + "\"";
			}

			item.Text = name;
		}

		void Insert(TreeNode parent, int index)
		{
			TreeNode child = new TreeNode();

			Element parentElement;
			if (parent.Tag is ObjectPair pair)
			{
				parentElement = pair.Value;
			}
			else
			{
				parentElement = (Element)parent.Tag;
			}

			if (parentElement is Array arrayValue)
			{
				Element toInsert = new Null();
				arrayValue.Values.Insert(index, toInsert);
				child.Tag = toInsert;
			}
			else if (parentElement is Object objectValue)
			{
				ObjectPair toInsert = new ObjectPair();
				objectValue.Values.Insert(index, toInsert);
				child.Tag = toInsert;
			}
			else
			{
				return;
			}

			SetText(child);
			parent.Nodes.Insert(index, child);
			_navigation.SelectedNode = child;
		}

		void UpdateDOM()
		{
			TreeNode selected = _navigation.SelectedNode;
			if (selected == null)
			{
				return;
			}

			Element element;
			switch (_typeValue.SelectedItem)
			{
				case _arrayType:
				case _objectType:
					element = _containerControl.Value;
					break;
				case _boolType:
					element = _booleanControl.Value;
					break;
				case _nullType:
					element = _nullControl.Value;
					break;
				case _numberType:
					element = _numberControl.Value;
					break;
				case _stringType:
					element = _stringControl.Value;
					break;
				default:
					Debug.Assert(false);
					return;
			}

			if (selected.Tag is ObjectPair pair)
			{
				pair.Key = _keyControl.Value;
				pair.Value = element;
			}
			else
			{
				selected.Tag = element;
			}
			SetText(selected);

			if (selected.Parent != null && selected.Parent.Tag is Array arrayContainer)
			{
				int index = selected.Parent.Nodes.IndexOf(selected);
				arrayContainer.Values[index] = element;
			}
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

		bool BytesStartWith(byte[] left, int leftLength, byte[] right)
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

		bool TryEncoding(Stream stream, Encoding encoding)
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
			LoadElement(new Null());
		}

		void OpenClick(object sender, EventArgs e)
		{
			if (_openDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			Element element;

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
					string message = string.Format("{1}{0}Check the file name and try again.", Environment.NewLine, _openDialog.FileName);
					_ = MessageBox.Show(message, "Open", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				using (reader)
				{
					CER.JSON.Stream.StreamReader json = new CER.JSON.Stream.StreamReader(reader);
					try
					{
						element = Element.Deserialize(json);
					}
					catch (Exception ex)
					{
						if (!(ex is InvalidDataException) &&
							!(ex is CER.JSON.Stream.InvalidTextException))
						{
							throw;
						}
						string message = string.Format("{1}{0}{2}", Environment.NewLine, _openDialog.FileName, ex.Message);
						_ = MessageBox.Show(message, "Open", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}
				}

				if (_detected == _ascii)
				{
					_autoOption.Text = "Auto (ASCII)";
				}
				else if (_detected ==_utf8)
				{
					_autoOption.Text = "Auto (UTF-8)";
				}
				else if (_detected == _utf8bom)
				{
					_autoOption.Text = "Auto (UTF-8-BOM)";
				}
				else if (_detected == _utf16le)
				{
					_autoOption.Text = "Auto (UTF-16 LE BOM)";
				}
				else if (_detected ==_utf32be)
				{
					_autoOption.Text = "Auto (UTF-16 BE BOM)";
				}
				else if (_detected == _utf32le)
				{
					_autoOption.Text = "Auto (UTF-32 LE BOM)";
				}
				else if (_detected == _utf32be)
				{
					_autoOption.Text = "Auto (UTF-32 BE BOM)";
				}
				else
				{
					Debug.Assert(false);
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
			if (_path == null)
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

			Element newValue;
			switch (_typeValue.SelectedItem)
			{
				case _arrayType:
					newValue = new Array();
					break;
				case _boolType:
					newValue = new Boolean(false);
					break;
				case _nullType:
					newValue = new Null();
					break;
				case _numberType:
					newValue = new Number(0);
					break;
				case _objectType:
					newValue = new Object();
					break;
				case _stringType:
					newValue = new String();
					break;
				default:
					Debug.Assert(false);
					return;
			}

			if (_navigation.SelectedNode.Tag is ObjectPair pair)
			{
				pair.Value = newValue;
			}
			else
			{
				_navigation.SelectedNode.Tag = newValue;
			}
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
			if (parent == null)
			{
				return;
			}

			int selectedIndex = parent.Nodes.IndexOf(_navigation.SelectedNode);

			Insert(parent, selectedIndex);
		}

		void InsertAfterClick(object sender, EventArgs e)
		{
			TreeNode parent = _navigation.SelectedNode.Parent;
			if (parent == null)
			{
				return;
			}

			int selectedIndex = parent.Nodes.IndexOf(_navigation.SelectedNode);

			Insert(parent, selectedIndex + 1);
		}

		void DeleteClick(object sender, EventArgs e)
		{
			TreeNode parent = _navigation.SelectedNode.Parent;
			if (parent == null)
			{
				return;
			}

			int selectedIndex = parent.Nodes.IndexOf(_navigation.SelectedNode);

			if (parent.Tag is Array arrayValue)
			{
				arrayValue.Values.RemoveAt(selectedIndex);
			}
			else if (parent.Tag is Object objectValue)
			{
				objectValue.Values.RemoveAt(selectedIndex);
			}
			else
			{
				return;
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
	}
}
