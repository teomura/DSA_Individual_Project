# REPORT.md - Individual Assignment

Keep this concise and specific to your implementation.

## 1. Initial analysis

- How are locations and connections represented in the baseline?
- Which operations appear frequent?
- Which operation(s) do you predict may scale poorly?
- What will you investigate first?

```text
Initial data:

Baseline world-query workload
Locations: 2500
Connections: 9800
Iterations: 5000
Bytes used: 1093072
Elapsed creation: 2 ms
Elapsed: 656 ms
Checksum: 6267100

- Arrays of Locations and Connections
- FindLocation and FindConnection (looping through all elements and comparisons)
- Looping through Locations and Connections in WorldData.
- Improve Connection storage in WorldQueries.cs
```

## 2. Data representation

Describe important changes/retained choices, operations affected, and time/memory trade-offs.

```text
Implemented a Dictionary for Connections to avoid looping through the whole list. Grouped connections by FromLocationId.

Baseline world-query workload
Locations: 2500
Connections: 9800
Iterations: 5000
Bytes used: 1528720
Elapsed creation: 2 ms
Elapsed: 71 ms
Checksum: 6267100

Implemented a Dictionary for Locations for a faster retrieval on average.

Baseline world-query workload
Locations: 2500
Connections: 9800
Iterations: 5000
Bytes used: 1744368
Elapsed creation: 3,2387 ms
Elapsed: 0,6565 ms
Checksum: 6267100

Traded-off increased memory usage due to two additional containers for a lower execution time. 
Spent more execution time at creating the containers.
```


## 3. Algorithms and adaptation

Explain your route finder at a useful high level and how the travel-budget query adapts the same ideas.

```text
The route finding algorithm is Dijkstra's shortest path algorithm (uses BFS to process Locations).
It goes through each Location and looks if there's a better cost found to its neighbors and updates it (the cost) accordingly.

Frontier is a MinPriorityQueue containing the unprocessed Locations and the best cost from the current Location, 
but can also contain previous entries with potentially stale costs.

The next candidate is the Location with the lowest known cost to it.

The best cost to a Location is stored in a Dictionary. It's tracked when a Location is first processed and potentially when a 
better candidate is found. This is also known as relaxation, which also updates how this location was reached (cameFrom).
Once the processing reaches the goal Location the algorithm will reconstruct the path backwards based on "cameFrom" entries,
and then reverse it to get the path in the correct order.
It will also output the best cost to the Location and the number of Locations processed to get there.

The reachability algorithm uses the same base Dijkstra approach but instead of stopping when reaching the goal,
it stops when there are no more nodes to process. 
Also, it doesn't add to bestCost the neighbors of a node that have a candidate cost larger than the budget cost, 
thus ensuring bestCost contains only the nodes within budget and best cost to reach them.
```


## 4. Complexity

| Operation					 | Your representation/algorithm | Complexity | Short explanation |
| ---						 | ---							 | ---		  | ---				  |
| Find location by ID		 |	Dictionary lookup			 |	O(1) on average	  | optimization trade-off - increased memory usage for faster execution time - keys hashed to a bucket, independent of cotainer size|
| Get outgoing connections	 |	Dictionary lookup			 |	O(1) on average	  | optimization trade-off - increased memory usage for faster execution time - keys hashed to a bucket, independent of cotainer size|
| Priority queue enqueue	 |	Binary heap					 |	O(log n)  | the complexity increases due to sifting and swapping|
| Priority queue dequeue-min |	Binary heap					 |	O(log n)  | the complexity increases due to sifting and swapping|
| Route finding				 |	Dijkstra + priority queue (binary heap)	 |O((V+E)logV)| always processing the node with the lowest cost in the frontier|
| Reachability within budget | 	Dijkstra + priority queue +budget check| O(V+E)logV)| Same process as Dijkstra - each Connection gets one O(1) budget check, but over budget Locations are never enqueued|
| Dictionary initialisation(world prep)| Array populating| O(V+E)| Hashing of all Locations and Connection into Dictionaries|



## 5. Correctness and testing

Important tests/edge cases and what they demonstrate.


| ---										| ---								| ---		|
|Dijkstra_MediumWorld_ReturnsLowestCostRoute| Functionality check, finding cheapest route between two connected Locations| Revealed a bug (see #7), returned cheapest route after fixing.|
|Dijkstra_MediumWorld_ReturnsRouteNotFound| Checking for a route to non existent Location| No route was found.|
|Dijkstra_SmallDisconnectedWorld_ReturnsRouteNotFound| Checking for route to a disconnected Location| No route was found.|
|Dijkstra_SmallWorldWithOneWay_ReturnsAlternateLongerRoute| Expected to respect directionality and cheapest cost and to go through Crossroads(310) and not Relay(205)| Performed as expected.|
|Reachability_Budget10_BasicFunctionality| Functionality check| Returned the correct Locations and their costs under a max cost of 10.|



## 6. Performance investigation

Record reproducible before/after measurements for loading/preparation, repeated runtime work, and relevant allocation behavior. Explain what changed and why.



| Measurement					 | Baseline			 |  Improved		 |
| ---							 | ---				 | ---				 |
| World preparation              | 2 ms				 | 3,2387 ms				 |
| Repeated lookup/query workload | 656 ms		   	 | 0,6565 ms				 |
| Allocated bytes                | 1093072			 | 1744368			 |


```text
Collected operation time and world preparation time in the baseline using Stopwatch. 
Tracked allocated bytes with GC before and after world preparation. 
Repeated lookup returned a rounded 0 ms reading which I improved with Elapsed.TotalMilliseconds to 0,6565 ms.


World preparation: increased from 2 ms to 3,2387 ms because of adding data to Dictionaries 
which iterates through all Locations and Connections (O(n)).

Repeated lookup/query workload: changed storage for Locations and Connections to use Dictionaries for 
faster average retrieval (O(1)).

Allocated bytes: increased because of storing a copy of the world data in Dictionaries 
(WorldQueries._worldConnections and WorldQueries._worldLocations).

In conclusion, caching data improves execution time in the long run, 
but requires additional memory and may take time to build it. 
```

## 7. Development reflection

Describe one bug, wrong assumption, failed approach, or surprising result and how you investigated it.


```text
On a medium world, finding a route led to an infinite loop due to a bug in the MinPriorityQueue 
where it wouldn't exit if there were two entries with equal or lower priorities. I investigated it by debugging 
and found that the index never changed from 0, thus never exiting from the loop. Added a break on the correct condition.

```

## 8. Tools and AI assistance

Briefly describe relevant assistance, if any.

```text
Used Gemini to get a route to use in the tests for a Medium World.

```

## 9. VG extension

Only if attempting VG: heuristic, traversal constraints, correctness, and Dijkstra/A* comparison.
