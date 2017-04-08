
using Blackjack.ObjectModel;

public class Policy
{
    private Action[] _actions = new Blackjack.ObjectModel.Action[200];

    public Policy()
    {
        for (int i = 0; i < 200; ++i)
        {
            _actions[i] = i % 10 > 1 ? Action.Hit : Action.Stick;
        }
    }

    public Action GetAction(int currentSum, bool hasUsableAce, int dealerCard)
    {
        int ace = hasUsableAce ? 1 : 0;
        return _actions[ace * 100 + (dealerCard % 10) * 10 +  currentSum % 10];
    }

    public Action GetAction(State s)
    {
        int ace = s.HasUsableAce ? 1 : 0;
        return _actions[ace * 100 + (s.DealerCard % 10) * 10 +  s.CurrentSum % 10];
    }

    public void ImprovePolicy(double[] qStar)
    {
        throw new System.NotImplementedException();
    }
}