namespace AvaloniaTranslate {
    public class TranslationString
    {
        private readonly string Value;

        public TranslationString(string value)
        {
            Value = value;
        }

        public static implicit operator TranslationString(string value)
        {
            return new TranslationString(value);
        }

        public static implicit operator string(TranslationString other)
        {
            return TranslationSingleton.Tranlsate(
                UserSingleton.CurrentUser.Locale, 
                other.Value
            );
        }
        
        public override string ToString()
        {
            return (string)this;
        }
    }
}