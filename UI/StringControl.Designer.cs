
namespace UI
{
	partial class StringControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StringControl));
			this._layout = new System.Windows.Forms.TableLayoutPanel();
			this._leadingGroup = new System.Windows.Forms.GroupBox();
			this._leading = new UI.WhitespaceControl();
			this._trailingGroup = new System.Windows.Forms.GroupBox();
			this._trailing = new UI.WhitespaceControl();
			this._literalLabel = new System.Windows.Forms.Label();
			this._jsonLabel = new System.Windows.Forms.Label();
			this._literalValue = new System.Windows.Forms.TextBox();
			this._jsonValue = new System.Windows.Forms.TextBox();
			this._layout.SuspendLayout();
			this._leadingGroup.SuspendLayout();
			this._trailingGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _layout
			// 
			resources.ApplyResources(this._layout, "_layout");
			this._layout.Controls.Add(this._leadingGroup, 0, 0);
			this._layout.Controls.Add(this._trailingGroup, 0, 3);
			this._layout.Controls.Add(this._literalLabel, 0, 1);
			this._layout.Controls.Add(this._jsonLabel, 0, 2);
			this._layout.Controls.Add(this._literalValue, 1, 1);
			this._layout.Controls.Add(this._jsonValue, 1, 2);
			this._layout.Name = "_layout";
			// 
			// _leadingGroup
			// 
			this._layout.SetColumnSpan(this._leadingGroup, 2);
			this._leadingGroup.Controls.Add(this._leading);
			resources.ApplyResources(this._leadingGroup, "_leadingGroup");
			this._leadingGroup.Name = "_leadingGroup";
			this._leadingGroup.TabStop = false;
			// 
			// _leading
			// 
			resources.ApplyResources(this._leading, "_leading");
			this._leading.Name = "_leading";
			this._leading.ValueChanged += new System.EventHandler(this.ComponentChanged);
			// 
			// _trailingGroup
			// 
			this._layout.SetColumnSpan(this._trailingGroup, 2);
			this._trailingGroup.Controls.Add(this._trailing);
			resources.ApplyResources(this._trailingGroup, "_trailingGroup");
			this._trailingGroup.Name = "_trailingGroup";
			this._trailingGroup.TabStop = false;
			// 
			// _trailing
			// 
			resources.ApplyResources(this._trailing, "_trailing");
			this._trailing.Name = "_trailing";
			this._trailing.ValueChanged += new System.EventHandler(this.ComponentChanged);
			// 
			// _literalLabel
			// 
			resources.ApplyResources(this._literalLabel, "_literalLabel");
			this._literalLabel.Name = "_literalLabel";
			// 
			// _jsonLabel
			// 
			resources.ApplyResources(this._jsonLabel, "_jsonLabel");
			this._jsonLabel.Name = "_jsonLabel";
			// 
			// _literalValue
			// 
			this._literalValue.AcceptsReturn = true;
			this._literalValue.AcceptsTab = true;
			resources.ApplyResources(this._literalValue, "_literalValue");
			this._literalValue.Name = "_literalValue";
			this._literalValue.TextChanged += new System.EventHandler(this.LiteralTextChanged);
			// 
			// _jsonValue
			// 
			resources.ApplyResources(this._jsonValue, "_jsonValue");
			this._jsonValue.Name = "_jsonValue";
			this._jsonValue.TextChanged += new System.EventHandler(this.JsonTextChanged);
			// 
			// StringControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._layout);
			this.Name = "StringControl";
			this._layout.ResumeLayout(false);
			this._layout.PerformLayout();
			this._leadingGroup.ResumeLayout(false);
			this._trailingGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private WhitespaceControl _leading;
		private WhitespaceControl _trailing;
		private System.Windows.Forms.TextBox _literalValue;
		private System.Windows.Forms.TextBox _jsonValue;
		private System.Windows.Forms.TableLayoutPanel _layout;
		private System.Windows.Forms.GroupBox _leadingGroup;
		private System.Windows.Forms.GroupBox _trailingGroup;
		private System.Windows.Forms.Label _literalLabel;
		private System.Windows.Forms.Label _jsonLabel;
	}
}
