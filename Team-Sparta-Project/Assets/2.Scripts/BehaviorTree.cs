using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    SUCCESS,
    FAIL,
    RUNNING
}

public abstract class Node
{
    protected List<Node> childern = new List<Node>();

    public void AddChild(Node node)
    {
        childern.Add(node);
    }

    public void AddChildern(List<Node> childern)
    {
        foreach (Node child in childern)
        {
            this.childern.Add(child);
        }
    }

    public virtual NodeState Run()
    {
        return NodeState.FAIL;
    }
}

public class Action : Node
{
    private System.Func<NodeState> action;

    public Action(System.Func<NodeState> action)
    {
        this.action = action;
    }

    public override NodeState Run()
    {
        return action();
    }
}

public class Sequence : Node
{
    public override NodeState Run()
    {
        bool isChildRunning = false;

        foreach (Node node in childern)
        {
            NodeState result = node.Run();

            if (result == NodeState.FAIL)
            {
                return NodeState.FAIL;
            }
            else if(result == NodeState.RUNNING)
            {
                isChildRunning = true;
            }
        }

        return isChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
    }
}

public class Selector : Node
{
    public override NodeState Run()
    {
        foreach(Node node in childern)
        {
            NodeState result = node.Run();
            if(result == NodeState.SUCCESS)
            {
                return NodeState.SUCCESS;
            }
            else if(result == NodeState.RUNNING)
            {
                return NodeState.RUNNING;
            }
        }

        return NodeState.FAIL;
    }
}

public class Condition : Node
{
    private System.Func<bool> condition;

    public Condition(System.Func<bool> condition)
    {
        this.condition = condition;
    }

    public override NodeState Run()
    {
        return condition() ? NodeState.SUCCESS : NodeState.FAIL;
    }
}

public class BehaviorTree : MonoBehaviour
{
   
}
