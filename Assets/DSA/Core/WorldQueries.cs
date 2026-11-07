#nullable enable

using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    /// <summary>
    /// Straightforward queries over the supplied WorldData.
    ///
    /// The starter project intentionally performs these queries directly over the stored
    /// collections. The implementation is correct, but students should analyze
    /// how its work scales as the world becomes larger and queries are repeated.
    ///
    /// Do not assume that every method must be replaced. Identify the operations
    /// that matter first, then justify any representation changes.
    /// </summary>
    public sealed class WorldQueries
    {
        private readonly WorldData _world;
        private readonly Dictionary<int, List<Connection>> _worldConnections = new Dictionary<int, List<Connection>>();
        private readonly Dictionary<int, Location> _worldLocations = new Dictionary<int, Location>();

        public WorldQueries(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));

            for (int i = 0; i < _world.Connections.Count; i++)
            {
                Connection connection = _world.Connections[i];
                if (!_worldConnections.ContainsKey(connection.FromLocationId))
                {
                    _worldConnections[connection.FromLocationId] = new List<Connection>();// create new entry space
                }
                _worldConnections[connection.FromLocationId].Add(connection);// add to collection
            }

            for (int i = 0; i < _world.Locations.Count; i++)// creating a location bucket
            {
                Location location = _world.Locations[i];
                _worldLocations.Add(location.Id, location);
            }
        }

        public Location? FindLocationById(int id)
        {
            return _worldLocations.GetValueOrDefault(id);
            /*for (int i = 0; i < _world.Locations.Count; ++i)
            {
                Location location = _world.Locations[i];
                if (location.Id == id)
                {
                    return location;
                }
            }

            return null;*/
        }

        public IReadOnlyList<Connection> FindOutgoingConnections(int fromLocationId)
        {
            var result = _worldConnections[fromLocationId] ?? new List<Connection>();
            /* for (int i = 0; i < _world.Connections.Count; ++i)
             {
                 Connection connection = _world.Connections[i];
                 if (connection.FromLocationId == fromLocationId)
                 {
                     result.Add(connection);
                 }
             }*/

            return result;
        }
    }
}