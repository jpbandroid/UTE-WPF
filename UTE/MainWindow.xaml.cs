using Fluent;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoUpdaterDotNET;

namespace UltraTextEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isDebug;

        public MainWindow()
        {
            InitializeComponent();
            string extendedUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string userName = Environment.UserName;
            user.Text = userName;
            FontFamilyBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            FontSizeBox.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            FontFamilyBox.SelectedItem = "Segoe UI";
            FontSizeBox.SelectedItem = 12;
            isDebug = false;
        }

        private void showinsiderinfo(object sender, RoutedEventArgs e)
        {
            ToggleThemeTeachingTip1.IsOpen = true;
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(editor.Document.ContentStart, editor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
                AppTitle.Text = dlg.SafeFileName + " - UltraTextEdit";
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(editor.Document.ContentStart, editor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
                AppTitle.Text = dlg.SafeFileName + " - UltraTextEdit";
            }
        }

        private void update(object sender, RoutedEventArgs e)
        { 
            if (isDebug == true)
            {
                AutoUpdater.Start("https://raw.githubusercontent.com/jpbandroid/jpbOffice-Resources/main/UltraTextEdit/updateinfo_debug.xml");

            } else {
                AutoUpdater.Start("https://raw.githubusercontent.com/jpbandroid/jpbOffice-Resources/main/UltraTextEdit/updateinfo.xml");
            }
        }

        private void About(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Show();
            about.Activate();
        }

        private void FontFamilyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyBox.SelectedItem != null)
                editor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, FontFamilyBox.SelectedItem);
        }

        private void FontSizeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            editor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, double.Parse(FontSizeBox.Text));
        }

        private void editor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = editor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            Bold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = editor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            Italic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = editor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            Underline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = editor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            FontFamilyBox.SelectedItem = temp;
            temp = editor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            FontSizeBox.Text = temp.ToString();
        }

        private void editor_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}