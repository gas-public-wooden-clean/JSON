
namespace UI
{
	partial class NullControl
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
			System.Windows.Forms.GroupBox _trailingGroup;
			System.Windows.Forms.GroupBox _leadingGroup;
			this._trailing = new UI.WhitespaceControl();
			this._leading = new UI.WhitespaceControl();
			_layout = new System.Windows.Forms.TableLayoutPanel();
			_trailingGroup = new System.Windows.Forms.GroupBox();
			_leadingGroup = new System.Windows.Forms.GroupBox();
			_layout.SuspendLayout();
			_trailingGroup.SuspendLayout();
			_leadingGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _layout
			// 
			_layout.ColumnCount = 1;
			_layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			_layout.Controls.Add(_trailingGroup, 0, 1);
			_layout.Controls.Add(_leadingGroup, 0, 0);
			_layout.Dock = System.Windows.Forms.DockStyle.Fill;
			_layout.Location = new System.Drawing.Point(0, 0);
			_layout.Name = "_layout";
			_layout.RowCount = 2;
			_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			_layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			_layout.Size = new System.Drawing.Size(379, 403);
			_layout.TabIndex = 0;
			// 
			// _trailingGroup
			// 
			_trailingGroup.Controls.Add(this._trailing);
			_trailingGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			_trailingGroup.Location = new System.Drawing.Point(3, 204);
			_trailingGroup.Name = "_trailingGroup";
			_trailingGroup.Size = new System.Drawing.Size(373, 196);
			_trailingGroup.TabIndex = 1;
			_trailingGroup.TabStop = false;
			_trailingGroup.Text = "Trailing Whitespace";
			// 
			// _trailing
			// 
			this._trailing.Dock = System.Windows.Forms.DockStyle.Fill;
			this._trailing.Location = new System.Drawing.Point(3, 16);
			this._trailing.Name = "_trailing";
			this._trailing.Size = new System.Drawing.Size(367, 177);
			this._trailing.TabIndex = 0;
			// 
			// _leadingGroup
			// 
			_leadingGroup.Controls.Add(this._leading);
			_leadingGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			_leadingGroup.Location = new System.Drawing.Point(3, 3);
			_leadingGroup.Name = "_leadingGroup";
			_leadingGroup.Size = new System.Drawing.Size(373, 195);
			_leadingGroup.TabIndex = 0;
			_leadingGroup.TabStop = false;
			_leadingGroup.Text = "Leading Whitespace";
			// 
			// _leading
			// 
			this._leading.Dock = System.Windows.Forms.DockStyle.Fill;
			this._leading.Location = new System.Drawing.Point(3, 16);
			this._leading.Name = "_leading";
			this._leading.Size = new System.Drawing.Size(367, 176);
			this._leading.TabIndex = 0;
			// 
			// NullControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(_layout);
			this.Name = "NullControl";
			this.Size = new System.Drawing.Size(379, 403);
			_layout.ResumeLayout(false);
			_trailingGroup.ResumeLayout(false);
			_leadingGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private WhitespaceControl _trailing;
		private WhitespaceControl _leading;
	}
}
