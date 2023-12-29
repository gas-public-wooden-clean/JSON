
namespace UI
{
	partial class EditJson
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditJson));
			_expand = new System.Windows.Forms.ToolStripMenuItem();
			_split = new System.Windows.Forms.SplitContainer();
			_typeLabel = new System.Windows.Forms.Label();
			_typeValue = new System.Windows.Forms.ComboBox();
			_navigation = new System.Windows.Forms.TreeView();
			_keyValueLayout = new System.Windows.Forms.TableLayoutPanel();
			_keyControl = new StringControl();
			_valuePanel = new System.Windows.Forms.Panel();
			_booleanControl = new BooleanControl();
			_numberControl = new NumberControl();
			_containerControl = new ContainerControl();
			_stringControl = new StringControl();
			_nullControl = new NullControl();
			_menu = new System.Windows.Forms.MenuStrip();
			_file = new System.Windows.Forms.ToolStripMenuItem();
			_new = new System.Windows.Forms.ToolStripMenuItem();
			_open = new System.Windows.Forms.ToolStripMenuItem();
			_save = new System.Windows.Forms.ToolStripMenuItem();
			_saveAs = new System.Windows.Forms.ToolStripMenuItem();
			_exit = new System.Windows.Forms.ToolStripMenuItem();
			_edit = new System.Windows.Forms.ToolStripMenuItem();
			_add = new System.Windows.Forms.ToolStripMenuItem();
			_insertBefore = new System.Windows.Forms.ToolStripMenuItem();
			_insertAfter = new System.Windows.Forms.ToolStripMenuItem();
			_delete = new System.Windows.Forms.ToolStripMenuItem();
			_view = new System.Windows.Forms.ToolStripMenuItem();
			_collapse = new System.Windows.Forms.ToolStripMenuItem();
			_encoding = new System.Windows.Forms.ToolStripMenuItem();
			_autoOption = new System.Windows.Forms.ToolStripMenuItem();
			_asciiOption = new System.Windows.Forms.ToolStripMenuItem();
			_utf8Option = new System.Windows.Forms.ToolStripMenuItem();
			_utf8bomOption = new System.Windows.Forms.ToolStripMenuItem();
			_utf16leOption = new System.Windows.Forms.ToolStripMenuItem();
			_utf16beOption = new System.Windows.Forms.ToolStripMenuItem();
			_utf32leOption = new System.Windows.Forms.ToolStripMenuItem();
			_utf32beOption = new System.Windows.Forms.ToolStripMenuItem();
			_cp1252Option = new System.Windows.Forms.ToolStripMenuItem();
			_openDialog = new System.Windows.Forms.OpenFileDialog();
			_saveDialog = new System.Windows.Forms.SaveFileDialog();
			_cp437Option = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)_split).BeginInit();
			_split.Panel1.SuspendLayout();
			_split.Panel2.SuspendLayout();
			_split.SuspendLayout();
			_keyValueLayout.SuspendLayout();
			_valuePanel.SuspendLayout();
			_menu.SuspendLayout();
			SuspendLayout();
			// 
			// _expand
			// 
			_expand.Name = "_expand";
			resources.ApplyResources(_expand, "_expand");
			_expand.Click += ExpandClick;
			// 
			// _split
			// 
			resources.ApplyResources(_split, "_split");
			_split.Name = "_split";
			// 
			// _split.Panel1
			// 
			_split.Panel1.Controls.Add(_typeLabel);
			_split.Panel1.Controls.Add(_typeValue);
			_split.Panel1.Controls.Add(_navigation);
			// 
			// _split.Panel2
			// 
			_split.Panel2.Controls.Add(_keyValueLayout);
			// 
			// _typeLabel
			// 
			resources.ApplyResources(_typeLabel, "_typeLabel");
			_typeLabel.Name = "_typeLabel";
			// 
			// _typeValue
			// 
			resources.ApplyResources(_typeValue, "_typeValue");
			_typeValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			_typeValue.FormattingEnabled = true;
			_typeValue.Name = "_typeValue";
			_typeValue.SelectedIndexChanged += SelectedTypeChanged;
			// 
			// _navigation
			// 
			resources.ApplyResources(_navigation, "_navigation");
			_navigation.Name = "_navigation";
			_navigation.BeforeSelect += BeforeSelect;
			_navigation.AfterSelect += AfterSelect;
			// 
			// _keyValueLayout
			// 
			resources.ApplyResources(_keyValueLayout, "_keyValueLayout");
			_keyValueLayout.Controls.Add(_keyControl, 0, 0);
			_keyValueLayout.Controls.Add(_valuePanel, 1, 0);
			_keyValueLayout.Name = "_keyValueLayout";
			// 
			// _keyControl
			// 
			resources.ApplyResources(_keyControl, "_keyControl");
			_keyControl.Name = "_keyControl";
			_keyControl.ValueChanged += KeyChanged;
			// 
			// _valuePanel
			// 
			_valuePanel.Controls.Add(_booleanControl);
			_valuePanel.Controls.Add(_numberControl);
			_valuePanel.Controls.Add(_containerControl);
			_valuePanel.Controls.Add(_stringControl);
			_valuePanel.Controls.Add(_nullControl);
			resources.ApplyResources(_valuePanel, "_valuePanel");
			_valuePanel.Name = "_valuePanel";
			// 
			// _booleanControl
			// 
			resources.ApplyResources(_booleanControl, "_booleanControl");
			_booleanControl.Name = "_booleanControl";
			_booleanControl.ValueChanged += BooleanChanged;
			// 
			// _numberControl
			// 
			resources.ApplyResources(_numberControl, "_numberControl");
			_numberControl.Name = "_numberControl";
			_numberControl.ValueChanged += NumberChanged;
			// 
			// _containerControl
			// 
			resources.ApplyResources(_containerControl, "_containerControl");
			_containerControl.Name = "_containerControl";
			// 
			// _stringControl
			// 
			resources.ApplyResources(_stringControl, "_stringControl");
			_stringControl.Name = "_stringControl";
			_stringControl.ValueChanged += StringChanged;
			// 
			// _nullControl
			// 
			resources.ApplyResources(_nullControl, "_nullControl");
			_nullControl.Name = "_nullControl";
			// 
			// _menu
			// 
			_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _file, _edit, _view, _encoding });
			resources.ApplyResources(_menu, "_menu");
			_menu.Name = "_menu";
			// 
			// _file
			// 
			_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _new, _open, _save, _saveAs, _exit });
			_file.Name = "_file";
			resources.ApplyResources(_file, "_file");
			// 
			// _new
			// 
			_new.Name = "_new";
			resources.ApplyResources(_new, "_new");
			_new.Click += NewClick;
			// 
			// _open
			// 
			_open.Name = "_open";
			resources.ApplyResources(_open, "_open");
			_open.Click += OpenClick;
			// 
			// _save
			// 
			_save.Name = "_save";
			resources.ApplyResources(_save, "_save");
			_save.Click += SaveClick;
			// 
			// _saveAs
			// 
			_saveAs.Name = "_saveAs";
			resources.ApplyResources(_saveAs, "_saveAs");
			_saveAs.Click += SaveAsClick;
			// 
			// _exit
			// 
			_exit.Name = "_exit";
			resources.ApplyResources(_exit, "_exit");
			_exit.Click += ExitClick;
			// 
			// _edit
			// 
			_edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _add, _insertBefore, _insertAfter, _delete });
			_edit.Name = "_edit";
			resources.ApplyResources(_edit, "_edit");
			// 
			// _add
			// 
			resources.ApplyResources(_add, "_add");
			_add.Name = "_add";
			_add.Click += AddClick;
			// 
			// _insertBefore
			// 
			resources.ApplyResources(_insertBefore, "_insertBefore");
			_insertBefore.Name = "_insertBefore";
			_insertBefore.Click += InsertBeforeClick;
			// 
			// _insertAfter
			// 
			resources.ApplyResources(_insertAfter, "_insertAfter");
			_insertAfter.Name = "_insertAfter";
			_insertAfter.Click += InsertAfterClick;
			// 
			// _delete
			// 
			resources.ApplyResources(_delete, "_delete");
			_delete.Name = "_delete";
			_delete.Click += DeleteClick;
			// 
			// _view
			// 
			_view.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _collapse, _expand });
			_view.Name = "_view";
			resources.ApplyResources(_view, "_view");
			// 
			// _collapse
			// 
			_collapse.Name = "_collapse";
			resources.ApplyResources(_collapse, "_collapse");
			_collapse.Click += CollapseClick;
			// 
			// _encoding
			// 
			_encoding.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _autoOption, _asciiOption, _utf8Option, _utf8bomOption, _utf16leOption, _utf16beOption, _utf32leOption, _utf32beOption, _cp1252Option, _cp437Option });
			_encoding.Name = "_encoding";
			resources.ApplyResources(_encoding, "_encoding");
			_encoding.DropDownItemClicked += EncodingDropDownItemClicked;
			// 
			// _autoOption
			// 
			_autoOption.Checked = true;
			_autoOption.CheckState = System.Windows.Forms.CheckState.Checked;
			_autoOption.Name = "_autoOption";
			resources.ApplyResources(_autoOption, "_autoOption");
			// 
			// _asciiOption
			// 
			_asciiOption.Name = "_asciiOption";
			resources.ApplyResources(_asciiOption, "_asciiOption");
			// 
			// _utf8Option
			// 
			_utf8Option.Name = "_utf8Option";
			resources.ApplyResources(_utf8Option, "_utf8Option");
			// 
			// _utf8bomOption
			// 
			_utf8bomOption.Name = "_utf8bomOption";
			resources.ApplyResources(_utf8bomOption, "_utf8bomOption");
			// 
			// _utf16leOption
			// 
			_utf16leOption.Name = "_utf16leOption";
			resources.ApplyResources(_utf16leOption, "_utf16leOption");
			// 
			// _utf16beOption
			// 
			_utf16beOption.Name = "_utf16beOption";
			resources.ApplyResources(_utf16beOption, "_utf16beOption");
			// 
			// _utf32leOption
			// 
			_utf32leOption.Name = "_utf32leOption";
			resources.ApplyResources(_utf32leOption, "_utf32leOption");
			// 
			// _utf32beOption
			// 
			_utf32beOption.Name = "_utf32beOption";
			resources.ApplyResources(_utf32beOption, "_utf32beOption");
			// 
			// _cp1252Option
			// 
			_cp1252Option.Name = "_cp1252Option";
			resources.ApplyResources(_cp1252Option, "_cp1252Option");
			// 
			// _openDialog
			// 
			_openDialog.DefaultExt = "json";
			resources.ApplyResources(_openDialog, "_openDialog");
			_openDialog.SupportMultiDottedExtensions = true;
			// 
			// _saveDialog
			// 
			_saveDialog.DefaultExt = "json";
			resources.ApplyResources(_saveDialog, "_saveDialog");
			_saveDialog.SupportMultiDottedExtensions = true;
			// 
			// _cp437Option
			// 
			_cp437Option.Name = "_cp437Option";
			resources.ApplyResources(_cp437Option, "_cp437Option");
			// 
			// EditJson
			// 
			resources.ApplyResources(this, "$this");
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Controls.Add(_split);
			Controls.Add(_menu);
			MainMenuStrip = _menu;
			Name = "EditJson";
			_split.Panel1.ResumeLayout(false);
			_split.Panel1.PerformLayout();
			_split.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)_split).EndInit();
			_split.ResumeLayout(false);
			_keyValueLayout.ResumeLayout(false);
			_valuePanel.ResumeLayout(false);
			_menu.ResumeLayout(false);
			_menu.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private System.Windows.Forms.TreeView _navigation;
		private NullControl _nullControl;
		private System.Windows.Forms.OpenFileDialog _openDialog;
		private System.Windows.Forms.SaveFileDialog _saveDialog;
		private System.Windows.Forms.ComboBox _typeValue;
		private StringControl _keyControl;
		private StringControl _stringControl;
		private ContainerControl _containerControl;
		private System.Windows.Forms.TableLayoutPanel _keyValueLayout;
		private System.Windows.Forms.ToolStripMenuItem _add;
		private System.Windows.Forms.ToolStripMenuItem _insertBefore;
		private System.Windows.Forms.ToolStripMenuItem _insertAfter;
		private System.Windows.Forms.ToolStripMenuItem _delete;
		private NumberControl _numberControl;
		private BooleanControl _booleanControl;
		private System.Windows.Forms.ToolStripMenuItem _asciiOption;
		private System.Windows.Forms.ToolStripMenuItem _utf8Option;
		private System.Windows.Forms.ToolStripMenuItem _utf8bomOption;
		private System.Windows.Forms.ToolStripMenuItem _utf16leOption;
		private System.Windows.Forms.ToolStripMenuItem _utf16beOption;
		private System.Windows.Forms.ToolStripMenuItem _utf32leOption;
		private System.Windows.Forms.ToolStripMenuItem _utf32beOption;
		private System.Windows.Forms.ToolStripMenuItem _autoOption;
		private System.Windows.Forms.ToolStripMenuItem _cp1252Option;
		private System.Windows.Forms.SplitContainer _split;
		private System.Windows.Forms.Label _typeLabel;
		private System.Windows.Forms.Panel _valuePanel;
		private System.Windows.Forms.MenuStrip _menu;
		private System.Windows.Forms.ToolStripMenuItem _file;
		private System.Windows.Forms.ToolStripMenuItem _new;
		private System.Windows.Forms.ToolStripMenuItem _open;
		private System.Windows.Forms.ToolStripMenuItem _save;
		private System.Windows.Forms.ToolStripMenuItem _saveAs;
		private System.Windows.Forms.ToolStripMenuItem _exit;
		private System.Windows.Forms.ToolStripMenuItem _edit;
		private System.Windows.Forms.ToolStripMenuItem _view;
		private System.Windows.Forms.ToolStripMenuItem _collapse;
		private System.Windows.Forms.ToolStripMenuItem _encoding;
		private System.Windows.Forms.ToolStripMenuItem _expand;
		private System.Windows.Forms.ToolStripMenuItem _cp437Option;
	}
}

