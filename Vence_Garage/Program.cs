using System;
using System.Collections.Generic;

class Garage
{
    List<List<Vehicle>> parkingSpaces;

    public Garage(int levels, int spacesPerLevel)
    {
        parkingSpaces = new List<List<Vehicle>>();

        for (int i = 0; i < levels; i++)
        {
            List<Vehicle> list = new List<Vehicle>();
            for (int j = 0; j < spacesPerLevel; j++)
            {
                list.Add(null);
            }
            parkingSpaces.Add(list);
        }
    }

    public bool IsValidLicensePlate(string licensePlate)
    {
        {
            if (string.IsNullOrEmpty(licensePlate)) return false;
        }

        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[A-Z]{1,3}-[A-Z]{1,2}-\d{1,4}$");

        if (!regex.IsMatch(licensePlate))
        {
            return false;
        }

        var (level, spot) = FindVehicle(licensePlate);
        if (level != -1 && spot != -1)
        {
            return false;
        }

        return true;
    }


    public (int level, int spot) ParkVehicle(Vehicle vehicle)
    {
        if (IsValidLicensePlate(vehicle.LicensePlate))
        {
            for (int i = 0; i < parkingSpaces.Count; i++)
            {
                for (int j = 0; j < parkingSpaces[i].Count; j++)
                {
                    if (parkingSpaces[i][j] == null)
                    {
                        parkingSpaces[i][j] = vehicle;
                        return (i, j);
                    }
                }
            }

            // Kein Parkplatz gefunden
            Console.WriteLine("Alle Parkplätze sind belegt");
            return (-1, -1);
        }

        else
        {
            Console.WriteLine("Ungültiges oder bereits geprarktes Kennzeichen");
            return (-1, -1);
        }
    }

    public bool UnparkVehicle(string licensePlate)
    {
        for (int i = 0; i < parkingSpaces.Count; i++)
        {
            for (int j = 0; j < parkingSpaces[i].Count; j++)
            {
                if (parkingSpaces[i][j]?.LicensePlate == licensePlate)
                {
                    parkingSpaces[i][j] = null;
                    return true;
                }
            }
        }
        return false;
    }

    public bool UnparkVehicle(int level, int spot)
    {
        if (level < 0 || level >= parkingSpaces.Count || spot < 0 || spot >= parkingSpaces[level].Count)
        {
            return false;
        }

        if (parkingSpaces[level][spot] == null)
        {
            return false;
        }

        parkingSpaces[level][spot] = null;
        return true;
    }

    public (int level, int spot) FindVehicle(string licensePlate)
    {
        for (int i = 0; i < parkingSpaces.Count; i++)
        {
            for (int j = 0; j < parkingSpaces[i].Count; j++)
            {
                if (parkingSpaces[i][j]?.LicensePlate == licensePlate)
                {
                    return (i, j);
                }
            }
        }

        return (-1, -1); // nicht gefunden
    }

    public int CountFreeSpaces()
    {
        int count = 0;

        foreach (var level in parkingSpaces)
        {
            foreach (var spot in level)
            {
                if (spot == null)
                {
                    count++;
                }
            }
        }
        
        return count;
    }
}


class Vehicle
{
    public string LicensePlate { get; set; }

    public Vehicle(string licensePlate)
    {
        this.LicensePlate = licensePlate;
    }
}

class Car : Vehicle
{
    public Car(string licensePlate) : base(licensePlate) { }
}

class Motorcycle : Vehicle
{
    public Motorcycle(string licensePlate) : base(licensePlate) { }
}


class Program
{
    static void Main(string[] args)
    {
        int levels;
        int spots;
        Garage garage = null;

        Console.WriteLine("Willkommen bei der Parkhaussimulation der Stadt Vence\n\n");
        Console.WriteLine("Bitte geben Sie die Anzahl der Etagen ein:");
        string inputLevels = Console.ReadLine();

        if (inputLevels != null && int.TryParse(inputLevels, out levels))
        {
            Console.WriteLine($"Sie haben {levels} Etagen gewählt\n");
            Console.WriteLine("Geben Sie nun die Anzahl an Parkplätzen je Etage an:");
            string inputSpots = Console.ReadLine();

            if (inputSpots != null && int.TryParse(inputSpots, out spots))
            {
                Console.WriteLine($"Sie haben {spots} Parkplätze je Etage angegeben");
                garage = new Garage(levels, spots);
            }
            else
            {
                Console.WriteLine("Ungültige Eingabe. Bitte geben Sie eine gültige Nummer für die Anzahl der Parkplätze ein.");
            }
        }
        else
        {
            Console.WriteLine("Ungültige Eingabe. Bitte geben Sie eine gültige Nummer für die Anzahl der Etagen ein.");
        }

        if (garage != null)
        {
            Console.WriteLine("Was möchten Sie tun? Geben Sie die Ziffer der Funktion ein, die Sie nutzen möchten:\n");
            Console.WriteLine("1 = Fahrzeug einparken\n2 = Fahrzeug ausparken\n3 = Fahrzeug finden\n4 = Anzahl verfügbarer Parkplätze anzeigen");
            string inputMode = Console.ReadLine();

            switch (inputMode)
            {
                case "1":
                    Console.WriteLine("Fahrzeug einparken");
                    // Code zum Einparken
                    break;
                case "2":
                    Console.WriteLine("Fahrzeug ausparken");
                    // Code zum Ausparken
                    break;
                case "3":
                    Console.WriteLine("Fahrzeug finden");
                    // Code zum Finden des Fahrzeugs
                    break;
                case "4":
                    Console.WriteLine($"Anzahl verfügbarer Parkplätze: {garage.CountFreeSpaces()}");
                    break;
                default:
                    Console.WriteLine("Ungültige Auswahl.");
                    break;
            }
        }
    }
}
