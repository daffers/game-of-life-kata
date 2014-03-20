using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace GameOfLife
{
    [TestFixture]
    public class GameOfLifeTestsCellsKnowTheBoard
    {
        [Test]
        public void CanCreateCell()
        {
            var cell = new Cell();
        }
        
        [Test]
        public void CanPlaceCellOnABoard()
        {
            var board = new Board(1, 1);
            var cell = new Cell();

            board.PlaceCell(cell, new Point(0, 0));
        }

        [Test]
        public void CanSetSizeOfBoard()
        {
            var board = new Board(1, 1);
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(-1, -1)]
        public void InvalidBoardSizeThrowsException(int maxX, int maxY)
        {
            Assert.Throws<Board.InvalidBoardSizeException>(() => new Board(maxX, maxY));
        }

        [Test]
        public void WhenCellIsCreatedCellIsAlive()
        {
            var cell = new Cell();
            Assert.That(cell.IsAlive, Is.True);
        }

        [Test]
        public void WhenASingleCellOnBoardWhenGameCycleCellDies()
        {
            var board = new Board(1, 1);
            var game = new Game(board);
            var cell = new Cell();
            game.PlaceCell(cell, new Point(0,0));
            game.Cycle();

            Assert.That(cell.IsAlive, Is.False);
        }

        [Test]
        public void CellBorderdBy2CellsWhenGameCyclesCellLives()
        {
            var board = new Board(2, 2);
            var game = new Game(board);
            var cell = new Cell();
            game.PlaceCell(cell, new Point(0,0));
            game.PlaceCell(new Cell(), new Point(1,0));
            game.PlaceCell(new Cell(), new Point(0,1));
            game.Cycle();

            Assert.That(cell.IsAlive, Is.True);
        }

        [Test]
        public void CanCreateGameWithBoard()
        {
            var board = new Board(1, 1);
            var game = new Game(board);
        }

        [Test]
        public void CanAddCellsToGame()
        {
            var board = new Board(1, 1);
            var game = new Game(board);
            var cell = new Cell();

            game.PlaceCell(cell, new Point(0, 0));
        }

        [Test]
        public void EqualPointsAreEqual()
        {
            var p1 = new Point(1, 1);
            var p2 = new Point(1, 1);

            Assert.That(p1 == p2, Is.True);
        }

        [Test]
        public void TestName()
        {

        }
    }

    public class Game
    {
        private readonly Board _board;

        public Game(Board board)
        {
            _board = board;
        }

        public void PlaceCell(Cell cell, Point point)
        {
            _board.PlaceCell(cell, point);
        }

        public void Cycle()
        {
            foreach (var position in _board.Positions)
            {
                position.CycleCell();
            }
        }
    }

    public struct Point
    {
        private readonly int _x;
        private readonly int _y;

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return (p1._x == p2._x && p1._y == p2._y);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }
    }

    public class Board
    {
        private  List<Position> _positions;

        public Board(int maxX, int maxY)
        {
            if (maxX <= 0 || maxY <= 0)
                throw new InvalidBoardSizeException();

            _positions = new List<Position>();
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    _positions.Add(new Position(new Point(x, y)));
                }
            }
        }

        public IEnumerable<Position> Positions
        {
            get { return _positions; }
        }

        public void PlaceCell(Cell cell, Point point)
        {
            _positions.Single(position => position.Point == point).PlaceCell(cell);
        }

        public class InvalidBoardSizeException : Exception
        {
        }
    }

    public class Position
    {
        public Point Point { get; private set; }
        private Cell _currentCell;

        public Position(Point point)
        {
            Point = point;
        }


        public void CycleCell()
        {
            if (_currentCell != null)
                _currentCell.Cycle();
        }

        public void PlaceCell(Cell cell)
        {
            _currentCell = cell;
        }
    }

    public class Cell
    {
        public Cell()
        {
            IsAlive = true;
        }

        public bool IsAlive { get; private set; }

        public void Cycle()
        {
            IsAlive = false;
        }
    }
}
