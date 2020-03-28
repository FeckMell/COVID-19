using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace COVID_19
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    MainWindowModel Model { get; set; }
    public MainWindow()
    {
      InitializeComponent();
      Model = new MainWindowModel(new Manager());
      DataContext = Model;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var dialog = new OpenFileDialog();
      if (dialog.ShowDialog() == true)
      {
        Model.ResetManager(new Manager(dialog.FileName));
        UIFilename.Content = dialog.FileName;
      }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      Model.SetSearch(UISearchIN.Text);
    }

    private void Button_Exchange(object sender, RoutedEventArgs e)
    {
      Model.ExchangeLists();
    }

    private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      Model.LastDayStat = (e.Source as DataGridRow).DataContext as DayStatModel;
    }
  }
}
