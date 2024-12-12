using System;

namespace DecisionTree
{
    public class ActionBranch : Branch
    {
        private readonly Action _executeAction;
        public ActionBranch (Action executeAction)
        {
            _executeAction = executeAction;
        }
        public override void Execute() => _executeAction();

    }
}
