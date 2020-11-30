using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TetrisFinal
{
    class BlockFactory
    {
        Random rand;
        ArrayList recentBlocks;
        public BlockFactory()
        {
            rand = new Random();
            recentBlocks = new ArrayList();
        }

        public Block newBlock()
        {
            int result = rand.Next(0, 7);
            while(recentBlocks.Contains(result)) //Ensures no duplicate blocks and better variety of block spawn
            {
                result = rand.Next(0, 7);
            }

            recentBlocks.Add(result);            

            if(recentBlocks.Count == 7)
            {
                recentBlocks.Clear();
            }

            switch(result)
            {
                case 0: 
                    return new LBlock1();
                case 1:
                    return new LBlock2();
                case 2:
                    return new TBlock();
                case 3:
                    return new SBlock2();
                case 4:
                    return new SBlock1();
                case 5:
                    return new SquareBlock();
                case 6:
                    return new LongBlock();
            }
            return new LBlock1();              
        }

    }

    abstract class Block
    {
        public String color;
        public String image;
        public int[,] block;
        public void rotateLeft()
        {
            int[,] tempArray = new int[4, 4];
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (block[x, y] == 1)
                    {
                        tempArray[y, 3 - x] = 1;
                    }
                }
            }
            setBlock(tempArray);
        }
        public void rotateRight()
        {
            int[,] tempArray = new int[4, 4];
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (block[x, y] == 1)
                    {
                        tempArray[3 - y, x] = 1;
                    }
                }
            }
            setBlock(tempArray);            
        }

        public void setBlock(int[,] newBlock)
        {
            this.block = newBlock;
        }
        public int[,] getBlock()
        {
            return block;
        }
        public String getColor()
        {
            return color;
        }

        public String getImage()
        {
            return image;
        }
        
    }

    class LBlock1 : Block
    {

        new int[,] block;
        new String color = "#FF0000FF";
        new String image = "LBlock1.png";
        public LBlock1()
        {
            block = new int[4, 4];
            block[1, 1] = 1;
            block[1, 2] = 1;
            block[1, 3] = 1;
            block[2, 3] = 1;
            base.block = block;
            base.color = color;
            base.image = image;
            
        }
    }

    class LBlock2 : Block
    {
        new int[,] block;
        new String color = "#FFFFA500";
        new String image = "LBlock2.png";
        public LBlock2()
        {
            block = new int[4, 4];
            block[2, 1] = 1;
            block[2, 2] = 1;
            block[2, 3] = 1;
            block[1, 3] = 1;
            base.block = block;
            base.color = color;
            base.image = image;
        }
    }


    class TBlock : Block
    {
        new int[,] block;
        new String color = "#FF800080";
        new String image = "TBlock.png";
        public TBlock()
        {
            block = new int[4, 4];
            block[0, 1] = 1;
            block[1, 1] = 1;
            block[1, 2] = 1;
            block[2, 1] = 1;
            base.block = block;
            base.color = color;
            base.image = image;
        }
             
    }

    class SBlock1 : Block
    {
        new int[,] block;
        new String color = "#FFADFF2F";
        new String image = "SBlock1.png";
        public SBlock1()
        {
            block = new int[4, 4];
            block[0, 2] = 1;
            block[1, 2] = 1;
            block[1, 1] = 1;
            block[2, 1] = 1;
            base.block = block;
            base.color = color;
            base.image = image;

        }        
    }

    class SBlock2 : Block
    {
        new int[,] block;
        new String color = "#FFFF0000";
        new String image = "SBlock2.png";
        public SBlock2()
        {
            block = new int[4, 4];
            block[0, 1] = 1;
            block[1, 2] = 1;
            block[1, 1] = 1;
            block[2, 2] = 1;
            base.block = block;
            base.color = color;
            base.image = image;

        }       
    }

    class SquareBlock : Block
    {
        new int[,] block;
        new String color = "#FFFFFF00";
        new String image = "SquareBlock.png";
        public SquareBlock()
        {
            block = new int[4, 4];
            block[1, 1] = 1;
            block[1, 2] = 1;
            block[2, 1] = 1;
            block[2, 2] = 1;
            base.block = block;
            base.color = color;
            base.image = image;

        }      
    }

    class LongBlock : Block
    {
        new int[,] block;
        new String color = "#FF008080";
        new String image = "IBlock.png";

        public LongBlock()
        {
            block = new int[4, 4];
            block[1, 0] = 1;
            block[1, 1] = 1;
            block[1, 2] = 1;
            block[1, 3] = 1;
            base.block = block;
            base.color = color;
            base.image = image;
        }
        
    }
}

