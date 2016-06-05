﻿using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Interfaces;
using Battleship.Utilities;

namespace Battleship.Implementations
{
    public class RandomFieldGenerator : IRandomFieldGenerator
    {
        private readonly IGameFieldBuilder builder;

        public RandomFieldGenerator(IGameFieldBuilder builder)
        {
            this.builder = builder;
        }

        public IGameField Generate(Predicate<CellPosition> canUseCell)
        {
            var allShips = builder.ShipsLeft.SelectMany(x => Enumerable.Repeat(x.Key, x.Value)).ToList();
            TryAddAllShips(allShips, canUseCell);
            return builder.Build();
        }

        private bool TryAddAllShips(IList<ShipType> ships, Predicate<CellPosition> canUseCell)
        {
            if (!ships.Any())
                return true;

            var ship = ships.Last();
            ships.RemoveAt(ships.Count - 1);

            var availablePlaces = builder.EnumeratePositions()
                .SelectMany(position => new[]
                {
                    new {Position = position, Vertical = true},
                    new {Position = position, Vertical = false}
                })
                .Where(x => builder.CanBeAddedSafely(ship, x.Position, x.Vertical, canUseCell))
                .Shuffle();

            foreach (var place in availablePlaces)
            {
                builder.TryAddFullShip(ship, place.Position, place.Vertical);
                if (TryAddAllShips(ships, canUseCell))
                    return true;
                builder.TryRemoveFullShip(ship, place.Position, place.Vertical);
            }

            ships.Add(ship);
            return false;
        }
    }
}
