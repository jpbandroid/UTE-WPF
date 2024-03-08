using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace Sheets
{
    class ExtendedRichTextBox : RichTextBox
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr LoadLibrary(string dllName);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams baseParams = base.CreateParams;
                if (LoadLibrary("msftedit.dll") != IntPtr.Zero)
                {
                    baseParams.ClassName = "RICHEDIT50W";
                }
                return baseParams;
            }
        }
    }
}