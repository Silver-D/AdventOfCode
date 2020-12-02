using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventOfCode
{
    static class Exersize
    {
        private static string input;
        private static Type exersizeClass;

        static Exersize()
        {
            input = "";
            exersizeClass = null;
        }

        public static void SetExersize(string year, string day)
        {
            input = GetInput(year, day);

            if (input.Length == 0)
            return;

            exersizeClass = Type.GetType("AdventOfCode.Exersize_" + year + "_" + day);

            exersizeClass.GetMethod("ParseInput").Invoke(null, new object[] { input });
        }

        public static string Run(int part)
        {
            if (input.Length == 0)
            return "";

            return exersizeClass.GetMethod("Part_" + part).Invoke(null, new object[] { }).ToString();
        }

        private static string GetInput(string year, string day)
        {
            string path = Directory.GetCurrentDirectory() + @"\inputs";
            string input;

            if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

            path += @"\" + year;

            if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

            path += @"\" + day + ".txt";

            if (File.Exists(path))
            {
                input = File.ReadAllText(path, Encoding.UTF8);

                if (input.Length > 0)
                return input;
            }

            Task<string> request = GetInputRemotely(year, day);

            request.Wait();

            File.WriteAllText(path, request.Result, Encoding.UTF8);

            return request.Result;
        }

        private static async Task<string> GetInputRemotely(string year, string day)
        {
            string session_file = Directory.GetCurrentDirectory() + @"\session_id.txt";
            string session_id   = "";
            string output       = "";
            bool   writeSession = false;

            if (File.Exists(session_file))
            session_id = File.ReadAllText(session_file, Encoding.UTF8);

            while(true)
            {
                HttpRequestMessage  request  = new HttpRequestMessage(HttpMethod.Get, "https://adventofcode.com/" + year + "/day/" + day + "/input");
                HttpResponseMessage response = null;

                if (session_id.Length > 0)
                {
                    request.Headers.Add("Cookie", "session=" + session_id);

                    response = await Program.http.SendAsync(request);
                }
                if (session_id.Length == 0 || response.StatusCode.ToString() == "BadRequest" || response.StatusCode.ToString() == "InternalServerError")
                {
                    Console.Write("AoC session id is either expired or missing. Paste your session id: ");

                    session_id = Console.ReadLine();

                    writeSession = true;

                    continue;
                }

                try { response.EnsureSuccessStatusCode(); }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("An error has occurred while fetching input: " + e.Message + " " + response.StatusCode);

                    break;
                }

                if (writeSession)
                File.WriteAllText(session_file, session_id, Encoding.UTF8);

                output = await response.Content.ReadAsStringAsync();

                if (output.Length == 0)
                Console.WriteLine("Fetching remote input has yielded an empty result.");

                break;
            }

            return output;
        }
    }
}