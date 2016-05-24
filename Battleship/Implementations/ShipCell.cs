﻿using System;
using Battleship.Interfaces;
using Battleship.Utilities;

namespace Battleship.Implementations
{
    public class ShipCell : IGameCell
    {
        public CellPosition Position { get; }
        public bool Damaged { get; set; }
        public IShip Ship { get; }

        public ShipCell(CellPosition position, IShip ship)
        {
            if (position == null)
                throw new ArgumentNullException(nameof(position));
            if (ship == null)
                throw new ArgumentNullException(nameof(ship));

            Position = position;
            Ship = ship;
        }

        protected bool Equals(ShipCell other)
        {
            return
                Equals(Position, other.Position) &&
                Equals(Damaged, other.Damaged);
        }

        public override bool Equals(object obj)
        {
            var other = obj as ShipCell;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            return -1;
        }
    }
}