using System;
using Akka.Actor;
using System.Threading.Tasks;

namespace RequestReply
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var system = ActorSystem.Create("Reply");

            var replyer = system.ActorOf(Props.Create<Replyer>());

            // Ask() will reply with a task
            Task<string> time = replyer.Ask<string>(new Replyer.ReadTime());
            time.Wait();
            Console.WriteLine("Time is: {0}", time.Result);

            Task<int> number = replyer.Ask<int>(new Replyer.Add4(17));
            number.Wait();
            Console.WriteLine("Number is: {0}", number.Result);

            Console.WriteLine("Press [enter] to continue");
            Console.ReadLine();

            system.Shutdown();
            system.AwaitTermination();
        }
    }
}
