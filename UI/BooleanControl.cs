using System.ComponentModel;
using System.Windows.Forms;
using Boolean = CER.JSON.DocumentObjectModel.Boolean;

namespace UI
{
	public partial class BooleanControl : UserControl
	{
		public BooleanControl()
		{
			InitializeComponent();
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Boolean Value
		{
			get { return new Boolean(_leading.Value, _trailing.Value, _value.Checked); }
			set
			{
				_leading.Value = value.Leading;
				_trailing.Value = value.Trailing;
				_value.Checked = value.Value;
			}
		}
	}
}
