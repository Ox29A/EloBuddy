using EloBuddy.SDK.Events;

namespace iTwitch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var twitch = new Twitch();
            Loading.OnLoadingComplete += twitch.OnGameLoad;
        }
    }
}