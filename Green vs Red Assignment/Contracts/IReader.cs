namespace Green_vs_Red_Assignment.Contracts
{
    public interface IReader
    {
        char[] ReadRow();

        int[] ReadSize();

        int[] ReadOutputCondition();
    }
}
