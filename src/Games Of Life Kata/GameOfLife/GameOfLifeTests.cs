using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using NUnit.Framework;

namespace GameOfLife
{
    [TestFixture]
    public class GameOfLifeTests
    {
        //Write this considering how it would be displayed on a screen
        
        [Test]
        public void CanSetBoardSize()
        {
            var game = new Game(new BoardSize(1, 1));
        }

        [Test]
        public void CanGetAViewOfTheBoard()
        {
            var game = new Game(new BoardSize(1, 1));
            var board = game.GetBoard();
        }

        //Skip board size checks

        [TestCase(1,1, 1)]
        [TestCase(2,2, 4)]
        [TestCase(3,2, 6)]
        public void BoardHasSameNumberOfPositionsAsGameBoard(int maxX, int maxY, int numberOfPositions)
        {
            var game = new Game(new BoardSize(maxX, maxY));
            var board = game.GetBoard();

            Assert.That(board.Positions.Count(), Is.EqualTo(numberOfPositions));
        }

        [Test]
        public void BoardFromGameWithNoCellsHasNoCells()
        {
            var game = new Game(new BoardSize(1, 1));
            var board = game.GetBoard();

            var singlePosition = board.Positions.First();

            Assert.That(singlePosition.HasLiveCell, Is.False);
        }

        [Test]
        public void CanAddACellatAPosition()
        {
            var game = new Game(new BoardSize(1, 1));
            game.AddCell(new Point(0, 0));
        }

        [Test]
        public void AfterAddingACellBoardHasLiveCellAtPosition()
        {
            var game = new Game(new BoardSize(1, 1));
            game.AddCell(new Point(0, 0));
            
            var board = game.GetBoard();
            var singlePosition = board.Positions.First();

            Assert.That(singlePosition.HasLiveCell, Is.True);
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
            return p1._x == p2._x && p1._y == p2._y;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }
    }

    public struct BoardSize
    {
        private readonly int _maxX;
        private readonly int _maxY;

        public BoardSize(int maxX, int maxY)
        {
            _maxX = maxX;
            _maxY = maxY;
        }

        public int MaxX { get { return _maxX; } }
        public int MaxY { get { return _maxY; } }

        public int NumberOfPositionsOnBoard()
        {
            return MaxX * MaxY;
        }
    }

    public class Game
    {
        private readonly BoardSize _boardSize;
        private List<Position> _positions;

        public Game(BoardSize boardSize)
        {
            _boardSize = boardSize;
            _positions = new List<Position>(_boardSize.NumberOfPositionsOnBoard());

            for (int x = 0; x < _boardSize.MaxX; x++)
            {
                for (int y = 0; y < _boardSize.MaxY; y++)
                {
                    _positions.Add(new Position(new Point(x, y)));
                }
            }
        }

        public Board GetBoard()
        {
           
            
            return new Board(_positions);
        }

        public void AddCell(Point point)
        {
            _positions.Single(position => position.Point == point).HasLiveCell = true;
        }
    }

    public class Board
    {
        private readonly IEnumerable<Position> _positions;

        public Board(IEnumerable<Position> positions)
        {
            _positions = positions;
        }

        public IEnumerable<Position> Positions
        {
            get { return _positions; }}
    }

    public class Position
    {
        private readonly Point _point;

        public Position(Point point)
        {
            _point = point;
        }

        public bool HasLiveCell { get; set; }

        public Point Point
        {
            get { return _point; }
        }
    }
}
