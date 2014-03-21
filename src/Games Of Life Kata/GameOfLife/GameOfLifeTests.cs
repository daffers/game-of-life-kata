using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using NUnit.Framework;

namespace GameOfLife
{
    [TestFixture]
    public class GameOfLifeTests
    {
        [Test]
        public void CanSpecifyGameWithStartSize()
        {
            var game = new GameOfLife(1, 1);
        }

        [TestCase(0,0)]
        [TestCase(1,0)]
        [TestCase(0,1)]
        [TestCase(0,-1)]
        [TestCase(-1,-1)]
        [TestCase(-1,0)]
        public void CannotCreateGameWithSizeLessThanOneCell(int maxX, int maxY)
        {
            Assert.Throws<GameOfLife.InvalidPlayAreaSizeException>(() => new GameOfLife(maxX, maxY));
        }

        [Test]
        public void CanPlaceCellInGame()
        {
            var game = new GameOfLife(1, 1);
            game.PlaceCell(0, 0);
        }

        [TestCase(1,0)]
        [TestCase(0,1)]
        [TestCase(1,1)]
        [TestCase(-1,-1)]
        [TestCase(0,-1)]
        public void CannotPlaceCellOutSideOfPlayArea(int x, int y)
        {
            var game = new GameOfLife(1, 1);
            Assert.Throws<GameOfLife.CellPlacedOutSideGameAreaException>(() => game.PlaceCell(x, y));
        }

        [Test]
        public void CanCycleTheGame()
        {
            var game = new GameOfLife(1, 1);
            game.PlaceCell(0,0);
            game.Cycle();
        }

        [Test]
        public void WhenNoCellsPlacedGameEndsAfterCycle()
        {
            var game = new GameOfLife(1, 1);
            game.Cycle();
            Assert.That(game.IsOver, Is.True);
        }

        [Test]
        public void CanRetrieveAliveStatusOfCellsPlaced()
        {
            var game = new GameOfLife(1, 1);
            game.PlaceCell(0,0);
            Assert.That(game.IsCellLive(0,0), Is.True);
        }

        [Test]
        public void WhenNoCellInPositionIsAliveIsFalse()
        {
            var game = new GameOfLife(1, 1);
            Assert.That(game.IsCellLive(0,0), Is.False);
        }

        [Test]
        public void WhenOneCellPlacedGameCyclesCellDies()
        {
            var game = new GameOfLife(1, 1);
            game.PlaceCell(0,0);
            game.Cycle();

            Assert.That(game.IsCellLive(0, 0), Is.False);
        }

        [Test]
        public void OneCellWithOneNeighborDiesOnGameCycle()
        {
             var game = new GameOfLife(2, 2);
            game.PlaceCell(0,0);
            game.PlaceCell(0,1);
            game.Cycle();

            Assert.That(game.IsCellLive(0, 0), Is.False);
        }

        [Test]
        public void OneCellWithTwoNeighborsLivesOnGameCycle()
        {
            var game = new GameOfLife(2, 2);
            game.PlaceCell(0,0);
            game.PlaceCell(0,1);
            game.PlaceCell(1,0);
            game.Cycle();

            Assert.That(game.IsCellLive(0, 0), Is.True);
        }


        [Test]
        public void OneCellStartingTopRightWithTwoNeighborsLivesOnGameCycle()
        {
            var game = new GameOfLife(2, 2);
            game.PlaceCell(1, 1);
            game.PlaceCell(0, 1);
            game.PlaceCell(1, 0);
            game.Cycle();

            Assert.That(game.IsCellLive(1, 1), Is.True);
        }
        //Prevent placement on live cell

        [Test]
        public void OneCellWithThreeNeighboursSurviesCycle()
        {
              var game = new GameOfLife(3, 3);
            game.PlaceCell(1, 1);
            
            game.PlaceCell(0, 1);
            game.PlaceCell(2, 1);
            game.PlaceCell(1, 2);

            game.Cycle();

            Assert.That(game.IsCellLive(1, 1), Is.True);
        }


        [Test]
        public void OneCellWithFourNeighboursDiesInCycle()
        {
            var game = new GameOfLife(3, 3);
            game.PlaceCell(1, 1);

            game.PlaceCell(0, 1);
            game.PlaceCell(2, 1);
            game.PlaceCell(1, 2);
            game.PlaceCell(1, 0);

            game.Cycle();

            Assert.That(game.IsCellLive(1, 1), Is.False);
        }
    }

    public class GameOfLife
    {
        private readonly int _maxX;
        private readonly int _maxY;

        private readonly bool[,] _playArea;

        public GameOfLife(int maxX, int maxY)
        {
            _maxX = maxX;
            _maxY = maxY;
            if (maxX <= 0 || maxY <= 0)
                throw new InvalidPlayAreaSizeException();

            _playArea = new bool[_maxX, _maxY];
        }

        public bool IsOver { get; private set; }

        public void PlaceCell(int x, int y)
        {
            if (x >= _maxX || x < 0 || y >= _maxY || y < 0)
                throw new CellPlacedOutSideGameAreaException();
            _playArea[x,y] = true;
        }

        public class InvalidPlayAreaSizeException : Exception
        {
        }

        public class CellPlacedOutSideGameAreaException : Exception
        {
        }

        public void Cycle()
        {
            for (int x = 0; x < _maxX; x++)
            {
                for (int y = 0; y < _maxY; y++)
                {
                    var numberOfNeighbors = GetNumberOfNeighbours(x, y);
                    if (HasNeighbourAbove(x, y) && HasNeighbourToRight(x, y))
                        _playArea[x, y] = true;
                    else
                        _playArea[x, y] = false;    
                }
            }
            
            IsOver = true;
        }

        private object GetNumberOfNeighbours(int x, int y)
        {
            
        }

        private bool HasNeighbourToRight(int x, int y)
        {
            if (x >= _maxX)
                return false;

            return _playArea[x,y];
        }

        private bool HasNeighbourAbove(int x, int y)
        {
            if (y >= _maxY)
                return false;

            return _playArea[x,y];
        }

        public bool IsCellLive(int x, int y)
        {
            return _playArea[x,y];
        }
    }
}
