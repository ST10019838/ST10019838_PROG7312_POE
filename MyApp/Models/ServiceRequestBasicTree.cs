namespace MyApp.Models
{

    // The following code was adapted from the prescribed module textbook
    // Author: Marcin Jamro
    // Title: C# Data Structures and Algorithms
    public class ServiceRequestBasicTree
    {
        public Node Root { get; set; }

        public Node? GetNodeByKey(ServiceRequestCategory key)
        {
            // The current basic tree implemented will not have a height greater than 2, therefore we only need to search
            // the first level of for a specific node
            return Root.Children.Find(node => node.Key.Equals(key));
        }


        public ServiceRequest? SearchById(Guid id)
        {
            // The root in the basic tree contains a list of all service requests, allowing for easier searching
            return Root.Data.Find(item => item.Id.Equals(id));
        }


        // The following method was generated and adapted by Poe AI
        // Link: https://poe.com/
        public static void TraverseTree(Node node, List<Node> listOfNodes)
        {
            if (node == null)
            {
                return;
            }

            listOfNodes.Add(node);

            foreach (var child in node.Children)
            {
                TraverseTree(child, listOfNodes);
            }
        }

        public static ServiceRequestBasicTree GenerateBasicTree(List<ServiceRequest> listOfRequests)
        {
            List<ServiceRequest> eventRequests = listOfRequests
                .Where(item => item.Category == ServiceRequestCategory.Event)
                .ToList();

            List<ServiceRequest> financialRequests = listOfRequests
               .Where(item => item.Category == ServiceRequestCategory.Financial)
               .ToList();

            List<ServiceRequest> healthcareRequests = listOfRequests
               .Where(item => item.Category == ServiceRequestCategory.Healthcare)
               .ToList();

            List<ServiceRequest> transportationRequests = listOfRequests
               .Where(item => item.Category == ServiceRequestCategory.Transportation)
               .ToList();

            List<ServiceRequest> homeRequests = listOfRequests
               .Where(item => item.Category == ServiceRequestCategory.Home)
               .ToList();

            List<ServiceRequest> customerSupportRequests = listOfRequests
               .Where(item => item.Category == ServiceRequestCategory.Customer_Support)
               .ToList();

            List<ServiceRequest> itRequests = listOfRequests
               .Where(item => item.Category == ServiceRequestCategory.IT)
               .ToList();

            List<ServiceRequest> maintenanceRequests = listOfRequests
               .Where(item => item.Category == ServiceRequestCategory.Maintenance)
               .ToList();


            var allServiceRequests = eventRequests.Concat(financialRequests)
                                        .Concat(healthcareRequests)
                                        .Concat(transportationRequests)
                                        .Concat(homeRequests)
                                        .Concat(customerSupportRequests)
                                        .Concat(itRequests)
                                        .Concat(maintenanceRequests)
                                        .ToList();

            ServiceRequestBasicTree tree = new ServiceRequestBasicTree();

            tree.Root = new Node { Data = allServiceRequests };

            tree.Root.Children = new List<Node>
            {
                new Node { Key = ServiceRequestCategory.Event, Data = eventRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Financial, Data = financialRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Healthcare, Data = healthcareRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Transportation, Data = transportationRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Home,  Data = homeRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Customer_Support, Data = customerSupportRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.IT, Data = itRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Maintenance, Data = maintenanceRequests, Parent = tree.Root },
            };

            return tree;
        }




        public class Node
        {
            public ServiceRequestCategory? Key { get; set; }
            public List<ServiceRequest> Data { get; set; }

            public Node Parent { get; set; }
            public List<Node> Children { get; set; } = new List<Node>();
        }
    }
}
