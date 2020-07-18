namespace Green_vs_Red_Assignment
{
    
    class StartUp
    {
        static void Main(string[] args)
        {
            // make a single instance of the grid
            var grid = Grid.Instance;

            // this method is doing all the work
            grid.Calculate();
            
        }
    }
  
}
