
namespace Opgave_3_Mastermind.Domain
{
    public class Respons
    {
        // Sprog: "da" for dansk, "en" for engelsk
        public static void SkrivRespons(Feedback feedback, Options options)
        {
            string blackText = options.sprog == Sprog.En ? "Black" : "Sort";
            string whiteText = options.sprog == Sprog.En ? "White" : "Hvid";
            Console.WriteLine($"{blackText}: {feedback.Black} | {whiteText}: {feedback.White}");
            if (options.showEmojis)
                Console.WriteLine($"({new string('⚫', feedback.Black)}{new string('⚪', feedback.White)})");
        }
    }
}