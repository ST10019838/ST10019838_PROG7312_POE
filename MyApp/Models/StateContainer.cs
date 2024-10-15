using Priority_Queue;

namespace MyApp.Models
{
    // The following class was adapted from the microsoft docs
    // Authors: guardrex, Rick-Anderson, ctrl-alt-d, rynowak
    // Link: https://learn.microsoft.com/en-us/aspnet/core/blazor/state-management?view=aspnetcore-8.0&pivots=server
    internal class StateContainer
    {
        List<Issue> _issues = new List<Issue>();
        public List<Issue> Issues
        {
            get => Repository.GetIssues();
            set
            {
                // "value" isn't used as the Issues state should be 
                // derived from the repository (db)
                _issues = Repository.GetIssues();
                NotifyStateChanged();
            }
        }



        private SortedSet<DateOnly> utilizedDates = new SortedSet<DateOnly>();

        public IEnumerable<DateOnly> GetUtilizedDates()
        {
            return utilizedDates.Reverse();
        }

        public bool UtilizedDatesAreEmpty()
        {
            return !utilizedDates.Any();
        }


        private SortedSet<EventCategory> utilizedCategories = new SortedSet<EventCategory>();

        public SortedSet<EventCategory> GetUtilizedCategories()
        {
            return utilizedCategories;
        }

        public bool UtilizedCategoriesAreEmpty()
        {
            return !utilizedCategories.Any();
        }



        private SortedDictionary<DateOnly, List<EventOrAnnouncement>> _eventsAndAnnouncements = new SortedDictionary<DateOnly, List<EventOrAnnouncement>>();

        public IEnumerable<KeyValuePair<DateOnly, List<EventOrAnnouncement>>> GetEventsAndAnnouncements(DateOnly? dateSelected = null, EventCategory? categorySelected = null)
        {
            var searchedItems = new SortedDictionary<DateOnly, List<EventOrAnnouncement>>();

            if (dateSelected is not null && categorySelected is not null)
            {
                foreach (var item in _eventsAndAnnouncements)
                {
                    if (item.Key != dateSelected) continue;

                    foreach (var eventOrAnnouncement in item.Value)
                    {
                        if (eventOrAnnouncement.Category != categorySelected) continue;

                        if (searchedItems.ContainsKey(item.Key))
                        {
                            searchedItems[item.Key].Add(eventOrAnnouncement);

                            continue;
                        }

                        List<EventOrAnnouncement> newList = new List<EventOrAnnouncement>();

                        newList.Add(eventOrAnnouncement);

                        searchedItems.Add(item.Key, newList);
                    }
                }
            }
            else if (dateSelected is not null)
            {
                // The date selected will come from the UtilizedDates set, meaning that we will have already
                // validated / ensured that date exists
                searchedItems.Add((DateOnly)dateSelected, _eventsAndAnnouncements[(DateOnly)dateSelected]);
            }
            else if (categorySelected is not null)
            {
                foreach (var item in _eventsAndAnnouncements)
                {
                    foreach (var eventOrAnnouncement in item.Value)
                    {
                        if (eventOrAnnouncement.Category != categorySelected) continue;

                        if (searchedItems.ContainsKey(item.Key))
                        {
                            searchedItems[item.Key].Add(eventOrAnnouncement);

                            continue;
                        }

                        List<EventOrAnnouncement> newList = new List<EventOrAnnouncement>();

                        newList.Add(eventOrAnnouncement);

                        searchedItems.Add(item.Key, newList);
                    }
                }
            }
            else
            {
                // Reverses the list so that the most recent dates can be shown first
                searchedItems = _eventsAndAnnouncements;
            }


            // Reverses the list so that the most recent dates can be shown first
            return searchedItems.Reverse();
        }

        public void AddEventOrAnnouncement(EventOrAnnouncement newItem)
        {
            UtilizeDates(newItem);

            UtilizeCategories(newItem);
        }

        private void UtilizeDates(EventOrAnnouncement newItem)
        {
            if (utilizedDates.Contains(newItem.CreatedAt))
            {
                _eventsAndAnnouncements[newItem.CreatedAt].Add(newItem);

                return;
            }

            utilizedDates.Add(newItem.CreatedAt);

            List<EventOrAnnouncement> newList = new List<EventOrAnnouncement>();

            newList.Add(newItem);

            _eventsAndAnnouncements.Add(newItem.CreatedAt, newList);
        }

        private void UtilizeCategories(EventOrAnnouncement newItem)
        {
            if (newItem.Category is null || utilizedCategories.Contains((EventCategory)newItem.Category)) return;

            utilizedCategories.Add((EventCategory)newItem.Category);
        }


        public bool EventsAndAnnouncementsAreEmpty()
        {
            return !_eventsAndAnnouncements.Any();
        }


        // The recommended events and announcements will be based on the users most searched categories
        SimplePriorityQueue<EventCategory> mostSearchedCategories = new SimplePriorityQueue<EventCategory>();


        public void CaptureSearchForRecommendations(EventCategory? categorySelected = null)
        {
            if (categorySelected is null) return;


            bool categorySuccessfullyQueued = mostSearchedCategories
                .EnqueueWithoutDuplicates(utilizedCategories.Where(item => item == categorySelected).First(), 0);

            if (!categorySuccessfullyQueued)// this means the category is alreay enqueued, meaning we have to update the priority
            {
                float priority = mostSearchedCategories.GetPriority((EventCategory)categorySelected);

                mostSearchedCategories.TryUpdatePriority((EventCategory)categorySelected, (priority + 1));
                // The reason why priority is being added and not subtracted is so that we can store
                // how many times the item was searched for. We can then invert the priority queue so that
                // the most searched item (with the highest number) will then become the priority
            }
        }


        public IEnumerable<KeyValuePair<DateOnly, List<EventOrAnnouncement>>> GetRecommendedEventsAndAnnouncements()
        {
            // As the priority queue is used in reverse to store the count of how many times a category
            // was searched for (which determines what should be recommended), we first need to get the
            // most recommended categories by ordering the items by the priority then reversing the output 
            // to find the highest numbers of searched for categories. The OrderByDescending was not used as
            // it caused type conflicts.
            var recommendedCategories = mostSearchedCategories.OrderBy(item => mostSearchedCategories.GetPriority(item)).Reverse();

            // Take the top 3 most searched categories which will be used to provide some recommendations
            if (mostSearchedCategories.Count >= 3)
            {
                // The following code was adapted from tutorialspoint.com
                // Author: Chandu yadav (https://www.tutorialspoint.com/authors/Chandu-Yadav-1)
                // Link: https://www.tutorialspoint.com/chash-program-to-display-the-last-three-elements-from-a-list-in-reverse-order#:~:text=To%20display%20the%20last%20three%20elements%20from%20a%20list%2C%20use,use%20the%20Reverse()%20method
                recommendedCategories = recommendedCategories.Take(3);
            }


            // Create a new dictionary that will store the recommended items
            var recommendedItems = new SortedDictionary<DateOnly, List<EventOrAnnouncement>>();


            // Search through the existing events and announcements to find the recommended categories
            foreach (var item in _eventsAndAnnouncements)
            {
                foreach (var eventOrAnnouncement in item.Value)
                {
                    if (eventOrAnnouncement.Category is null || !recommendedCategories.Contains((EventCategory)eventOrAnnouncement.Category)) continue;

                    if (recommendedItems.ContainsKey(item.Key))
                    {
                        recommendedItems[item.Key].Add(eventOrAnnouncement);

                        continue;
                    }

                    List<EventOrAnnouncement> newList = new List<EventOrAnnouncement>();

                    newList.Add(eventOrAnnouncement);

                    recommendedItems.Add(item.Key, newList);
                }
            }

            // Reverse the list so that the most recent dates can be shown first
            return recommendedItems.Reverse();
        }


        public bool HaveCategoriesBeenSearched()
        {
            return mostSearchedCategories.Any();
        }


        public void AddDummyData()
        {
            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a MUSIC title",
                Description = "This is a MUSIC description that will be carrying on to test the responsiveness of the ui",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Music,
                Organiser = "MUSIC Organises Things",
                Location = "Over there!",
                Date = new DateOnly(2024, 11, 01),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a MUSIC title 2",
                Description = "This is a MUSIC description 2",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Music,
                Organiser = "MUSIC Organises Things 2",
                Location = "Over there!",
                CreatedAt = new DateOnly(2024, 12, 02),
                Date = new DateOnly(2024, 12, 02),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a MUSIC title 3",
                Description = "This is a MUSIC description 3 ",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Music,
                Organiser = "MUSIC Organises Things 3",
                Location = "Over there!",
                CreatedAt = new DateOnly(2024, 10, 01),
                Date = new DateOnly(2024, 10, 01),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Corporate title",
                Description = "This is a Corporate description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Corporate,
                Organiser = "Corporate Organises Things",
                Location = "Over there!",
                Date = new DateOnly(2024, 11, 01),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Conference title",
                Description = "This is a Conference description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Conference,
                Organiser = "Conference Organises Things",
                Location = "Over there!",
                Date = new DateOnly(2024, 11, 03),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Workshop title",
                Description = "This is a Workshop description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Workshop,
                Organiser = "Workshop Organises Things",
                Location = "Over there!",
                CreatedAt = new DateOnly(2024, 11, 01),
                Date = new DateOnly(2024, 11, 01),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Educational title",
                Description = "This is a Educational description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Educational,
                Organiser = "Educational Organises Things",
                Location = "Over there!",
                Date = new DateOnly(2022, 11, 01),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Tech title",
                Description = "This is a Tech description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Tech,
                Organiser = "Tech Organises Things",
                Location = "Over there!",
                CreatedAt = new DateOnly(2024, 12, 01),
                Date = new DateOnly(2024, 12, 01),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Tech title 2",
                Description = "This is a Tech description 2",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Tech,
                Organiser = "Tech Organises Things 2",
                Location = "Over there!",
                CreatedAt = new DateOnly(2024, 11, 09),
                Date = new DateOnly(2024, 11, 09),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Environmental title",
                Description = "This is a Environmental description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Environmental,
                Organiser = "Environmental Organises Things",
                Location = "Over there!",
                CreatedAt = new DateOnly(2024, 11, 13),
                Date = new DateOnly(2024, 11, 13),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Sports title",
                Description = "This is a Sports description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Sports,
                Organiser = "Sports Organises Things",
                Location = "Over there!",
                CreatedAt = new DateOnly(2024, 11, 01),
                Date = new DateOnly(2024, 11, 01),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Food_and_Drink title",
                Description = "This is a Food_and_Drink description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Food_and_Drink,
                Organiser = "Food_and_Drink Organises Things",
                Location = "Over there!",
                CreatedAt = new DateOnly(2024, 11, 05),
                Date = new DateOnly(2024, 11, 05),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Webinar title",
                Description = "This is a Webinar description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Webinar,
                Organiser = "Webinar Organises Things",
                Location = "Over there!",
                CreatedAt = new DateOnly(2024, 9, 11),
                Date = new DateOnly(2024, 9, 11),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });


            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Religious title",
                Description = "This is a Religious description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Religious,
                Organiser = "Religious Organises Things",
                Location = "Over there!",
                CreatedAt = new DateOnly(2024, 12, 25),
                Date = new DateOnly(2024, 12, 25),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Workshop title",
                Description = "This is a Workshop description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Workshop,
                Organiser = "Workshop Organises Things",
                Location = "Over there!",
                CreatedAt = new DateOnly(2023, 11, 01),
                Date = new DateOnly(2023, 11, 01),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is a Health_and_Wellness title",
                Description = "This is a Health_and_Wellness description",
                Type = EventOrAnnouncementType.Event,
                Category = EventCategory.Health_and_Wellness,
                Organiser = "Health_and_Wellness Organises Things",
                Location = "Over there!",
                CreatedAt = new DateOnly(2024, 11, 06),
                Date = new DateOnly(2024, 11, 06),
                StartTime = new TimeOnly(17, 30),
                EndTime = new TimeOnly(21, 00)
            });


            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is an announcement",
                Description = "This is an announcement description",
                Type = EventOrAnnouncementType.Announcement,
                CreatedAt = new DateOnly(2023, 11, 01)
            });

            AddEventOrAnnouncement(new EventOrAnnouncement()
            {
                Title = "This is an announcement",
                Description = "This is an announcement description 2",
                Type = EventOrAnnouncementType.Announcement,
                CreatedAt = new DateOnly(2023, 11, 02)
            });
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
