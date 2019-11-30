using System.Text;
using System.Windows.Forms;

namespace wyDay.Controls
{
	internal class RichTextBoxEx : RichTextBox
	{
		private string defaultRTFHeader;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 131072;
				return createParams;
			}
		}

		public RichTextBoxEx()
		{
			base.DetectUrls = false;
			base.BorderStyle = BorderStyle.None;
		}

		public string SterilizeRTF(string text)
		{
			if (defaultRTFHeader == null)
			{
				defaultRTFHeader = base.Rtf;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = defaultRTFHeader.IndexOf("\\fonttbl") + 8;
			int num2 = num;
			int num3 = 0;
			do
			{
				if (defaultRTFHeader[num2] == '{')
				{
					num3++;
				}
				else if (defaultRTFHeader[num2] == '}')
				{
					num3--;
				}
				num2++;
			}
			while (num3 > -1);
			string value = defaultRTFHeader.Substring(num, num2 - num);
			num2 = (num = text.IndexOf("\\fonttbl") + 8);
			stringBuilder.Append(text.Substring(0, num));
			stringBuilder.Append(value);
			num3 = 0;
			do
			{
				if (text[num2] == '{')
				{
					num3++;
				}
				else if (text[num2] == '}')
				{
					num3--;
				}
				num2++;
			}
			while (num3 > -1);
			text = text.Substring(num2, text.Length - num2);
			bool flag = false;
			num = 0;
			do
			{
				num2 = (num = defaultRTFHeader.IndexOf("\\fs", num) + 3);
				while (char.IsDigit(defaultRTFHeader, num2))
				{
					num2++;
					flag = true;
				}
			}
			while (!flag);
			string value2 = defaultRTFHeader.Substring(num, num2 - num);
			num2 = 0;
			do
			{
				int num4 = 0;
				int num5 = text.IndexOf("\\fs", num2);
				num2 = (num = num5 + 3);
				while (num5 > 1 && text[--num5] == '\\')
				{
					num4++;
				}
				if (num > 2 && num4 % 2 == 0)
				{
					stringBuilder.Append(text.Substring(0, num));
					stringBuilder.Append(value2);
					for (; char.IsDigit(text, num); num++)
					{
					}
					text = text.Substring(num, text.Length - num);
					num2 = 0;
				}
			}
			while (num > 2);
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}
	}
}
