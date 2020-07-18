namespace Green_vs_Red_Assignment.Implementations
{
    using System;
    using System.Linq;
    using Green_vs_Red_Assignment.Contracts;
    public class ConsoleWorker : IReader, IWriter
    {
        // first two properties are using same logic, so i make a private method with the logic
        public int[] ReadOutputCondition() => this.ReadLine();
        public int[] ReadSize() => this.ReadLine();
        public char[] ReadRow()
            => Console.ReadLine().ToCharArray();

        private int[] ReadLine()
              => Console.ReadLine().Split(", ").Select(int.Parse).ToArray();

        public void Write(int changesCount)
            => Console.WriteLine(changesCount);
    }
}
