using System;
using Akka.Actor;
using XmasTree.Actors;
using XmasTree.Messages;
using System.Collections.Generic;

namespace XmasTree
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            const int NrRows = 7;

            var random = new Random();

            var system = ActorSystem.Create("Xmas");

            var writer = system.ActorOf(Props.Create<Writer>());
            var controller = system.ActorOf(Props.Create<Controller>());

            for (var row = 0; row < NrRows; row++)
            {
                var x = NrRows - row;
                var y = row * 2;

                for (var d = 0; d <= row; d++)
                {
                    Color color = (Color) random.Next((int) Color.NrValues);

                    system.ActorOf(Props.Create<Light>(x + d * 2, y, color, writer));

                    if (row > 0)
                        writer.Tell(new ShowLeaf(x + d * 2, y - 1));
                }
            }

            writer.Tell(new ShowRoot());

            system
                .Scheduler
                .ScheduleTellRepeatedly(
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromMilliseconds(300),
                    controller,
                    new Tick(),
                    ActorRefs.NoSender);

            Console.ReadLine();

            system.Shutdown();
            system.AwaitTermination();
        }
    }
}
