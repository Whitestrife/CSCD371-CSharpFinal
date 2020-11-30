using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace TetrisFinal
{
    class Board
    {
        public int[,] boardGrid;
        TetrisWindow window;
        Brush NoBrush = Brushes.Transparent;
        Brush SilverBrush = Brushes.Gray;              
        BrushConverter converter;
        Brush[,] blockColor;
        public Label[,] BlockControls;
        public int totalClearedLines;
        public int currentScore;

        public Board(TetrisWindow window)
        {
            boardGrid = new int[10,18];
            blockColor = new Brush[10, 18];
            this.window = window;
            totalClearedLines = 0;
            currentScore = 0;
            converter = new BrushConverter();
            initializeBoard();
        }

        public void initializeBoard()
        {
            BlockControls = new Label[10, 18];

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 18; y++)
                {
                    BlockControls[x, y] = new Label();
                    BlockControls[x, y].Background = NoBrush;
                    BlockControls[x, y].BorderBrush = SilverBrush;
                    BlockControls[x, y].BorderThickness = new Thickness(1, 1, 1, 1);
                    Grid.SetRow(BlockControls[x, y], y);
                    Grid.SetColumn(BlockControls[x, y], x);
                    window.MainGrid.Children.Add(BlockControls[x, y]);
                }

            }
        }
       
        public void drawBlockToBoard(Block currentBlock, int[,] currentBlockPosition)
        {
            Brush color = (Brush)converter.ConvertFromString(currentBlock.getColor());


            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 18; y++)
                {
                    if (currentBlockPosition[x, y] == 1)
                    {
                        BlockControls[x, y].Background = color;
                    }
                }
            }
        }

        public void cleanBoard()
        {

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 18; y++)
                {
                    if (boardGrid[x, y] == 0)
                    {
                        BlockControls[x, y].Background = NoBrush;
                    }
                }
            }
        }

        public void checkForLines()
        {
            int count = 0;
            int clearedLines = 0;
            Boolean clearedLine = false;
            for (int y = 17; y >= 0; y--)
            {
                count = 0;
                for (int x = 0; x < 10; x++)
                {
                    if (boardGrid[x, y] == 0)
                    {
                        break;
                    }
                    count++;
                    if (count == 10)
                    {
                        clearedLine = true;
                        clearedLines++;
                        totalClearedLines++;
                        tempoCheck();
                        clearLine(y);
                        getBoardColors(y);
                        shiftBoardDown(y);
                        y++;
                    }
                }
            }

            if (clearedLine == true)
            {
                currentScore += ((100 * clearedLines) * window.getLevel());
                if (clearedLines > 1)
                {
                    currentScore += ((50 * (clearedLines - 1)) * window.getLevel());
                }
                window.setScore(currentScore);
            }
        }

        public void clearLine(int index)
        {
            for (int x = 0; x < 10; x++)
            {
                boardGrid[x, index] = 0;
            }
        }

        public void tempoCheck()
        {
            if (totalClearedLines % 10 == 0)
            {
                window.setTempo();
                window.setLevel();
            }
        }

        public void getBoardColors(int index)
        {
            for (int y = index - 1; y >= 0; y--)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (boardGrid[x, y] == 1)
                    {
                        blockColor[x, y] = BlockControls[x, y].Background;
                    }
                }
            }
        }

        public void shiftBoardDown(int index)
        {
            for (int y = index - 1; y >= 0; y--)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (boardGrid[x, y] == 1)
                    {
                        boardGrid[x, y] = 0;
                        boardGrid[x, y + 1] = 1;
                        BlockControls[x, y].Background = NoBrush;
                        BlockControls[x, y + 1].Background = blockColor[x, y];
                    }
                }
            }
        }
    }
}
