namespace AppSystemSimulator.TrafficSimulation
{
    public class SimulationExecutorBase : ITrafficExecutor
    {
        System.Timers.Timer HaltingTimer_;
        private Configurations.CommonConfiguration Config_;

        public SimulationExecutorBase(Configurations.CommonConfiguration config)
        {
            this.HaltingTimer_ = new System.Timers.Timer(config.HaltingDurationInSeconds * 1000 /* 대기시간 */);
            this.HaltingTimer_.Elapsed += this.AfterHaltHandler;
        }

        public void AfterHaltHandler(object sender, System.Timers.ElapsedEventArgs args)
        {
            this.StartNetworkService();
        }

        public void Execute()
        {
            this.HaltingTimer_.Start();
        }

        public void StartNetworkService()
        {

        }

    }
}
