using MyGraph;



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

foreach (var node in SortedTopology(graph))
{
    Console.WriteLine(node.value);
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
