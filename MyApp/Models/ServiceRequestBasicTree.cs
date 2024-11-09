using static MyApp.Models.ServiceRequest;

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

        public static ServiceRequestBasicTree GenerateBasicTree()
        {
            var EventServiceRequests = GenerateEventServiceRequests();
            var FinancialServiceRequests = GenerateFinancialServiceRequests();
            var HealthcareServiceRequests = GenerateHealthcareServiceRequests();
            var TransportationServiceRequests = GenerateTransportationServiceRequests();
            var HomeServiceRequests = GenerateHomeServiceRequests();
            var CustomerSupportServiceRequests = GenerateCustomerSupportServiceRequests();
            var ITServiceRequests = GenerateITServiceRequests();
            var MaintenanceServiceRequests = GenerateMaintenanceServiceRequests();


            var allServiceRequests = EventServiceRequests.Concat(FinancialServiceRequests)
                                        .Concat(HealthcareServiceRequests)
                                        .Concat(TransportationServiceRequests)
                                        .Concat(HomeServiceRequests)
                                        .Concat(CustomerSupportServiceRequests)
                                        .Concat(ITServiceRequests)
                                        .Concat(MaintenanceServiceRequests)
                                        .ToList();

            ServiceRequestBasicTree tree = new ServiceRequestBasicTree();

            tree.Root = new Node { Data = allServiceRequests };

            tree.Root.Children = new List<Node>
            {
                new Node { Key = ServiceRequestCategory.Event, Data = EventServiceRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Financial, Data = FinancialServiceRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Healthcare, Data = HealthcareServiceRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Transportation, Data = TransportationServiceRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Home,  Data = HomeServiceRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Customer_Support, Data = CustomerSupportServiceRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.IT, Data = ITServiceRequests, Parent = tree.Root },
                new Node { Key = ServiceRequestCategory.Maintenance, Data = MaintenanceServiceRequests, Parent = tree.Root },

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
