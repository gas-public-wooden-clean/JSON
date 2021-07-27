
namespace UI
{
	partial class WhitespaceControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhitespaceControl));
			this._layout = new System.Windows.Forms.TableLayoutPanel();
			this._literalLabel = new System.Windows.Forms.Label();
			this._literalValue = new System.Windows.Forms.TextBox();
			this._escapedLabel = new System.Windows.Forms.Label();
			this._escapedValue = new System.Windows.Forms.TextBox();
			this._layout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _layout
			// 
			resources.ApplyResources(this._layout, "_layout");
			this._layout.Controls.Add(this._literalLabel, 0, 0);
			this._layout.Controls.Add(this._literalValue, 1, 0);
			this._layout.Controls.Add(this._escapedLabel, 0, 1);
			this._layout.Controls.Add(this._escapedValue, 1, 1);
			this._layout.Name = "_layout";
			// 
			// _literalLabel
			// 
			resources.ApplyResources(this._literalLabel, "_literalLabel");
			this._literalLabel.Name = "_literalLabel";
			// 
			// _literalValue
			// 
			this._literalValue.AcceptsReturn = true;
			this._literalValue.AcceptsTab = true;
			resources.ApplyResources(this._literalValue, "_literalValue");
			this._literalValue.Name = "_literalValue";
			this._literalValue.TextChanged += new System.EventHandler(this.LiteralTextChanged);
			// 
			// _escapedLabel
			// 
			resources.ApplyResources(this._escapedLabel, "_escapedLabel");
			this._escapedLabel.Name = "_escapedLabel";
			// 
			// _escapedValue
			// 
			resources.ApplyResources(this._escapedValue, "_escapedValue");
			this._escapedValue.Name = "_escapedValue";
			this._escapedValue.TextChanged += new System.EventHandler(this.EscapedTextChanged);
			// 
			// WhitespaceControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._layout);
			this.Name = "WhitespaceControl";
			this._layout.ResumeLayout(false);
			this._layout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox _literalValue;
		private System.Windows.Forms.Label _literalLabel;
		private System.Windows.Forms.Label _escapedLabel;
		private System.Windows.Forms.TextBox _escapedValue;
		private System.Windows.Forms.TableLayoutPanel _layout;
	}
}
