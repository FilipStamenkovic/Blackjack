using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blackjack.ObjectModel
{
    public class MonteCarlo : Policy
    {
        public MonteCarlo(){}

        public override bool EvaluateAndImprovePolicy(double reward, bool isFinal = true)
        {
            bool actionChanged = false;
            foreach (var keyValue in _history)
            {
                int a = (int)keyValue.Value;
                int ace = keyValue.Key.HasUsableAce ? 1 : 0;
                int actionIndex = ace * 100 + (keyValue.Key.DealerCard % 10) * 10 + keyValue.Key.CurrentSum % 10;
                int qIndex = a * 200 + actionIndex;

                //eval
                q[qIndex] = q[qIndex] + 1.0 / timesVisited[qIndex] * (reward - q[qIndex]);
                //improve
                Action previousAction = _actions[actionIndex];
                _actions[actionIndex] = q[actionIndex] > q[actionIndex + 200] ? Action.Stick : Action.Hit;

                actionChanged = actionChanged || previousAction != _actions[actionIndex];
            }
            return actionChanged;
        }
    }
}