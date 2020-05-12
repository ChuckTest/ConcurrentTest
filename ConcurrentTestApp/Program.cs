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
        private static readonly Random Random = new Random();
        private static readonly List<Task> TaskList = new List<Task>();
        static async Task Main(string[] args)
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
                        task = new Task(AddTask, i);
                    }
                    else if (j == 2)
                    {
                        task = new Task(RemoveTask, i);
                    }
                    else if (j == 3)
                    {
                        task = new Task(IterateTask, i);
                        j = 0;
                    }
                    else
                    {
                        throw new Exception();
                    }
                    TaskList.Add(task);
                }

                foreach (var task in TaskList)
                {
                    task.Start();
                }

                var resultTask = Task.WhenAll(TaskList.ToArray());
                await resultTask;
                //TaskStatus.RanToCompletion means "The task completed execution successfully."
                Console.WriteLine($"resultTask.Status = {resultTask.Status}");

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

        public static void AddTask(object obj)
        {
            Console.WriteLine($"AddTask with {obj} Thread id = {Thread.CurrentThread.ManagedThreadId}");
            for (int i = 0; i < 100; i++)
            {
                var flag = Dictionary.TryAdd(i, i);
                if (flag)
                {
                    AddSuccessfully.Add($"AddTask with {obj} add {i} successfully {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                    //Console.WriteLine();
                }
                Thread.Sleep(1);
            }
        }

        public static void RemoveTask(object obj)
        {
            Console.WriteLine(
                $"RemoveTask with {obj} Thread id = {Thread.CurrentThread.ManagedThreadId}, count = {Dictionary.Count}");
            var toRemove = Random.Next(0, 100);
            for (int i = 0; i < 100; i++)
            {
                var flag = Dictionary.TryRemove(toRemove, out _);
                if (flag)
                {
                    RemoveSuccessfully.Add($"RemoveTask with {obj} remove {toRemove} successfully. {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                }
                Thread.Sleep(1);
            }
        }

        public static void IterateTask(object obj)
        {
            Console.WriteLine(
                $"IterateTask with {obj} Thread id = {Thread.CurrentThread.ManagedThreadId}, count = {Dictionary.Count}");
            foreach (var item in Dictionary)
            {
                IterateString.Add($"IterateTask with {obj} iterate {item.Key} {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                Thread.Sleep(2);
            }
        }
    }
}
