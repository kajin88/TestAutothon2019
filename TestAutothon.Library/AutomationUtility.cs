﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestAutothon.Library
{
    public static class AutomationUtility
    {
        public static double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }

        public static string ExcludeSymbols(string src) => new string(src.Where(char.IsLetterOrDigit).ToArray());

        public static string GetOutputDirectory(string directory = "Results")
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var dir = $"{directory}\\Report -{ DateTime.UtcNow.ToString("MM-dd-yyyy h-mm-ss-ms")}";
            Directory.CreateDirectory(dir);
            return dir;
        }

        public static void SaveReport(string report, string filePath)
        {
            File.WriteAllText(filePath, report);
        }

        public static void Serialize<T>(T data, string filePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public static T Deserialize<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stream);
                }
            }

            return default(T);
        }

        public static string GetFromAPI(string url)
        {
            string result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        public static string UploadFile(string url, string filePath)
        {
            string response = null;
            WebClient client = new WebClient();            
            byte[] rawResponse = client.UploadFile(url, filePath);
            if(rawResponse != null)
            {
                response = System.Text.Encoding.ASCII.GetString(rawResponse);
            }

            return response;
        }

        private static int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }
    }
}
