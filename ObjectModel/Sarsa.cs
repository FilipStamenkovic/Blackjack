using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.ObjectModel
{
    public class Sarsa : Policy
    {
        private bool _updated;

        public override void ClearHistory()
        {
            base.ClearHistory();
            _updated = false;
        }

        public override bool EvaluateAndImprovePolicy(double reward, bool isFinal = true)
        {
            int numOfPrevSteps = _history.Count;
            KeyValuePair<State, Action> currStateAction = _history[numOfPrevSteps - 1];
            int currAction = (int)currStateAction.Value;
            int currAce = currStateAction.Key.HasUsableAce ? 1 : 0;
            int currActionIndex = currAce * 100 + (currStateAction.Key.DealerCard % 10) * 10 + currStateAction.Key.CurrentSum % 10;
            int currQIndex = currAction * 200 + currActionIndex;
            Action previousAction;

            if (numOfPrevSteps > 1 && !isFinal)
            {
                KeyValuePair<State, Action> prevStateAction = _history[numOfPrevSteps - 2];
                int prevAction = (int)prevStateAction.Value;
                int prevAce = prevStateAction.Key.HasUsableAce ? 1 : 0;
                int prevActionIndex = prevAce * 100 + (prevStateAction.Key.DealerCard % 10) * 10 + prevStateAction.Key.CurrentSum % 10;
                int prevQIndex = prevAction * 200 + prevActionIndex;

                //eval
                q[prevQIndex] = q[prevQIndex] + alpha * ((reward + discount * q[currQIndex]) - q[prevQIndex]);
                //improve
                previousAction = _actions[prevActionIndex];
                _updated = _updated || (previousAction != (_actions[prevActionIndex] = q[prevActionIndex] > q[prevActionIndex + 200] ? Action.Stick : Action.Hit));
            } 
            else if (isFinal)
            {
                //eval
                q[currQIndex] = q[currQIndex] + alpha * (reward - q[currQIndex]);
                //improve
                previousAction = _actions[currActionIndex];
                _updated = _updated || (previousAction != (_actions[currActionIndex] = q[currActionIndex] > q[currActionIndex + 200] ? Action.Stick : Action.Hit));
            }

            return _updated;
        }

        public override Action GetAction(int currentSum, bool hasUsableAce, int dealerCard)
        {
            Action currentAction = base.GetAction(currentSum, hasUsableAce, dealerCard);

            EvaluateAndImprovePolicy(0.0, false);

            return currentAction;
        }
    }
}
