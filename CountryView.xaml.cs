using System.Windows.Controls;

namespace COVID_19
{
  /// <summary>
  /// Interaction logic for CountryView.xaml
  /// </summary>
  public partial class CountryView : UserControl
  {
    /// <summary>
    /// Model
    /// </summary>
    public CountryModel Model { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public CountryView(CountryModel model)
    {
      InitializeComponent();
      Model = model;
      DataContext = model;
    }

    private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
    {
      if(!Model.IsExcluded)
        Model.IsExcluded = true;
    }

    private void CheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
    {
      if (Model.IsExcluded)
        Model.IsExcluded = false;
    }
  }
}