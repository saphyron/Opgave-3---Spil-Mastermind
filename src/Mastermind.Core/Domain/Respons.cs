namespace Mastermind.Core.Domain
{
    /// <summary>
    /// Håndterer visning af feedback til brugeren baseret på evalueringsresultater.
    /// </summary>
    /// <remarks>
    /// Denne klasse bruger Options til at tilpasse visningen baseret på spillets indstillinger,
    /// såsom sprog og om emojis skal vises.
    /// </remarks>
    public class Respons
    {
        /// <summary>
        /// Skriver feedback til konsollen baseret på evalueringsresultater.
        /// </summary>
        /// <param name="feedback"></param>
        /// <param name="options"></param>
        /// <remarks>
        /// Denne metode viser feedback til brugeren, inklusive antal sorte og hvide markører.
        /// Hvis emojis er aktiveret i Options, vises også en visuel repræsentation af feedbacken.
        /// </remarks>
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