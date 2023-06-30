using CER.Json.DocumentObjectModel;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace UI
{
	partial class BooleanControl : UserControl
	{
		public BooleanControl() => InitializeComponent();

		bool _updating;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public JsonBoolean Value
		{
			get
			{
				return new JsonBoolean(_value.Checked)
				{
					Leading = _leading.Value,
					Trailing = _trailing.Value
				};
			}
			set
			{
				_leading.Value = value.Leading;
				_trailing.Value = value.Trailing;
				_updating = true;
				_value.Checked = value.Value;
				_updating = false;
			}
		}

		void CheckedChanged(object sender, EventArgs e)
		{
			if (!_updating)
			{
				ValueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		void WhitespaceChanged(object sender, EventArgs e)
		{
			ValueChanged?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler ValueChanged;
	}
}
