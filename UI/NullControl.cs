using CER.JSON.DocumentObjectModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace UI
{
	public partial class NullControl : UserControl
	{
		public NullControl() => InitializeComponent();

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Null Value
		{
			get => new Null(_leading.Value, _trailing.Value);
			set
			{
				_leading.Value = value.Leading;
				_trailing.Value = value.Trailing;
			}
		}
	}
}
