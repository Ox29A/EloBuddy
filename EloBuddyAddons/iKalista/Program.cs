using EloBuddy.SDK.Events;

namespace iKalista
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Kalista.OnGameLoad;
        }
    }
}