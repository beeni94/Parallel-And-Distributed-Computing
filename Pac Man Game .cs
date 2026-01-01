using System;
using System.Threading;
class Program
{
    static void task1()
    {
        string line = "  ---------------------";
        int length = line.Length;
        for (int i = 0; i < length; i++)
        {
            Console.Write("\r" + new string(' ', i) + "C" + line.Substring(i + 1));
            Thread.Sleep(200);
            Console.Write("\r" + new string(' ', i+1) + "C" + line.Substring(i + 1));
}
        Console.Write("\r" + new string(' ', length) + "C");
        Console.WriteLine("\n\nGame Over!Pac-Man ate all the dots! ");
    }
    static void Main()
    {
        Thread t1=new Thread(task1);
        t1.Start();
        t1.Join();
    }
}