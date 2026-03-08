using Bonsai.Core;
using Bonsai.Standard;
using NUnit.Framework;

namespace Tests
{
  public class IncludeTests
  {
    [Test]
    public void IncludeWithoutSubtreeFailsWithoutThrowing()
    {
      var include = Helper.CreateNode<Include>();

      BehaviourTree tree = Helper.CreateTree();
      tree.SetNodes(include);
      Helper.StartBehaviourTree(tree);

      BehaviourNode.Status result = BehaviourNode.Status.Running;
      Assert.DoesNotThrow(() => result = Helper.StepBehaviourTree(tree));
      Assert.AreEqual(BehaviourNode.Status.Failure, result);
    }
  }
}
