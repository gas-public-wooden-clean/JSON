
namespace UI
{
	partial class ContainerControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContainerControl));
			this._trailingGroup = new System.Windows.Forms.GroupBox();
			this._trailing = new UI.WhitespaceControl();
			this._leadingGroup = new System.Windows.Forms.GroupBox();
			this._leading = new UI.WhitespaceControl();
			this._emptyGroup = new System.Windows.Forms.GroupBox();
			this._empty = new UI.WhitespaceControl();
			this._layout = new System.Windows.Forms.TableLayoutPanel();
			this._trailingGroup.SuspendLayout();
			this._leadingGroup.SuspendLayout();
			this._emptyGroup.SuspendLayout();
			this._layout.SuspendLayout();
			this.SuspendLayout();
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
			// 
			// _emptyGroup
			// 
			this._emptyGroup.Controls.Add(this._empty);
			resources.ApplyResources(this._emptyGroup, "_emptyGroup");
			this._emptyGroup.Name = "_emptyGroup";
			this._emptyGroup.TabStop = false;
			// 
			// _empty
			// 
			resources.ApplyResources(this._empty, "_empty");
			this._empty.Name = "_empty";
			// 
			// _layout
			// 
			resources.ApplyResources(this._layout, "_layout");
			this._layout.Controls.Add(this._trailingGroup, 0, 2);
			this._layout.Controls.Add(this._leadingGroup, 0, 0);
			this._layout.Controls.Add(this._emptyGroup, 0, 1);
			this._layout.Name = "_layout";
			// 
			// ContainerControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._layout);
			this.Name = "ContainerControl";
			this._trailingGroup.ResumeLayout(false);
			this._leadingGroup.ResumeLayout(false);
			this._emptyGroup.ResumeLayout(false);
			this._layout.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private WhitespaceControl _trailing;
		private WhitespaceControl _leading;
		private WhitespaceControl _empty;
		private System.Windows.Forms.TableLayoutPanel _layout;
		private System.Windows.Forms.GroupBox _trailingGroup;
		private System.Windows.Forms.GroupBox _leadingGroup;
		private System.Windows.Forms.GroupBox _emptyGroup;
	}
}
