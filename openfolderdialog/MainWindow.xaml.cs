using System;
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
            // get selected folder path from label
            var folderPathObj = mappa_select_label.Content;
            if (folderPathObj == null)
            {
                MessageBox.Show("No folder selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string sourceFolder = folderPathObj.ToString().Trim();
            if (string.IsNullOrEmpty(sourceFolder) || !Directory.Exists(sourceFolder))
            {
                MessageBox.Show("Selected folder path is invalid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // get commit name from textbox and prefix with a dot
            string commitName = (committextbox.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(commitName))
            {
                MessageBox.Show("Please enter a commit name.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // create destination folder inside the source folder
            string destFolder = IOPath.Combine(sourceFolder, "." + commitName);

            try
            {
                CopyAll(sourceFolder, destFolder);
                MessageBox.Show($"Files copied to: {destFolder}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error copying files: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Open a folder selection dialog
            var openFolderDialog = new Microsoft.Win32.OpenFolderDialog();

            openFolderDialog.Title = "choose a folder";
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

        private void CopyAll(string sourceDir, string targetDir)
        {
            // create target directory if it doesn't exist
            Directory.CreateDirectory(targetDir);

            // copy files
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var fileName = IOPath.GetFileName(file);
                var destFile = IOPath.Combine(targetDir, fileName);
                File.Copy(file, destFile, true);
            }

            // copy subdirectories recursively, but skip the target directory if it's inside the source
            string fullTarget = IOPath.GetFullPath(targetDir).TrimEnd(IOPath.DirectorySeparatorChar, IOPath.AltDirectorySeparatorChar);
            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                string fullDir = IOPath.GetFullPath(dir).TrimEnd(IOPath.DirectorySeparatorChar, IOPath.AltDirectorySeparatorChar);
                if (fullDir.Equals(fullTarget, StringComparison.OrdinalIgnoreCase) || fullDir.StartsWith(fullTarget + IOPath.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                {
                    // skip copying the destination (or anything inside it) back into itself
                    continue;
                }

                var dirName = IOPath.GetFileName(dir);
                var destSubDir = IOPath.Combine(targetDir, dirName);
                CopyAll(dir, destSubDir);
            }
        }
    }
}