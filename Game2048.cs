using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Game2048
{
    public partial class Game2048Form : Form
    {
        private const int GRID_SIZE = 4;
        private const int TILE_SIZE = 100;
        private const int TILE_MARGIN = 5;
        private int[,] grid;
        private Label[,] tileLabels;
        private Random random;
        private int score;
        private Label scoreLabel;

        private readonly Color[] tileColors = {
            Color.FromArgb(238, 228, 218), // 2
            Color.FromArgb(237, 224, 200), // 4
            Color.FromArgb(242, 177, 121), // 8
            Color.FromArgb(245, 149, 99),  // 16
            Color.FromArgb(246, 124, 95),  // 32
            Color.FromArgb(246, 94, 59),   // 64
            Color.FromArgb(237, 207, 114), // 128
            Color.FromArgb(237, 204, 97),  // 256
            Color.FromArgb(237, 200, 80),  // 512
            Color.FromArgb(237, 197, 63),  // 1024
            Color.FromArgb(237, 194, 46)   // 2048
        };

        public Game2048Form()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            this.Size = new Size(GRID_SIZE * (TILE_SIZE + TILE_MARGIN) + TILE_MARGIN + 16,
                               GRID_SIZE * (TILE_SIZE + TILE_MARGIN) + TILE_MARGIN + 100);
            this.Text = "2048 Game";
            this.BackColor = Color.FromArgb(187, 173, 160);
            this.KeyPreview = true;
            //set window property to be unsizeble
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            
            
                grid = new int[GRID_SIZE, GRID_SIZE];
            tileLabels = new Label[GRID_SIZE, GRID_SIZE];
            random = new Random();
            score = 0;

            // Create score label
            scoreLabel = new Label
            {
                Text = "Score: 0",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(TILE_MARGIN, 10),
                AutoSize = true
            };
            this.Controls.Add(scoreLabel);

            // Create tile grid
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    var label = new Label
                    {
                        Size = new Size(TILE_SIZE, TILE_SIZE),
                        Location = new Point(j * (TILE_SIZE + TILE_MARGIN) + TILE_MARGIN,
                                          i * (TILE_SIZE + TILE_MARGIN) + TILE_MARGIN + 50),
                        BackColor = Color.FromArgb(205, 193, 180),
                        Font = new Font("Arial", 24, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    tileLabels[i, j] = label;
                    this.Controls.Add(label);
                }
            }

            this.KeyDown += Game2048Form_KeyDown;
            AddNewTile();
            AddNewTile();
            UpdateDisplay();
        }

        private void Game2048Form_KeyDown(object sender, KeyEventArgs e)
        {
            bool moved = false;
            switch (e.KeyCode)
            {
                case Keys.Left:
                    moved = MoveLeft();
                    break;
                case Keys.Right:
                    moved = MoveRight();
                    break;
                case Keys.Up:
                    moved = MoveUp();
                    break;
                case Keys.Down:
                    moved = MoveDown();
                    break;
            }

            if (moved)
            {
                AddNewTile();
                UpdateDisplay();
                if (IsGameOver())
                {
                    MessageBox.Show($"Game Over! Final Score: {score}", "Game Over");
                    InitializeGame();
                }
            }
        }

        private bool MoveLeft()
        {
            bool moved = false;
            for (int i = 0; i < GRID_SIZE; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (grid[i, j] != 0)
                        row.Add(grid[i, j]);
                }

                for (int j = 0; j < row.Count - 1; j++)
                {
                    if (row[j] == row[j + 1])
                    {
                        row[j] *= 2;
                        score += row[j];
                        row.RemoveAt(j + 1);
                        moved = true;
                    }
                }

                while (row.Count < GRID_SIZE)
                    row.Add(0);

                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (grid[i, j] != row[j])
                        moved = true;
                    grid[i, j] = row[j];
                }
            }
            return moved;
        }

        private bool MoveRight()
        {
            bool moved = false;
            for (int i = 0; i < GRID_SIZE; i++)
            {
                List<int> row = new List<int>();
                for (int j = GRID_SIZE - 1; j >= 0; j--)
                {
                    if (grid[i, j] != 0)
                        row.Add(grid[i, j]);
                }

                for (int j = 0; j < row.Count - 1; j++)
                {
                    if (row[j] == row[j + 1])
                    {
                        row[j] *= 2;
                        score += row[j];
                        row.RemoveAt(j + 1);
                        moved = true;
                    }
                }

                while (row.Count < GRID_SIZE)
                    row.Add(0);

                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (grid[i, GRID_SIZE - 1 - j] != row[j])
                        moved = true;
                    grid[i, GRID_SIZE - 1 - j] = row[j];
                }
            }
            return moved;
        }

        private bool MoveUp()
        {
            bool moved = false;
            for (int j = 0; j < GRID_SIZE; j++)
            {
                List<int> column = new List<int>();
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    if (grid[i, j] != 0)
                        column.Add(grid[i, j]);
                }

                for (int i = 0; i < column.Count - 1; i++)
                {
                    if (column[i] == column[i + 1])
                    {
                        column[i] *= 2;
                        score += column[i];
                        column.RemoveAt(i + 1);
                        moved = true;
                    }
                }

                while (column.Count < GRID_SIZE)
                    column.Add(0);

                for (int i = 0; i < GRID_SIZE; i++)
                {
                    if (grid[i, j] != column[i])
                        moved = true;
                    grid[i, j] = column[i];
                }
            }
            return moved;
        }

        private bool MoveDown()
        {
            bool moved = false;
            for (int j = 0; j < GRID_SIZE; j++)
            {
                List<int> column = new List<int>();
                for (int i = GRID_SIZE - 1; i >= 0; i--)
                {
                    if (grid[i, j] != 0)
                        column.Add(grid[i, j]);
                }

                for (int i = 0; i < column.Count - 1; i++)
                {
                    if (column[i] == column[i + 1])
                    {
                        column[i] *= 2;
                        score += column[i];
                        column.RemoveAt(i + 1);
                        moved = true;
                    }
                }

                while (column.Count < GRID_SIZE)
                    column.Add(0);

                for (int i = 0; i < GRID_SIZE; i++)
                {
                    if (grid[GRID_SIZE - 1 - i, j] != column[i])
                        moved = true;
                    grid[GRID_SIZE - 1 - i, j] = column[i];
                }
            }
            return moved;
        }

        private void AddNewTile()
        {
            List<(int, int)> emptyCells = new List<(int, int)>();
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    if (grid[i, j] == 0)
                        emptyCells.Add((i, j));
                }
            }

            if (emptyCells.Count > 0)
            {
                var cell = emptyCells[random.Next(emptyCells.Count)];
                grid[cell.Item1, cell.Item2] = random.Next(10) == 0 ? 4 : 2;
            }
        }

        private void UpdateDisplay()
        {
            scoreLabel.Text = $"Score: {score}";
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    var label = tileLabels[i, j];
                    var value = grid[i, j];
                    label.Text = value == 0 ? "" : value.ToString();
                    label.BackColor = value == 0 ? Color.FromArgb(205, 193, 180) : 
                                    GetTileColor(value);
                    label.ForeColor = value <= 4 ? Color.FromArgb(119, 110, 101) : Color.White;
                }
            }
        }

        private Color GetTileColor(int value)
        {
            int power = (int)Math.Log(value, 2) - 1;
            return power < tileColors.Length ? tileColors[power] : tileColors.Last();
        }

        private bool IsGameOver()
        {
            // Check for empty cells
            for (int i = 0; i < GRID_SIZE; i++)
                for (int j = 0; j < GRID_SIZE; j++)
                    if (grid[i, j] == 0)
                        return false;

            // Check for possible merges
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE - 1; j++)
                {
                    if (grid[i, j] == grid[i, j + 1] || grid[j, i] == grid[j + 1, i])
                        return false;
                }
            }

            return true;
        }
    }
}
