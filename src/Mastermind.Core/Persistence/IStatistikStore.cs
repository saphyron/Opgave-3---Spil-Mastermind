using Mastermind.Core.Domain;

namespace Mastermind.Core.Persistence
{
    /// <summary>
    /// Vedvarende lager for statistik (flere GameResult)
    /// </summary>
    /// <remarks>
    /// Implementeres som fil-lager i StatistikStore.
    /// </remarks>
    public interface IStatistikStore
    {
        IReadOnlyList<GameResult> LoadAll();
        void Append(GameResult result);
        void Reset();
    }
}
