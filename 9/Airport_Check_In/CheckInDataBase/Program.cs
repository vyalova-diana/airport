using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Collections.Generic;

namespace CheckInDataBase
{
    public class Passenger
    {
        // public static Fsm fsm;
        private static readonly HttpClient client = new HttpClient();
        public Passport Passport { get; set; }
        public uint BaggageWeight { get; set; }
        public TypeOfFood TypeOfFood { get; set; }
        public Ticket Ticket { get; set; }

    }

    public class Passport
    {
        public Guid Guid { get; set; }
        public string Surname { get; set; }
        public string GivenNames { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
    }

    public class Ticket
    {
        public readonly int tID; //id билета 
        public int pID; //id пассажира 
        public int fID; //id рейса 
        public string city; //место назначения 
        public string pName; //имя 
        public string pSecondName; //фамилия 
        public Sex sex; //пол 
    }

    public enum TypeOfFood
    {
        Normal,
        Vegan
    }

    public enum Sex
    {
        Male,
        Female
    }
    // Еда 
    public class Food
    {
        public int FlightNumber { get; set; }
        public int Normal { get; set; }
        public int Vegan { get; set; }
    }

    // Багаж
    public class Baggage
    {
        public int FlightNumber { get; set; }
        public int PassengerId { get; set; }
        public uint BaggageWeight { get; set; }
    }

    public class CheckInStatus
    {
        public int FlightNumber { get; set; }
        public CheckIn_Status Status { get; set; }

    }
    public enum CheckIn_Status
    {
        Start,
        Finish
    }
    public class CheckInSchedule
    {
        public static List<CheckInStatus> StatusList = new List<CheckInStatus>();

        private static CheckInSchedule instance;
        private CheckInSchedule()
        {

        }
        public static CheckInSchedule getInstance()
        {
            if (instance == null)
            {
                instance = new CheckInSchedule();
            }
            return instance;
        }

        
        public void ChangeStatus(CheckInStatus newStatus) //добавляем в список , если регистрация началась
        {                                                 //удаляем, если закончилась

            if (newStatus.Status == CheckIn_Status.Start)
            {
                StatusList.Add(newStatus);
            }
            else
            {

                foreach (var i in StatusList)
                {
                    if (newStatus.FlightNumber == i.FlightNumber)
                    {
                        StatusList.Remove(i);
                        break;
                    }
                }
            }
        }

        public int find(int fID)
        {
            foreach (var i in StatusList)
            {
                if (fID == i.FlightNumber)
                {
                    return 1;
                }
            }
            return 0;
        }

        public void print()
        {
            foreach (var i in StatusList)
            {
                Console.WriteLine(i.FlightNumber);
                Console.WriteLine(i.Status);
            }

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //CheckInStatus newStatus1 = new CheckInStatus();
            //newStatus1.FlightNumber = 1;
            //newStatus1.Status = CheckIn_Status.Start;

            //CheckInSchedule.getInstance().ChangeStatus(newStatus1);

            //CheckInStatus newStatus2 = new CheckInStatus();
            //newStatus2.FlightNumber = 2;
            //newStatus2.Status = CheckIn_Status.Start;

            //CheckInSchedule.getInstance().ChangeStatus(newStatus2);

            //CheckInStatus newStatus3 = new CheckInStatus();
            //newStatus3.FlightNumber = 3;
            //newStatus3.Status = CheckIn_Status.Start;

            //CheckInSchedule.getInstance().ChangeStatus(newStatus3);

            //CheckInStatus newStatus4 = new CheckInStatus();
            //newStatus4.FlightNumber = 4;
            //newStatus4.Status = CheckIn_Status.Start;

            //CheckInSchedule.getInstance().ChangeStatus(newStatus4);
            //CheckInSchedule.getInstance().print();

            //CheckInStatus Status4 = new CheckInStatus();
            //Status4.FlightNumber = 2;
            //Status4.Status = CheckIn_Status.Finish;
            //CheckInSchedule.getInstance().ChangeStatus(Status4);
            //CheckInSchedule.getInstance().print();


            //Console.ReadKey();
        }
    }
}
