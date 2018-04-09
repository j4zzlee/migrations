namespace st2forget.utils.commands
{
    public class CommandArgument
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public bool IsUninary { get; set; }
        public string Format { get; set; }
    }
}