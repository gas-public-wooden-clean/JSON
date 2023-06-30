
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
			this._expand = new System.Windows.Forms.ToolStripMenuItem();
			this._split = new System.Windows.Forms.SplitContainer();
			this._typeLabel = new System.Windows.Forms.Label();
			this._typeValue = new System.Windows.Forms.ComboBox();
			this._navigation = new System.Windows.Forms.TreeView();
			this._keyValueLayout = new System.Windows.Forms.TableLayoutPanel();
			this._keyControl = new UI.StringControl();
			this._valuePanel = new System.Windows.Forms.Panel();
			this._booleanControl = new UI.BooleanControl();
			this._numberControl = new UI.NumberControl();
			this._containerControl = new UI.ContainerControl();
			this._stringControl = new UI.StringControl();
			this._nullControl = new UI.NullControl();
			this._menu = new System.Windows.Forms.MenuStrip();
			this._file = new System.Windows.Forms.ToolStripMenuItem();
			this._new = new System.Windows.Forms.ToolStripMenuItem();
			this._open = new System.Windows.Forms.ToolStripMenuItem();
			this._save = new System.Windows.Forms.ToolStripMenuItem();
			this._saveAs = new System.Windows.Forms.ToolStripMenuItem();
			this._exit = new System.Windows.Forms.ToolStripMenuItem();
			this._edit = new System.Windows.Forms.ToolStripMenuItem();
			this._add = new System.Windows.Forms.ToolStripMenuItem();
			this._insertBefore = new System.Windows.Forms.ToolStripMenuItem();
			this._insertAfter = new System.Windows.Forms.ToolStripMenuItem();
			this._delete = new System.Windows.Forms.ToolStripMenuItem();
			this._view = new System.Windows.Forms.ToolStripMenuItem();
			this._collapse = new System.Windows.Forms.ToolStripMenuItem();
			this._encoding = new System.Windows.Forms.ToolStripMenuItem();
			this._autoOption = new System.Windows.Forms.ToolStripMenuItem();
			this._asciiOption = new System.Windows.Forms.ToolStripMenuItem();
			this._utf8Option = new System.Windows.Forms.ToolStripMenuItem();
			this._utf8bomOption = new System.Windows.Forms.ToolStripMenuItem();
			this._utf16leOption = new System.Windows.Forms.ToolStripMenuItem();
			this._utf16beOption = new System.Windows.Forms.ToolStripMenuItem();
			this._utf32leOption = new System.Windows.Forms.ToolStripMenuItem();
			this._utf32beOption = new System.Windows.Forms.ToolStripMenuItem();
			this._cp1252Option = new System.Windows.Forms.ToolStripMenuItem();
			this._openDialog = new System.Windows.Forms.OpenFileDialog();
			this._saveDialog = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this._split)).BeginInit();
			this._split.Panel1.SuspendLayout();
			this._split.Panel2.SuspendLayout();
			this._split.SuspendLayout();
			this._keyValueLayout.SuspendLayout();
			this._valuePanel.SuspendLayout();
			this._menu.SuspendLayout();
			this.SuspendLayout();
			// 
			// _expand
			// 
			this._expand.Name = "_expand";
			resources.ApplyResources(this._expand, "_expand");
			this._expand.Click += new System.EventHandler(this.ExpandClick);
			// 
			// _split
			// 
			resources.ApplyResources(this._split, "_split");
			this._split.Name = "_split";
			// 
			// _split.Panel1
			// 
			this._split.Panel1.Controls.Add(this._typeLabel);
			this._split.Panel1.Controls.Add(this._typeValue);
			this._split.Panel1.Controls.Add(this._navigation);
			// 
			// _split.Panel2
			// 
			this._split.Panel2.Controls.Add(this._keyValueLayout);
			// 
			// _typeLabel
			// 
			resources.ApplyResources(this._typeLabel, "_typeLabel");
			this._typeLabel.Name = "_typeLabel";
			// 
			// _typeValue
			// 
			resources.ApplyResources(this._typeValue, "_typeValue");
			this._typeValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._typeValue.FormattingEnabled = true;
			this._typeValue.Name = "_typeValue";
			this._typeValue.SelectedIndexChanged += new System.EventHandler(this.SelectedTypeChanged);
			// 
			// _navigation
			// 
			resources.ApplyResources(this._navigation, "_navigation");
			this._navigation.Name = "_navigation";
			this._navigation.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.BeforeSelect);
			this._navigation.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelect);
			// 
			// _keyValueLayout
			// 
			resources.ApplyResources(this._keyValueLayout, "_keyValueLayout");
			this._keyValueLayout.Controls.Add(this._keyControl, 0, 0);
			this._keyValueLayout.Controls.Add(this._valuePanel, 1, 0);
			this._keyValueLayout.Name = "_keyValueLayout";
			// 
			// _keyControl
			// 
			resources.ApplyResources(this._keyControl, "_keyControl");
			this._keyControl.Name = "_keyControl";
			this._keyControl.ValueChanged += new System.EventHandler(this.KeyChanged);
			// 
			// _valuePanel
			// 
			this._valuePanel.Controls.Add(this._booleanControl);
			this._valuePanel.Controls.Add(this._numberControl);
			this._valuePanel.Controls.Add(this._containerControl);
			this._valuePanel.Controls.Add(this._stringControl);
			this._valuePanel.Controls.Add(this._nullControl);
			resources.ApplyResources(this._valuePanel, "_valuePanel");
			this._valuePanel.Name = "_valuePanel";
			// 
			// _booleanControl
			// 
			resources.ApplyResources(this._booleanControl, "_booleanControl");
			this._booleanControl.Name = "_booleanControl";
			this._booleanControl.ValueChanged += new System.EventHandler(this.BooleanChanged);
			// 
			// _numberControl
			// 
			resources.ApplyResources(this._numberControl, "_numberControl");
			this._numberControl.Name = "_numberControl";
			this._numberControl.ValueChanged += new System.EventHandler(this.NumberChanged);
			// 
			// _containerControl
			// 
			resources.ApplyResources(this._containerControl, "_containerControl");
			this._containerControl.Name = "_containerControl";
			// 
			// _stringControl
			// 
			resources.ApplyResources(this._stringControl, "_stringControl");
			this._stringControl.Name = "_stringControl";
			this._stringControl.ValueChanged += new System.EventHandler(this.StringChanged);
			// 
			// _nullControl
			// 
			resources.ApplyResources(this._nullControl, "_nullControl");
			this._nullControl.Name = "_nullControl";
			// 
			// _menu
			// 
			this._menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._file,
            this._edit,
            this._view,
            this._encoding});
			resources.ApplyResources(this._menu, "_menu");
			this._menu.Name = "_menu";
			// 
			// _file
			// 
			this._file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._new,
            this._open,
            this._save,
            this._saveAs,
            this._exit});
			this._file.Name = "_file";
			resources.ApplyResources(this._file, "_file");
			// 
			// _new
			// 
			this._new.Name = "_new";
			resources.ApplyResources(this._new, "_new");
			this._new.Click += new System.EventHandler(this.NewClick);
			// 
			// _open
			// 
			this._open.Name = "_open";
			resources.ApplyResources(this._open, "_open");
			this._open.Click += new System.EventHandler(this.OpenClick);
			// 
			// _save
			// 
			this._save.Name = "_save";
			resources.ApplyResources(this._save, "_save");
			this._save.Click += new System.EventHandler(this.SaveClick);
			// 
			// _saveAs
			// 
			this._saveAs.Name = "_saveAs";
			resources.ApplyResources(this._saveAs, "_saveAs");
			this._saveAs.Click += new System.EventHandler(this.SaveAsClick);
			// 
			// _exit
			// 
			this._exit.Name = "_exit";
			resources.ApplyResources(this._exit, "_exit");
			this._exit.Click += new System.EventHandler(this.ExitClick);
			// 
			// _edit
			// 
			this._edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._add,
            this._insertBefore,
            this._insertAfter,
            this._delete});
			this._edit.Name = "_edit";
			resources.ApplyResources(this._edit, "_edit");
			// 
			// _add
			// 
			resources.ApplyResources(this._add, "_add");
			this._add.Name = "_add";
			this._add.Click += new System.EventHandler(this.AddClick);
			// 
			// _insertBefore
			// 
			resources.ApplyResources(this._insertBefore, "_insertBefore");
			this._insertBefore.Name = "_insertBefore";
			this._insertBefore.Click += new System.EventHandler(this.InsertBeforeClick);
			// 
			// _insertAfter
			// 
			resources.ApplyResources(this._insertAfter, "_insertAfter");
			this._insertAfter.Name = "_insertAfter";
			this._insertAfter.Click += new System.EventHandler(this.InsertAfterClick);
			// 
			// _delete
			// 
			resources.ApplyResources(this._delete, "_delete");
			this._delete.Name = "_delete";
			this._delete.Click += new System.EventHandler(this.DeleteClick);
			// 
			// _view
			// 
			this._view.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._collapse,
            this._expand});
			this._view.Name = "_view";
			resources.ApplyResources(this._view, "_view");
			// 
			// _collapse
			// 
			this._collapse.Name = "_collapse";
			resources.ApplyResources(this._collapse, "_collapse");
			this._collapse.Click += new System.EventHandler(this.CollapseClick);
			// 
			// _encoding
			// 
			this._encoding.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._autoOption,
            this._asciiOption,
            this._utf8Option,
            this._utf8bomOption,
            this._utf16leOption,
            this._utf16beOption,
            this._utf32leOption,
            this._utf32beOption,
            this._cp1252Option});
			this._encoding.Name = "_encoding";
			resources.ApplyResources(this._encoding, "_encoding");
			this._encoding.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.EncodingDropDownItemClicked);
			// 
			// _autoOption
			// 
			this._autoOption.Checked = true;
			this._autoOption.CheckState = System.Windows.Forms.CheckState.Checked;
			this._autoOption.Name = "_autoOption";
			resources.ApplyResources(this._autoOption, "_autoOption");
			// 
			// _asciiOption
			// 
			this._asciiOption.Name = "_asciiOption";
			resources.ApplyResources(this._asciiOption, "_asciiOption");
			// 
			// _utf8Option
			// 
			this._utf8Option.Name = "_utf8Option";
			resources.ApplyResources(this._utf8Option, "_utf8Option");
			// 
			// _utf8bomOption
			// 
			this._utf8bomOption.Name = "_utf8bomOption";
			resources.ApplyResources(this._utf8bomOption, "_utf8bomOption");
			// 
			// _utf16leOption
			// 
			this._utf16leOption.Name = "_utf16leOption";
			resources.ApplyResources(this._utf16leOption, "_utf16leOption");
			// 
			// _utf16beOption
			// 
			this._utf16beOption.Name = "_utf16beOption";
			resources.ApplyResources(this._utf16beOption, "_utf16beOption");
			// 
			// _utf32leOption
			// 
			this._utf32leOption.Name = "_utf32leOption";
			resources.ApplyResources(this._utf32leOption, "_utf32leOption");
			// 
			// _utf32beOption
			// 
			this._utf32beOption.Name = "_utf32beOption";
			resources.ApplyResources(this._utf32beOption, "_utf32beOption");
			// 
			// _cp1252Option
			// 
			this._cp1252Option.Name = "_cp1252Option";
			resources.ApplyResources(this._cp1252Option, "_cp1252Option");
			// 
			// _openDialog
			// 
			this._openDialog.DefaultExt = "json";
			resources.ApplyResources(this._openDialog, "_openDialog");
			this._openDialog.SupportMultiDottedExtensions = true;
			// 
			// _saveDialog
			// 
			this._saveDialog.DefaultExt = "json";
			resources.ApplyResources(this._saveDialog, "_saveDialog");
			this._saveDialog.SupportMultiDottedExtensions = true;
			// 
			// EditJson
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._split);
			this.Controls.Add(this._menu);
			this.MainMenuStrip = this._menu;
			this.Name = "EditJson";
			this._split.Panel1.ResumeLayout(false);
			this._split.Panel1.PerformLayout();
			this._split.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._split)).EndInit();
			this._split.ResumeLayout(false);
			this._keyValueLayout.ResumeLayout(false);
			this._valuePanel.ResumeLayout(false);
			this._menu.ResumeLayout(false);
			this._menu.PerformLayout();
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
	}
}

