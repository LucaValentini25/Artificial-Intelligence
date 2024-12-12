using System;

namespace DecisionTree
{
    public class QuestionBranch : Branch
    {
        private readonly Branch _trueBranch;
        private readonly Branch _falseBranch;
        private readonly Func<bool> _question;
        public QuestionBranch(Branch trueBranch,Branch falseBranch,Func<bool> question)
        {
            _trueBranch = trueBranch;
            _falseBranch = falseBranch;
            _question = question;
        }
        public override void Execute()
        {
            if (_question())
                _trueBranch.Execute();
            else _falseBranch.Execute();
        }

    }
}
