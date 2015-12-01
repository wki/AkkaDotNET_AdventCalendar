using System;
using Akka.Actor;

namespace RequestReply
{
	public class Replyer : ReceiveActor
	{
        #region command messages
        public class ReadTime {}

        public class Add4 
        {
            public int Number { get; private set; }

            public Add4 (int number)
            {
                Number = number;
            }
        }
        #endregion

        public Replyer()
        {
            Receive<ReadTime>(_ => 
                Sender.Tell(String.Format("{0:HH:mm:ss}", DateTime.Now))
            );

            Receive<Add4>(add4 => 
                Sender.Tell(add4.Number + 4)
            );
        }
	}
}
