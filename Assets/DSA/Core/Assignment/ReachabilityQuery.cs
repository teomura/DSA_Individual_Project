using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    // Assignment Phase 5. Adapt shortest-path reasoning to a travel-cost budget.

    /// <summary>
    /// Finds locations that can be reached from a starting location without
    /// exceeding a maximum total travel cost.
    ///
    /// This is an adaptation of shortest-path reasoning: instead of searching
    /// for one goal, the query determines the cheapest known cost to every
    /// location that fits within the supplied budget.
    ///
    /// This class is intentionally incomplete in the assignment starter.
    ///
    /// What do we mean by "query"? In this context, a query is simply a one-time search for information.
    /// </summary>
    public sealed class ReachabilityQuery
    {
        private readonly WorldData _world;

        /// <summary>
        /// Creates a reachability query operating on the supplied world.
        /// </summary>
        public ReachabilityQuery(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        /// <summary>
        /// Returns reachable location IDs mapped to their cheapest known
        /// travel cost from the starting location.
        ///
        /// Only locations whose cheapest cost is within maxTravelCost should
        /// be included. The starting location has cost 0.
        /// </summary>
        public IReadOnlyDictionary<int, int> FindReachable(int startLocationId, int maxTravelCost)
        {
            // TODO: adapt shortest-path reasoning to the travel-cost budget.

            WorldQueries queries = new WorldQueries(_world);
            if (queries == null)
            {
                throw new ArgumentNullException(nameof(queries));
            }

            var bestCost = new Dictionary<int, int>();

            if (queries.FindLocationById(startLocationId) == null)
            {
                return bestCost;
            }

            if (maxTravelCost < 0)
            {
                return bestCost;
            }

            var frontier = new MinPriorityQueue<int>();

            bestCost[startLocationId] = 0;
            frontier.Enqueue(startLocationId, 0);

            while (frontier.Count > 0)
            {
                int priority = frontier.LowestPriority();
                int entry = frontier.Dequeue();

                if (bestCost.TryGetValue(entry, out int cost) && cost != priority)
                {
                    continue;
                }

                foreach (Connection edge in queries.FindOutgoingConnections(entry))
                {
                    int candidate = bestCost[entry] + edge.TravelCost;

                    if (candidate > maxTravelCost)
                    {
                        continue;
                    }
                    if (!bestCost.TryGetValue(edge.ToLocationId, out int knownCost) || knownCost > candidate)
                    {
                        bestCost[edge.ToLocationId] = candidate;

                        frontier.Enqueue(edge.ToLocationId, candidate);
                    }
                }
            }
            return bestCost;
        }
    }
}