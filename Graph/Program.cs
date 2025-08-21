using MyGraph;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text.Json.Serialization;



//Graph graph = new Graph();
//Node node1 = new Node(1);
//Node node2 = new Node(2);
//Node node3 = new Node(3);
//Node node4 = new Node(4);

//Edge edge1_2 = new Edge(1, node1, node2);
//Edge edge1_3 = new Edge(1, node1, node3);
//Edge edge2_3 = new Edge(1, node2, node3);
//Edge edge2_4 = new Edge(1, node2, node4);
//Edge edge3_4 = new Edge(1, node3, node4);

//node1._in = 0; node1._out = 2;
//node2._in = 1; node2._out = 2;
//node3._in = 2; node3._out = 1;
//node4._in = 2; node4._out = 0;

//node1.nexts.Add(node2);
//node1.nexts.Add(node3);
//node2.nexts.Add(node3);
//node2.nexts.Add(node4);
//node3.nexts.Add(node4);

//node1.edges.Add(edge1_2);
//node1.edges.Add(edge1_3);
//node2.edges.Add(edge2_3);
//node2.edges.Add(edge2_4);
//node3.edges.Add(edge3_4);

//graph.nodes.Add(1, node1);
//graph.nodes.Add(2, node2);
//graph.nodes.Add(3, node3);
//graph.nodes.Add(4, node4);

//graph.edges.Add(edge1_2);
//graph.edges.Add(edge1_3);
//graph.edges.Add(edge2_3);
//graph.edges.Add(edge2_4);
//graph.edges.Add(edge3_4);

//graph.RemoveNode(2);

// 注意这里是交错数组还不是二维数组
//int[][] times = { new int[]{ 0, 1, 100 }, 
//                new int[] { 1, 2, 100 },
//                new int[] { 2, 0, 100 },
//                new int[] { 1, 3, 600},
//                new int[] { 2, 3, 200} };
//int n = 4;
//int k = 1;
//int src = 0;
//int dst = 3;
int[][] times = { new int[] { 2,1,1},
                  new int[]{ 2,3,1},
                new int[]{ 3,4,1} };
int n = 4;
int k = 2;
Console.WriteLine(NetworkDelayTime3(times, n, k));


static int FindCheapestPrice(int n, int[][] flights, int src, int dst, int k)
{
    // 这个思路不行
    // https://leetcode.cn/problems/cheapest-flights-within-k-stops/description/?envType=problem-list-v2&envId=vzsxaVPG
    Graph graph = Graph.ArrayToGraph(flights);
    Dictionary<Node, int> dic = FindCheapestPriceDijkstra(graph.nodes[src], k);

    if(dic.ContainsKey(graph.nodes[dst]))
    {
        return dic[graph.nodes[dst]];
    }
    return -1;
}
static Dictionary<Node, int> FindCheapestPriceDijkstra(Node head, int k)
{
    Dictionary<Node, int> distanceDic = new Dictionary<Node, int>();        // 从head到其他节点的距离
    distanceDic.Add(head, 0);

    HashSet<Node> selectedNodes = new HashSet<Node>();      // 已经被利用完的节点
    Node cur = GetMinDistanceAndUnselectedNode(distanceDic, selectedNodes);     // 在distanceDic中选一个value最小的  且不在selectedNodes中
    while (cur != null)
    {
        foreach (var edge in cur.edges)
        {
            if (!distanceDic.ContainsKey(edge.to))
            {
                distanceDic.Add(edge.to, int.MaxValue);      // dic中没有这个节点那就直接赋一个无穷大
            }

            if ((edge.weight + distanceDic[edge.from] < distanceDic[edge.to]) && (selectedNodes.Count < k + 1))    // 看看能否更新距离
            {
                distanceDic[edge.to] = edge.weight + distanceDic[edge.from];
            }
        }
        selectedNodes.Add(cur);         // 利用完cur节点的所有边后   就加入set中
        cur = GetMinDistanceAndUnselectedNode(distanceDic, selectedNodes);
    }
    return distanceDic;
}
static int NetworkDelayTime(int[][] times, int n, int k)
{
    // https://leetcode.cn/problems/network-delay-time/description/?envType=problem-list-v2&envId=vzsxaVPG
    // 图的题目我都是用左神的框架写的  下次记得练一下二维数组框架下的Coding
    // 先生成一个图
    Graph graph = Graph.ArrayToGraph(times);

    Dictionary<Node, int> dic = Dijkstra(graph.nodes[k]);

    if (dic.Count != n)
        return -1;
    return dic.Values.Max();
}
static Dictionary<Node, int> Dijkstra(Node head)
{
    // 左神还有一个堆优化的版本在高级班
    Dictionary<Node, int> distanceDic = new Dictionary<Node, int>();        // 从head到其他节点的距离
    distanceDic.Add(head, 0);

    HashSet<Node> selectedNodes = new HashSet<Node>();      // 已经被利用完的节点
    Node cur = GetMinDistanceAndUnselectedNode(distanceDic, selectedNodes);     // 在distanceDic中选一个value最小的  且不在selectedNodes中
    while (cur != null)
    {
        foreach (var edge in cur.edges)
        {
            if (!distanceDic.ContainsKey(edge.to))
            {
                distanceDic.Add(edge.to, int.MaxValue);      // dic中没有这个节点那就直接赋一个无穷大
            }

            if (edge.weight + distanceDic[edge.from] < distanceDic[edge.to])    // 看看能否更新距离
            {
                distanceDic[edge.to] = edge.weight + distanceDic[edge.from];
            }
        }
        selectedNodes.Add(cur);         // 利用完cur节点的所有边后   就加入set中
        cur = GetMinDistanceAndUnselectedNode(distanceDic, selectedNodes);  
    } 
    return distanceDic;
}
static Node GetMinDistanceAndUnselectedNode(Dictionary<Node, int> dict, HashSet<Node> set)
{
    Node result = null;
    int min = int.MaxValue;
    foreach (var node in dict.Keys)
    {
        if (set.Contains(node)) 
            continue;
        if (dict[node] < min)
        {
            result = node;
            min = dict[node];
        }
    }
    return result;
}
static int NetworkDelayTime2(int[][] times, int n, int k)
{
    // 使用Floyd算法  直接使用邻接矩阵
    int[,] w = new int[n,n];
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            w[i, j] = i == j ? 0 : int.MaxValue >> 1;
        }
    }
    for(int i = 0; i < times.GetLength(0); i++)     // 题目给的路径全部写进去
    {
        w[times[i][0] - 1, times[i][1] - 1] = times[i][2];
    }
    Floyd(w);

    int result = 0;
    for (int i = 0; i < n; i++)
    {
        result = Math.Max(w[k - 1, i], result);
    }
    return result >= int.MaxValue >> 1 ? -1 : result;
}
static void Floyd(int[,] w)
{
    // Floyd算法  邻接矩阵实现
    // 三层循环嵌套   中间-起始-终点
    int n = w.GetLength(0);
    for(int p = 0; p < n; p++)
    {
        for(int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                w[i, j] = Math.Min(w[i,j], w[i,p] + w[p,j]);
            }
        }
    }
}
static int NetworkDelayTime3(int[][] times, int n, int k)
{
    // 邻接矩阵实现Dijkstra算法
    int[,] w = new int[n, n];
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            w[i, j] = i == j ? 0 : int.MaxValue;
        }
    }
    for (int i = 0; i < times.GetLength(0); i++)     // 题目给的路径全部写进去
    {
        w[times[i][0] - 1, times[i][1] - 1] = times[i][2];
    }


    int[] result = new int[n];
    Array.Fill(result, int.MaxValue >> 1);
    result[k - 1] = 0;
    bool[] isLocked = new bool[n];
    Array.Fill(isLocked, false);

    int curr = GetMinDistanceAndUnselectedInt(result, isLocked);
    while(curr != -1)
    {
        for (int i = 0; i < n; i++)
        {
            int distance = w[curr, i] + result[curr];
            if (distance < result[i])
            {
                result[curr] = distance;
            }
        }
        isLocked[curr] = true;
        curr = GetMinDistanceAndUnselectedInt(result, isLocked);
    }

    int max = result.Max();
    return max >= int.MaxValue >> 1 ? -1 : max;

}
static int GetMinDistanceAndUnselectedInt(int[] result, bool[] isLocked)
{
    int curr = int.MaxValue;
    for (int i = 0; i < result.Length; i++)
    {
        if (isLocked[i]) continue;
        curr = Math.Min(curr, result[i]);
    }
    return curr == int.MaxValue ? -1 : curr;
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