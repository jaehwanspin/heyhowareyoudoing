namespace AppSystemSimulator.TrafficSimulation
{
    public interface ITrafficExecutor
    {
        void Execute();
        void AfterHaltHandler(object sender, System.Timers.ElapsedEventArgs args);

        void StartNetworkService();
    }
}
