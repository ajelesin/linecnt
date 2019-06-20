namespace LineCnt.UI
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string TitleTemplate = "LineCounter Progress: {0} from {1}";

        public MainWindow()
        {
            InitializeComponent();
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            BtnCtrl.IsEnabled = false;
            ResultCtrl.Text = string.Empty;

            var lines = FileNamesCtrl.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var total = lines.Length;
            var processed = 0;

            foreach (var line in lines)
            {
                ResultCtrl.Text += $"{await TotalLines(line)}{Environment.NewLine}";
                ResultCtrl.ScrollToEnd();

                processed += 1;
                Title = string.Format(TitleTemplate, processed, total);
            }

            ResultCtrl.Text += $"Done";
            BtnCtrl.IsEnabled = true;
        }

        async private static Task<int> TotalLines(string filePath)
        {
            const int bufferSize = 60 * 1024 * 1024;
            var buffer = new char[bufferSize];

            var lines = 0;

            using (var r = new StreamReader(filePath))
            {
                while (true)
                { 
                    var red = await r.ReadAsync(buffer, 0, bufferSize);
                    if (red == 0) return lines;

                    for (var i = 0; i < red; i += 1)
                        if (buffer[i] == '\n') lines += 1;
                }
            }
        }
    }
}
