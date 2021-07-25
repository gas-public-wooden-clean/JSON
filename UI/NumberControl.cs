using CER.Json.DocumentObjectModel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace UI
{
	public partial class NumberControl : UserControl
	{
		public NumberControl() => InitializeComponent();

		string _json;
		bool _updating;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public JsonNumber Value
		{
			get
			{
				JsonNumber retval = new JsonNumber(_json)
				{
					Leading = _leading.Value,
					Trailing = _trailing.Value
				};
				return retval;
			}
			set
			{
				_json = value.Json;
				_valueText.Text = value.Value.ToString("G");
				_jsonValue.Text = value.Json;
				_leading.Value = value.Leading;
				_trailing.Value = value.Trailing;
			}
		}

		void ValueTextChanged(object sender, EventArgs e)
		{
			if (_updating)
			{
				return;
			}

			decimal newValue;
			if (!decimal.TryParse(_valueText.Text, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out newValue))
			{
				_valueText.BackColor = Color.Red;
				return;
			}

			_valueText.BackColor = Color.FromName("Window");

			JsonNumber validated = new JsonNumber(newValue);
			_updating = true;
			_jsonValue.Text = validated.Json;
			_updating = false;
			_jsonValue.BackColor = Color.FromName("Window");
		}

		void JsonTextChanged(object sender, EventArgs e)
		{
			if (_updating)
			{
				return;
			}

			JsonNumber newValue;
			try
			{
				newValue = new JsonNumber(_jsonValue.Text);
			}
			catch (FormatException)
			{
				_jsonValue.BackColor = Color.Red;
				return;
			}

			_jsonValue.BackColor = Color.FromName("Window");

			_updating = true;
			_valueText.Text = newValue.Value.ToString("G");
			_updating = false;
			_valueText.BackColor = Color.FromName("Window");
		}
	}
}
