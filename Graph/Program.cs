using MyGraph;
using System.Collections.Generic;



Graph graph = new Graph();
Node node1 = new Node(1);
Node node2 = new Node(2);
Node node3 = new Node(3);
Node node4 = new Node(4);

Edge edge1_2 = new Edge(1, node1, node2);
Edge edge1_3 = new Edge(1, node1, node3);
Edge edge2_3 = new Edge(1, node2, node3);
Edge edge2_4 = new Edge(1, node2, node4);
Edge edge3_4 = new Edge(1, node3, node4);

node1._in = 0; node1._out = 2;
node2._in = 1; node2._out = 2;
node3._in = 2; node3._out = 1;
node4._in = 2; node4._out = 0;

node1.nexts.Add(node2);
node1.nexts.Add(node3);
node2.nexts.Add(node3);
node2.nexts.Add(node4);
node3.nexts.Add(node4);

node1.edges.Add(edge1_2);
node1.edges.Add(edge1_3);
node2.edges.Add(edge2_3);
node2.edges.Add(edge2_4);
node3.edges.Add(edge3_4);

graph.nodes.Add(1, node1);
graph.nodes.Add(2, node2);
graph.nodes.Add(3, node3);
graph.nodes.Add(4, node4);

graph.edges.Add(edge1_2);
graph.edges.Add(edge1_3);
graph.edges.Add(edge2_3);
graph.edges.Add(edge2_4);
graph.edges.Add(edge3_4);

//graph.RemoveNode(2);

foreach (var node in SortedTopology2(graph))
{
    Console.WriteLine(node.value);
}
static int MinCostConnectPoints(int[][] points)
{
    return 0;
}
static Dictionary<Node, int> Dijkstra(Node head)
{
    Dictionary<Node, int> distanceDic = new Dictionary<Node, int>();
    distanceDic.Add(head, 0);

    HashSet<Node> selectedNodes = new HashSet<Node>();      // 已经被利用完的节点
    Node cur = GetMinDistanceAndUnselectedNode(distanceDic, selectedNodes);
    foreach (var edge in cur.edges)
    {
        if(!distanceDic.ContainsKey(edge.to))
        {
            distanceDic.Add(edge.to, int.MaxValue);
        }

        if(edge.weight + distanceDic[edge.from] < distanceDic[edge.to])
        {
            distanceDic[edge.to] = edge.weight;
        }
        selectedNodes.Add(edge.from);
    }
}
static Node GetMinDistanceAndUnselectedNode(Dictionary<Node, int> dict, HashSet<Node> set)
{

}
static HashSet<Edge> PrimMST(Graph graph)
{
    // 最小生成树   prim算法

    HashSet<Edge> result = new HashSet<Edge>();
    HashSet<Node> visited = new HashSet<Node>();                                //已经经过的节点
    PriorityQueue<Edge, int> priorityQueue = new PriorityQueue<Edge, int>();    // 待选择的边

    foreach (var node in graph.nodes.Values)        // 这里的for循环是因为图可能不连通  那么每个子图都有一个最小生成树
                                                    // 如果说了连通的那么就不用循环
    {
        if (!visited.Contains(node))
        {
            visited.Add(node);
            foreach (var edge in node.edges)
            {
                priorityQueue.Enqueue(edge, edge.weight);
            }
            while (priorityQueue.Count != 0)
            {
                Edge edge = priorityQueue.Dequeue();
                Node toNode = edge.to;
                if (!visited.Contains(toNode))
                {
                    visited.Add(toNode);
                    result.Add(edge);
                    foreach (var nextEdge in toNode.edges)              // 这里会重复地把一些边加入到队列中 但是不影响结果
                                                                        // 因为节点重复了还是会不要的
                    {
                        priorityQueue.Enqueue(nextEdge, nextEdge.weight);
                    }
                }
            }
        }
    }
    return result;
}
static HashSet<Edge> KruskalMST(Graph graph)
{
    // 最小生成树   kruskal算法
    HashSet<Edge> result = new HashSet<Edge>();
    MySets mySets = new MySets(graph.nodes.Values.ToList<Node>());
    
    // 这里用优先级队列应该会更好
    List<Edge> edges = graph.edges.ToList<Edge>();
    // 排序
    edges.Sort((a, b) =>
    {
        if (a.weight < b.weight)
            return -1;
        return 1;
    });

    foreach (var edge in edges)
    {
        if (!mySets.isSameSet(edge.from, edge.to))
        {
            result.Add(edge);
            mySets.Union(edge.from, edge.to);   
        }
    }
    return result;   
}
static List<Node> SortedTopology(Graph graph)
{
    List<Node> result = new List<Node>();
    Node node = null;
    int id = 0;

    while (graph.nodes.Count != 0)
    {
        foreach (var i in graph.nodes.Keys)
        {
            if (graph.nodes[i]._in == 0)
            {
                id = i;
                node = graph.nodes[i];
                break;
            }
        }
        // 判断node是否为空
        result.Add(node);
        graph.RemoveNode(id);
    }
    return result;
    
}
static List<Node> SortedTopology2(Graph graph)
{
    // 左神的写法
    Dictionary<Node, int> inDic = new Dictionary<Node, int>();
    Queue<Node> zeroInQueue = new Queue<Node>();

    foreach (var node in graph.nodes.Values)
    {
        inDic.Add(node, node._in);
        if(node._in == 0)
            zeroInQueue.Enqueue(node);
    }

    List<Node> result = new List<Node>();
    while(zeroInQueue.Count != 0)
    {
        Node cur = zeroInQueue.Dequeue();
        result.Add(cur);
        foreach (var node in cur.nexts)
        {
            inDic[node]--;
            if(inDic[node] == 0)
            {
                zeroInQueue.Enqueue(node);
            }
        }
    }
    return result;
}
static void BFS(Node node)
{
    if(node == null) 
        return;
    Queue<Node> queue = new Queue<Node>();
    // 如果题目中明确说了是有一个范围的  那么就可以直接用数组存  更快
    HashSet<Node> visited = new HashSet<Node>();
    
    queue.Enqueue(node);
    visited.Add(node);
    while(queue.Count > 0)
    {
        Node curr = queue.Dequeue();
        // 在出队列的时候执行逻辑
        foreach (var neighbor in curr.nexts)
        {
            if(!visited.Contains(neighbor))
            {
                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }
    }
}
static void DFS(Node node)
{
    // DFS的逻辑和我想的有一点不一样  仔细看
    if (node == null)
        return;
    
    Stack<Node> stack = new Stack<Node>();
    HashSet<Node> visited = new HashSet<Node>();

    stack.Push(node);
    visited.Add(node);
    // 在Push的时候就执行逻辑
    Console.WriteLine(node.value);
    while (stack.Count > 0)
    {
        Node curr = stack.Pop();
        foreach (var neighbor in curr.nexts)
        {
            if(!visited.Contains(neighbor))
            {
                // 这里把curr重新入栈  是因为要保持栈中存在的Node就是你遍历的路径！！！
                stack.Push(curr);
                stack.Push(neighbor);
                visited.Add(neighbor);
                Console.WriteLine(neighbor.value);
                break;
            }
        }
    }
}