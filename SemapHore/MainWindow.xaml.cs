using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SemapHore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int counter = 0;

        public MainWindow()
        {
            InitializeComponent();


        }
        public List<Thread> threads { get; set; } = new List<Thread>();
        public List<string> threadnames { get; set; } = new List<string>();

        public List<Thread> waitingThreads { get; set; } = new List<Thread>();
        public List<string> waitingThreadNames { get; set; } = new List<string>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread new_thread = new(Counter);
            new_thread.Start();
            new_thread.Name = "Thread->>"+counter.ToString();
            threadnames.Add(new_thread.Name.ToString());
            //MessageBox.Show(new_thread.Name);
            threads.Add(new_thread);
            ListViewCreating.ItemsSource = null;
            ListViewCreating.ItemsSource = threadnames;
        }



        void Counter()
        {
            counter++;
        }





        void SomeMethod(object? state)
        {
            var semaphore = state as Semaphore;

            if (semaphore is null)
                return;


            bool isFinish = false;

            while (!isFinish)
            {
                if (semaphore.WaitOne(2000))
                {
                    try
                    {
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} got the key");
                        Thread.Sleep(6000);
                    }
                    finally
                    {
                        isFinish = true;
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} returned the key");
                        semaphore.Release();
                    }
                }
                else
                {
                    Console.WriteLine($"Mister Thread number {Thread.CurrentThread.ManagedThreadId}, we have not enough keys. Please wait ...");
                }
            }
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ListViewCreating.SelectedItem.ToString());
        }

        private void ListViewCreating_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(ListViewCreating.SelectedItem.ToString());
            waitingThreadNames.Add(ListViewCreating.SelectedItem.ToString());
            threadnames.Remove(ListViewCreating.SelectedItem.ToString());
            ListViewCreating.ItemsSource = null;
            ListViewCreating.ItemsSource = threadnames;
            ListViewWaiting.ItemsSource = null;
            ListViewWaiting.ItemsSource = waitingThreadNames;
        }
    }
}
