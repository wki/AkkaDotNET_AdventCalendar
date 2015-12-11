using System;
using Akka.Actor;
using System.Text.RegularExpressions;
using WordCount.Messages;

namespace WordCount.Actors
{
    /// <summary>
    /// Split a string stream into words
    /// </summary>
	public class WordSplitter : ReceiveActor
	{
        private IActorRef next;

        public WordSplitter(IActorRef next)
        {
            this.next = next;

            Receive<string>(SplitIntoWords);
            Receive<End>(next.Tell);
        }

        private void SplitIntoWords(string s)
        {
            var matches = Regex.Matches(s, @"\w+[^\s]*\w+|\w");
            foreach (Match match in matches)
                next.Tell(match.Value);
        }
	}
}
