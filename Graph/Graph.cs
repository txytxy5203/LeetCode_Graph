namespace MyGraph
{
    public class Graph
    {
        public Dictionary<int, Node> nodes;
        public HashSet<Edge> edges;
        public Graph() 
        { 
            nodes = new Dictionary<int, Node>();
            edges = new HashSet<Edge>();
        }
        public void RemoveNode(int id)
        {
            if (!nodes.ContainsKey(id))
            {
                Console.WriteLine("不存在该节点");
                return;
            }

            
            // 删去该节点的出边 和 邻居节点的入度
            foreach (var edge in nodes[id].edges)
            { 
                edge.to._in--;
                edges.Remove(edge);
            }
            nodes.Remove(id);
        }
    }
    public class Node
    {
        public int value;
        // 和关键字重名了 只能这样了
        // 表示入度和出度
        public int _in;
        public int _out;
        /// <summary>
        /// 由当前节点出发  可以到达的节点（边是有向的）
        /// </summary>
        public List<Node> nexts;
        /// <summary>
        /// 由当前节点出发的边（边是有向的）
        /// </summary>
        public List<Edge> edges;
        public Node(int value)
        {
            this.value = value;
            _in = 0;
            _out = 0;
            nexts = new List<Node>();
            edges = new List<Edge>();
        }
    }
    public class Edge
    {
        public int weight;
        public Node from;
        public Node to;
        public Edge(int weight, Node from, Node to)
        {
            this.weight = weight;
            this.from = from;
            this.to = to;
        }
    }

    /// <summary>
    /// 左神的简单并查集
    /// </summary>
    public class MySets
    {
        public Dictionary<Node, List<Node>> setMap;
        public MySets(List<Node> nodes)
        {
            setMap = new Dictionary<Node, List<Node>>();
            foreach (var node in nodes)
            {
                List<Node> set = new List<Node>(){ node };
                setMap.Add(node, set);
            }
        }
        public bool isSameSet(Node from, Node to)
        {
            if (setMap[from] == setMap[to])
            {
                return true;
            }
            return false;
        }
        public void Union(Node from, Node to)
        {
            List<Node> fromSet = setMap[from];
            List<Node> toSet = setMap[to];
            // 全部添加到 fromSet中   并且修改toSet中的节点的setMap
            foreach (var node in toSet)
            {
                fromSet.Add(node);
                setMap[node] = fromSet;
            }                     
        }
    }
}
