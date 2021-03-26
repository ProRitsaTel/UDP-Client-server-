using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeverUdp_5._0
{
    class ThreadServer
    {

        static Socket listeningSocket; // создание переменной для соккета 
        static IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0); //сетевая конечная точка (IPEndPoint Представляет конечную точку сети в виде IP-адреса и номера порта.)
        static EndPoint Remote = (EndPoint)(sender);//определяет сетевой адрес конечной точки         
        Utilities utilities = new Utilities();

        public void Server(int numberPort)// создание сервера 
        {
            try
            {
                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); // Создание сокета   
                listeningSocket.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.105"), Convert.ToInt32(23000 + numberPort))); // задаём порт и ip нашего сервера                
            }//IPAddress.Parse("192.168.0.105")
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        void searchIP()
        {
            try
            {
                IPAddress ipa = null;
                ipa = findMyIPV4Address();
                string IP = "";
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

        public void serverStart(int number)
        {
            try
            {
                searchIP();
                Server(number);// инициализация сервера
                         //textOutput.Invoke(new Action(() => textOutput.Text = "Server is running"));
                Task listeningTask = new Task(Listen);//создание потока для постоянного прослушивания входящих сообщений
                listeningTask.Start(); // Запуск потока

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void Listen() // функция прослушивания входных данных приходящих на сервер
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
                        break;
                    }

                    while (listeningSocket.Available > 0);///!!!!
                    IPEndPoint remoteFullIp = Remote as IPEndPoint;
                    byte[] qerty = new byte[bytes];


                    short chislo = BitConverter.ToInt16(data, 0);//переменная,которая обозначает код проекта в заголовке                
                    short command = BitConverter.ToInt16(data, 2);//переменная,которая обозначает номер команды в заголовке
                    short numberOfCar = BitConverter.ToInt16(data, 4);//переменная,которая обозначает номер машины в заголовке
                    short nenushnayaInfa = BitConverter.ToInt16(data, 6);//переменная,которая обозначает резерв в заголовке
                    string path = @"E:\Belaz\";//путь для сохранения файлов
                    string pathSys1 = @"E:\Belaz\" + numberOfCar + "\\" + "sys1.bin";
                    string pathSys2 = @"E:\Belaz\" + numberOfCar + "\\" + "sys2.bin";

                    Directory.CreateDirectory(path + numberOfCar);//создание директории
                    Directory.CreateDirectory(path + numberOfCar + "\\K1");
                    Directory.CreateDirectory(path + numberOfCar + "\\K2");
                    using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(path) + "\\" + String.Format("{0}.{1}", "log", "txt"), true))//запись в файл
                    {
                        sw.WriteLine(String.Format("{0} {1}", DateTime.Now.ToString() + ":", chislo));
                    }


                    if (command == 3)//если пришла команда 3 от клиента
                    {
                        byte[] massiv = new byte[100];
                        string message = "OK";
                        massiv = Encoding.UTF8.GetBytes(message);
                        listeningSocket.SendTo(massiv, 0, message.Length, 0, Remote);//отправляем OK

                        Thread.Sleep(2000);


                        string message_sys1 = "SYS1";
                        massiv = Encoding.UTF8.GetBytes(message_sys1);
                        listeningSocket.SendTo(massiv, 0, message_sys1.Length, 0, Remote);//отправляем запрос на получение sys1
                    }
                    if (command == 11)//команда 11 - получение данных структуры sys1
                    {
                        
                        utilities.Sys1_(data, qerty, pathSys1);//заносим полученные данные в структуру sys1

                        string message_sys2 = "SYS2";
                        byte[] massiv = new byte[100];
                        massiv = Encoding.UTF8.GetBytes(message_sys2);
                        listeningSocket.SendTo(massiv, 0, message_sys2.Length, 0, Remote);//отправляем запрос на получение sys1
                    }
                    if (command == 13)//команда 13 - получение данных структуры sys2
                    {
                        utilities.Sys2_(data, qerty, pathSys2);//заносим полученные данные в структуру sys2
                    }

                    qerty = new byte[bytes];
                    data = new byte[1024 * 8];
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

