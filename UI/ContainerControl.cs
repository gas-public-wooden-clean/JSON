using CER.Json.DocumentObjectModel;
using System.ComponentModel;
using System.Windows.Forms;

namespace UI
{
	public partial class ContainerControl : UserControl
	{
		public ContainerControl() => InitializeComponent();

		JsonElement _value;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public JsonElement Value
		{
			get
			{
				_value.Leading = _leading.Value;
				_value.Trailing = _trailing.Value;
				if (_value is JsonArray array)
				{
					array.EmptyWhiteSpace = _empty.Value;
				}
				else
				{
					((JsonObject)_value).EmptyWhiteSpace = _empty.Value;
				}
				return _value;
			}
			set
			{
				_leading.Value = value.Leading;
				_trailing.Value = value.Trailing;
				if (value is JsonArray array)
				{
					_empty.Value = array.EmptyWhiteSpace;
				}
				else
				{
					_empty.Value = ((JsonObject)value).EmptyWhiteSpace;
				}
				_value = value;
			}
		}
	}
}
