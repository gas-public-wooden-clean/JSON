using CER.JSON.DocumentObjectModel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
	public partial class WhitespaceControl : UserControl
	{
		public WhitespaceControl()
		{
			InitializeComponent();
		}

		private Whitespace _value;
		private bool _updating;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Whitespace Value
		{
			get { return _value; }
			set
			{
				_value = value;
				SetText(_literalValue, value.Value);
				SetText(_escapedValue, Escape(value.Value));
			}
		}

		private void SetText(TextBox control, string text)
		{
			_updating = true;
			control.Text = text;
			_updating = false;
			control.BackColor = Color.FromName("Window");
		}

		private static string Escape(string literal)
		{
			return literal.Replace("\t", "\\t").Replace("\r", "\\r").Replace("\n", "\\n");
		}

		private void LiteralTextChanged(object sender, EventArgs e)
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

		private void EscapedTextChanged(object sender, EventArgs e)
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

		private bool Update(string literal)
		{
			try
			{
				Value = new Whitespace(literal);
			}
			catch (ArgumentException)
			{
				return false;
			}

			return true;
		}
	}
}
