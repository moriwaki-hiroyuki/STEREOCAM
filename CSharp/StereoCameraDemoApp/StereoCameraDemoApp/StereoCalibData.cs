﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using StereoCameraDemoApp;
//
//    var stereoCalibration = StereoCalibration.FromJson(jsonString);

namespace StereoCameraDemoApp
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class StereoCalibration
    {
        public static Dictionary<string, List<List<double>>> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, List<List<double>>>>(json, StereoCameraDemoApp.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Dictionary<string, List<List<double>>> self) => JsonConvert.SerializeObject(self, StereoCameraDemoApp.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}