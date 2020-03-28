using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace COVID_19
{
  /// <summary>
  /// Model for <see cref="DayStat"/>
  /// </summary>
  public class DayStatModel : NotifyPropertyChanged
  {
    public string Day => m_stat.Date.ToString("dd.MM.yyyy");
    public DateTime Date => m_stat.Date.Date;
    public double Confirmed => m_stat.CountryStats.Values.Where(x => !m_excludeCountries.Contains(x.Country)).Sum(x => x.Cases);
    public double Deaths => m_stat.CountryStats.Values.Where(x => !m_excludeCountries.Contains(x.Country)).Sum(x => x.Deaths);
    public double Recovered => m_stat.CountryStats.Values.Where(x => !m_excludeCountries.Contains(x.Country)).Sum(x => x.Recovered);
    public string DeathRate1 => (Deaths + Recovered == 0) ? "0%" : $"{Deaths / (Deaths + Recovered) * 100:0.00}%";
    public string DeathRate2 => (Confirmed == 0) ? "0%" : $"{Deaths / Confirmed * 100:0.00}%";
    public List<CountryStatModel> CountryStats => m_countryModels.Where(x => !m_excludeCountries.Contains(x.Name)).ToList();


    /// <summary>
    /// instance
    /// </summary>
    private DayStat m_stat;

    /// <summary>
    /// List of models
    /// </summary>
    private List<CountryStatModel> m_countryModels;

    /// <summary>
    /// List of exclude countries
    /// </summary>
    private List<string> m_excludeCountries = new List<string>();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dayStat"></param>
    public DayStatModel(DayStat dayStat)
    {
      m_stat = dayStat;
      m_countryModels = m_stat.CountryStats.Values.Select(x => new CountryStatModel(x)).ToList();
    }

    /// <summary>
    /// Sets exclude list
    /// </summary>
    /// <param name="exclude"></param>
    public void SetExcludeList(List<string> exclude)
    {
      m_excludeCountries = exclude;
      OnChanged(nameof(Day));
      OnChanged(nameof(Confirmed));
      OnChanged(nameof(Deaths));
      OnChanged(nameof(Recovered));
      OnChanged(nameof(DeathRate1));
      OnChanged(nameof(DeathRate2));
    }
  }
}