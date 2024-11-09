namespace MyApp.Models
{
    public class ServiceRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string CreatedBy { get; set; } = "User";

        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public string? Location { get; set; } = null;

        public ServiceRequestCategory Category { get; set; }
        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Submitted;



        public static List<ServiceRequest> GenerateServiceRequestData()
        {
            return GenerateEventServiceRequests()
                        .Concat(GenerateFinancialServiceRequests())
                        .Concat(GenerateHealthcareServiceRequests())
                        .Concat(GenerateTransportationServiceRequests())
                        .Concat(GenerateHomeServiceRequests())
                        .Concat(GenerateCustomerSupportServiceRequests())
                        .Concat(GenerateITServiceRequests())
                        .Concat(GenerateMaintenanceServiceRequests())
                        .ToList();
        }

        public static List<ServiceRequest> GenerateEventServiceRequests()
        {
            return new List<ServiceRequest>
            {
                new ServiceRequest(){
                    Title = "Event Title",
                    Description = "This is a description",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Event,
                    Status = ServiceRequestStatus.Submitted
                },
                new ServiceRequest(){
                    Title = "Event Title 2",
                    Description = "This is a description for Event Title 2",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Event,
                    Status = ServiceRequestStatus.Pending_Customer_Action
                },
                new ServiceRequest(){
                    Title = "Event Title 3",
                    Description = "This is a description for Event Title 3",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Event,
                    Status = ServiceRequestStatus.Resolved
                }
            };
        }

        public static List<ServiceRequest> GenerateFinancialServiceRequests()
        {
            return new List<ServiceRequest>
            {
                new ServiceRequest(){
                    Title = "Financial Title",
                    Description = "This is a description",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Financial,
                    Status = ServiceRequestStatus.On_Hold
                },
                new ServiceRequest(){
                    Title = "Financial Title 2",
                    Description = "This is a description for Financial Title 2",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Financial,
                    Status = ServiceRequestStatus.Assigned
                },
                new ServiceRequest(){
                    Title = "Financial Title 3",
                    Description = "This is a description for Financial Title 3",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Financial,
                    Status = ServiceRequestStatus.Escalated
                }
            };
        }

        public static List<ServiceRequest> GenerateHealthcareServiceRequests()
        {
            return new List<ServiceRequest>
            {
                new ServiceRequest(){
                    Title = "Healthcare Title",
                    Description = "This is a description",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Healthcare,
                    Status = ServiceRequestStatus.Cancelled
                },
                new ServiceRequest(){
                    Title = "Healthcare Title 2",
                    Description = "This is a description for Healthcare Title 2",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Healthcare,
                    Status = ServiceRequestStatus.Closed
                },
                new ServiceRequest(){
                    Title = "Healthcare Title 3",
                    Description = "This is a description for Healthcare Title 3",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Healthcare,
                    Status = ServiceRequestStatus.Cancelled
                }
            };
        }

        public static List<ServiceRequest> GenerateTransportationServiceRequests()
        {
            return new List<ServiceRequest>
            {
                new ServiceRequest(){
                    Title = "Transportation Title",
                    Description = "This is a description",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Transportation,
                    Status = ServiceRequestStatus.Resolved
                },
                new ServiceRequest(){
                    Title = "Transportation Title 2",
                    Description = "This is a description for Transportation Title 2",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Transportation,
                    Status = ServiceRequestStatus.Submitted
                },
                new ServiceRequest(){
                    Title = "Transportation Title 3",
                    Description = "This is a description for Transportation Title 3",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Transportation,
                    Status = ServiceRequestStatus.Pending_Customer_Action
                }
            };
        }

        public static List<ServiceRequest> GenerateHomeServiceRequests()
        {
            return new List<ServiceRequest>
            {
                new ServiceRequest(){
                    Title = "Home Title",
                    Description = "This is a description",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Home,
                    Status = ServiceRequestStatus.Pending_Customer_Action
                },
                new ServiceRequest(){
                    Title = "Home Title 2",
                    Description = "This is a description for Home Title 2",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Home,
                    Status = ServiceRequestStatus.Assigned
                },
                new ServiceRequest(){
                    Title = "Home Title 3",
                    Description = "This is a description for Home Title 3",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Home,
                    Status = ServiceRequestStatus.Assigned
                }
            };
        }

        public static List<ServiceRequest> GenerateCustomerSupportServiceRequests()
        {
            return new List<ServiceRequest>
            {
                new ServiceRequest(){
                    Title = "Customer Support Title",
                    Description = "This is a description",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Customer_Support,
                    Status = ServiceRequestStatus.On_Hold
                },
                new ServiceRequest(){
                    Title = "Customer Support Title 2",
                    Description = "This is a description for Customer Support Title 2",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Customer_Support,
                    Status = ServiceRequestStatus.On_Hold
                },
                new ServiceRequest(){
                    Title = "Customer Support Title 3",
                    Description = "This is a description for Customer Support Title 3",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Customer_Support,
                    Status = ServiceRequestStatus.On_Hold
                }
            };
        }

        public static List<ServiceRequest> GenerateITServiceRequests()
        {
            return new List<ServiceRequest>
            {
                new ServiceRequest(){
                    Title = "IT Title",
                    Description = "This is a description",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.IT,
                    Status = ServiceRequestStatus.Resolved
                },
                new ServiceRequest(){
                    Title = "IT Title 2",
                    Description = "This is a description for IT Title 2",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.IT,
                    Status = ServiceRequestStatus.Resolved
                },
                new ServiceRequest(){
                    Title = "IT Title 3",
                    Description = "This is a description for IT Title 3",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.IT,
                    Status = ServiceRequestStatus.Closed
                }
            };
        }

        public static List<ServiceRequest> GenerateMaintenanceServiceRequests()
        {
            return new List<ServiceRequest>
            {
                new ServiceRequest(){
                    Title = "Maintenance Title",
                    Description = "This is a description",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Maintenance,
                    Status = ServiceRequestStatus.In_Progress
                },
                new ServiceRequest(){
                    Title = "Maintenance Title 2",
                    Description = "This is a description for Maintenance Title 2",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Maintenance,
                    Status = ServiceRequestStatus.Assigned
                },
                new ServiceRequest(){
                    Title = "Maintenance Title 3",
                    Description = "This is a description for Maintenance Title 3",
                    Location = "Over there street",
                    Category = ServiceRequestCategory.Maintenance,
                    Status = ServiceRequestStatus.In_Progress
                }
            };
        }

    }

    // The following categories were generated by Poe AI
    // Link: https://poe.com
    public enum ServiceRequestCategory
    {
        Event,
        Financial,
        Healthcare,
        Transportation,
        Home,
        Customer_Support,
        IT,
        Maintenance
    }

    // The following categories were generated by Poe AI
    // Link: https://poe.com
    public enum ServiceRequestStatus
    {
        Submitted,
        Assigned,
        In_Progress,
        Resolved,

        On_Hold,
        Pending_Customer_Action,
        Closed,
        Cancelled,
        Escalated,
        Reopened
    }
}
