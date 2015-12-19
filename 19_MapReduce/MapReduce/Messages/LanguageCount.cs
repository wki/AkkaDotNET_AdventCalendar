namespace MapReduce.Messages
{
    /// <summary>
    /// save a tuple of language and count
    /// </summary>
    public class LanguageCount
    {
        public string Language { get; set; }
        public int Count { get; set; }

        public LanguageCount(string language, int count = 1)
        {
            Language = language;
            Count = count;
        }
    }
}
