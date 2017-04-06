using Blackjack.ObjectModel;

namespace Blackjack.ObjectModel.Interfaces
{
    public interface IDeck
    {
        void Initialize();
        Card GetNextCard();
        bool HasCards(); 
    }
}