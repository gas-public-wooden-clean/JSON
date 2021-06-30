
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
			System.Windows.Forms.TableLayoutPanel _layout;
			this._literalValue = new System.Windows.Forms.TextBox();
			this._literalLabel = new System.Windows.Forms.Label();
			this._escapedLabel = new System.Windows.Forms.Label();
			this._escapedValue = new System.Windows.Forms.TextBox();
			_layout = new System.Windows.Forms.TableLayoutPanel();
			_layout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _literalValue
			// 
			this._literalValue.AcceptsReturn = true;
			this._literalValue.AcceptsTab = true;
			this._literalValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this._literalValue.Location = new System.Drawing.Point(58, 3);
			this._literalValue.Multiline = true;
			this._literalValue.Name = "_literalValue";
			this._literalValue.Size = new System.Drawing.Size(265, 149);
			this._literalValue.TabIndex = 1;
			this._literalValue.TextChanged += new System.EventHandler(this.LiteralTextChanged);
			// 
			// _layout
			// 
			_layout.ColumnCount = 2;
			_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
			_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			_layout.Controls.Add(this._literalLabel, 0, 0);
			_layout.Controls.Add(this._literalValue, 1, 0);
			_layout.Controls.Add(this._escapedLabel, 0, 1);
			_layout.Controls.Add(this._escapedValue, 1, 1);
			_layout.Dock = System.Windows.Forms.DockStyle.Fill;
			_layout.Location = new System.Drawing.Point(0, 0);
			_layout.Name = "_layout";
			_layout.RowCount = 2;
			_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			_layout.Size = new System.Drawing.Size(326, 311);
			_layout.TabIndex = 1;
			// 
			// _literalLabel
			// 
			this._literalLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._literalLabel.AutoSize = true;
			this._literalLabel.Location = new System.Drawing.Point(10, 71);
			this._literalLabel.Name = "_literalLabel";
			this._literalLabel.Size = new System.Drawing.Size(35, 13);
			this._literalLabel.TabIndex = 0;
			this._literalLabel.Text = "Literal";
			// 
			// _escapedLabel
			// 
			this._escapedLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._escapedLabel.AutoSize = true;
			this._escapedLabel.Location = new System.Drawing.Point(3, 226);
			this._escapedLabel.Name = "_escapedLabel";
			this._escapedLabel.Size = new System.Drawing.Size(49, 13);
			this._escapedLabel.TabIndex = 2;
			this._escapedLabel.Text = "Escaped";
			// 
			// _escapedValue
			// 
			this._escapedValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this._escapedValue.Location = new System.Drawing.Point(58, 158);
			this._escapedValue.Multiline = true;
			this._escapedValue.Name = "_escapedValue";
			this._escapedValue.Size = new System.Drawing.Size(265, 150);
			this._escapedValue.TabIndex = 3;
			this._escapedValue.TextChanged += new System.EventHandler(this.EscapedTextChanged);
			// 
			// WhitespaceControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(_layout);
			this.Name = "WhitespaceControl";
			this.Size = new System.Drawing.Size(326, 311);
			_layout.ResumeLayout(false);
			_layout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox _literalValue;
		private System.Windows.Forms.Label _literalLabel;
		private System.Windows.Forms.Label _escapedLabel;
		private System.Windows.Forms.TextBox _escapedValue;
	}
}
