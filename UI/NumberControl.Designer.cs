
namespace UI
{
	partial class NumberControl
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
			System.Windows.Forms.GroupBox _leadingGroup;
			System.Windows.Forms.GroupBox _trailingGroup;
			System.Windows.Forms.Label _valueLabel;
			System.Windows.Forms.Label _jsonLabel;
			this._leading = new UI.WhiteSpaceControl();
			this._trailing = new UI.WhiteSpaceControl();
			this._valueText = new System.Windows.Forms.TextBox();
			this._jsonValue = new System.Windows.Forms.TextBox();
			_layout = new System.Windows.Forms.TableLayoutPanel();
			_leadingGroup = new System.Windows.Forms.GroupBox();
			_trailingGroup = new System.Windows.Forms.GroupBox();
			_valueLabel = new System.Windows.Forms.Label();
			_jsonLabel = new System.Windows.Forms.Label();
			_layout.SuspendLayout();
			_leadingGroup.SuspendLayout();
			_trailingGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _layout
			// 
			_layout.ColumnCount = 2;
			_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
			_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			_layout.Controls.Add(_leadingGroup, 0, 0);
			_layout.Controls.Add(_trailingGroup, 0, 3);
			_layout.Controls.Add(_valueLabel, 0, 1);
			_layout.Controls.Add(_jsonLabel, 0, 2);
			_layout.Controls.Add(this._valueText, 1, 1);
			_layout.Controls.Add(this._jsonValue, 1, 2);
			_layout.Dock = System.Windows.Forms.DockStyle.Fill;
			_layout.Location = new System.Drawing.Point(0, 0);
			_layout.Name = "_layout";
			_layout.RowCount = 4;
			_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			_layout.Size = new System.Drawing.Size(297, 329);
			_layout.TabIndex = 1;
			// 
			// _leadingGroup
			// 
			_leadingGroup.CausesValidation = false;
			_layout.SetColumnSpan(_leadingGroup, 2);
			_leadingGroup.Controls.Add(this._leading);
			_leadingGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			_leadingGroup.Location = new System.Drawing.Point(3, 3);
			_leadingGroup.Name = "_leadingGroup";
			_leadingGroup.Size = new System.Drawing.Size(291, 76);
			_leadingGroup.TabIndex = 0;
			_leadingGroup.TabStop = false;
			_leadingGroup.Text = "Leading Whitespace";
			// 
			// _leading
			// 
			this._leading.Dock = System.Windows.Forms.DockStyle.Fill;
			this._leading.Location = new System.Drawing.Point(3, 16);
			this._leading.Name = "_leading";
			this._leading.Size = new System.Drawing.Size(285, 57);
			this._leading.TabIndex = 0;
			// 
			// _trailingGroup
			// 
			_layout.SetColumnSpan(_trailingGroup, 2);
			_trailingGroup.Controls.Add(this._trailing);
			_trailingGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			_trailingGroup.Location = new System.Drawing.Point(3, 249);
			_trailingGroup.Name = "_trailingGroup";
			_trailingGroup.Size = new System.Drawing.Size(291, 77);
			_trailingGroup.TabIndex = 1;
			_trailingGroup.TabStop = false;
			_trailingGroup.Text = "Trailing Whitespace";
			// 
			// _trailing
			// 
			this._trailing.Dock = System.Windows.Forms.DockStyle.Fill;
			this._trailing.Location = new System.Drawing.Point(3, 16);
			this._trailing.Name = "_trailing";
			this._trailing.Size = new System.Drawing.Size(285, 58);
			this._trailing.TabIndex = 0;
			// 
			// _valueLabel
			// 
			_valueLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			_valueLabel.AutoSize = true;
			_valueLabel.Location = new System.Drawing.Point(15, 116);
			_valueLabel.Name = "_valueLabel";
			_valueLabel.Size = new System.Drawing.Size(34, 13);
			_valueLabel.TabIndex = 2;
			_valueLabel.Text = "Value";
			// 
			// _jsonLabel
			// 
			_jsonLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			_jsonLabel.AutoSize = true;
			_jsonLabel.Location = new System.Drawing.Point(15, 198);
			_jsonLabel.Name = "_jsonLabel";
			_jsonLabel.Size = new System.Drawing.Size(35, 13);
			_jsonLabel.TabIndex = 3;
			_jsonLabel.Text = "Json";
			// 
			// _valueText
			// 
			this._valueText.AcceptsReturn = true;
			this._valueText.AcceptsTab = true;
			this._valueText.Dock = System.Windows.Forms.DockStyle.Fill;
			this._valueText.Location = new System.Drawing.Point(68, 85);
			this._valueText.Multiline = true;
			this._valueText.Name = "_valueText";
			this._valueText.Size = new System.Drawing.Size(226, 76);
			this._valueText.TabIndex = 4;
			this._valueText.TextChanged += new System.EventHandler(this.ValueTextChanged);
			// 
			// _jsonValue
			// 
			this._jsonValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this._jsonValue.Location = new System.Drawing.Point(68, 167);
			this._jsonValue.Multiline = true;
			this._jsonValue.Name = "_jsonValue";
			this._jsonValue.Size = new System.Drawing.Size(226, 76);
			this._jsonValue.TabIndex = 5;
			this._jsonValue.TextChanged += new System.EventHandler(this.JsonTextChanged);
			// 
			// NumberControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(_layout);
			this.Name = "NumberControl";
			this.Size = new System.Drawing.Size(297, 329);
			_layout.ResumeLayout(false);
			_layout.PerformLayout();
			_leadingGroup.ResumeLayout(false);
			_trailingGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private WhiteSpaceControl _leading;
		private WhiteSpaceControl _trailing;
		private System.Windows.Forms.TextBox _valueText;
		private System.Windows.Forms.TextBox _jsonValue;
	}
}
