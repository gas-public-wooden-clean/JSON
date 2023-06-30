using CER.Json.DocumentObjectModel;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace UI
{
	partial class NullControl : UserControl
	{
		public NullControl() => InitializeComponent();

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public JsonNull Value
		{
			get
			{
				return new JsonNull()
				{
					Leading = _leading.Value,
					Trailing = _trailing.Value
				};
			}
			set
			{
				_leading.Value = value.Leading;
				_trailing.Value = value.Trailing;
			}
		}

		void WhitespaceChanged(object sender, EventArgs e)
		{
			ValueChanged?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler ValueChanged;
	}
}
