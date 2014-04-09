using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NUnit.Framework;

namespace GameOfLife
{
    [TestFixture]
    public class GameOfLifeTests
    {
        [Test]
        public void CanMakeABoard()
        {
            var board = CreateBoard(1, 1);
        }

        [TestCase(0,0)]
        [TestCase(0,1)]
        [TestCase(1,0)]
        [TestCase(-1,-1)]
        public void BoardSizeOfXorYLessThanOneCreatesAnUnplayableGame(int maxX, int maxY)
        {
            var board = CreateBoard(maxX, maxY);

            Assert.That(board, Is.TypeOf<UnplayableGame>());
        }

        [Test]
        public void BoardSizeOfGreaterThan0ForXAndYCreateGameOfLife()
        {
            var board = CreateBoard(1, 1);

            Assert.That(board, Is.TypeOf<Game>());
        }

        [Test]
        public void CanSeedCellsWithListOfPositions()
        {
            var board = CreateBoard(1, 1) as Game;

            board.SeedWithCells(new List<Position>());
        }

        [Test]
        public void CanGetAViewOfTheBoard()
        {
            var board = CreateBoard(1, 1) as Game;

            var view = board.ViewBoard();

            Assert.That(view, Is.TypeOf<GameView>());
        }

        [Test]
        public void ABoardWithNotCellsSeededHasAnEmptyView()
        {
            var board = CreateBoard(1, 1) as Game;

            var view = board.ViewBoard();

            Assert.That(view.Cells.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ABoardWithOneSeededCellHasAViewWithOneCell()
        {
            var board = CreateBoard(1, 1) as Game;

            board.SeedWithCells(new List<Position>(){new Position(0,0)});

            var view = board.ViewBoard();

            Assert.That(view.Cells.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhenOneCellOnBoardAndGameCyclesCellDies()
        {
            var board = CreateBoard(1, 1) as Game;

            board.SeedWithCells(new List<Position>(){new Position(0,0)});
            board.Cycle();

            var view = board.ViewBoard();

            Assert.That(view.Cells.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhenCellHasOneNeighbourCellDiesOnGameCycle()
        {
             var board = CreateBoard(1, 2) as Game;

            board.SeedWithCells(new List<Position>(){{new Position(0,0)}, {new Position(0,1)}});
            board.Cycle();

            var view = board.ViewBoard();

            Assert.That(view.Cells.Count(), Is.EqualTo(0));
        }

        [Test]
        public void WhenCellHasTwoNeighboursCellLivesOnGameCycle()
        {
             var board = CreateBoard(1, 2) as Game;

            board.SeedWithCells(new List<Position>()
            {
                new Position(0,0), new Position(0,1), new Position(1,0)
            });
            board.Cycle();

            var view = board.ViewBoard();

            Assert.That(view.Cells.Count(), Is.EqualTo(1));
        }

        private object CreateBoard(int maxX, int maxY)
        {
            if (maxX < 1 || maxY < 1)
                return new UnplayableGame();

            return new Game();
        }
    }

    public class GameView
    {
        public GameView(IEnumerable<Cell> cells)
        {
            Cells = cells;
        }
        public IEnumerable<Cell> Cells { get; private set; }
    }

    public class Cell
    {
    }

    public struct Position
    {
        private readonly int _x;
        private readonly int _y;

        public Position(int x, int y)
        {
            _x = x;
            _y = y;
        }


        public bool IsNeighbor(Position positionForComparison)
        {
            return !(positionForComparison._y - _y > 1) && !(positionForComparison._x - _x > 1);
            
        }
    }

    [TestFixture]
    public class PositionTests
    {
        [TestCase(0, 1, true)]
        [TestCase(1, 0, true)]
        [TestCase(1, 1, false)]
        [TestCase(0, 2, false)]
        [TestCase(2, 0, false)]
        public void CanDetermineWhenACellNeighboursIt(int comparisonX, int comparisonY, bool isNeighbor)
        {
            var positionUnderTest = new Position(0, 0);
            var positionForComparison = new Position(comparisonX, comparisonY);

            var result = positionUnderTest.IsNeighbor(positionForComparison);

            Assert.That(result, Is.EqualTo(isNeighbor));
        }
    }

    public class Game
    {
        private List<Position> _cellPositions;

        public Game()
        {
            _cellPositions = new List<Position>();
        }

        public void SeedWithCells(List<Position> positions)
        {
            _cellPositions.AddRange(positions);
        }

        public GameView ViewBoard()
        {
            var cells = new List<Cell>();
            foreach (var position in _cellPositions)
            {
                cells.Add(new Cell());
            }
            return new GameView(cells);
        }

        public void Cycle()
        {
            _cellPositions.Clear();
        }
    }

    public class UnplayableGame
    {
    }
}
