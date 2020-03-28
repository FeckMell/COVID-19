using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace COVID_19
{
  /// <summary>
  ///
  /// </summary>
  public class MainWindowModel : NotifyPropertyChanged
  {
    /// <summary>
    /// Manager instance
    /// </summary>
    private Manager m_manager;

    /// <summary>
    /// Last day statistics instance
    /// </summary>
    public DayStatModel LastDayStat { get => m_lastDayStat; set { m_lastDayStat = value; OnChanged(); } }
    private DayStatModel m_lastDayStat;

    /// <summary>
    /// Day statistics list
    /// </summary>
    public List<DayStatModel> DayStatModels { get; private set; } = new List<DayStatModel>();

    /// <summary>
    /// List of countries
    /// </summary>
    public List<CountryModel> Countries { get; private set; } = new List<CountryModel>();

    /// <summary>
    /// Excluded countries
    /// </summary>
    public List<CountryModel> ExcludedCountries => Countries.Where(x => x.IsExcluded).OrderBy(x => x.Name).ToList();

    /// <summary>
    /// Included countries
    /// </summary>
    public List<CountryModel> IncludedCountries => m_filteredCountries.Where(x => !x.IsExcluded).OrderBy(x => x.Name).ToList();

    /// <summary>
    /// default countries to exclude
    /// </summary>
    private List<string> defaultExclude = new List<string> { "China", "Korea South", "Iran" };

    /// <summary>
    /// private selection of excluded countries
    /// </summary>
    private List<string> m_excludedCountries => ExcludedCountries.Select(x => x.Name).ToList();
    private List<CountryModel> m_filteredCountries => Countries.Where(x => x.Name.ToUpper().Contains(m_search.ToUpper())).ToList();
    private string m_search = "";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="manager"></param>
    public MainWindowModel(Manager manager)
    {
      ResetManager(manager);
    }

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="search"></param>
    public void SetSearch(string search)
    {
      m_search = search;
      OnChanged(nameof(ExcludedCountries));
      OnChanged(nameof(IncludedCountries));
    }

    /// <summary>
    /// Exchanges lists of include and Exclude
    /// </summary>
    public void ExchangeLists()
    {
      m_exchangeBlock = true;
      foreach (var e in Countries)
        e.IsExcluded = !e.IsExcluded;
      m_exchangeBlock = false;

      SetExcludeList();
    }
    private bool m_exchangeBlock = false;

    /// <summary>
    /// ResetManager
    /// </summary>
    public void ResetManager(Manager manager)
    {
      foreach (var e in Countries)
        e.ValueChanged -= Country_ValueChanged;

      m_manager = manager;
      if (m_manager.DayStats.Find(x => x.Date == m_manager.DayStats.Max(y => y.Date)) == null)
        return;
      LastDayStat = new DayStatModel(m_manager.DayStats.Find(x => x.Date == m_manager.DayStats.Max(y => y.Date)));
      DayStatModels = m_manager.DayStats.Select(x => new DayStatModel(x)).ToList();
      Countries = manager.DayStats.SelectMany(x => x.CountryStats.Values.Select(y => y.Country)).Distinct().Select(x => new CountryModel(x)).ToList();
      foreach (var e in Countries)
      {
        try
        {
          if (defaultExclude.Contains(e.Name))
            e.IsExcluded = true;
          e.ValueChanged += Country_ValueChanged;
        }
        catch
        {
          ;
        }
      }

      SetExcludeList();
      OnChanged(nameof(DayStatModels));
    }

    /// <summary>
    /// Handler for changes in countries exclude list
    /// </summary>
    private void Country_ValueChanged(bool isExcluded)
    {
      if (m_exchangeBlock)
        return;
      SetExcludeList();
    }

    /// <summary>
    /// Sets exclude lists for all models
    /// </summary>
    private void SetExcludeList()
    {
      LastDayStat.SetExcludeList(m_excludedCountries);
      foreach (var e in DayStatModels)
        e.SetExcludeList(m_excludedCountries);
      OnChanged(nameof(ExcludedCountries));
      OnChanged(nameof(IncludedCountries));
      OnChanged(nameof(LastDayStat));
    }
  }
}