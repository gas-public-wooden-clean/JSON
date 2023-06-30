using CER.Json.DocumentObjectModel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace UI
{
	partial class NumberControl : UserControl
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
				_updating = true;
				_valueText.Text = value.Value.ToString("G", CultureInfo.InvariantCulture);
				_jsonValue.Text = value.Json;
				_updating = false;
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
			_json = _jsonValue.Text;
			_jsonValue.BackColor = Color.FromName("Window");

			ValueChanged?.Invoke(this, EventArgs.Empty);
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
			_valueText.Text = newValue.Value.ToString("G", CultureInfo.InvariantCulture);
			_updating = false;
			_json = _jsonValue.Text;
			_valueText.BackColor = Color.FromName("Window");


			ValueChanged?.Invoke(this, EventArgs.Empty);
		}

		void WhitespaceChanged(object sender, EventArgs e)
		{
			ValueChanged?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler ValueChanged;
	}
}
