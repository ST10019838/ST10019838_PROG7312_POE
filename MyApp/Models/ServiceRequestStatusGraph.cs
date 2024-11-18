namespace MyApp.Models
{

    // The following code was adapted from the prescribed module textbook
    // Author: Marcin Jamro
    // Title: C# Data Structures and Algorithms
    public class ServiceRequestStatusGraph
    {
        private bool _isDirected = false;
        private bool _isWeighted = false;

        public List<Node> Nodes { get; set; } = new List<Node>();

        public ServiceRequestStatusGraph(bool isDirected, bool isWeighted)
        {
            _isDirected = isDirected;
            _isWeighted = isWeighted;
        }

        public Edge this[int from, int to]
        {
            get
            {
                Node nodeFrom = Nodes[from];
                Node nodeTo = Nodes[to];

                int i = nodeFrom.Neighbors.IndexOf(nodeTo);
                if (i >= 0)
                {
                    Edge edge = new Edge()
                    {
                        From = nodeFrom,
                        To = nodeTo,
                        Weight = i < nodeFrom.Weights.Count ? nodeFrom.Weights[i] : 0
                    };
                }

                return null;
            }
        }

        public Node AddNode(List<ServiceRequest> value, ServiceRequestStatus key)
        {
            Node node = new Node() { Data = value, Key = key };
            Nodes.Add(node);
            UpdateIndices();
            return node;
        }

        public void RemoveNode(Node nodeToRemove)
        {
            Nodes.Remove(nodeToRemove);
            UpdateIndices();

            foreach (Node node in Nodes)
            {
                RemoveEdge(node, nodeToRemove);
            }
        }

        public void AddEdge(Node from, Node to, int weight = 0)
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

        public void RemoveEdge(Node from, Node to)
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


        public List<Edge> GetEdges()
        {
            List<Edge> edges = new List<Edge>();
            foreach (Node from in Nodes)
            {
                for (int i = 0; i < from.Neighbors.Count; i++)
                {
                    Edge edge = new Edge()
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


        public static ServiceRequestStatusGraph GenerateStatusGraph(List<ServiceRequest> listOfRequests)
        {
            ServiceRequestStatusGraph graph = new ServiceRequestStatusGraph(isDirected: false, isWeighted: false);

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


        private void DFS(bool[] isVisited, Node node, List<Node> result)
        {
            result.Add(node);
            isVisited[node.Index] = true;

            foreach (Node neighbor in node.Neighbors)
            {
                if (!isVisited[neighbor.Index])
                {
                    DFS(isVisited, neighbor, result);
                }
            }
        }

        public List<Node> DFS()
        {
            bool[] isVisited = new bool[Nodes.Count];
            List<Node> result = new List<Node>();
            DFS(isVisited, Nodes[0], result);
            return result;
        }


        private List<Node> BFS(Node node)
        {
            bool[] isVisited = new bool[Nodes.Count];
            isVisited[node.Index] = true;

            List<Node> result = new List<Node>();
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                Node next = queue.Dequeue();
                result.Add(next);

                foreach (Node neighbor in next.Neighbors)
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

        public List<Node> BFS()
        {
            return BFS(Nodes[0]);
        }


        private ServiceRequest? IdSearchUsingBFS(Guid id, Node node)
        {
            bool[] isVisited = new bool[Nodes.Count];
            isVisited[node.Index] = true;

            //List<Node> result = new List<Node>();

            ServiceRequest? result = null;
            bool itemFound = false;
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(node);

            while (queue.Count > 0 && !itemFound)
            {
                Node next = queue.Dequeue();
                //result.Add(next);

                // Check the nodes data
                foreach (var item in next.Data)
                {
                    if (item.Id.Equals(id))
                    {
                        itemFound = true;
                        result = item;
                        break;
                    }
                }

                if (itemFound)
                {
                    break;
                }

                // Search the next neighbor if the service request was not found
                foreach (Node neighbor in next.Neighbors)
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

        public ServiceRequest? SearchByIdUsingBFS(Guid id)
        {
            return IdSearchUsingBFS(id, Nodes[0]);
        }


        private void IdSearchUsingDFS(Guid id, bool[] isVisited, Node node, out ServiceRequest? result)
        {
            //result.Add(node);

            result = null;

            bool itemFound = false;
            foreach (var item in node.Data)
            {
                if (item.Id.Equals(id))
                {
                    itemFound = true;
                    result = item;
                    return;
                }
            }

            isVisited[node.Index] = true;

            foreach (Node neighbor in node.Neighbors)
            {
                if (!isVisited[neighbor.Index])
                {
                    IdSearchUsingDFS(id, isVisited, neighbor, out result);
                }
            }
        }
        public ServiceRequest? SearchByIdUsingDFS(Guid id)
        {
            bool[] isVisited = new bool[Nodes.Count];
            ServiceRequest? result = null;

            IdSearchUsingDFS(id, isVisited, Nodes[0], out result);
            return result;
        }

        public class Node
        {
            public int Index { get; set; }

            public ServiceRequestStatus Key { get; set; }
            public List<ServiceRequest> Data { get; set; }

            public List<Node> Neighbors { get; set; } = new List<Node>();

            public List<int> Weights { get; set; } = new List<int>();
        }

        public class Edge
        {
            public Node From { get; set; }
            public Node To { get; set; }
            public int Weight { get; set; }
        }


        public enum GraphSearchMethod
        {
            DFS,
            BFS
        }

    }
}
