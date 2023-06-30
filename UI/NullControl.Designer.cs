
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NullControl));
			this._layout = new System.Windows.Forms.TableLayoutPanel();
			this._trailingGroup = new System.Windows.Forms.GroupBox();
			this._trailing = new UI.WhitespaceControl();
			this._leadingGroup = new System.Windows.Forms.GroupBox();
			this._leading = new UI.WhitespaceControl();
			this._layout.SuspendLayout();
			this._trailingGroup.SuspendLayout();
			this._leadingGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _layout
			// 
			resources.ApplyResources(this._layout, "_layout");
			this._layout.Controls.Add(this._trailingGroup, 0, 1);
			this._layout.Controls.Add(this._leadingGroup, 0, 0);
			this._layout.Name = "_layout";
			// 
			// _trailingGroup
			// 
			this._trailingGroup.Controls.Add(this._trailing);
			resources.ApplyResources(this._trailingGroup, "_trailingGroup");
			this._trailingGroup.Name = "_trailingGroup";
			this._trailingGroup.TabStop = false;
			// 
			// _trailing
			// 
			resources.ApplyResources(this._trailing, "_trailing");
			this._trailing.Name = "_trailing";
			this._trailing.ValueChanged += new System.EventHandler(this.WhitespaceChanged);
			// 
			// _leadingGroup
			// 
			this._leadingGroup.Controls.Add(this._leading);
			resources.ApplyResources(this._leadingGroup, "_leadingGroup");
			this._leadingGroup.Name = "_leadingGroup";
			this._leadingGroup.TabStop = false;
			// 
			// _leading
			// 
			resources.ApplyResources(this._leading, "_leading");
			this._leading.Name = "_leading";
			this._leading.ValueChanged += new System.EventHandler(this.WhitespaceChanged);
			// 
			// NullControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._layout);
			this.Name = "NullControl";
			this._layout.ResumeLayout(false);
			this._trailingGroup.ResumeLayout(false);
			this._leadingGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private WhitespaceControl _trailing;
		private WhitespaceControl _leading;
		private System.Windows.Forms.TableLayoutPanel _layout;
		private System.Windows.Forms.GroupBox _trailingGroup;
		private System.Windows.Forms.GroupBox _leadingGroup;
	}
}
