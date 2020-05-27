using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLRR
{
    class Process
    {
        public int Priority;
        public int U = 0;
        public int WaitTime = 0;
        public int WorkTime = 0;
        public int BornTime;
        public int LongTime;
        public bool Start = false;
        public List<int> State;
        public bool End = false;
    }
    class HLRR
    {
        public int DT = 3;
        private Process [] Elements;
        private int CountProcesses;
        private int DefaultPriority;
        private int CurrentProcess;
        private int EndProcess = 0;
        private bool exit = false;
        public HLRR()
        {
            string Data;
            bool Result;

            do
            {
                Console.WriteLine("Введите количество процессов");
                Data = Console.ReadLine();
                Result = int.TryParse(Data, out CountProcesses);
            } while(!Result);

            do
            {
                Console.WriteLine("Введите приоритет P0");
                Data = Console.ReadLine();
                Result = int.TryParse(Data, out DefaultPriority);
            } while (!Result);

            Elements = new Process[CountProcesses];

            for(int i = 0; i < CountProcesses; i++)
            {
                Elements[i] = new Process();
                Elements[i].State = new List<int>();
                Elements[i].Priority = DefaultPriority;
                do
                {
                    Console.WriteLine("Введите время появления");
                    Data = Console.ReadLine();
                    Result = int.TryParse(Data, out Elements[i].BornTime);
                } while (!Result);
                do
                {
                    Console.WriteLine("Введите время работы");
                    Data = Console.ReadLine();
                    Result = int.TryParse(Data, out Elements[i].LongTime);
                } while (!Result);

            }

            FindFirstProcess();
        }
        
        private void FindFirstProcess()
        {
            int First = 0;
            for(int i = 0; i < CountProcesses; i++)
            {
                if(Elements[First].BornTime > Elements[i].BornTime)
                    First = i;
            }
            CurrentProcess = First;
        }

        public void Run()
        {
            int CountQuant = 1;
            Elements[CurrentProcess].Start = true;
            while (CountQuant != Elements[CurrentProcess].BornTime)
            {
                for(int i = 0; i < CountProcesses; i++)
                {
                    Elements[i].State.Add(3);
                }
                CountQuant++;
            }
            while (!exit)
            {  
                for(int i = 0; i < CountProcesses; i++)
                {
                    if(i == CurrentProcess)
                    {
                        Elements[i].State.Add(1);
                        Elements[i].WorkTime += 1;
                        for(int j = 0; j < DT; j++)
                            Elements[CurrentProcess].U += 1;
                    }
                    else
                    {
                        if(Elements[i].BornTime == CountQuant)
                        {
                            Elements[i].State.Add(2);
                            Elements[i].Start = true;
                        }
                        else if(Elements[i].Start)
                        {
                            Elements[i].State.Add(2);
                            Elements[i].WaitTime += 1;
                        }
                        else
                        {
                            Elements[i].State.Add(3);
                        }
                    }
                    Elements[i].U /= 2;
                    Elements[i].Priority = DefaultPriority + Elements[i].U;
                }
                NextProcess();
                CountQuant++;
                if (EndProcess == CountProcesses)
                    break;
            }
            PrintState();
        }

        private void NextProcess()
        {
            if(Elements[CurrentProcess].WorkTime == Elements[CurrentProcess].LongTime)
            {
                Elements[CurrentProcess].End = true;
                Elements[CurrentProcess].Start = false;
                EndProcess++;
                for(int i = 0; i < CountProcesses; i++)//Просто перебросили индекс на какой то процесс
                {
                    if (!Elements[i].End)
                    {
                        CurrentProcess = i;
                    }
                }
            }

            for (int i = 0; i < CountProcesses; i++)
            {
                if (Elements[i].State.Last() == 2)
                {
                    if (Elements[i].Priority < Elements[CurrentProcess].Priority)
                    {
                        CurrentProcess = i;
                    }
                    else if (Elements[i].Priority == Elements[CurrentProcess].Priority)
                    {
                        if (Elements[i].WaitTime > Elements[CurrentProcess].WaitTime)
                            CurrentProcess = i;
                    }
                }
            }
        }

        private void PrintState()
        {
            foreach(var element in Elements)
            {
                Console.Write("Время рождения: " + element.BornTime + "\nВремя работы: " + element.LongTime + "\n");
                foreach (var node in element.State)
                {
                    if (node == 1)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (node == 2)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (node == 3)
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(node + " ");
                }
                Console.ResetColor();
                Console.Write('\n');
            }
        }
    }
}
