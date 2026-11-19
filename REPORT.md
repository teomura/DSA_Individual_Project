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

- Arrays of Location and Connection
- FindLocation and FindConnection (looping through all elements and comparisons)
- Looping through elements
- Improve Connection storage in WorldQueries.cs
```

## 2. Data representation

Describe important changes/retained choices, operations affected, and time/memory trade-offs.

```text
Implemented a dictionary for Connections to avoid looping through the whole list. Grouped connections by FromLocationId.

Baseline world-query workload
Locations: 2500
Connections: 9800
Iterations: 5000
Bytes used: 1528720
Elapsed creation: 2 ms
Elapsed: 71 ms
Checksum: 6267100

Implemented a dictionary for Locations for a faster retrieval on average.

Baseline world-query workload
Locations: 2500
Connections: 9800
Iterations: 5000
Bytes used: 1744368
Elapsed creation: 3 ms
Elapsed: 0 ms
Checksum: 6267100

Trade-off increased memory usage due to two additional containers for a lower execution time. Spent more execution time at creating the containers.
```

## 3. Algorithms and adaptation

Explain your route finder at a useful high level and how the travel-budget query adapts the same ideas.

## 4. Complexity

| Operation					 | Your representation/algorithm | Complexity | Short explanation |
| ---						 | ---							 | ---		  | ---				  |
| Find location by ID		 |	Dictionary lookup			 |	O(1)	  | optimization trade-off - increased memory usage for faster execution time|
| Get outgoing connections	 |	Dictionary lookup			 |	O(1)	  | optimization trade-off - increased memory usage for faster execution time|
| Priority queue enqueue	 |	Binary heap					 |	O(log n)  | the complexity increases due to sifting and swapping|
| Priority queue dequeue-min |	Binary heap					 |	O(log n)  | the complexity increases due to sifting and swapping|
| Route finding				 |	Dijkstra + priority queue	 |O((V+E)logV)| always processing the node with the lowest cost in the frontier|
|							 |				(binary heap)	 |				
## 5. Correctness and testing

Important tests/edge cases and what they demonstrate.

## 6. Performance investigation

Record reproducible before/after measurements for loading/preparation, repeated runtime work, and relevant allocation behavior. Explain what changed and why.



| Measurement					 | Baseline			 |  Improved		 |
| ---							 | ---				 | ---				 |
| World preparation              | 2 ms				 ||
| Repeated lookup/query workload | 656 ms		   	 | 0 ms				 |
| Allocated bytes                | 1093072			 ||

//describe the workload, how measurements were collected, and what
conclusions can reasonably be drawn.

```text
Collected operation times with Stopwatch and allocated bytes with GC before and after execution.


World preparation: 
Repeated lookup/query workload: changed storage for Locations and Connections to use Dictionaries for faster average retrieval (O(1)).
Allocated bytes:
```

## 7. Development reflection

Describe one bug, wrong assumption, failed approach, or surprising result and how you investigated it.


```text
Indexing in a dictionary for an inexistent key created an entry for that key.

On a medium world, finding a route led to an infinite loop due to a bug in the MinPriorityQueue where I wouldn't exit if there were two entries with equal or lower priorities.
I investigated it by debugging and found that the index never changed from 0, thus never exiting from the loop. Added a break on the correct condition.

```

## 8. Tools and AI assistance

Briefly describe relevant assistance, if any.

```text
Used Gemini to get a route to use in the tests.

```

## 9. VG extension

Only if attempting VG: heuristic, traversal constraints, correctness, and Dijkstra/A* comparison.
