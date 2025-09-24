using Mastermind.Core.Domain;

namespace Mastermind.Core.Persistence
{
    public interface IStatistikStore
    {
        IReadOnlyList<GameResult> LoadAll();
        void Append(GameResult result);
        void Reset();
    }
}
