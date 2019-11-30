using GLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;

namespace SingerOSK
{
	public class SingerOnScreenKeyboard : UserControl, IComponentConnector
	{
		private List<Button> keyCollection = new List<Button>();

		private Window parentWindow;

		private IInputElement focusedInputElement;

		private IntPtr handleRef;

		internal Grid allKeysGrid;

		internal Grid keyboardGrid;

		internal Button qButton;

		internal Button wButton;

		internal Button eButton;

		internal Button rButton;

		internal Button tButton;

		internal Button yButton;

		internal Button uButton;

		internal Button iButton;

		internal Button oButton;

		internal Button pButton;

		internal Button sboButton;

		internal Button sbcButton;

		internal Button plusButton;

		internal Button minusButton;

		internal Button aButton;

		internal Button sButton;

		internal Button dButton;

		internal Button fButton;

		internal Button gButton;

		internal Button hButton;

		internal Button jButton;

		internal Button kButton;

		internal Button lButton;

		internal Button colonButton;

		internal Button atButton;

		internal Button hashButton;

		internal Button zButton;

		internal Button xButton;

		internal Button cButton;

		internal Button vButton;

		internal Button bButton;

		internal Button nButton;

		internal Button mButton;

		internal Button ltButton;

		internal Button gtButton;

		internal Button fslashButton;

		internal Button commarButton;

		internal Button dotButton;

		internal Button crlfButton;

		internal Button shiftButton;

		internal Button spaceButton;

		internal Button backspaceButton;

		internal Button systemButton;

		internal Grid keyPadGrid;

		internal Button sevenButton;

		internal Button eightButton;

		internal Button nineButton;

		internal Button fourButton;

		internal Button fiveButton;

		internal Button sixButton;

		internal Button oneButton;

		internal Button twoButton;

		internal Button threeButton;

		internal Button zeroButton;

		internal Button countryButton;

		private bool _contentLoaded;

		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

		public SingerOnScreenKeyboard(Window parent)
			: this()
		{
			parentWindow = parent;
			setupKeyboardControl();
		}

		public SingerOnScreenKeyboard(IntPtr parentHWnd)
			: this()
		{
			handleRef = parentHWnd;
			setupKeyboardControl();
		}

		public SingerOnScreenKeyboard(IInputElement elementToFocusOn)
			: this()
		{
			focusedInputElement = elementToFocusOn;
			setupKeyboardControl();
		}

		private void setupKeyboardControl()
		{
			InitializeComponent();
			addAllKeysToInternalCollection();
			installAllClickEventsForCollection(keyCollection);
			switchCase(keyCollection);
		}

		private void addAllKeysToInternalCollection()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			foreach (Panel child in ((Panel)allKeysGrid).get_Children())
			{
				Panel val = (Panel)(object)child;
				foreach (Button child2 in val.get_Children())
				{
					Button item = (Button)(object)child2;
					keyCollection.Add(item);
				}
			}
		}

		private void installAllClickEventsForCollection(List<Button> keysToInstall)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			foreach (Button item in keysToInstall)
			{
				((ButtonBase)item).add_Click((RoutedEventHandler)(object)new RoutedEventHandler(buttonElement_Click));
			}
		}

		private void controlFocusIssues(IntPtr hWnd)
		{
			int nIndex = -20;
			GetWindowLong(handleRef, nIndex);
			SetWindowLong(handleRef, nIndex, (IntPtr)134217728);
		}

		private void switchCase(List<Button> keysToSwitch)
		{
			foreach (Button item in keysToSwitch)
			{
				if (((ContentControl)item).get_Content().ToString().Length == 1)
				{
					((ContentControl)item).set_Content((object)switchCase(((ContentControl)item).get_Content().ToString()));
					((ButtonBase)item).set_CommandParameter((object)switchCase(((ButtonBase)item).get_CommandParameter().ToString()));
				}
			}
			((ContentControl)shiftButton).set_Content((object)switchCase(((ContentControl)shiftButton).get_Content().ToString()));
		}

		private string switchCase(string inputString)
		{
			if (!string.IsNullOrEmpty(inputString))
			{
				string text = "";
				for (int i = 0; i < inputString.Length; i++)
				{
					char c = inputString[i];
					text = ((c >= 'A' && c <= 'Z') ? (text + c.ToString().ToLower()) : ((c < 'a' || c > 'z') ? (text + c.ToString()) : (text + c.ToString().ToUpper())));
				}
				return text;
			}
			return "";
		}

		private void buttonElement_Click(object sender, RoutedEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			string text = "";
			try
			{
				e.set_Handled(true);
				text = ((ButtonBase)(Button)sender).get_CommandParameter().ToString();
				if (text.ToLower() == "f16")
				{
					text = text.ToUpper();
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (text.Length > 1)
					{
						text = "{" + text + "}";
					}
					if (focusedInputElement != null)
					{
						Keyboard.Focus(focusedInputElement);
						focusedInputElement.Focus();
					}
					if (text == "{F16}")
					{
						PipeClient pipeClient = new PipeClient();
						pipeClient.Send("ADMIN", "Kiosk");
					}
					else
					{
						SendKeys.SendWait(text);
					}
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Could not send key press: {0}", text);
			}
		}

		private void shiftButton_Click(object sender, RoutedEventArgs e)
		{
			switchCase(keyCollection);
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (parentWindow != null)
			{
				handleRef = new WindowInteropHelper(parentWindow).get_Handle();
			}
			controlFocusIssues(handleRef);
		}

		private void countryButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Environment.Exit(0);
			}
			catch
			{
			}
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri uri = new Uri("/KVKeyboard;component/singeronscreenkeyboard.xaml", UriKind.Relative);
				Application.LoadComponent((object)this, uri);
			}
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Expected O, but got Unknown
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Expected O, but got Unknown
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Expected O, but got Unknown
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Expected O, but got Unknown
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Expected O, but got Unknown
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Expected O, but got Unknown
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Expected O, but got Unknown
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Expected O, but got Unknown
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Expected O, but got Unknown
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Expected O, but got Unknown
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Expected O, but got Unknown
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Expected O, but got Unknown
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Expected O, but got Unknown
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Expected O, but got Unknown
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Expected O, but got Unknown
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Expected O, but got Unknown
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Expected O, but got Unknown
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Expected O, but got Unknown
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Expected O, but got Unknown
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Expected O, but got Unknown
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Expected O, but got Unknown
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Expected O, but got Unknown
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Expected O, but got Unknown
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Expected O, but got Unknown
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Expected O, but got Unknown
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Expected O, but got Unknown
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Expected O, but got Unknown
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Expected O, but got Unknown
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Expected O, but got Unknown
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Expected O, but got Unknown
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Expected O, but got Unknown
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Expected O, but got Unknown
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Expected O, but got Unknown
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Expected O, but got Unknown
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Expected O, but got Unknown
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Expected O, but got Unknown
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Expected O, but got Unknown
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Expected O, but got Unknown
			//IL_033d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Expected O, but got Unknown
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Expected O, but got Unknown
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Expected O, but got Unknown
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Expected O, but got Unknown
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Expected O, but got Unknown
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Expected O, but got Unknown
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Expected O, but got Unknown
			//IL_0398: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a2: Expected O, but got Unknown
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Expected O, but got Unknown
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Expected O, but got Unknown
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Expected O, but got Unknown
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Expected O, but got Unknown
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Expected O, but got Unknown
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Expected O, but got Unknown
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Expected O, but got Unknown
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Expected O, but got Unknown
			//IL_0417: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Expected O, but got Unknown
			switch (connectionId)
			{
			case 1:
				((FrameworkElement)(SingerOnScreenKeyboard)target).add_Loaded((RoutedEventHandler)(object)new RoutedEventHandler(UserControl_Loaded));
				break;
			case 2:
				allKeysGrid = (Grid)(object)(Grid)target;
				break;
			case 3:
				keyboardGrid = (Grid)(object)(Grid)target;
				break;
			case 4:
				qButton = (Button)(object)(Button)target;
				break;
			case 5:
				wButton = (Button)(object)(Button)target;
				break;
			case 6:
				eButton = (Button)(object)(Button)target;
				break;
			case 7:
				rButton = (Button)(object)(Button)target;
				break;
			case 8:
				tButton = (Button)(object)(Button)target;
				break;
			case 9:
				yButton = (Button)(object)(Button)target;
				break;
			case 10:
				uButton = (Button)(object)(Button)target;
				break;
			case 11:
				iButton = (Button)(object)(Button)target;
				break;
			case 12:
				oButton = (Button)(object)(Button)target;
				break;
			case 13:
				pButton = (Button)(object)(Button)target;
				break;
			case 14:
				sboButton = (Button)(object)(Button)target;
				break;
			case 15:
				sbcButton = (Button)(object)(Button)target;
				break;
			case 16:
				plusButton = (Button)(object)(Button)target;
				break;
			case 17:
				minusButton = (Button)(object)(Button)target;
				break;
			case 18:
				aButton = (Button)(object)(Button)target;
				break;
			case 19:
				sButton = (Button)(object)(Button)target;
				break;
			case 20:
				dButton = (Button)(object)(Button)target;
				break;
			case 21:
				fButton = (Button)(object)(Button)target;
				break;
			case 22:
				gButton = (Button)(object)(Button)target;
				break;
			case 23:
				hButton = (Button)(object)(Button)target;
				break;
			case 24:
				jButton = (Button)(object)(Button)target;
				break;
			case 25:
				kButton = (Button)(object)(Button)target;
				break;
			case 26:
				lButton = (Button)(object)(Button)target;
				break;
			case 27:
				colonButton = (Button)(object)(Button)target;
				break;
			case 28:
				atButton = (Button)(object)(Button)target;
				break;
			case 29:
				hashButton = (Button)(object)(Button)target;
				break;
			case 30:
				zButton = (Button)(object)(Button)target;
				break;
			case 31:
				xButton = (Button)(object)(Button)target;
				break;
			case 32:
				cButton = (Button)(object)(Button)target;
				break;
			case 33:
				vButton = (Button)(object)(Button)target;
				break;
			case 34:
				bButton = (Button)(object)(Button)target;
				break;
			case 35:
				nButton = (Button)(object)(Button)target;
				break;
			case 36:
				mButton = (Button)(object)(Button)target;
				break;
			case 37:
				ltButton = (Button)(object)(Button)target;
				break;
			case 38:
				gtButton = (Button)(object)(Button)target;
				break;
			case 39:
				fslashButton = (Button)(object)(Button)target;
				break;
			case 40:
				commarButton = (Button)(object)(Button)target;
				break;
			case 41:
				dotButton = (Button)(object)(Button)target;
				break;
			case 42:
				crlfButton = (Button)(object)(Button)target;
				break;
			case 43:
				shiftButton = (Button)(object)(Button)target;
				((ButtonBase)shiftButton).add_Click((RoutedEventHandler)(object)new RoutedEventHandler(shiftButton_Click));
				break;
			case 44:
				spaceButton = (Button)(object)(Button)target;
				break;
			case 45:
				backspaceButton = (Button)(object)(Button)target;
				break;
			case 46:
				systemButton = (Button)(object)(Button)target;
				break;
			case 47:
				keyPadGrid = (Grid)(object)(Grid)target;
				break;
			case 48:
				sevenButton = (Button)(object)(Button)target;
				break;
			case 49:
				eightButton = (Button)(object)(Button)target;
				break;
			case 50:
				nineButton = (Button)(object)(Button)target;
				break;
			case 51:
				fourButton = (Button)(object)(Button)target;
				break;
			case 52:
				fiveButton = (Button)(object)(Button)target;
				break;
			case 53:
				sixButton = (Button)(object)(Button)target;
				break;
			case 54:
				oneButton = (Button)(object)(Button)target;
				break;
			case 55:
				twoButton = (Button)(object)(Button)target;
				break;
			case 56:
				threeButton = (Button)(object)(Button)target;
				break;
			case 57:
				zeroButton = (Button)(object)(Button)target;
				break;
			case 58:
				countryButton = (Button)(object)(Button)target;
				((ButtonBase)countryButton).add_Click((RoutedEventHandler)(object)new RoutedEventHandler(countryButton_Click));
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
