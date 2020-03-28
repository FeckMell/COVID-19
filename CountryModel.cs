using System;

namespace COVID_19
{
  /// <summary>
  /// Model for country
  /// </summary>
  public class CountryModel : NotifyPropertyChanged
  {
    /// <summary>
    /// Event raised what is excluded changed
    /// </summary>
    public event Action<bool> ValueChanged;

    /// <summary>
    /// Name of country
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Is country excluded
    /// </summary>
    public bool IsExcluded { get => m_isExcluded; set { m_isExcluded = value; OnChanged(); ValueChanged?.Invoke(value); } }
    private bool m_isExcluded = false;

    /// <summary>
    /// Constructor
    /// </summary>
    public CountryModel(string name)
    {
      Name = name;
    }
  }
}