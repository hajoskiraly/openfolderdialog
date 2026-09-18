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
using static System.Runtime.InteropServices.JavaScript.JSType;
using IOPath = System.IO.Path;

namespace openfolderdialog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string sourceDir = @"C:\Forrasmappa";
            string targetDir = @"C:\Celmappa";

            try
            {
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                if (!Directory.Exists(sourceDir))
                {
                    Console.WriteLine($"forrasmappa nem talalhato" +
                        $": {sourceDir}");
                    return;
                }

                string[] files = Directory.GetFiles(sourceDir);

                foreach (string file in files)
                {
                    string fileName = IOPath.GetFileName(file);
                    string destFile = IOPath.Combine(targetDir, fileName);
                    File.Copy(file, destFile, true);
                    Console.WriteLine($"{fileName} sikeresen atmasolva");
                }

                Console.WriteLine("A fajlok sikeresen atmasolva");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba tortent a fajlok atmasolasa soran: {ex.Message}");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            var openFolderDialog = new Microsoft.Win32.OpenFolderDialog();

            openFolderDialog.Title = "mappa kivalasztasa";
            openFolderDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            openFolderDialog.Multiselect = false;

            if (openFolderDialog.ShowDialog() == true)
            {
                string folderPath = openFolderDialog.FolderName;
                mappa_select_label.Content = "";
                mappa_select_label.Content = folderPath;
            }


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}