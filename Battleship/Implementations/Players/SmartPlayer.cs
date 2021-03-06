﻿using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Base;
using Battleship.Interfaces;
using Battleship.Utilities;

namespace Battleship.Implementations.Players
{
    public class SmartPlayer : PlayerBase
    {
        public SmartPlayer(IGameField selfField) : base(selfField)
        {
        }

        public override CellPosition NextTarget
        {
            get
            {
                var prediction = GenerateNewPrediction();
                if (!CanPredictionBeReal(prediction))
                    throw null;

                IOrderedEnumerable<CellPosition> targets;
                var damagedShip = FindDamagedShip().ToList();
                if (damagedShip.Any())
                {
                    targets = damagedShip.SelectMany(x => x.ByEdgeNeighbours)
                        .Where(x => IsGoodTarget(x, prediction))
                        .OrderBy(x => 0);
                }
                else
                {
                    targets = prediction.EnumeratePositions()
                        .Where(x => IsGoodTarget(x, prediction))
                        .OrderByDescending(x => ((IShipCell) prediction[x]).Ship.Length);
                }

                return targets.ThenShuffle().First();
            }
        }

        private bool IsGoodTarget(CellPosition target, IGameField prediction)
        {
            return
                OpponentFieldKnowledge.Contains(target) &&
                prediction[target] is IShipCell &&
                !OpponentFieldKnowledge[target].HasValue;
        }

        protected IGameField GenerateNewPrediction()
        {
            var builder = new GameFieldBuilder();
            foreach (var position in OpponentFieldKnowledge.EnumeratePositions())
                if (OpponentFieldKnowledge[position] == true)
                    builder.TryAddShipCell(position);

            var generator = new RandomFieldGenerator(builder);
            var damagedShip = FindDamagedShip().ToList();
            if (damagedShip.Any())
            {
                foreach (var cell in damagedShip)
                    builder.TryRemoveShipCell(cell);

                var variants = new[] {4, 3, 2, 1}.SelectMany(x => new[]
                {
                    new {Ship = (ShipType) x, Vertical = true},
                    new {Ship = (ShipType) x, Vertical = false}
                }).SelectMany(x => GenerateContinuesForDamagedShip(damagedShip, builder, x.Vertical, x.Ship));

                foreach (var variant in variants)
                {
                    builder.TryAddFullShip(variant.Item1, variant.Item2, variant.Item3);
                    var prediction = generator.Generate(x => OpponentFieldKnowledge[x] != false);
                    if (prediction != null)
                        return prediction;
                    builder.TryRemoveFullShip(variant.Item1, variant.Item2, variant.Item3);
                }
            }
            return generator.Generate(x => OpponentFieldKnowledge[x] != false);
        }

        private IEnumerable<Tuple<ShipType, CellPosition, bool>> GenerateContinuesForDamagedShip(
            IList<CellPosition> damagedShipCells, IGameFieldBuilder builder, bool vertical, ShipType ship)
        {
            if (builder.ShipsLeft[ship] == 0)
                yield break;

            var topLeftCell = damagedShipCells.Min();
            var delta = vertical ? CellPosition.DeltaDown : CellPosition.DeltaRight;

            var start = vertical
                ? new CellPosition(0, topLeftCell.Column)
                : new CellPosition(topLeftCell.Row, 0);
            for (; builder.Contains(start); start += delta)
            {
                if (!builder.CanBeAddedSafely(ship, start, vertical, x => OpponentFieldKnowledge[x] != false))
                    continue;
                var newShipCells = Enumerable.Range(0, ship.GetLength()).Select(x => start + delta*x).ToList();
                if (damagedShipCells.Any(x => !newShipCells.Contains(x)))
                    continue;
                yield return Tuple.Create(ship, start, vertical);
            }
        }

        protected IEnumerable<CellPosition> FindDamagedShip()
        {
            var damagedShip = OpponentFieldKnowledge.EnumeratePositions()
                .FirstOrDefault(position =>
                    OpponentFieldKnowledge[position] == true &&
                    position.AllNeighbours.Any(neighbour =>
                        OpponentFieldKnowledge.Contains(neighbour) &&
                        OpponentFieldKnowledge[neighbour] == null));
            return damagedShip == null
                ? Enumerable.Empty<CellPosition>()
                : OpponentFieldKnowledge.FindAllConnectedByEdgeCells(damagedShip, knowledge => knowledge == true);
        }

        protected bool CanPredictionBeReal(IGameField prediction)
        {
            if (prediction == null)
                return false;

            return (
                from position in OpponentFieldKnowledge.EnumeratePositions()
                let knowledge = OpponentFieldKnowledge[position]
                where knowledge.HasValue
                select prediction[position] is IShipCell == knowledge)
                .All(x => x);
        }
    }
}
