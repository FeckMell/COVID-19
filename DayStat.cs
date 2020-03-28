using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace COVID_19
{
  /// <summary>
  /// Stat by day
  /// </summary>
  public class DayStat
  {
    /// <summary>
    /// Filename
    /// </summary>
    public string Filename { get; private set; }

    /// <summary>
    /// Date of file
    /// </summary>
    public DateTimeOffset Date { get; private set; }

    /// <summary>
    /// Resulting statistic per country
    /// </summary>
    public Dictionary<string, CountryStat> CountryStats { get; private set; } = new Dictionary<string, CountryStat>();

    /// <summary>
    /// List of errors
    /// </summary>
    public List<string> Errors { get; private set; } = new List<string>();

    private List<string> m_headers = new List<string>();
    private List<string> m_data = new List<string>();

    /// <summary>
    /// Constructor
    /// </summary>
    public DayStat(string filename)
    {
      try
      {
        Filename = filename;

        // get date
        if (!DateTimeOffset.TryParseExact(Path.GetFileNameWithoutExtension(filename), "MM-dd-yyyy", CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out DateTimeOffset date))
        {
          Errors.Add($"Could not get date from filename {filename}");
          return;
        }
        Date = date;

        // read file as line array
        var lines = File.ReadAllLines(Filename).ToList();

        // get headers
        m_headers.AddRange(lines[0].Split(new[] { "," }, StringSplitOptions.None));
        lines.RemoveAt(0);

        m_data.AddRange(lines);

        for (int i = 0; i < m_data.Count; i++)
        {
          try
          {
            ParseDataEntry(i);
          }
          catch
          {
            ;
          }
        }
      }
      catch (Exception ex)
      {
        Errors.Add($"Couldn't parse file {filename}: {ex}");
      }
    }

    /// <summary>
    /// parses line from file
    /// </summary>
    private void ParseDataEntry(int i)
    {
      var stat = new CountryStat(m_data[i], m_headers);
      if (!stat.IsParsed)
      {
        Errors.Add($"Couldn't parse line {i} in {Filename}");
        return;
      }

      if (CountryStats.TryGetValue(stat.Country, out var existingStat))
      {
        existingStat.Cases += stat.Cases;
        existingStat.Deaths += stat.Deaths;
        existingStat.Recovered += stat.Recovered;
      }
      else
      {
        CountryStats.Add(stat.Country, stat);
      }
    }

  }
}