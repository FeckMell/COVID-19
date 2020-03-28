using System;
using System.Collections.Generic;
using System.Linq;

namespace COVID_19
{
  /// <summary>
  ///
  /// </summary>
  public class CountryStat
  {
    /// <summary>
    /// Country name
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Confirmed cases
    /// </summary>
    public double Cases { get; set; }

    /// <summary>
    /// Recovered
    /// </summary>
    public double Recovered { get; set; }

    /// <summary>
    /// Deaths
    /// </summary>
    public double Deaths { get; set; }

    /// <summary>
    /// Was instance successfully parsed
    /// </summary>
    public bool IsParsed { get; set; } = true;

    /// <summary>
    /// Dictionary matching fields of <see cref="CountryStat"/> and headers of stat file
    /// </summary>
    private static readonly Dictionary<string, List<string>> headerMapping = new Dictionary<string, List<string>>()
    {
      { nameof(CountryStat.Country), new List<string>{"Country_Region", "Country/Region" } },
      { nameof(CountryStat.Cases), new List<string>{"Confirmed"} },
      { nameof(CountryStat.Deaths), new List<string>{ "Deaths" } },
      { nameof(CountryStat.Recovered), new List<string>{ "Recovered" } },
    };

    private static readonly Dictionary<string, string> countryMapping = new Dictionary<string, string>()
    {
      { "Mainland China", "China" },
      { "Hong Kong", "China" },
      { "Hong Kong SAR", "China" },

      { "Bahamas The", "Bahamas" },
      { "The Bahamas", "Bahamas" },

      { "Congo (Brazzaville)", "Congo" },
      { "Congo (Kinshasa)", "Congo" },
      { "Republic of Congo", "Congo" },


      { "Gambia The", "Gambia" },
      { "The Gambia", "Gambia" },

      { "Republic of Moldova", "Moldova" },
      { "Iran (Islamic Republic of)", "Iran" },
      { "Korea, South", "Korea South" },
      { "South Korea", "Korea South" },
      { "Macao SAR", "Macau" },
      { "Russian Federation", "Russia" },
      { "Taiwan*","Taiwan" },
      { "United Kingdom", "UK"},
      { "Viet Nam", "Vietnam" },
      { "Republic of Irland", "Irland" },
      { "Republic of Korea", "Korea North" },




    };

    /// <summary>
    /// Constructor
    /// </summary>
    public CountryStat(string line, List<string> headers)
    {
      try
      {
        var data = SplitData(line);
        int indexOfName = GetIndex(nameof(CountryStat.Country), headers);
        int indexOfCasese = GetIndex(nameof(CountryStat.Cases), headers);
        int indexOfDeaths = GetIndex(nameof(CountryStat.Deaths), headers);
        int indexOfRecovered = GetIndex(nameof(CountryStat.Recovered), headers);

        Country = data[indexOfName].Trim();
        if (countryMapping.TryGetValue(Country, out var country))
          Country = country;
        Cases = double.Parse(string.IsNullOrWhiteSpace(data[indexOfCasese]) ? "0" : data[indexOfCasese]);
        Deaths = double.Parse(string.IsNullOrWhiteSpace(data[indexOfDeaths]) ? "0" : data[indexOfDeaths]);
        Recovered = double.Parse(string.IsNullOrWhiteSpace(data[indexOfRecovered]) ? "0" : data[indexOfRecovered]);
      }
      catch (Exception ex)
      {
        IsParsed = false;
      }
    }

    /// <summary>
    /// Handles "State,Sub-state",Country,1,2,3
    /// </summary>
    private List<string> SplitData(string line)
    {
      int index = line.IndexOf('"');
      if (index != -1)
      {
        line = line.Remove(line.IndexOf(',', index), 1);
        line = line.Remove(index, 1);
        line = line.Remove(line.IndexOf('"'), 1);
      }

      return line.Split(new[] { "," }, StringSplitOptions.None).ToList();
    }

    /// <summary>
    /// Gets index of column
    /// </summary>
    private int GetIndex(string property, List<string> headers)
    {
      // search for matching header
      var header = headers.Intersect(headerMapping[property]).FirstOrDefault();
      if (string.IsNullOrWhiteSpace(header))
        throw new Exception($"Didn't find header for property {property}");

      // return it's index
      return headers.IndexOf(header);
    }
  }
}