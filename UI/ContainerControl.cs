using CER.JSON.DocumentObjectModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace UI
{
	public partial class ContainerControl : UserControl
	{
		public ContainerControl()
		{
			InitializeComponent();
		}

		private Element _value;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Element Value
		{
			get
			{
				_value.Leading = _leading.Value;
				_value.Trailing = _trailing.Value;
				if (_value is Array array)
				{
					array.EmptyWhitespace = _empty.Value;
				}
				else
				{
					((Object)_value).EmptyWhitespace = _empty.Value;
				}
				return _value;
			}
			set
			{
				_leading.Value = value.Leading;
				_trailing.Value = value.Trailing;
				if (value is Array array)
				{
					_empty.Value = array.EmptyWhitespace;
				}
				else
				{
					_empty.Value = ((Object)value).EmptyWhitespace;
				}
				_value = value;
			}
		}
	}
}
