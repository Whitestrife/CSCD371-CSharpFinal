using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TetrisFinal
{
    /// <summary>
    /// Interaction logic for TetrisWindow.xaml
    /// </summary>
    public partial class TetrisWindow : Window
    {

        DispatcherTimer timer;
        GameStateManager gsm;
        int currentScore;
        int level;
        double timerSpeed;        
        Boolean gameOver = false;
        public TetrisWindow()
        {
            InitializeComponent();
            SubmitUserName.IsEnabled = false;
            SubmitUserName.Visibility = Visibility.Hidden;
            UserName.IsEnabled = false;
            UserName.Visibility = Visibility.Hidden;
            currentScore = 0;
            level = 1;
            scoreBox.Text = currentScore.ToString();
            levelBox.Text = level.ToString();
            this.DataContext = this;
            gsm = new GameStateManager(this);
            gsm.initializeGame();
            timerSpeed = 500;
            SetupTimer();
        }

        public void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(timerSpeed);
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        //For pausing and continuing purposes, also used for taking input, etc
        public void TimerToggle()
        {
            if (!gameOver)
            {
                if (timer.IsEnabled)
                {
                    timer.Stop();
                }
                else
                {
                    timer.Start();
                }
            }
        }

        //Updates at the specified TimeSpan rate
        void Timer_Tick(object sender, EventArgs e)
        {
            gsm.blockMoveDown();                       
        }

        public void gameComplete(int currentScore)
        {
            timer.Stop();
            SubmitUserName.IsEnabled = true;
            SubmitUserName.Visibility = Visibility.Visible;
            UserName.IsEnabled = true;
            UserName.Visibility = Visibility.Visible;
            
        }

        public void saveGame()
        {

        }

        private void updateScoresToDatabase(String Username)
        {
            ArrayList dbReads = new ArrayList();
            DatabaseRead lastPlace;
            dbReads = DatabaseHandler.readDatabase();
            if (dbReads.Count != 0) //If HighScores are empty
            {
                dbReads.Sort();
                if (dbReads.Count == 10)
                {
                    lastPlace = (DatabaseRead)dbReads[9];
                    //Get a Name to add to the database
                    if (currentScore > lastPlace.Score) //If new score is high score-TODO
                    {
                        dbReads.RemoveAt(9);
                        dbReads.Add(new DatabaseRead { Name = Username, Score = currentScore });
                        dbReads.Sort();
                        DatabaseHandler.clearDBTable("scores");
                        DatabaseHandler.initializeDatabase();
                        for (int i = 0; i < 10; i++)
                        {
                            DatabaseRead read = (DatabaseRead)dbReads[i];
                            DatabaseHandler.preWriteStorage(read.Name, read.Score);
                        }
                    }
                }
                else if (dbReads.Count < 10) //If there are less than 10 reads it is a guaranteed top 10 high score, currently dont have scores sorted
                {
                    DatabaseHandler.preWriteStorage(Username, currentScore);
                }

            }
            if (dbReads.Count == 0) //Enter the highscore if there are no inputs
            {
                DatabaseHandler.preWriteStorage(Username, currentScore);
            }

            DatabaseHandler.writeToDatabase();
            dbReads.Clear();
        }

        public void setScore(int score)
        {
            this.currentScore = score;
            scoreBox.Text = currentScore.ToString();
        }

        public void setTempo()
        {
            timerSpeed *= .75;
            timer.Interval = TimeSpan.FromMilliseconds(timerSpeed);
        }

        public void resetTempo()
        {
            timerSpeed = 500;
            timer.Interval = TimeSpan.FromMilliseconds(timerSpeed);
        }

        public void setLevel()
        {
            level++;
            levelBox.Text = level.ToString();
        }

        public void resetLevel()
        {
            level = 0;
            levelBox.Text = level.ToString();
        }

        public int getLevel()
        {
            return level;
        }


        private void MainWindow_OnKeyDown(object sender, KeyboardEventArgs e)
        {                    
            
            if (Keyboard.IsKeyDown(Key.Down) && timer.IsEnabled) 
            {
                gsm.blockMoveDown();
            }
            if (Keyboard.IsKeyDown(Key.Left) && timer.IsEnabled) 
            {
                gsm.blockMoveLeft();
            }
            if (Keyboard.IsKeyDown(Key.Right) && timer.IsEnabled)
            {
                gsm.blockMoveRight();
            }
            if (Keyboard.IsKeyDown(Key.Up) && timer.IsEnabled)
            {
                gsm.blockMoveAllTheWayDown();
            }
            if (Keyboard.IsKeyDown(Key.Z) && timer.IsEnabled) 
            {
                gsm.leftRotateCheck();
            }
            if (Keyboard.IsKeyDown(Key.C) && timer.IsEnabled)
            {
                gsm.rightRotateCheck();
            }
            if (Keyboard.IsKeyDown(Key.X) && timer.IsEnabled)
            {
                gsm.setStorageBlock();
            }
            if (Keyboard.IsKeyDown(Key.Home))
            {
                setLevel();
            }
            if (Keyboard.IsKeyDown(Key.S))
            {
                gsm.saveGame();
            }
            if (Keyboard.IsKeyDown(Key.L))
            {
                gsm.loadGame();
            }

            //Continue or Pause
            if (Keyboard.IsKeyDown(Key.Space)) //Spacebar
            {
                TimerToggle();
            }

        }

        private void SubmitUserName_Click(object sender, RoutedEventArgs e)
        {

            String userName = UserName.Text;
            SubmitUserName.IsEnabled = false;
            SubmitUserName.Visibility = Visibility.Hidden;
            UserName.IsEnabled = false;
            UserName.Visibility = Visibility.Hidden;
            updateScoresToDatabase(userName);
        }
    }
}
