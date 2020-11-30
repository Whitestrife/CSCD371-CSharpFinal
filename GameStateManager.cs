using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;

namespace TetrisFinal
{
    class GameStateManager
    {        
        BlockFactory bf;
        Board board;
        int[,] currentBlockPosition;       
        int[] root;       
        TetrisWindow window;
        Block currentBlock;
        Block nextBlock;
        Block storageBlock;
        BrushConverter converter;
        ImageSourceConverter imageConverter;
               
        public GameStateManager(TetrisWindow window) 
        {
            this.window = window;
        }

        public void initializeGame()
        {            
            window.setScore(0);
            root = new int[2];           
            root[0] = 3;
            root[1] = 0;
            converter = new BrushConverter();
            imageConverter = new ImageSourceConverter();
            board = new Board(window);
            bf = new BlockFactory();
            nextBlock = bf.newBlock();
            storageBlock = null;
            currentBlockPosition = new int[10, 18];
            currentBlock = bf.newBlock();                                            
            setNewBlock(currentBlock.getBlock());
            window.nextImage.Source = (ImageSource)imageConverter.ConvertFromString("pack://application:,,,/images/" + nextBlock.getImage());
        }              

        public void setNextBlock()
        {
            nextBlock = bf.newBlock();
            window.nextImage.Source = (ImageSource)imageConverter.ConvertFromString("pack://application:,,,/images/" + nextBlock.getImage());
        }

        public void setStorageBlock()
        {
            Block tempBlock = currentBlock;
            if (storageBlock == null)
            {
                storageBlock = tempBlock;
                currentBlock = nextBlock;
                setNextBlock();
            }
            else
            {
                currentBlock = storageBlock;
                storageBlock = tempBlock;
            }
            setActiveBlock(currentBlock.getBlock());
        }

        //Brand new block placement at the top
        public void setNewBlock(int[,] block)
        {           
            Array.Clear(currentBlockPosition, 0, currentBlockPosition.Length);
            root[0] = 3;
            root[1] = 0;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (currentBlock.getBlock()[x, y] == 1)
                    {
                        currentBlockPosition[x + 3, y] = 1;
                    }
                }
            }
            if(!checkCollisions(currentBlockPosition))
            {
                setNextBlock();
                board.drawBlockToBoard(currentBlock, currentBlockPosition);
                return;
            }
            board.drawBlockToBoard(currentBlock, currentBlockPosition);
            window.gameComplete(board.currentScore);
            
        }
        //Setting a block that is in motion and not yet placed
        public void setActiveBlock(int[,] block)
        {
            int[,] tempBlockPosition = new int[10, 18];            
            
            for(int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if((root[0] + x) > 9 || (root[0] + x) < 0 || ((root[1] + y) > 17))
                    {
                        return;
                    }
                    tempBlockPosition[root[0] + x, root[1] + y] = block[x, y];
                }
            }
            Array.Clear(currentBlockPosition, 0, currentBlockPosition.Length);
            if(!checkCollisions(tempBlockPosition))
            {
                if(storageBlock != null) 
                {
                    window.heldImage.Source = (ImageSource)imageConverter.ConvertFromString("pack://application:,,,/images/" + storageBlock.getImage());
                }
               
                board.cleanBoard();
                currentBlock.setBlock(block);
                currentBlockPosition = tempBlockPosition;
                board.drawBlockToBoard(currentBlock, currentBlockPosition);
            }           
        }

        public void blockMoveLeft()
        {
            int[,] tempBlockPosition = new int[10, 18];
            
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 18; y++)
                {
                    if (currentBlockPosition[x, y] == 1 && (x == 0 || board.boardGrid[x - 1, y] == 1))
                    {
                        return;
                    }
                    else if (currentBlockPosition[x, y] == 1)
                    {
                        tempBlockPosition[x - 1, y] = 1;
                    }
                }
            }
            if (!checkCollisions(tempBlockPosition))
            {
                root[0] -= 1;
                board.cleanBoard();
                Array.Clear(currentBlockPosition, 0, currentBlockPosition.Length);
                currentBlockPosition = tempBlockPosition;
                board.drawBlockToBoard(currentBlock, currentBlockPosition);
            }
        }

        public void blockMoveRight() 
        {
            int[,] tempBlockPosition = new int[10, 18];
            
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 18; y++)
                {
                    if (currentBlockPosition[x, y] == 1 && (x == 9 || board.boardGrid[x + 1, y] == 1))
                    {
                        return;
                    }
                    else if (currentBlockPosition[x, y] == 1)
                    {
                        tempBlockPosition[x + 1, y] = 1;
                    }
                }
            }
            if (!checkCollisions(tempBlockPosition))
            {
                root[0] += 1;
                board.cleanBoard();
                Array.Clear(currentBlockPosition, 0, currentBlockPosition.Length);
                currentBlockPosition = tempBlockPosition;
                board.drawBlockToBoard(currentBlock, currentBlockPosition);
            }
        }

        public void blockMoveDown() 
        {
            int[,] tempBlockPosition = new int[10, 18];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 18; y++)
                {
                    int yCheck = y - 1;
                    if(yCheck < 0)
                    {
                        yCheck = 0;
                    }
                    if (currentBlockPosition[x, y] == 1 && (y == 17 || board.boardGrid[x, yCheck] == 1))
                    {
                        blockPlaced();
                        return;
                    }
                    else if (currentBlockPosition[x, y] == 1)
                    {
                        tempBlockPosition[x, y + 1] = 1;
                    }
                }
            }
            if (!checkCollisions(tempBlockPosition))
            {
                root[1] += 1;
                board.cleanBoard();
                Array.Clear(currentBlockPosition, 0, currentBlockPosition.Length);
                currentBlockPosition = tempBlockPosition;               
                board.drawBlockToBoard(currentBlock, currentBlockPosition);
                return;
            }
            blockPlaced();
        }

        public void blockMoveAllTheWayDown()
        {            
            String nextColor = nextBlock.getColor();
            while(currentBlock.getColor() != nextColor)
            {
                blockMoveDown();
            }           
        }
        //Ensure you can rotate left
        public void leftRotateCheck() 
        {            
            Block tempBlock = currentBlock;    
            tempBlock.rotateLeft();           
            setActiveBlock(tempBlock.getBlock());                        
        }
        //Ensure you can rotate right
        public void rightRotateCheck() 
        {            
            Block tempBlock = currentBlock;
            tempBlock.rotateRight();            
            setActiveBlock(tempBlock.getBlock());
        }
        
        public Boolean checkCollisions(int[,] blockCheck)
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 18; y++)
                {
                    if (board.boardGrid[x, y] == 1 && blockCheck[x, y] == 1)
                    {                       
                        return true;
                    }
                }
            }
            return false;
        }

        public void blockPlaced() 
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 18; y++)
                {
                    if (currentBlockPosition[x, y] == 1)
                    {
                        board.boardGrid[x, y] = 1;
                    }
                }
            }
            board.drawBlockToBoard(currentBlock, currentBlockPosition);
            board.cleanBoard();
            currentBlock = nextBlock;
            board.checkForLines();
            setNewBlock(currentBlock.getBlock());

        }
            
        public void saveGame() //Doesnt store currBlock or nextBlock or heldBlock
        {
            File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + "/saves/saveFile.txt", String.Empty);
            StreamWriter file = new StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + "/saves/saveFile.txt");
            
            for (int y = 17; y >= 0; y--)
            {
                for (int x = 0; x < 10; x++)
                {
                    file.WriteLine(board.boardGrid[x, y] + "," + board.BlockControls[x,y].Background.ToString());
                }
            }
            file.WriteLine(board.currentScore);
            file.WriteLine(board.totalClearedLines);

            file.Close();
        }

        public void loadGame() 
        {
            Array.Clear(currentBlockPosition, 0, currentBlockPosition.Length);
            Array.Clear(board.boardGrid, 0, board.boardGrid.Length);
            board.cleanBoard();
            initializeGame();
            window.resetLevel();
            window.resetTempo();
            window.TimerToggle();           

            StreamReader file = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "/saves/saveFile.txt");
            
            for (int y = 17; y >= 0; y--)
            {
                for (int x = 0; x < 10; x++)
                {                    
                    String line = file.ReadLine();
                    String[] values = line.Split(",");
                    board.boardGrid[x, y] = Int32.Parse(values[0]);
                    board.BlockControls[x,y].Background = (Brush)converter.ConvertFromString(values[1]);
                }
            }
            board.currentScore = Int32.Parse(file.ReadLine());
            window.setScore(board.currentScore);
            board.totalClearedLines = Int32.Parse(file.ReadLine());
            int numberOfCalls = (int)Math.Floor((decimal)(board.totalClearedLines / 10));
            for(int i = 0; i < numberOfCalls; i++)
            {
                window.setTempo();
                window.setLevel();
            }
            
            file.Close();
            window.TimerToggle();

        }
    }
}
