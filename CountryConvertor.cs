using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace COVID_19
{
  /// <summary>
  ///
  /// </summary>
  public class CountryConvertor : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is CountryModel model)
        return new CountryView(model);
      if (value is IEnumerable<CountryModel> models)
        return models.Select(x => new CountryView(x)).ToList();
      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}