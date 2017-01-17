using EloBuddy.SDK.Events;

namespace iTwitch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Twitch.OnGameLoad;
        }
    }
}