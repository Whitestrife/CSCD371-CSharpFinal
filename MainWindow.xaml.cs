using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TetrisFinal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DatabaseHandler.initializeDatabase();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var tetris = new TetrisWindow();
            tetris.Show();
            tetris.Topmost = true;
        }

        private void HighScoreButton_Click(object sender, RoutedEventArgs e)
        {
            var highScore = new HighScores();
            highScore.Show();
            highScore.Topmost = true;
        }

        private void MenuTutorial_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Welcome to Tetris.\n\n" +
               "Try to drop the different shaped blocks so they form an entire horizontal line.\n\n" +
               "This will clear them to create more space and also net the player points.\n\n" +
               "Go for High Scores to beat your friends!";
            string caption = "Tutorial";
            MessageBox.Show(messageBoxText, caption);
        }

        private void MenuControls_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Controls: \n\n" +
               "Left Arrow: Move Block Left\n\n" +
               "Right Arrow: Move Block Right\n\n" +
               "Down Arrow: Move Block Down\n\n"+
               "Up Arrow: Move Block All The Way Down\n\n" +
               "Z Key: Rotate Block Left\n\n" +
               "C Key: Rotate Block Right\n\n" +
               "X Key: Swap Current Block and Held Block\n\n" +
               "S Key: To Save State\n\n" +
               "L Key: To Load State\n\n" +
               "Spacebar: Pause Game\n\n";
            string caption = "Controls";
            MessageBox.Show(messageBoxText, caption);
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Author Cole Wolff.\n\n" +
               "This is my final project for CSCD 371 .Net Programming at EWU.";
            string caption = "About";
            MessageBox.Show(messageBoxText, caption);
        }

        private void MenuClose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
