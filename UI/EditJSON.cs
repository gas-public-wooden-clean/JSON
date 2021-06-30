﻿using CER.JSON.DocumentObjectModel;
using System;
using System.IO;
using System.Security;
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

			_typeValue.Items.Add(_arrayType);
			_typeValue.Items.Add(_boolType);
			_typeValue.Items.Add(_nullType);
			_typeValue.Items.Add(_numberType);
			_typeValue.Items.Add(_objectType);
			_typeValue.Items.Add(_stringType);

			LoadElement(new Null());
		}

		private string _path;
		private bool _updating;
		private const string _arrayType = "Array";
		private const string _boolType = "Boolean";
		private const string _nullType = "Null";
		private const string _numberType = "Number";
		private const string _objectType = "Object";
		private const string _stringType = "String";

		private void LoadElement(Element element)
		{
			_navigation.Nodes.Clear();
			TreeNode root = new TreeNode();
			root.Tag = element;
			SetText(root);
			_ = _navigation.Nodes.Add(root);

			LoadChildren(root, element);

			_navigation.SelectedNode = root;
		}

		private bool SaveElement(string fileName)
		{
			UpdateDOM();

			TextWriter writer;
			try
			{
				writer = new StreamWriter(fileName);
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

		private void SaveAs()
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

		private void UpdateSelectionDisplay(TreeNode node)
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

		private void LoadChildren(TreeNode parent, Element parentValue)
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

		private void PopulateArray(TreeNode parent, Array arrayValue)
		{
			foreach (Element element in arrayValue.Values)
			{
				TreeNode child = new TreeNode();
				child.Tag = element;
				SetText(child);
				_ = parent.Nodes.Add(child);
				LoadChildren(child, element);
			}
		}

		private void PopulateObject(TreeNode parent, Object objectValue)
		{
			foreach (ObjectPair pair in objectValue.Values)
			{
				TreeNode child = new TreeNode();
				child.Tag = pair;
				SetText(child);
				_ = parent.Nodes.Add(child);
				LoadChildren(child, pair.Value);
			}
		}

		private void SetText(TreeNode item)
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

		private void Insert(TreeNode parent, int index)
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

		private void UpdateDOM()
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
					// This should never happen.
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

		private void BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			UpdateDOM();
		}

		private void AfterSelect(object sender, TreeViewEventArgs e)
		{
			UpdateSelectionDisplay(e.Node);
		}

		private void NewClick(object sender, EventArgs e)
		{
			LoadElement(new Null());
		}

		private void OpenClick(object sender, EventArgs e)
		{
			if (_openDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			Element element;

			TextReader reader;
			try
			{
				reader = new StreamReader(_openDialog.FileName);
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
			LoadElement(element);

			if (!_openDialog.ReadOnlyChecked)
			{
				_path = _openDialog.FileName;
			}
		}

		private void SaveClick(object sender, EventArgs e)
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

		private void SaveAsClick(object sender, EventArgs e)
		{
			SaveAs();
		}

		private void ExitClick(object sender, EventArgs e)
		{
			Close();
		}

		private void SelectedTypeChanged(object sender, EventArgs e)
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
					// This should never happen.
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

		private void AddClick(object sender, EventArgs e)
		{
			Insert(_navigation.SelectedNode, _navigation.SelectedNode.Nodes.Count);
		}

		private void InsertBeforeClick(object sender, EventArgs e)
		{
			TreeNode parent = _navigation.SelectedNode.Parent;
			if (parent == null)
			{
				return;
			}

			int selectedIndex = parent.Nodes.IndexOf(_navigation.SelectedNode);

			Insert(parent, selectedIndex);
		}

		private void InsertAfterClick(object sender, EventArgs e)
		{
			TreeNode parent = _navigation.SelectedNode.Parent;
			if (parent == null)
			{
				return;
			}

			int selectedIndex = parent.Nodes.IndexOf(_navigation.SelectedNode);

			Insert(parent, selectedIndex + 1);
		}

		private void DeleteClick(object sender, EventArgs e)
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

		private void CollapseClick(object sender, EventArgs e)
		{
			_navigation.SelectedNode.Collapse(false);
		}

		private void ExpandClick(object sender, EventArgs e)
		{
			_navigation.SelectedNode.ExpandAll();
		}
	}
}
