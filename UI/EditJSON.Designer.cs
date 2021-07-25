
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
			System.Windows.Forms.SplitContainer _split;
			System.Windows.Forms.Label _typeLabel;
			System.Windows.Forms.Panel _valuePanel;
			System.Windows.Forms.MenuStrip _menu;
			System.Windows.Forms.ToolStripMenuItem _file;
			System.Windows.Forms.ToolStripMenuItem _new;
			System.Windows.Forms.ToolStripMenuItem _open;
			System.Windows.Forms.ToolStripMenuItem _save;
			System.Windows.Forms.ToolStripMenuItem _saveAs;
			System.Windows.Forms.ToolStripMenuItem _exit;
			System.Windows.Forms.ToolStripMenuItem _edit;
			System.Windows.Forms.ToolStripMenuItem _view;
			System.Windows.Forms.ToolStripMenuItem _collapse;
			System.Windows.Forms.ToolStripMenuItem _expand;
			System.Windows.Forms.ToolStripMenuItem _encoding;
			this._typeValue = new System.Windows.Forms.ComboBox();
			this._navigation = new System.Windows.Forms.TreeView();
			this._keyValueLayout = new System.Windows.Forms.TableLayoutPanel();
			this._keyControl = new UI.StringControl();
			this._booleanControl = new UI.BooleanControl();
			this._numberControl = new UI.NumberControl();
			this._containerControl = new UI.ContainerControl();
			this._stringControl = new UI.StringControl();
			this._nullControl = new UI.NullControl();
			this._add = new System.Windows.Forms.ToolStripMenuItem();
			this._insertBefore = new System.Windows.Forms.ToolStripMenuItem();
			this._insertAfter = new System.Windows.Forms.ToolStripMenuItem();
			this._delete = new System.Windows.Forms.ToolStripMenuItem();
			this._autoOption = new System.Windows.Forms.ToolStripMenuItem();
			this._asciiOption = new System.Windows.Forms.ToolStripMenuItem();
			this._utf8Option = new System.Windows.Forms.ToolStripMenuItem();
			this._utf8bomOption = new System.Windows.Forms.ToolStripMenuItem();
			this._utf16leOption = new System.Windows.Forms.ToolStripMenuItem();
			this._utf16beOption = new System.Windows.Forms.ToolStripMenuItem();
			this._utf32leOption = new System.Windows.Forms.ToolStripMenuItem();
			this._utf32beOption = new System.Windows.Forms.ToolStripMenuItem();
			this._openDialog = new System.Windows.Forms.OpenFileDialog();
			this._saveDialog = new System.Windows.Forms.SaveFileDialog();
			this._cp1252Option = new System.Windows.Forms.ToolStripMenuItem();
			_split = new System.Windows.Forms.SplitContainer();
			_typeLabel = new System.Windows.Forms.Label();
			_valuePanel = new System.Windows.Forms.Panel();
			_menu = new System.Windows.Forms.MenuStrip();
			_file = new System.Windows.Forms.ToolStripMenuItem();
			_new = new System.Windows.Forms.ToolStripMenuItem();
			_open = new System.Windows.Forms.ToolStripMenuItem();
			_save = new System.Windows.Forms.ToolStripMenuItem();
			_saveAs = new System.Windows.Forms.ToolStripMenuItem();
			_exit = new System.Windows.Forms.ToolStripMenuItem();
			_edit = new System.Windows.Forms.ToolStripMenuItem();
			_view = new System.Windows.Forms.ToolStripMenuItem();
			_collapse = new System.Windows.Forms.ToolStripMenuItem();
			_expand = new System.Windows.Forms.ToolStripMenuItem();
			_encoding = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(_split)).BeginInit();
			_split.Panel1.SuspendLayout();
			_split.Panel2.SuspendLayout();
			_split.SuspendLayout();
			this._keyValueLayout.SuspendLayout();
			_valuePanel.SuspendLayout();
			_menu.SuspendLayout();
			this.SuspendLayout();
			// 
			// _split
			// 
			_split.Dock = System.Windows.Forms.DockStyle.Fill;
			_split.Location = new System.Drawing.Point(0, 24);
			_split.Name = "_split";
			// 
			// _split.Panel1
			// 
			_split.Panel1.Controls.Add(_typeLabel);
			_split.Panel1.Controls.Add(this._typeValue);
			_split.Panel1.Controls.Add(this._navigation);
			// 
			// _split.Panel2
			// 
			_split.Panel2.Controls.Add(this._keyValueLayout);
			_split.Size = new System.Drawing.Size(800, 426);
			_split.SplitterDistance = 265;
			_split.TabIndex = 0;
			// 
			// _typeLabel
			// 
			_typeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			_typeLabel.AutoSize = true;
			_typeLabel.Location = new System.Drawing.Point(12, 405);
			_typeLabel.Name = "_typeLabel";
			_typeLabel.Size = new System.Drawing.Size(31, 13);
			_typeLabel.TabIndex = 2;
			_typeLabel.Text = "Type";
			// 
			// _typeValue
			// 
			this._typeValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._typeValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._typeValue.FormattingEnabled = true;
			this._typeValue.Location = new System.Drawing.Point(49, 402);
			this._typeValue.Name = "_typeValue";
			this._typeValue.Size = new System.Drawing.Size(214, 21);
			this._typeValue.TabIndex = 1;
			this._typeValue.SelectedIndexChanged += new System.EventHandler(this.SelectedTypeChanged);
			// 
			// _navigation
			// 
			this._navigation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._navigation.Location = new System.Drawing.Point(3, 3);
			this._navigation.Name = "_navigation";
			this._navigation.Size = new System.Drawing.Size(259, 393);
			this._navigation.TabIndex = 0;
			this._navigation.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.BeforeSelect);
			this._navigation.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelect);
			// 
			// _keyValueLayout
			// 
			this._keyValueLayout.ColumnCount = 2;
			this._keyValueLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._keyValueLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._keyValueLayout.Controls.Add(this._keyControl, 0, 0);
			this._keyValueLayout.Controls.Add(_valuePanel, 1, 0);
			this._keyValueLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._keyValueLayout.Location = new System.Drawing.Point(0, 0);
			this._keyValueLayout.Name = "_keyValueLayout";
			this._keyValueLayout.RowCount = 1;
			this._keyValueLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._keyValueLayout.Size = new System.Drawing.Size(531, 426);
			this._keyValueLayout.TabIndex = 1;
			// 
			// _keyControl
			// 
			this._keyControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._keyControl.Location = new System.Drawing.Point(3, 3);
			this._keyControl.Name = "_keyControl";
			this._keyControl.Size = new System.Drawing.Size(259, 420);
			this._keyControl.TabIndex = 1;
			// 
			// _valuePanel
			// 
			_valuePanel.Controls.Add(this._booleanControl);
			_valuePanel.Controls.Add(this._numberControl);
			_valuePanel.Controls.Add(this._containerControl);
			_valuePanel.Controls.Add(this._stringControl);
			_valuePanel.Controls.Add(this._nullControl);
			_valuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			_valuePanel.Location = new System.Drawing.Point(268, 3);
			_valuePanel.Name = "_valuePanel";
			_valuePanel.Size = new System.Drawing.Size(260, 420);
			_valuePanel.TabIndex = 2;
			// 
			// _booleanControl
			// 
			this._booleanControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._booleanControl.Location = new System.Drawing.Point(0, 0);
			this._booleanControl.Name = "_booleanControl";
			this._booleanControl.Size = new System.Drawing.Size(260, 420);
			this._booleanControl.TabIndex = 4;
			// 
			// _numberControl
			// 
			this._numberControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._numberControl.Location = new System.Drawing.Point(0, 0);
			this._numberControl.Name = "_numberControl";
			this._numberControl.Size = new System.Drawing.Size(260, 420);
			this._numberControl.TabIndex = 3;
			// 
			// _containerControl
			// 
			this._containerControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._containerControl.Location = new System.Drawing.Point(0, 0);
			this._containerControl.Name = "_containerControl";
			this._containerControl.Size = new System.Drawing.Size(260, 420);
			this._containerControl.TabIndex = 2;
			// 
			// _stringControl
			// 
			this._stringControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._stringControl.Location = new System.Drawing.Point(0, 0);
			this._stringControl.Name = "_stringControl";
			this._stringControl.Size = new System.Drawing.Size(260, 420);
			this._stringControl.TabIndex = 1;
			// 
			// _nullControl
			// 
			this._nullControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._nullControl.Location = new System.Drawing.Point(0, 0);
			this._nullControl.Name = "_nullControl";
			this._nullControl.Size = new System.Drawing.Size(260, 420);
			this._nullControl.TabIndex = 0;
			this._nullControl.Visible = false;
			// 
			// _menu
			// 
			_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            _file,
            _edit,
            _view,
            _encoding});
			_menu.Location = new System.Drawing.Point(0, 0);
			_menu.Name = "_menu";
			_menu.Size = new System.Drawing.Size(800, 24);
			_menu.TabIndex = 1;
			_menu.Text = "Menu";
			// 
			// _file
			// 
			_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            _new,
            _open,
            _save,
            _saveAs,
            _exit});
			_file.Name = "_file";
			_file.Size = new System.Drawing.Size(37, 20);
			_file.Text = "File";
			// 
			// _new
			// 
			_new.Name = "_new";
			_new.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			_new.Size = new System.Drawing.Size(195, 22);
			_new.Text = "New";
			_new.Click += new System.EventHandler(this.NewClick);
			// 
			// _open
			// 
			_open.Name = "_open";
			_open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			_open.Size = new System.Drawing.Size(195, 22);
			_open.Text = "Open...";
			_open.Click += new System.EventHandler(this.OpenClick);
			// 
			// _save
			// 
			_save.Name = "_save";
			_save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			_save.Size = new System.Drawing.Size(195, 22);
			_save.Text = "Save";
			_save.Click += new System.EventHandler(this.SaveClick);
			// 
			// _saveAs
			// 
			_saveAs.Name = "_saveAs";
			_saveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			_saveAs.Size = new System.Drawing.Size(195, 22);
			_saveAs.Text = "Save As...";
			_saveAs.Click += new System.EventHandler(this.SaveAsClick);
			// 
			// _exit
			// 
			_exit.Name = "_exit";
			_exit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			_exit.Size = new System.Drawing.Size(195, 22);
			_exit.Text = "Exit";
			_exit.Click += new System.EventHandler(this.ExitClick);
			// 
			// _edit
			// 
			_edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._add,
            this._insertBefore,
            this._insertAfter,
            this._delete});
			_edit.Name = "_edit";
			_edit.Size = new System.Drawing.Size(39, 20);
			_edit.Text = "Edit";
			// 
			// _add
			// 
			this._add.Enabled = false;
			this._add.Name = "_add";
			this._add.Size = new System.Drawing.Size(140, 22);
			this._add.Text = "Add";
			this._add.Click += new System.EventHandler(this.AddClick);
			// 
			// _insertBefore
			// 
			this._insertBefore.Enabled = false;
			this._insertBefore.Name = "_insertBefore";
			this._insertBefore.Size = new System.Drawing.Size(140, 22);
			this._insertBefore.Text = "Insert Before";
			this._insertBefore.Click += new System.EventHandler(this.InsertBeforeClick);
			// 
			// _insertAfter
			// 
			this._insertAfter.Enabled = false;
			this._insertAfter.Name = "_insertAfter";
			this._insertAfter.Size = new System.Drawing.Size(140, 22);
			this._insertAfter.Text = "Insert After";
			this._insertAfter.Click += new System.EventHandler(this.InsertAfterClick);
			// 
			// _delete
			// 
			this._delete.Enabled = false;
			this._delete.Name = "_delete";
			this._delete.Size = new System.Drawing.Size(140, 22);
			this._delete.Text = "Delete";
			this._delete.Click += new System.EventHandler(this.DeleteClick);
			// 
			// _view
			// 
			_view.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            _collapse,
            _expand});
			_view.Name = "_view";
			_view.Size = new System.Drawing.Size(44, 20);
			_view.Text = "View";
			// 
			// _collapse
			// 
			_collapse.Name = "_collapse";
			_collapse.Size = new System.Drawing.Size(136, 22);
			_collapse.Text = "Collapse All";
			_collapse.Click += new System.EventHandler(this.CollapseClick);
			// 
			// _expand
			// 
			_expand.Name = "_expand";
			_expand.Size = new System.Drawing.Size(136, 22);
			_expand.Text = "Expand All";
			_expand.Click += new System.EventHandler(this.ExpandClick);
			// 
			// _encoding
			// 
			_encoding.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._autoOption,
            this._asciiOption,
            this._utf8Option,
            this._utf8bomOption,
            this._utf16leOption,
            this._utf16beOption,
            this._utf32leOption,
            this._utf32beOption,
            this._cp1252Option});
			_encoding.Name = "_encoding";
			_encoding.Size = new System.Drawing.Size(69, 20);
			_encoding.Text = "Encoding";
			_encoding.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.EncodingDropDownItemClicked);
			// 
			// _autoOption
			// 
			this._autoOption.Checked = true;
			this._autoOption.CheckState = System.Windows.Forms.CheckState.Checked;
			this._autoOption.Name = "_autoOption";
			this._autoOption.Size = new System.Drawing.Size(180, 22);
			this._autoOption.Text = "Auto (UTF-8)";
			// 
			// _asciiOption
			// 
			this._asciiOption.Name = "_asciiOption";
			this._asciiOption.Size = new System.Drawing.Size(180, 22);
			this._asciiOption.Text = "ASCII";
			// 
			// _utf8Option
			// 
			this._utf8Option.Name = "_utf8Option";
			this._utf8Option.Size = new System.Drawing.Size(180, 22);
			this._utf8Option.Text = "UTF-8";
			// 
			// _utf8bomOption
			// 
			this._utf8bomOption.Name = "_utf8bomOption";
			this._utf8bomOption.Size = new System.Drawing.Size(180, 22);
			this._utf8bomOption.Text = "UTF-8-BOM";
			// 
			// _utf16leOption
			// 
			this._utf16leOption.Name = "_utf16leOption";
			this._utf16leOption.Size = new System.Drawing.Size(180, 22);
			this._utf16leOption.Text = "UTF-16 LE BOM";
			// 
			// _utf16beOption
			// 
			this._utf16beOption.Name = "_utf16beOption";
			this._utf16beOption.Size = new System.Drawing.Size(180, 22);
			this._utf16beOption.Text = "UTF-16 BE BOM";
			// 
			// _utf32leOption
			// 
			this._utf32leOption.Name = "_utf32leOption";
			this._utf32leOption.Size = new System.Drawing.Size(180, 22);
			this._utf32leOption.Text = "UTF-32 LE BOM";
			// 
			// _utf32beOption
			// 
			this._utf32beOption.Name = "_utf32beOption";
			this._utf32beOption.Size = new System.Drawing.Size(180, 22);
			this._utf32beOption.Text = "UTF-32 BE BOM";
			// 
			// _openDialog
			// 
			this._openDialog.DefaultExt = "json";
			this._openDialog.Filter = "JSON files|*.json|All files|*.*";
			this._openDialog.SupportMultiDottedExtensions = true;
			// 
			// _saveDialog
			// 
			this._saveDialog.DefaultExt = "json";
			this._saveDialog.Filter = "JSON files|*.json|All files|*.*";
			this._saveDialog.SupportMultiDottedExtensions = true;
			// 
			// _cp1252Option
			// 
			this._cp1252Option.Name = "_cp1252Option";
			this._cp1252Option.Size = new System.Drawing.Size(180, 22);
			this._cp1252Option.Text = "CP-1252";
			// 
			// EditJson
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(_split);
			this.Controls.Add(_menu);
			this.MainMenuStrip = _menu;
			this.Name = "EditJson";
			this.Text = "EditJson";
			_split.Panel1.ResumeLayout(false);
			_split.Panel1.PerformLayout();
			_split.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(_split)).EndInit();
			_split.ResumeLayout(false);
			this._keyValueLayout.ResumeLayout(false);
			_valuePanel.ResumeLayout(false);
			_menu.ResumeLayout(false);
			_menu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

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
	}
}

