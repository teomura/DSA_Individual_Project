using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    // Assignment Phase 4.

    /// <summary>
    /// Finds minimum-total-cost routes through a Frontier Dispatch world
    /// using Dijkstra's shortest-path algorithm.
    ///
    /// This class is intentionally incomplete in the assignment starter.
    /// It should eventually use the student's prepared world representation
    /// and MinPriorityQueue implementation.
    /// </summary>
    public sealed class DijkstraRouteFinder
    {
        private readonly WorldData _world;

        /// <summary>
        /// Constructor. Creates a route finder operating on the supplied world.
        /// </summary>
        public DijkstraRouteFinder(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        /// <summary>
        /// Finds a minimum-total-travel-cost route from the start location
        /// to the goal location.
        ///
        /// A successful RouteResult should contain the route as location IDs,
        /// including both the start and goal locations.
        /// </summary>
        public RouteResult FindRoute(int startLocationId, int goalLocationId)
        {
            // TODO: implement Dijkstra using your prepared world representation
            // and MinPriorityQueue<T>.
            WorldQueries queries = new WorldQueries(_world);
            if (queries == null)
            {
                throw new ArgumentNullException(nameof(queries));
            }

            if (queries.FindLocationById(startLocationId) == null || queries.FindLocationById(goalLocationId) == null)
            {
                return RouteResult.NotFound(0);
            }

            if (startLocationId == goalLocationId)
            {
                return RouteResult.Success(new[] { startLocationId }, 0, 0);
            }

            var bestCost = new Dictionary<int, int>();
            var cameFrom = new Dictionary<int, int>();
            var frontier = new MinPriorityQueue<int>();

            bestCost[startLocationId] = 0;
            frontier.Enqueue(startLocationId, 0);

            int expandedNodes = 0;

            while (frontier.Count > 0)
            {
                int priority = frontier.LowestPriority();
                int entry = frontier.Dequeue();

                if (bestCost.TryGetValue(entry, out int cost) && cost != priority)
                {
                    continue;
                }

                expandedNodes++;

                if (entry == goalLocationId)
                {
                    List<int> path = new List<int>();
                    int currentNode = goalLocationId;

                    while (currentNode != startLocationId)
                    {
                        path.Add(currentNode);
                        currentNode = cameFrom[currentNode];
                    }
                    path.Add(startLocationId);
                    path.Reverse();
                    return RouteResult.Success(path, bestCost[goalLocationId], expandedNodes);
                }

                foreach (Connection edge in queries.FindOutgoingConnections(entry))
                {
                    int candidate = bestCost[entry] + edge.TravelCost;
                    if (!bestCost.TryGetValue(edge.ToLocationId, out int knownCost) || knownCost > candidate)
                    {
                        bestCost[edge.ToLocationId] = candidate;
                        cameFrom[edge.ToLocationId] = entry;
                        frontier.Enqueue(edge.ToLocationId, candidate);
                    }
                }
            }
            return RouteResult.NotFound(expandedNodes);
        }
    }
}