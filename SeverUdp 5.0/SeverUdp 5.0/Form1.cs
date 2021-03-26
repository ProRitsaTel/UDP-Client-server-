using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeverUdp_5._0
{
    public partial class UDP_Server : Form
    {
        static Socket listeningSocket; // создание переменной для соккета 
        static IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0); //сетевая конечная точка (IPEndPoint Представляет конечную точку сети в виде IP-адреса и номера порта.)
        static EndPoint Remote = (EndPoint)(sender);//определяет сетевой адрес конечной точки         
        public IPAddress ipa = null;
        ThreadServer[] threadServer = new ThreadServer[1000];//массив экзмпляров класса
        public string IP = "";

        public UDP_Server()
        {
            InitializeComponent();
            
        }
        private void UDP_Server_Load(object sender, EventArgs e)
        {
            startServer();
        }
        public void Server()// создание сервера 
        {
            try
            {
                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); // Создание сокета   
                listeningSocket.Bind(new IPEndPoint(ipa, 23000)); // задаём порт и ip нашего сервера                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void searchIP()
        {
            try
            {                
                ipa = findMyIPV4Address();
                
                if (ipa != null)
                {
                    IP = ipa.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void startServer()
        {
            try
            {
                searchIP();
                Server();// инициализация сервера
                         //textOutput.Invoke(new Action(() => textOutput.Text = "Server is running"));
                Task listeningTask = new Task(Listen);//создание потока для постоянного прослушивания входящих сообщений
                listeningTask.Start(); // Запуск потока

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public  void Listen() // функция прослушивания входных данных приходящих на сервер
        {
            try
            {             
                while (true)
                {              
                    int bytes = 0;
                    byte[] data = new byte[1024 * 8];
                    byte[] data1 = new byte[1024 * 8];//размер массива байтов                      
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
                                     
                    do
                    {
                        bytes = listeningSocket.ReceiveFrom(data, ref Remote);//получаем наши данные, записываем их в data,а в bytes записываем их количество
                        
                    }
                    while (listeningSocket.Available > 0);

                     IPEndPoint remoteFullIp = Remote as IPEndPoint;
                    short chislo = BitConverter.ToInt16(data, 0);//переменная,которая обозначает код проекта в заголовке                
                    short command = BitConverter.ToInt16(data, 2);//переменная,которая обозначает номер команды в заголовке
                    short numberOfCar = BitConverter.ToInt16(data, 4);//переменная,которая обозначает номер машины в заголовке
                    short nenushnayaInfa = BitConverter.ToInt16(data, 6);//переменная,которая обозначает резерв в заголовке
                    string path = @"E:\Belaz\";//путь для сохранения файлов

                    Directory.CreateDirectory(path + numberOfCar);//создание директории
                    Directory.CreateDirectory(path + numberOfCar + "\\K1");
                    Directory.CreateDirectory(path + numberOfCar + "\\K2");                   
                    
                    using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(path) + "\\" + String.Format("{0}.{1}", "log", "txt"), true))//запись в файл
                    {
                        sw.WriteLine(String.Format("{0} {1}", DateTime.Now.ToString() + ":", command));
                    }

                    data = new byte[1024 * 8];

                    threadServer[numberOfCar] = new ThreadServer();//создаём экземпляр класса ThreadServer и передаём в него номер машины
                    new Thread(() => threadServer[numberOfCar].serverStart(numberOfCar)).Start();


                    if (!listBox1.Items.Contains(Remote.ToString()))//добавление клиента в список клиентов, если такой клиент там уже есть, то не добавляем его повторно 
                        listBox1.Invoke(new Action(() => listBox1.Items.Add(remoteFullIp.ToString())));

                    if (command == 1)//если от клиента пришла команда 1
                    {
                        byte[] massiv = new byte[100];
                        string message = "OK";
                        massiv = Encoding.UTF8.GetBytes(message);
                        listeningSocket.SendTo(massiv,0, message.Length,0, Remote);//отправляем OK
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            
        }
   
        IPAddress findMyIPV4Address()// функция для поиска своего ipv4 адреса
        {           
            string strThisHostName = string.Empty;
            IPHostEntry thisHostDNSEntry = null;
            IPAddress[] allIPsOfThisHost = null;
            IPAddress ipv4Ret = null;
            
            try
            {
                strThisHostName = System.Net.Dns.GetHostName();
                thisHostDNSEntry = System.Net.Dns.GetHostEntry(strThisHostName);
                allIPsOfThisHost = thisHostDNSEntry.AddressList;
                
                for (int idx = allIPsOfThisHost.Length - 1; idx >= 0; idx--)
                {
                    if (allIPsOfThisHost[idx].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4Ret = allIPsOfThisHost[idx];
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            return ipv4Ret;
        }

    }

}