using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using Number = CER.JSON.DocumentObjectModel.Number;

namespace UI
{
	public partial class NumberControl : UserControl
	{
		public NumberControl() => InitializeComponent();

		string _json;
		bool _updating;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Number Value
		{
			get
			{
				Number retval = new Number(_json)
				{
					Leading = _leading.Value,
					Trailing = _trailing.Value
				};
				return retval;
			}
			set
			{
				_json = value.JSON;
				_valueText.Text = value.Value.ToString("G");
				_jsonValue.Text = value.JSON;
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

			Number validated = new Number(newValue);
			_updating = true;
			_jsonValue.Text = validated.JSON;
			_updating = false;
			_jsonValue.BackColor = Color.FromName("Window");
		}

		void JSONTextChanged(object sender, EventArgs e)
		{
			if (_updating)
			{
				return;
			}

			Number newValue;
			try
			{
				newValue = new Number(_jsonValue.Text);
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
