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
Elapsed: 616 ms
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
Elapsed: 70 ms
Checksum: 6267100

Implemented a dictionary for Locations for a faster retrieval on average.

Baseline world-query workload
Locations: 2500
Connections: 9800
Iterations: 5000
Elapsed: 0 ms
Checksum: 6267100

Trade-off increased memory usage due to two additional containers for a lower execution time.
```

## 3. Algorithms and adaptation

Explain your route finder at a useful high level and how the travel-budget query adapts the same ideas.

## 4. Complexity

| Operation					 | Your representation/algorithm | Complexity | Short explanation |
| ---						 | ---							 | ---		  | ---				  |
| Find location by ID		 |	Dictionary lookup			 |	O(1)	  | optimization	  |
| Get outgoing connections	 |	Dictionary lookup			 |	O(1)	  | optimization	  |
| Priority queue enqueue	 |								 |			  | |
| Priority queue dequeue-min |								 |			  | |
| Route finding				 |								 |			  | |

## 5. Correctness and testing

Important tests/edge cases and what they demonstrate.

## 6. Performance investigation

Record reproducible before/after measurements for loading/preparation, repeated runtime work, and relevant allocation behavior. Explain what changed and why.

## 7. Development reflection

Describe one bug, wrong assumption, failed approach, or surprising result and how you investigated it.

```text
Indexing in a dictionary for an inexistent key created an entry for that key.
```

## 8. Tools and AI assistance

Briefly describe relevant assistance, if any.

## 9. VG extension

Only if attempting VG: heuristic, traversal constraints, correctness, and Dijkstra/A* comparison.
