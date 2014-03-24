using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace GameOfLife
{
    [TestFixture]
    public class FunctionalGameOfLifeTests
    {
        [Test]
        public void CanSpecifySizeOfBoard()
        {
            var board = new GameOfLifeBoard(1, 1);
        }

        [TestCase(0,0)]
        [TestCase(1,0)]
        [TestCase(0,1)]
        [TestCase(-1,-1)]
        [TestCase(0,-1)]
        [TestCase(-1, 0)]
        public void InvalidBoardSizeThrowsExceptionWhenBuilt(int maxX, int maxY)
        {
            Assert.Throws<GameOfLifeBoard.InvalidBoardSizeException>(() => new GameOfLifeBoard(maxX, maxY));
        }

        [Test]
        public void WhenBoardIsNewNoLiveCells()
        {
            var board = new GameOfLifeBoard(1, 1);

            Assert.That(board.IsLiveCell(0,0), Is.False);
        }

        [Test]
        public void AddingACellReturnsANewBoard()
        {
            var board = new GameOfLifeBoard(1, 1);
            var nextState = board.AddCell(0, 0);

            Assert.That(board.IsLiveCell(0, 0), Is.False);
            Assert.That(nextState.IsLiveCell(0, 0), Is.True);
        }

        [Test]
        public void CyclingTheGameReturnsANewBoard()
        {
            var board = new GameOfLifeBoard(1, 1);
            var newBoard = board.Cycle();

            Assert.That(newBoard, Is.Not.SameAs(board));
        }

        [Test]
        public void OnBoardWithSingleCellOnCycleDies()
        {
            var board = new GameOfLifeBoard(1, 1);
            board = board.AddCell(0, 0);
            board = board.Cycle();

            Assert.That(board.IsLiveCell(0, 0), Is.False);
        }

        [Test]
        public void CellWithTwoNeighboursLives()
        {
            var board = new GameOfLifeBoard(2, 2);
            board = board.AddCell(0, 0);
            board = board.AddCell(0, 1);
            board = board.AddCell(1, 0);
            board = board.Cycle();

            Assert.That(board.IsLiveCell(0, 0), Is.True);
        }
    }

    public class GameOfLifeBoard
    {
        private readonly int _maxX;
        private readonly int _maxY;

        private bool[,] _board;

        public GameOfLifeBoard(int maxX, int maxY)
        {
            if (maxX < 1 || maxY < 1)
                throw new InvalidBoardSizeException();
            
            _maxX = maxX;
            _maxY = maxY;

            _board = new bool[_maxX, _maxY];
        }

        private GameOfLifeBoard(int maxX, int maxY, bool[,] board)
        {
            _maxX = maxX;
            _maxY = maxY;
            _board = board;
        }

        public class InvalidBoardSizeException : Exception
        {
        }

        public GameOfLifeBoard AddCell(int x, int y)
        {
            bool[,] newBoard = new bool[_maxX, _maxY];
            for (int x1 = 0; x1 < _maxX; x1 ++)
            {
                for (int y1 = 0; y1 < _maxY; y1++)
                {
                    newBoard[x1, y1] = _board[x1, y1];
                }
            }
            newBoard[x, y] = true;
            return new GameOfLifeBoard(_maxX, _maxY, newBoard);
        }

        public bool IsLiveCell(int x, int y)
        {
            return _board[x,y];
        }

        public GameOfLifeBoard Cycle()
        {
            return new GameOfLifeBoard(_maxY, _maxY);
        }
    }
}
