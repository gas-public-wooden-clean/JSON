using CER.Json.DocumentObjectModel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
	public partial class WhiteSpaceControl : UserControl
	{
		public WhiteSpaceControl() => InitializeComponent();

		WhiteSpace _value;
		bool _updating;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WhiteSpace Value
		{
			get => _value;
			set
			{
				_value = value;
				SetText(_literalValue, value.Value);
				SetText(_escapedValue, Escape(value.Value));
			}
		}

		void SetText(TextBox control, string text)
		{
			_updating = true;
			control.Text = text;
			_updating = false;
			control.BackColor = Color.FromName("Window");
		}

		static string Escape(string literal)
		{
			return literal.Replace("\t", "\\t").Replace("\r", "\\r").Replace("\n", "\\n");
		}

		void LiteralTextChanged(object sender, EventArgs e)
		{
			if (_updating)
			{
				return;
			}

			if (!Update(_literalValue.Text))
			{
				_literalValue.BackColor = Color.Red;
				return;
			}

			_literalValue.BackColor = Color.FromName("Window");

			SetText(_escapedValue, Escape(Value.Value));
		}

		void EscapedTextChanged(object sender, EventArgs e)
		{
			if (_updating)
			{
				return;
			}

			string literal = _escapedValue.Text.Replace("\\t", "\t").Replace("\\r", "\r").Replace("\\n", "\n");
			if (!Update(literal))
			{
				_escapedValue.BackColor = Color.Red;
				return;
			}

			_escapedValue.BackColor = Color.FromName("Window");

			SetText(_literalValue, Value.Value);
		}

		bool Update(string literal)
		{
			try
			{
				Value = new WhiteSpace(literal);
			}
			catch (ArgumentException)
			{
				return false;
			}

			return true;
		}
	}
}
