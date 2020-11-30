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

namespace TetrisFinal
{
    /// <summary>
    /// Interaction logic for HighScores.xaml
    /// </summary>
    public partial class HighScores : Window
    {
        public HighScores()
        {
            InitializeComponent();
            SetupDataGrid();
        }

        private void SetupDataGrid()
        {            
            ArrayList databaseReads = DatabaseHandler.readDatabase();

            if (databaseReads.Count == 0)
            {                
                return;
            }

            HighScoreTable.ItemsSource = databaseReads;
        }
    }
}
