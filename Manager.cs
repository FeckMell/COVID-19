using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace COVID_19
{
  /// <summary>
  ///
  /// </summary>
  public class Manager
  {
    /// <summary>
    /// path to directory with files
    /// </summary>
    public string FilePath { get; private set; } = "";

    /// <summary>
    /// List of stats
    /// </summary>
    public List<DayStat> DayStats { get; private set; } = new List<DayStat>();

    /// <summary>
    /// List of errors
    /// </summary>
    public List<string> Error { get; private set; } = new List<string>();

    public Manager()
    {

    }

    /// <summary>
    /// Constructor
    /// </summary>
    public Manager(string file)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(file))
        {
          Error.Add("Path is empty");
          return;
        }

        if (!File.Exists(file))
        {
          Error.Add("File does not exist");
          return;
        }

        var filename = Path.GetFileName(file);
        FilePath = file.Remove(file.Length - filename.Length, filename.Length);

        var files = Directory.GetFiles(FilePath) // get all files in directory
                                                 .Select(x => new FileInfo(x)) // get file infos
                                                 .Where(x => x.Extension == ".csv") // get all files with this extension
                                                 .Select(x => x.FullName).ToList(); // select filenames
        foreach (var e in files)
        {
          var dayStat = new DayStat(e);
          if (dayStat.Errors.Count == 0)
          {
            DayStats.Add(dayStat);
          }
          else
          {
            Error.Add($"Couldn't parse file {e}");
            Error.AddRange(dayStat.Errors.Select(x => "  " + x));
          }
        }
      }
      catch (Exception ex)
      {
        Error.Add($"Exception in manager: {ex}");
      }
    }
  }
}