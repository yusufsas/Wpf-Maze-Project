using System;
using System.Collections;
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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        private Grid grid;
        private Button generateButtton;
        private Button start;
        private int rowCount = 20; // Toplam satır sayısı
        private int columnCount = 20;
        
        private Random random = new Random();
        private Border cell;
        private Border indicatorCell;
        private HashSet<(int, int)> visitedCells = new HashSet<(int, int)>(); // Ziyaret edilen hücrelerin konumlarını tutar
        private int[,] gridArray = new int[20, 20];
        private int totalPrize;
        private int prizecounter;
        private void PaintRandomCellRed()
        {
            // Rastgele bir satır ve sütun seç
            int randomRow = random.Next(0, rowCount);
            int randomColumn = random.Next(0, columnCount);

            // Hücreyi oluştur ve boyayı ayarla
            cell = new Border();
            cell.Background = Brushes.Red;
            Grid.SetRow(cell, randomRow);
            Grid.SetColumn(cell, randomColumn);

            // Hücreyi gride ekle
            grid.Children.Add(cell);

            // Ziyaret edilen hücrelerin listesine ekle
            visitedCells.Add((randomRow, randomColumn));
        }

       


        private async void StartCellMovement()
        {
            Random random = new Random();
            while (true)
            {
                // Rastgele bir yön seç
                int direction = random.Next(0, 4); // 0: sağ, 1: sol, 2: yukarı, 3: aşağı

                // Hücreyi yeni konumuna taşı
                MoveCell(direction);

                // 0.5 saniye bekle
                await Task.Delay(50);
            }
        }

        private void MoveCell(int direction)
        {
            // Hücrenin mevcut konumunu al
            int? currentRow = Grid.GetRow(cell);
            int? currentColumn = Grid.GetColumn(cell);

            // Mevcut satır ve sütun null değilse hareket et
            if (currentRow != null && currentColumn != null)
            {
                // Geçerli konumun sağ, sol, yukarı ve aşağıdaki hücrelerin durumlarını kontrol et
                bool canGoRight = CanMoveTo(currentRow.Value, currentColumn.Value + 1);
                bool canGoLeft = CanMoveTo(currentRow.Value, currentColumn.Value - 1);
                bool canGoUp = CanMoveTo(currentRow.Value - 1, currentColumn.Value);
                bool canGoDown = CanMoveTo(currentRow.Value + 1, currentColumn.Value);

                // Belirli yönde engel yoksa hareket et
                switch (direction)
                {
                    case 0: // Sağ
                        if (canGoRight && currentColumn < columnCount - 1)
                            Grid.SetColumn(cell, currentColumn.Value + 1);
                        break;
                    case 1: // Sol
                        if (canGoLeft && currentColumn > 0)
                            Grid.SetColumn(cell, currentColumn.Value - 1);
                        break;
                    case 2: // Yukarı
                        if (canGoUp && currentRow > 0)
                            Grid.SetRow(cell, currentRow.Value - 1);
                        break;
                    case 3: // Aşağı
                        if (canGoDown && currentRow < rowCount - 1)
                            Grid.SetRow(cell, currentRow.Value + 1);
                        break;
                }

                // Mevcut hücrenin konumunu ziyaret edildi olarak işaretle
                visitedCells.Add(((int)currentRow, (int)currentColumn));

                // Ziyaret edilen hücrelerin dizideki karşılığını 5 yap
                foreach (var visitedCell in visitedCells)
                {
                    int rowIndex = visitedCell.Item1;
                    int columnIndex = visitedCell.Item2;

                    // Eğer bu hücre engel değilse (değeri 0 veya 2 ise)
                    if (gridArray[rowIndex, columnIndex] == 0)
                    {
                        // Hücrenin dizideki değerini 5 yap
                        gridArray[rowIndex, columnIndex] = 5;
                    }
                    else if(gridArray[rowIndex, columnIndex] == 2)
                    {
                        gridArray[rowIndex, columnIndex] = 10;
                        prizecounter += 1;
                    }
                }

                // Grid'i güncelle
                UpdateGrid();
              
                   
            }
        }

        private bool CanMoveTo(int row, int column)
        {
            // Geçerli konumun dizide olup olmadığını ve engel olup olmadığını kontrol et
            return row >= 0 && row < rowCount && column >= 0 && column < columnCount && gridArray[row, column] != 1;
        }

        private void UpdateGrid()
        {
            // Ziyaret edilen hücreleri kırmızıya boyamak için grid'i güncelle
            foreach (var visitedCell in visitedCells)
            {
                int rowIndex = visitedCell.Item1;
                int columnIndex = visitedCell.Item2;

                // Dizideki değeri kontrol et
                if (gridArray[rowIndex, columnIndex] == 5 || gridArray[rowIndex, columnIndex] == 10)
                {
                    // Hücreyi kırmızıya boyamak için border oluştur
                    Border visitedCellBorder = new Border();
                    visitedCellBorder.Background = Brushes.Red;
                    Grid.SetRow(visitedCellBorder, rowIndex);
                    Grid.SetColumn(visitedCellBorder, columnIndex);

                    // Hücreyi gride ekle (eğer daha önce eklenmemişse)
                    if (!grid.Children.Contains(visitedCellBorder))
                        grid.Children.Add(visitedCellBorder);
                }
            }
        }

        //public MainWindow()
        //{
        //    InitializeComponent();
        //    InitializeGrid();
        //    InitializeButtons();
        //}
        private void InitializeGrid()
        {
            // Grid oluştur
            grid = new Grid();
            grid.ShowGridLines = true;
            
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    gridArray[i, j] = 0;
                }
            }

            // Arka plan rengini lineer gradient olarak sağdan sola ayarla
            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = new Point(1, 0.5);
            gradientBrush.EndPoint = new Point(0, 0.5);
            gradientBrush.GradientStops.Add(new GradientStop(Colors.LightBlue, 0.0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.LightGreen, 1.0));
            grid.Background = gradientBrush;

            // Satır ve sütun tanımlarını ekle
            for (int i = 0; i < rowCount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < columnCount; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Grid'i window içerisine ekle
            this.Content = grid;


            Random random = new Random();
            int randomSayi = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Wall[] walls = new Wall[randomSayi];
            for (int i = 0; i < randomSayi; i++)
            {
                int xpos = random.Next(0, 19);
                int ypos = random.Next(0, 19);
                
                if (gridArray[xpos, ypos] == 0)
                {
                    walls[i] = new Wall("duvar", 2, new Location(xpos, ypos), "C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\firewall.png");


                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(walls[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = walls[i].size * 50;
                    imageControl3.Width = walls[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, walls[i].location.X*20/ columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, walls[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, walls[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, walls[i].size);
                    grid.Children.Add(imageControl3);

                    gridArray[xpos, ypos] = 1;
                    gridArray[xpos + 1, ypos] = 1;
                    gridArray[xpos, ypos + 1] = 1;
                    gridArray[xpos + 1, ypos + 1] = 1;


                }
                else
                {
                    i--;
                }

            }

            int randomSayi1 = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Mountain[] mountains = new Mountain[randomSayi1];
            for (int i = 0; i < randomSayi1; i++)
            {
                int size = random.Next(1, 3);
                int xpos = random.Next(0, 19);
                int ypos = random.Next(0, 19);
                if (gridArray[xpos, ypos] == 0)
                {
                    mountains[i] = new Mountain("dağ", size, new Location(xpos, ypos), "C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\mountain.png");


                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(mountains[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = mountains[i].size * 50;
                    imageControl3.Width = mountains[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, mountains[i].location.X*20/ columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, mountains[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, mountains[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, mountains[i].size);
                    grid.Children.Add(imageControl3);
                    if (size == 2) {
                        gridArray[xpos, ypos] = 1;
                        gridArray[xpos+1, ypos] = 1;
                        gridArray[xpos, ypos+1] = 1;
                        gridArray[xpos+1, ypos+1] = 1;

                    }
                    else {
                        gridArray[xpos, ypos] = 1;
                    }
                    
                }
                else
                {
                    i--;
                }

            }

            int randomSayi2 = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Mountain[] mountainsS = new Mountain[randomSayi2];
            for (int i = 0; i < randomSayi2; i++)
            {
                int size = random.Next(1, 3);
                int xpos = random.Next(0, 19);
                int ypos = random.Next(0, 19);
                if (gridArray[xpos, ypos] == 0)
                {
                    mountainsS[i] = new Mountain("dağ", size, new Location(xpos, ypos), "C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\mountainSnowy.png");


                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(mountainsS[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = mountainsS[i].size * 50;
                    imageControl3.Width = mountainsS[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, mountainsS[i].location.X*20 / columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, mountainsS[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, mountainsS[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, mountainsS[i].size);
                    grid.Children.Add(imageControl3);

                    if (size == 2)
                    {
                        gridArray[xpos, ypos] = 1;
                        gridArray[xpos + 1, ypos] = 1;
                        gridArray[xpos, ypos + 1] = 1;
                        gridArray[xpos + 1, ypos + 1] = 1;

                    }
                    else
                    {
                        gridArray[xpos, ypos] = 1;
                    }
                }
                else
                {
                    i--;
                }


            }

            int randomSayi3 = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Rock[] stones = new Rock[randomSayi3];
            for (int i = 0; i < randomSayi3; i++)
            {
                int xpos = random.Next(0, 19);
                int ypos = random.Next(0, 19);
                int size = random.Next(1, 3);
                if (gridArray[xpos, ypos] == 0)
                {
                    stones[i] = new Rock("kaya", size, new Location(xpos, ypos), "C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\stone.png");


                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(stones[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = stones[i].size * 50;
                    imageControl3.Width = stones[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, stones[i].location.X*20 / columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, stones[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, stones[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, stones[i].size);
                    grid.Children.Add(imageControl3);

                    if (size == 2)
                    {
                        gridArray[xpos, ypos] = 1;
                        gridArray[xpos + 1, ypos] = 1;
                        gridArray[xpos, ypos + 1] = 1;
                        gridArray[xpos + 1, ypos + 1] = 1;

                    }
                    else
                    {
                        gridArray[xpos, ypos] = 1;
                    }
                }
                else
                {
                    i--;
                }


            }
            int randomSayi4 = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Tree[] trees = new Tree[randomSayi4];
            for (int i = 0; i < randomSayi4; i++)
            {
                int xpos = random.Next(0, 19);
                int ypos = random.Next(0, 19);
                int size = random.Next(1, 3);
                if (gridArray[xpos, ypos] == 0)
                {
                    trees[i] = new Tree("agac", size, new Location(xpos, ypos), "C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\tree.png");


                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(trees[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = trees[i].size * 50;
                    imageControl3.Width = trees[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, trees[i].location.X*20 / columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, trees[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, trees[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, trees[i].size);
                    grid.Children.Add(imageControl3);

                    if (size == 2)
                    {
                        gridArray[xpos, ypos] = 1;
                        gridArray[xpos + 1, ypos] = 1;
                        gridArray[xpos, ypos + 1] = 1;
                        gridArray[xpos + 1, ypos + 1] = 1;

                    }
                    else
                    {
                        gridArray[xpos, ypos] = 1;
                    }
                }
                else
                {
                    i--;
                }


            }
            int randomSayi5 = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Tree[] treesS = new Tree[randomSayi5];
            for (int i = 0; i < randomSayi5; i++)
            {
                int xpos = random.Next(0, 19);
                int ypos = random.Next(0, 19);
                int size = random.Next(1, 3);
                if (gridArray[xpos, ypos] == 0)
                {
                    treesS[i] = new Tree("agac", size, new Location(xpos, ypos), "C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\treeSnowy.png");


                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(treesS[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = treesS[i].size * 50;
                    imageControl3.Width = treesS[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, treesS[i].location.X*20 / columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, treesS[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, treesS[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, treesS[i].size);
                    grid.Children.Add(imageControl3);

                    if (size == 2)
                    {
                        gridArray[xpos, ypos] = 1;
                        gridArray[xpos + 1, ypos] = 1;
                        gridArray[xpos, ypos + 1] = 1;
                        gridArray[xpos + 1, ypos + 1] = 1;

                    }
                    else
                    {
                        gridArray[xpos, ypos] = 1;
                    }
                }
                else
                {
                    i--;
                }

            }

            int randomSayi6 = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Bee[] bees = new Bee[randomSayi6];
            for (int i = 0; i < randomSayi6; i++)
            {
                int xpos = random.Next(0, 20);
                int ypos = random.Next(0, 20);
              
                if (gridArray[xpos, ypos] == 0)
                {
                    bees[i] = new Bee("arı", 1, new Location(xpos, ypos), "C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\bee.png");


                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(bees[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = bees[i].size * 50;
                    imageControl3.Width = bees[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, bees[i].location.X*20 / columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, bees[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, bees[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, bees[i].size);
                    grid.Children.Add(imageControl3);

                    gridArray[xpos, ypos] = 1;
                }
                else
                {
                    i--;
                }

            }

            int randomSayi7 = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Bird[] birds = new Bird[randomSayi7];
            for (int i = 0; i < randomSayi7; i++)
            {
                int xpos = random.Next(0, 20);
                int ypos = random.Next(0, 20);

                if (gridArray[xpos, ypos] == 0)
                {
                    birds[i] = new Bird("kus", 1, new Location(xpos, ypos), "C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\bird.png");


                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(birds[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = birds[i].size * 50;
                    imageControl3.Width = birds[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, birds[i].location.X*20 / columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, birds[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, birds[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, birds[i].size);
                    grid.Children.Add(imageControl3);

                    gridArray[xpos, ypos] = 1;
                }
                else
                {
                    i--;
                }

            }
            int randomSayi8 = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Prize[] coppers = new Prize[randomSayi8];
            for (int i = 0; i < randomSayi8; i++)
            {
                int xpos = random.Next(0, 20);
                int ypos = random.Next(0, 20);

                if (gridArray[xpos, ypos] == 0)
                {
                    coppers[i] = new Prize("C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\copper.png", "bakır", 1, new Location(xpos, ypos));



                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(coppers[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = coppers[i].size * 50;
                    imageControl3.Width = coppers[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, coppers[i].location.X*20 / columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, coppers[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, coppers[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, coppers[i].size);
                    grid.Children.Add(imageControl3);

                    gridArray[xpos, ypos] = 2;
                }
                else
                {
                    i--;
                }


            }
            int randomSayi9 = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Prize[] emeralds = new Prize[randomSayi9];
            for (int i = 0; i < randomSayi9; i++)
            {
                int xpos = random.Next(0, 20);
                int ypos = random.Next(0, 20);

                if (gridArray[xpos, ypos] == 0)
                {
                    emeralds[i] = new Prize("C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\emerald.png", "zümrüt", 1, new Location(xpos, ypos));



                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(emeralds[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = emeralds[i].size * 50;
                    imageControl3.Width = emeralds[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, emeralds[i].location.X*20 / columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, emeralds[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, emeralds[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, emeralds[i].size);
                    grid.Children.Add(imageControl3);
                    gridArray[xpos, ypos] = 2;
                }
                else
                {
                    i--;
                }

            }
            int randomSayi10 = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Prize[] golds = new Prize[randomSayi10];
            for (int i = 0; i < randomSayi10; i++)
            {
                int xpos = random.Next(0, 20);
                int ypos = random.Next(0, 20);

                if (gridArray[xpos, ypos] == 0)
                {
                    golds[i] = new Prize("C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\golds.png", "altın", 1, new Location(xpos, ypos));



                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(golds[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = golds[i].size * 50;
                    imageControl3.Width = golds[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, golds[i].location.X*20 / columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, golds[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, golds[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, golds[i].size);
                    grid.Children.Add(imageControl3);

                    gridArray[xpos, ypos] = 2;
                }
                else
                {
                    i--;
                }

            }
            int randomSayi11 = random.Next(2, 4); // 3 ile 5 arasında rastgele bir sayı üretir (3 dahil, 6 hariç)
            Prize[] silvers = new Prize[randomSayi11];
            for (int i = 0; i < randomSayi11; i++)
            {
                int xpos = random.Next(0, 20);
                int ypos = random.Next(0, 20);

                if (gridArray[xpos, ypos] == 0)
                {
                    silvers[i] = new Prize("C:\\Users\\yusuf\\Desktop\\c++\\WpfApp2\\WpfApp2\\images\\silver.png", "gümüş", 1, new Location(xpos, ypos));



                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(silvers[i].filepath, UriKind.RelativeOrAbsolute);
                    bitmap3.EndInit();

                    // Resmi göstermek için bir Image nesnesi oluşturun
                    Image imageControl3 = new Image();
                    imageControl3.Source = bitmap3;
                    imageControl3.Stretch = System.Windows.Media.Stretch.Uniform;
                    imageControl3.Height = silvers[i].size * 50;
                    imageControl3.Width = silvers[i].size * 50;
                    // Resmi Grid'e ekleyin
                    Grid.SetRow(imageControl3, silvers[i].location.X*20 / columnCount); // Satırı hesaplayın
                    Grid.SetColumn(imageControl3, silvers[i].location.Y*20 / columnCount);
                    // Sütunu hesaplayın
                    Grid.SetRowSpan(imageControl3, silvers[i].size); // Görüntüyü 4 satır boyunca genişletin
                    Grid.SetColumnSpan(imageControl3, silvers[i].size);
                    grid.Children.Add(imageControl3);

                    gridArray[xpos, ypos] = 2;
                }
                else
                {
                    i--;
                }

            }

            totalPrize = randomSayi11 + randomSayi10 + randomSayi9 + randomSayi8;
            prizecounter = 0;
            PaintRandomCellRed();
        }


        private void InitializeButtons()
        {
            generateButtton = new Button();
            generateButtton.Content = "Generate Map";
            generateButtton.HorizontalAlignment = HorizontalAlignment.Center;
            generateButtton.VerticalAlignment = VerticalAlignment.Center;
            generateButtton.Click += Button_Click;
            Grid.SetRow(generateButtton, 0); // Satır indeksi 1 (sıfırdan başlar)
            Grid.SetColumn(generateButtton, 0);
            grid.Children.Add(generateButtton);

            start = new Button();
            start.Content = "Start";
            start.HorizontalAlignment = HorizontalAlignment.Center;
            start.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(start, 0); // Satır indeksi 1 (sıfırdan başlar)
            Grid.SetColumn(start, 1);
            grid.Children.Add(start);


        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Mevcut öğeleri temizle
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

     
            InitializeGrid();

            // Diğer öğeleri yeniden ekle
            grid.Children.Add(generateButtton);
            grid.Children.Add(start);
        }


      


        public MainWindow()
        {
            InitializeComponent();
            InitializeGrid();
            InitializeButtons();
            //PaintRandomCellRed();
            //PaintIndicatorCellGreen();
            StartCellMovement();








        }
    }
}
