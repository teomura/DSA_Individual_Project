using System.Collections.Generic;
using NUnit.Framework;
using FrontierDispatch.Core;

namespace FrontierDispatch.Tests
{
    /// <summary>
    /// Small contract examples for later assignment phases.
    ///
    /// These are not an exhaustive assignment test suite.
    /// Students are expected to enable the relevant tests as they reach each
    /// phase and add meaningful tests of their own, including edge cases.
    /// </summary>
    public sealed class AssignmentStarterTests
    {
        // These tests document later assignment contracts without making the
        // untouched baseline red. Remove Ignore when you reach the named phase.

        [Test]
        public void PriorityQueue_EnqueueAndDequeue_ReturnsLowestPriorityFirst()
        {
            var queue = new MinPriorityQueue<string>();
            queue.Enqueue("expensive", 9);
            queue.Enqueue("cheap", 2);
            queue.Enqueue("medium", 5);
            Assert.That(queue.Dequeue(), Is.EqualTo("cheap"));
        }

        [Test]
        public void Dijkstra_StartEqualsGoal_ReturnsZeroCostRoute()
        {
            var finder = new DijkstraRouteFinder(WorldDatasetFactory.CreateSmall());
            RouteResult result = finder.FindRoute(0, 0);
            Assert.That(result.Found, Is.True);
            Assert.That(result.TotalCost, Is.EqualTo(0));
            Assert.That(result.LocationIds, Is.EqualTo(new[] { 0 }));
        }

        [Test]
        public void Dijkstra_MediumWorld_ReturnsLowestCostRoute()
        {
            var finder = new DijkstraRouteFinder(WorldDatasetFactory.CreateMedium());
            RouteResult result = finder.FindRoute(0, 254);
            Assert.That(result.Found, Is.True);
            Assert.That(result.TotalCost, Is.EqualTo(25));
            Assert.That(result.LocationIds, Is.EqualTo(new[] { 0, 50, 100, 101, 151, 152, 202, 203, 253, 254 }));
        }

        [Test]
        public void Dijkstra_MediumWorld_ReturnsRouteNotFound()
        {
            var finder = new DijkstraRouteFinder(WorldDatasetFactory.CreateMedium());
            RouteResult result = finder.FindRoute(0, 5000);
            Assert.That(result.Found, Is.False);
            Assert.That(result.TotalCost, Is.EqualTo(0));
            Assert.That(result.LocationIds, Is.Empty);
        }

        [Test]
        public void Dijkstra_SmallDisconnectedWorld_ReturnsRouteNotFound()
        {
            var finder = new DijkstraRouteFinder(SampleWorldFactory.CreateDisconnectedSmall());
            RouteResult result = finder.FindRoute(101, 901);
            Assert.That(result.Found, Is.False);
            Assert.That(result.TotalCost, Is.EqualTo(0));
            Assert.That(result.LocationIds, Is.Empty);
        }

        [Test]
        public void Dijkstra_SmallWorldWithOneWay_ReturnsAlternateLongerRoute()
        {
            var finder = new DijkstraRouteFinder(SampleWorldFactory.CreateSmallWithOneWay());
            RouteResult result = finder.FindRoute(101, 901);
            Assert.That(result.Found, Is.True);
            Assert.That(result.TotalCost, Is.EqualTo(21));
            Assert.That(result.LocationIds, Is.EqualTo(new[] { 101, 310, 512, 734, 901 }));
        }

        [Test]
        public void Reachability_ZeroBudget_IncludesStartAtZeroCost()
        {
            var query = new ReachabilityQuery(WorldDatasetFactory.CreateSmall());
            var result = query.FindReachable(0, 0);
            Assert.That(result.ContainsKey(0), Is.True);
            Assert.That(result[0], Is.EqualTo(0));
        }

        [Test]
        public void Reachability_Budget10_BasicFunctionality()
        {
            var query = new ReachabilityQuery(WorldDatasetFactory.CreateSmall());
            var result = query.FindReachable(0, 10);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(new List<int>(result.Keys), Is.EqualTo(new[] { 0, 1, 5, 10, 11 }));
            Assert.That(new List<int>(result.Values), Is.EqualTo(new[] { 0, 9, 5, 7, 10 }));
        }
    }
}