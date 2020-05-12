using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentTestApp
{
    class Program
    {
        private static readonly ConcurrentDictionary<int, int> Dictionary = new ConcurrentDictionary<int, int>();
        private static readonly List<string> AddSuccessfully = new List<string>();
        private static readonly List<string> RemoveSuccessfully = new List<string>();
        private static readonly List<string> IterateString = new List<string>();
        private static int _tryRemoveCount;
        private static readonly Random Random = new Random();
        private static readonly List<Task> TaskList = new List<Task>();
        static void Main(string[] args)
        {
            try
            {
                int j = 0;
                for (int i = 1; i <= 12; i++)
                {
                    j++;
                    Task task;
                    if (j == 1)
                    {
                        task = new Task(DoTask1, i);
                    }
                    else if (j == 2)
                    {
                        task = new Task(DoTask1, i);
                    }
                    else if (j == 3)
                    {
                        task = new Task(DoTask1, i);
                        j = 0;
                    }
                    else
                    {
                        throw new Exception();
                    }
                    TaskList.Add(task);
                }

                Console.WriteLine($"addSuccessfully.Count = {AddSuccessfully.Count}");
                foreach (var item in AddSuccessfully)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine($"removeSuccessfully.Count = {RemoveSuccessfully.Count}");
                foreach (var item in RemoveSuccessfully)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine($"IterateString.Count = {IterateString.Count}");
                foreach (var item in IterateString)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        public static void DoTask1(object obj)
        {
            Console.WriteLine($"DoTask1 with {obj} Thread id = {Thread.CurrentThread.ManagedThreadId}");
            for (int i = 0; i < 100; i++)
            {
                var flag = Dictionary.TryAdd(i, i);
                if (flag)
                {
                    AddSuccessfully.Add($"DoTask1 with {obj} add {i} successfully {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                    //Console.WriteLine();
                }
                Thread.Sleep(1);
            }
        }

        public static void DoTask2(object obj)
        {
            _tryRemoveCount++;
            Console.WriteLine(
                $"DoTask2 with {obj} Thread id = {Thread.CurrentThread.ManagedThreadId}, count = {Dictionary.Count}");
            var toRemove = Random.Next(0, 100);
            for (int i = 0; i < 100; i++)
            {
                var flag = Dictionary.TryRemove(toRemove, out _);
                if (flag)
                {
                    RemoveSuccessfully.Add($"DoTask2 with {obj} remove {toRemove} successfully. {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                }
                Thread.Sleep(1);
            }
        }

        public static void DoTask3(object obj)
        {
            _tryRemoveCount++;
            Console.WriteLine(
                $"DoTask3 with {obj} Thread id = {Thread.CurrentThread.ManagedThreadId}, count = {Dictionary.Count}");
            foreach (var item in Dictionary)
            {
                IterateString.Add($"DoTask3 with {obj} iterate {item.Key} {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                Thread.Sleep(2);
            }
        }
    }
}
