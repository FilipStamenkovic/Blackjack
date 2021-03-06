﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.ObjectModel
{
    public class BackwardSarsa : Policy
    {
        private double[] eligibility = new double[400];
        private bool updated;
        private double lambda = 0.8;
        private List<int> indexes;
        public BackwardSarsa() { }

        public override void ClearHistory()
        {
            base.ClearHistory();
            updated = false;
            indexes = new List<int>();
            for (int i = 0; i < eligibility.Length; i++)
                eligibility[i] = 0.0;
        }

        public override bool EvaluateAndImprovePolicy(double reward, bool isFinal = true)
        {
            int numOfPrevSteps = _history.Count;
            KeyValuePair<State, Action> currStateAction = _history[numOfPrevSteps - 1];
            int currAction = (int)currStateAction.Value;
            int currAce = currStateAction.Key.HasUsableAce ? 1 : 0;
            int currActionIndex = currAce * 100 + (currStateAction.Key.DealerCard % 10) * 10 + currStateAction.Key.CurrentSum % 10;
            int currQIndex = currAction * 200 + currActionIndex;
            double correction;

            if (numOfPrevSteps > 1 && !isFinal)
            {
                KeyValuePair<State, Action> prevStateAction = _history[numOfPrevSteps - 2];
                int prevAction = (int)prevStateAction.Value;
                int prevAce = prevStateAction.Key.HasUsableAce ? 1 : 0;
                int prevActionIndex = prevAce * 100 + (prevStateAction.Key.DealerCard % 10) * 10 + prevStateAction.Key.CurrentSum % 10;
                int prevQIndex = prevAction * 200 + prevActionIndex;

                //eval
                correction = reward + discount * q[currQIndex] - q[prevQIndex];
            }
            else if (isFinal)
            {
                //eval
                correction = reward - q[currQIndex];
            }
            else
                return false;

            foreach (int i in indexes)
            {
                UpdateValues(i, correction);
            }

            foreach (int i in indexes)
            {
                int actionIndex = i % _actions.Length;
                Action previousAction = _actions[actionIndex];
                _actions[actionIndex] = q[i] > q[(i + _actions.Length) % 400] ? Action.Stick : Action.Hit;

                updated = updated || previousAction != _actions[actionIndex];
            }

            return updated;
        }

        private void UpdateValues(int index, double correction)
        {
            q[index] = q[index] + alpha * correction * eligibility[index];
            eligibility[index] = discount * lambda * eligibility[index];
        }

        public override Action GetAction(int currentSum, bool hasUsableAce, int dealerCard)
        {
            Action currentAction = base.GetAction(currentSum, hasUsableAce, dealerCard);
            int ace = hasUsableAce ? 1 : 0;
            int index = (int)currentAction * 200 + ace * 100 + (dealerCard % 10) * 10 + currentSum % 10;

            eligibility[index] = 1.0;
            indexes.Add(index);
            EvaluateAndImprovePolicy(0.0, false);

            return currentAction;
        }
    }
}
