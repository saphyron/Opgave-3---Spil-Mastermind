namespace Opgave_3_Mastermind.Domain
{
    public enum Sprog
    {
        Da,
        En
    }
    public static class SprogExtensions
    {
        public static Sprog ToSprog(this string str) =>
            str.ToLower() switch
            {
                "da" => Sprog.Da,
                "en" => Sprog.En,
                _ => throw new ArgumentException("Ugyldigt sprog"),
            };
    }
}