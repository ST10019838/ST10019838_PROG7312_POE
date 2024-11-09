namespace MyApp.Models
{

    // The following code was adapted from the prescribed module textbook
    // Author: Marcin Jamro
    // Title: C# Data Structures and Algorithms
    public class ServiceRequestStatusGraph<T>
    {
        private bool _isDirected = false;
        private bool _isWeighted = false;

        public List<Node<T>> Nodes { get; set; } = new List<Node<T>>();

        public ServiceRequestStatusGraph(bool isDirected, bool isWeighted)
        {
            _isDirected = isDirected;
            _isWeighted = isWeighted;
        }

        public Edge<T> this[int from, int to]
        {
            get
            {
                Node<T> nodeFrom = Nodes[from];
                Node<T> nodeTo = Nodes[to];

                int i = nodeFrom.Neighbors.IndexOf(nodeTo);
                if (i >= 0)
                {
                    Edge<T> edge = new Edge<T>()
                    {
                        From = nodeFrom,
                        To = nodeTo,
                        Weight = i < nodeFrom.Weights.Count ? nodeFrom.Weights[i] : 0
                    };
                }

                return null;
            }
        }

        public Node<T> AddNode(T value, ServiceRequestStatus key)
        {
            Node<T> node = new Node<T>() { Data = value, Key = key };
            Nodes.Add(node);
            UpdateIndices();
            return node;
        }

        public void RemoveNode(Node<T> nodeToRemove)
        {
            Nodes.Remove(nodeToRemove);
            UpdateIndices();

            foreach (Node<T> node in Nodes)
            {
                RemoveEdge(node, nodeToRemove);
            }
        }

        public void AddEdge(Node<T> from, Node<T> to, int weight = 0)
        {
            from.Neighbors.Add(to); ;
            if (_isWeighted)
            {
                from.Weights.Add(weight);
            }

            if (!_isDirected)
            {
                to.Neighbors.Add(from);
                if (_isWeighted)
                {
                    to.Weights.Add(weight);
                }
            }
        }

        public void RemoveEdge(Node<T> from, Node<T> to)
        {
            int index = from.Neighbors.FindIndex(n => n == to);
            if (index >= 0)
            {
                from.Neighbors.RemoveAt(index);

                if (_isWeighted)
                {
                    from.Weights.RemoveAt(index);
                }
            }
        }


        public List<Edge<T>> GetEdges()
        {
            List<Edge<T>> edges = new List<Edge<T>>();
            foreach (Node<T> from in Nodes)
            {
                for (int i = 0; i < from.Neighbors.Count; i++)
                {
                    Edge<T> edge = new Edge<T>()
                    {
                        From = from,
                        To = from.Neighbors[i],
                        Weight = i < from.Weights.Count ? from.Weights[i] : 0
                    };
                    edges.Add(edge);
                }

            }

            return edges;
        }

        private void UpdateIndices()
        {
            int i = 0;
            Nodes.ForEach(n => n.Index = i++);
        }


        public static ServiceRequestStatusGraph<List<ServiceRequest>> GenerateStatusGraph(List<ServiceRequest> listOfRequests)
        {
            ServiceRequestStatusGraph<List<ServiceRequest>> graph = new ServiceRequestStatusGraph<List<ServiceRequest>>(isDirected: false, isWeighted: false);

            List<ServiceRequest> submittedRequests = listOfRequests
                .Where(item => item.Status == ServiceRequestStatus.Submitted)
                .ToList();

            List<ServiceRequest> assignedRequests = listOfRequests
                .Where(item => item.Status == ServiceRequestStatus.Assigned)
                .ToList();

            List<ServiceRequest> escalatedRequests = listOfRequests
                .Where(item => item.Status == ServiceRequestStatus.Escalated)
                .ToList();

            List<ServiceRequest> onHoldRequests = listOfRequests
                .Where(item => item.Status == ServiceRequestStatus.On_Hold)
                .ToList();

            List<ServiceRequest> reopenedRequests = listOfRequests
                .Where(item => item.Status == ServiceRequestStatus.Reopened)
                .ToList();

            List<ServiceRequest> inProgressRequests = listOfRequests
                .Where(item => item.Status == ServiceRequestStatus.In_Progress)
                .ToList();

            List<ServiceRequest> cancelledRequests = listOfRequests
                .Where(item => item.Status == ServiceRequestStatus.Cancelled)
                .ToList();

            List<ServiceRequest> pendingCustomerActionRequests = listOfRequests
                .Where(item => item.Status == ServiceRequestStatus.Pending_Customer_Action)
                .ToList();

            List<ServiceRequest> resolvedRequests = listOfRequests
                .Where(item => item.Status == ServiceRequestStatus.Resolved)
                .ToList();

            List<ServiceRequest> closedRequests = listOfRequests
                .Where(item => item.Status == ServiceRequestStatus.Closed)
                .ToList();


            var submittedNode = graph.AddNode(submittedRequests, ServiceRequestStatus.Submitted);
            var assignedNode = graph.AddNode(assignedRequests, ServiceRequestStatus.Assigned);
            var escalatedNode = graph.AddNode(escalatedRequests, ServiceRequestStatus.Escalated);
            var onHoldNode = graph.AddNode(onHoldRequests, ServiceRequestStatus.On_Hold);
            var reopenedNode = graph.AddNode(reopenedRequests, ServiceRequestStatus.Reopened);
            var inProgressNode = graph.AddNode(inProgressRequests, ServiceRequestStatus.In_Progress);
            var cancelledNode = graph.AddNode(cancelledRequests, ServiceRequestStatus.Cancelled);
            var pendingCustomerActionNode = graph.AddNode(pendingCustomerActionRequests, ServiceRequestStatus.Pending_Customer_Action);
            var resolvedNode = graph.AddNode(resolvedRequests, ServiceRequestStatus.Resolved);
            var closedNode = graph.AddNode(closedRequests, ServiceRequestStatus.Closed);


            graph.AddEdge(submittedNode, onHoldNode);
            graph.AddEdge(submittedNode, assignedNode);
            graph.AddEdge(submittedNode, escalatedNode);

            graph.AddEdge(onHoldNode, assignedNode);
            graph.AddEdge(assignedNode, escalatedNode);
            graph.AddEdge(onHoldNode, inProgressNode);
            graph.AddEdge(inProgressNode, pendingCustomerActionNode);
            graph.AddEdge(onHoldNode, pendingCustomerActionNode);
            graph.AddEdge(onHoldNode, cancelledNode);
            graph.AddEdge(onHoldNode, escalatedNode);

            graph.AddEdge(assignedNode, inProgressNode);
            graph.AddEdge(assignedNode, pendingCustomerActionNode);
            graph.AddEdge(assignedNode, cancelledNode);
            graph.AddEdge(inProgressNode, escalatedNode);
            graph.AddEdge(escalatedNode, pendingCustomerActionNode);
            graph.AddEdge(inProgressNode, cancelledNode);
            graph.AddEdge(pendingCustomerActionNode, cancelledNode);
            graph.AddEdge(escalatedNode, cancelledNode);

            graph.AddEdge(inProgressNode, resolvedNode);
            graph.AddEdge(resolvedNode, closedNode);
            graph.AddEdge(closedNode, reopenedNode);
            graph.AddEdge(cancelledNode, reopenedNode);

            graph.AddEdge(reopenedNode, assignedNode);
            graph.AddEdge(reopenedNode, onHoldNode);
            graph.AddEdge(reopenedNode, escalatedNode);


            return graph;
        }


        private void DFS(bool[] isVisited, Node<T> node, List<Node<T>> result)
        {
            result.Add(node);
            isVisited[node.Index] = true;

            foreach (Node<T> neighbor in node.Neighbors)
            {
                if (!isVisited[neighbor.Index])
                {
                    DFS(isVisited, neighbor, result);
                }
            }
        }

        public List<Node<T>> DFS()
        {
            bool[] isVisited = new bool[Nodes.Count];
            List<Node<T>> result = new List<Node<T>>();
            DFS(isVisited, Nodes[0], result);
            return result;
        }


        private List<Node<T>> BFS(Node<T> node)
        {
            bool[] isVisited = new bool[Nodes.Count];
            isVisited[node.Index] = true;

            List<Node<T>> result = new List<Node<T>>();
            Queue<Node<T>> queue = new Queue<Node<T>>();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                Node<T> next = queue.Dequeue();
                result.Add(next);

                foreach (Node<T> neighbor in next.Neighbors)
                {
                    if (!isVisited[neighbor.Index])
                    {
                        isVisited[neighbor.Index] = true;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return result;
        }

        public List<Node<T>> BFS()
        {
            return BFS(Nodes[0]);
        }


        public class Node<T>
        {
            public int Index { get; set; }

            public ServiceRequestStatus Key { get; set; }
            public T Data { get; set; }

            public List<Node<T>> Neighbors { get; set; } = new List<Node<T>>();

            public List<int> Weights { get; set; } = new List<int>();
        }

        public class Edge<T>
        {
            public Node<T> From { get; set; }
            public Node<T> To { get; set; }
            public int Weight { get; set; }
        }
    }
}
