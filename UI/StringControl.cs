using CER.Json.DocumentObjectModel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
	public partial class StringControl : UserControl
	{
		public StringControl() => InitializeComponent();

		string _json;
		bool _updating;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public JsonString Value
		{
			get
			{
				JsonString retval = new JsonString(_json, true)
				{
					Leading = _leading.Value,
					Trailing = _trailing.Value
				};
				return retval;
			}
			set
			{
				_json = value.Json;
				_literalValue.Text = value.Value;
				_jsonValue.Text = value.Json;
				_leading.Value = value.Leading;
				_trailing.Value = value.Trailing;
			}
		}

		void LiteralTextChanged(object sender, EventArgs e)
		{
			if (_updating)
			{
				return;
			}

			JsonString newValue = new JsonString(_literalValue.Text, false);
			_json = newValue.Json;

			_updating = true;
			_jsonValue.Text = newValue.Json;
			_updating = false;
			_jsonValue.BackColor = Color.FromName("Window");
		}

		void JsonTextChanged(object sender, EventArgs e)
		{
			if (_updating)
			{
				return;
			}

			JsonString newValue;
			try
			{
				newValue = new JsonString(_jsonValue.Text, true);
			}
			catch (FormatException)
			{
				_jsonValue.BackColor = Color.Red;
				return;
			}

			_json = newValue.Json;

			_jsonValue.BackColor = Color.FromName("Window");

			_updating = true;
			_literalValue.Text = newValue.Value;
			_updating = false;
		}
	}
}
