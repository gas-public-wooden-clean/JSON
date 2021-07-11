using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using String = CER.JSON.DocumentObjectModel.String;

namespace UI
{
	public partial class StringControl : UserControl
	{
		public StringControl() => InitializeComponent();

		string _json;
		bool _updating;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public String Value
		{
			get
			{
				String retval = new String(_json, true)
				{
					Leading = _leading.Value,
					Trailing = _trailing.Value
				};
				return retval;
			}
			set
			{
				_json = value.JSON;
				_literalValue.Text = value.Value;
				_jsonValue.Text = value.JSON;
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

			String newValue = new String(_literalValue.Text, false);
			_json = newValue.JSON;

			_updating = true;
			_jsonValue.Text = newValue.JSON;
			_updating = false;
			_jsonValue.BackColor = Color.FromName("Window");
		}

		void JSONTextChanged(object sender, EventArgs e)
		{
			if (_updating)
			{
				return;
			}

			String newValue;
			try
			{
				newValue = new String(_jsonValue.Text, true);
			}
			catch (FormatException)
			{
				_jsonValue.BackColor = Color.Red;
				return;
			}

			_json = newValue.JSON;

			_jsonValue.BackColor = Color.FromName("Window");

			_updating = true;
			_literalValue.Text = newValue.Value;
			_updating = false;
		}
	}
}
