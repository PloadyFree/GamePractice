﻿using Battleship.Base;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CellPosition_Should
    {
        #region Base tests

        [Test]
        public void ReturnRealEdgesLength()
        {
            var row = 1234;
            var column = 12342;

            var position = new CellPosition(row, column);

            position.Row.Should().Be(row);
            position.Column.Should().Be(column);
        }

        [Test]
        public void CompareRight()
        {
            var pos1 = new CellPosition(54, 2);
            var pos2 = new CellPosition(4, 200);
            var pos3 = new CellPosition(54, 2);

            pos1.Should().BeGreaterThan(pos2);
            pos2.Should().BeLessThan(pos1);

            pos3.Should().BeGreaterOrEqualTo(pos1);
            pos1.Should().BeGreaterOrEqualTo(pos3);
        }

        #endregion

        #region All neighbours tests

        [Test]
        public void ReturnAllNeighboursCorrectly_WhenRequestedZeroCell()
        {
            var cell = new CellPosition(0, 0);

            var neighbours = new[]
            {
                new CellPosition(-1, -1), new CellPosition(-1, 0), new CellPosition(-1, 1),
                new CellPosition(0, -1), new CellPosition(0, 1),
                new CellPosition(1, -1), new CellPosition(1, 0), new CellPosition(1, 1)
            };

            cell.AllNeighbours.Should().BeEquivalentTo(neighbours);
        }

        [Test]
        public void ReturnAllNeighboursCorrectly_WhenCoordinatesArePositive()
        {
            var cell = new CellPosition(50, 100);

            var neighbours = new[]
            {
                new CellPosition(49, 99), new CellPosition(49, 100), new CellPosition(49, 101),
                new CellPosition(50, 99), new CellPosition(50, 101),
                new CellPosition(51, 99), new CellPosition(51, 100), new CellPosition(51, 101)
            };

            cell.AllNeighbours.Should().BeEquivalentTo(neighbours);
        }

        [Test]
        public void ReturnAllNeighboursCorrectly_WhenCoordinatesAreNegative()
        {
            var cell = new CellPosition(-50, -100);

            var neighbours = new[]
            {
                new CellPosition(-51, -101), new CellPosition(-51, -100), new CellPosition(-51, -99),
                new CellPosition(-50, -101), new CellPosition(-50, -99),
                new CellPosition(-49, -101), new CellPosition(-49, -100), new CellPosition(-49, -99)
            };

            cell.AllNeighbours.Should().BeEquivalentTo(neighbours);
        }

        #endregion

        #region By vertex neighbours tests

        [Test]
        public void ReturnByVertexNeighboursCorrectry_WhenRequestedZeroCell()
        {
            var cell = new CellPosition(0, 0);

            var neighbours = new[]
            {
                new CellPosition(-1, -1),
                new CellPosition(-1, 1),
                new CellPosition(1, -1),
                new CellPosition(1, 1)
            };

            cell.ByVertexNeighbours.Should().BeEquivalentTo(neighbours);
        }

        [Test]
        public void ReturnByVertexNeighboursCorrectry_WhenCoordinatesArePositive()
        {
            var cell = new CellPosition(50, 100);

            var neighbours = new[]
            {
                new CellPosition(49, 99),
                new CellPosition(49, 101),
                new CellPosition(51, 99),
                new CellPosition(51, 101)
            };

            cell.ByVertexNeighbours.Should().BeEquivalentTo(neighbours);
        }

        [Test]
        public void ReturnByVertexNeighboursCorrectry_WhenCoordinatesAreNegative()
        {
            var cell = new CellPosition(-50, -100);

            var neighbours = new[]
            {
                new CellPosition(-51, -101),
                new CellPosition(-49, -99),
                new CellPosition(-51, -99),
                new CellPosition(-49, -101)
            };

            cell.ByVertexNeighbours.Should().BeEquivalentTo(neighbours);
        }

        #endregion

        #region By edge neighbours tests

        [Test]
        public void ReturnByEdgeNeighboursCorrectry_WhenRequestedZeroCell()
        {
            var cell = new CellPosition(0, 0);

            var neighbours = new[]
            {
                new CellPosition(-1, 0),
                new CellPosition(0, 1),
                new CellPosition(1, 0),
                new CellPosition(0, -1)
            };

            cell.ByEdgeNeighbours.Should().BeEquivalentTo(neighbours);
        }

        [Test]
        public void ReturnByEdgeNeighboursCorrectry_WhenCoordinatesArePositive()
        {
            var cell = new CellPosition(50, 100);

            var neighbours = new[]
            {
                new CellPosition(49, 100),
                new CellPosition(50, 101),
                new CellPosition(51, 100),
                new CellPosition(50, 99)
            };

            cell.ByEdgeNeighbours.Should().BeEquivalentTo(neighbours);
        }

        [Test]
        public void ReturnByEdgeNeighboursCorrectry_WhenCoordinatesAreNegative()
        {
            var cell = new CellPosition(-50, -100);

            var neighbours = new[]
            {
                new CellPosition(-49, -100),
                new CellPosition(-50, -99),
                new CellPosition(-51, -100),
                new CellPosition(-50, -101)
            };

            cell.ByEdgeNeighbours.Should().BeEquivalentTo(neighbours);
        }

        #endregion

        #region Add delta tests

        [Test]
        public void AddDeltaCorrectly_WhenBothDeltasArePositive()
        {
            var cell = new CellPosition(12, 13);
            var delta = new CellPosition(3, 1);

            var result = cell + delta;

            result.Row.Should().Be(12 + 3);
            result.Column.Should().Be(13 + 1);
        }

        [Test]
        public void AddDeltaCorrectly_WhenDeltaRowIsZero()
        {
            var cell = new CellPosition(12, 13);
            var delta = new CellPosition(0, 1);

            var result = cell + delta;

            result.Row.Should().Be(12);
            result.Column.Should().Be(13 + 1);
        }

        [Test]
        public void RemainTheSame_WhenDeltaIsZero()
        {
            var cell = new CellPosition(12, 13);
            var delta = new CellPosition(0, 0);

            var result = cell + delta;

            result.Row.Should().Be(12);
            result.Column.Should().Be(13);
        }

        [Test]
        public void AddDeltaCorrectly_WhenCellRowIsNegative()
        {
            var cell = new CellPosition(-12, 13);
            var delta = new CellPosition(10, 44);

            var result = cell + delta;

            result.Row.Should().Be(-12 + 10);
            result.Column.Should().Be(13 + 44);
        }

        [Test]
        public void AddDeltaCorrectly_WhenDeltaIsNegative()
        {
            var cell = new CellPosition(-12, 13);
            var delta = new CellPosition(-10, -44);

            var result = cell + delta;

            result.Row.Should().Be(-12 - 10);
            result.Column.Should().Be(13 - 44);
        }

        #endregion
    }
}
