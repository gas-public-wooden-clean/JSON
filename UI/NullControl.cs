using CER.Json.DocumentObjectModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace UI
{
	public partial class NullControl : UserControl
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
	}
}
