using System;
using System.Collections.Generic;
using System.IO;

namespace StereoCameraDemoApp
{
    public partial class StereoCalibDataFunc
    {
        public StereoCalibDataFunc()
        {
        }
        public static Dictionary<string, List<List<double>>> FromFile(string FilePath)
        {
            if (String.IsNullOrEmpty(FilePath))
                return null;
            using (var strReader = new StreamReader(FilePath, System.Text.Encoding.UTF8))
            {
                var jsonData = strReader.ReadToEnd();
                return StereoCalibration.FromJson(jsonData);
            }
        }
    }
}
