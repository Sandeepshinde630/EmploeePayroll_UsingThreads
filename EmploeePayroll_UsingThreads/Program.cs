using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace EmploeePayroll_UsingThreads
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Employee Payroll using Threads");
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt");

            #region ParallelTasks
            System.Threading.Tasks.Parallel.Invoke(() =>
            {
                Console.WriteLine("Begin first task...");
                GetLongestWord(words);
            },// close 1st action
            () =>
            {
                Console.WriteLine("Begin second task...");
                GetMostCommonWords(words);
            },
            () =>
            {
                Console.WriteLine("Begin third task...");
                GetCountForWord(words, "sleep");
            }// close third action
            );// close parallel.invoke
            #endregion
            Console.WriteLine("Perss Any Key to Exit");
            Console.ReadKey();
        }

        private static void GetCountForWord(string[] words, string term)
        {
            var findWord = from word in words
                           where word.ToUpper().Contains(term.ToUpper())
                           select word;
            Console.WriteLine($@"Task 3 -- The Word ""{term}""Occurs {findWord.Count()} times.");
                          
        }

        private static void GetMostCommonWords(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into q
                                 orderby q.Count() descending
                                 select q.Key;
            var commonWord = frequencyOrder.Take(10);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Task 2 -- The most Common Words are :");
            foreach (var v in commonWord)
            {
                sb.AppendLine(" " + v);
            }
            Console.WriteLine(sb.ToString());
        }

        public static string[] CreateWordArray(string url)
        {
            Console.WriteLine($"Retrieving from {url}");
            //Download a web page the easy way
            string blog = new WebClient().DownloadString(url);
            return blog.Split(
                new char[] { ' ', '\u000A', '.', ',', '-', '_', '/' },
                StringSplitOptions.RemoveEmptyEntries);
        }
        private static string GetLongestWord(string[] words)
        {
            string longestWord = (from w in words
                                  orderby w.Length descending
                                  select w).First();

            Console.WriteLine($"Task 1 -- The longest word is {longestWord}.");
            return longestWord;
        }

    }
}

