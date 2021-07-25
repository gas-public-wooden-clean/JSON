
namespace UI
{
	partial class BooleanControl
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
			this._leading = new UI.WhiteSpaceControl();
			this._trailing = new UI.WhiteSpaceControl();
			this._value = new System.Windows.Forms.CheckBox();
			_layout = new System.Windows.Forms.TableLayoutPanel();
			_leadingGroup = new System.Windows.Forms.GroupBox();
			_trailingGroup = new System.Windows.Forms.GroupBox();
			_layout.SuspendLayout();
			_leadingGroup.SuspendLayout();
			_trailingGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _layout
			// 
			_layout.ColumnCount = 1;
			_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			_layout.Controls.Add(_leadingGroup, 0, 0);
			_layout.Controls.Add(_trailingGroup, 0, 2);
			_layout.Controls.Add(this._value, 0, 1);
			_layout.Dock = System.Windows.Forms.DockStyle.Fill;
			_layout.Location = new System.Drawing.Point(0, 0);
			_layout.Name = "_layout";
			_layout.RowCount = 3;
			_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			_layout.Size = new System.Drawing.Size(298, 347);
			_layout.TabIndex = 2;
			// 
			// _leadingGroup
			// 
			_leadingGroup.Controls.Add(this._leading);
			_leadingGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			_leadingGroup.Location = new System.Drawing.Point(3, 3);
			_leadingGroup.Name = "_leadingGroup";
			_leadingGroup.Size = new System.Drawing.Size(292, 109);
			_leadingGroup.TabIndex = 0;
			_leadingGroup.TabStop = false;
			_leadingGroup.Text = "Leading Whitespace";
			// 
			// _leading
			// 
			this._leading.Dock = System.Windows.Forms.DockStyle.Fill;
			this._leading.Location = new System.Drawing.Point(3, 16);
			this._leading.Name = "_leading";
			this._leading.Size = new System.Drawing.Size(286, 90);
			this._leading.TabIndex = 0;
			// 
			// _trailingGroup
			// 
			_trailingGroup.Controls.Add(this._trailing);
			_trailingGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			_trailingGroup.Location = new System.Drawing.Point(3, 233);
			_trailingGroup.Name = "_trailingGroup";
			_trailingGroup.Size = new System.Drawing.Size(292, 111);
			_trailingGroup.TabIndex = 1;
			_trailingGroup.TabStop = false;
			_trailingGroup.Text = "Trailing Whitespace";
			// 
			// _trailing
			// 
			this._trailing.Dock = System.Windows.Forms.DockStyle.Fill;
			this._trailing.Location = new System.Drawing.Point(3, 16);
			this._trailing.Name = "_trailing";
			this._trailing.Size = new System.Drawing.Size(286, 92);
			this._trailing.TabIndex = 0;
			// 
			// _value
			// 
			this._value.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._value.AutoSize = true;
			this._value.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._value.Location = new System.Drawing.Point(122, 164);
			this._value.Name = "_value";
			this._value.Size = new System.Drawing.Size(53, 17);
			this._value.TabIndex = 3;
			this._value.Text = "Value";
			this._value.UseVisualStyleBackColor = true;
			// 
			// BooleanControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(_layout);
			this.Name = "BooleanControl";
			this.Size = new System.Drawing.Size(298, 347);
			_layout.ResumeLayout(false);
			_layout.PerformLayout();
			_leadingGroup.ResumeLayout(false);
			_trailingGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private WhiteSpaceControl _leading;
		private WhiteSpaceControl _trailing;
		private System.Windows.Forms.CheckBox _value;
	}
}
