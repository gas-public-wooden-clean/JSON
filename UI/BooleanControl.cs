using CER.Json.DocumentObjectModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace UI
{
	public partial class BooleanControl : UserControl
	{
		public BooleanControl() => InitializeComponent();

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public JsonBoolean Value
		{
			get => new JsonBoolean(_leading.Value, _trailing.Value, _value.Checked);
			set
			{
				_leading.Value = value.Leading;
				_trailing.Value = value.Trailing;
				_value.Checked = value.Value;
			}
		}
	}
}
