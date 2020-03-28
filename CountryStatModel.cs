namespace COVID_19
{
  /// <summary>
  /// Model for <see cref="CountryStat"/>
  /// </summary>
  public class CountryStatModel
  {
    public string Name => m_stat.Country;
    public double Confirmed => m_stat.Cases;
    public double Deaths => m_stat.Deaths;
    public double Recovered => m_stat.Recovered;
    public string DeathRate1 => (Deaths + Recovered == 0) ? "0%" : $"{Deaths / (Deaths + Recovered) * 100:0.00}%";
    public string DeathRate2 => (Confirmed == 0) ? "0%" : $"{Deaths / Confirmed * 100:0.00}%";

    public CountryStat m_stat { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public CountryStatModel(CountryStat stat)
    {
      m_stat = stat;
    }
  }
}