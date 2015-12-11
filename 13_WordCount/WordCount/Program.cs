using System;
using Akka.Actor;
using Akka.Routing;
using WordCount.Actors;

namespace WordCount
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("Words");

            var console = system.ActorOf(Props.Create<ConsoleWriter>());
            var counter = system.ActorOf(Props.Create<WordCounter>(console));
            var splitter = system.ActorOf(Props.Create<WordSplitter>(counter));
            var caser = system.ActorOf(Props.Create<LowerCaser>(splitter));
            system.ActorOf(Props.Create<FileReader>(args[0], caser));

            system.AwaitTermination();
        }
    }
}
