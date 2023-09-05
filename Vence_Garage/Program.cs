using System;
using System.Collections.Generic;

// Garage class represents parking garage and its levels and parking spots
class Garage
{
    // twodimensional list to represent spots per level
    List<List<Vehicle>> parkingSpaces;

    // define constructor to build garage
    public Garage(int levels, int spacesPerLevel)
    {
        // initialise twodimensional list
        parkingSpaces = new List<List<Vehicle>>();

        // initialise with "null" (null represents open spots)
        // cycle through levels with outer for-loop
        for (int i = 0; i < levels; i++)
        {
            List<Vehicle> list = new List<Vehicle>();

            // cycle through levels with inner for-loop
            for (int j = 0; j < spacesPerLevel; j++)
            {
                list.Add(null);
            }
            parkingSpaces.Add(list);
        }
    }

    // check license plates for validity
    public bool IsValidLicensePlate(string licensePlate)
    {
        {
            // if string null or empty: invalid
            if (string.IsNullOrEmpty(licensePlate)) return false;
        }

        // regex to check for format: 1-5 letters (1-3 letters for city, 1-2 letters from owner-chosen letters (thus 2-5 letters), 1-4 numbers)
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[A-Z]{2,5}\d{1,4}$");

        // if regex doesn't match: invalid -> untruthy return
        if (!regex.IsMatch(licensePlate))
        {
            return false;
        }

        // check if vehicle is already parked
        var (level, spot) = FindVehicle(licensePlate);

        // if level or spot not -1: vehicle already parked
        if (level != -1 && spot != -1)
        {
            return false;
        }

        // truthy return only if all checks passed
        return true;
    }


    // implement parking
    public (int level, int spot) ParkVehicle(Vehicle vehicle)
    {
        // needs truth return from validity check
        if (IsValidLicensePlate(vehicle.LicensePlate))
        {
            // check for next available level and spot
            for (int i = 0; i < parkingSpaces.Count; i++)
            {
                for (int j = 0; j < parkingSpaces[i].Count; j++)
                {
                    // since null = free spot, if for-incrementors find null in 2D list -> vehicle gets parked
                    if (parkingSpaces[i][j] == null)
                    {
                        parkingSpaces[i][j] = vehicle;
                        return (i, j);
                    }
                }
            }

            // no spot found
            Console.WriteLine("Alle Parkplätze sind belegt");
            return (-1, -1);
        }

        // untruthy return from validity check
        else
        {
            Console.WriteLine("Ungültiges oder bereits geparktes Kennzeichen");
            return (-1, -1);
        }
    }

    // implement unparking by license plate
    public bool UnparkVehicle(string licensePlate)
    {
        for (int i = 0; i < parkingSpaces.Count; i++)
        {
            for (int j = 0; j < parkingSpaces[i].Count; j++)
            {
                // if license plate found: unpark
                if (parkingSpaces[i][j]?.LicensePlate == licensePlate)
                {
                    parkingSpaces[i][j] = null;
                    return true;
                }
            }
        }
        // else vehicle not parked
        return false;
    }

    // implement unparking by level & spot
    public bool UnparkVehicle(int level, int spot)
    {
        // check for valid level / spot
        if (level < 0 || level >= parkingSpaces.Count || spot < 0 || spot >= parkingSpaces[level].Count)
        {
            // untruthy return
            return false;
        }

        // if spot empty: can't unpark
        if (parkingSpaces[level][spot] == null)
        {
            // untruthy return
            return false;
        }

        // set spot to null (-> available)
        parkingSpaces[level][spot] = null;
        return true;
    }

    // implement finding vehicle by license plate
    public (int level, int spot) FindVehicle(string licensePlate)
    {
        // search levels/spots
        for (int i = 0; i < parkingSpaces.Count; i++)
        {
            for (int j = 0; j < parkingSpaces[i].Count; j++)
            {
                if (parkingSpaces[i][j]?.LicensePlate == licensePlate)
                {
                    // return coordinates if found
                    return (i, j);
                }
            }
        }

        // not found
        return (-1, -1);
    }

    // count all spaces which are null (empty)
    public int CountFreeSpaces()
    {
        int count = 0;

        foreach (var level in parkingSpaces)
        {
            foreach (var spot in level)
            {
                // increment counter if null found
                if (spot == null)
                {
                    count++;
                }
            }
        }
        
        return count;
    }

    // modification of level count during runtime
    public bool ModifyLevels(int newLevelCount)
    {
        // add levels:
        if (newLevelCount > parkingSpaces.Count)
        {
            int levelsToAdd = newLevelCount - parkingSpaces.Count;
            for (int i = 0; i < levelsToAdd; i++)
            {
                List<Vehicle> newLevel = new List<Vehicle>();
                for (int j = 0; j < parkingSpaces[0].Count; j++)
                {
                    newLevel.Add(null);
                }
                parkingSpaces.Add(newLevel);
            }
            return true;
        }
        // remove levels:
        else if (newLevelCount < parkingSpaces.Count)
        {
            int levelsToRemove = parkingSpaces.Count - newLevelCount;

            // for-loop removes levels until number of removals reached
            for (int i = 0; i < levelsToRemove; i++)
            {
                // try reparking 
                if (!ModifyLevels(parkingSpaces.Count - 1))
                {
                    return false; // not enough spots for reparking
                }

                // remove empty levels
                parkingSpaces.RemoveAt(parkingSpaces.Count - 1);
            }
            return true;
        }
        return false; // no change if unsuccessful
    }
}

// define vehicle class
class Vehicle
{
    public string LicensePlate { get; set; }
    
    // constructor
    public Vehicle(string licensePlate)
    {
        this.LicensePlate = licensePlate;
    }
}

// subclass for cars
class Car : Vehicle
{
    public Car(string licensePlate) : base(licensePlate) { }
}

// subclass for motorcycles
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

        // initialise garage as null, on valid user input garage no longer null
        // -> can check for garage being null -> input error
        Garage garage = null;

        // begin user prompt
        Console.WriteLine("Willkommen bei der Parkhaussimulation der Stadt Vence\n\n");
        Console.WriteLine("Bitte geben Sie die Anzahl der Etagen ein:");
        string inputLevels = Console.ReadLine();

        // if level input not null & casting successful: valid input
        if (inputLevels != null && int.TryParse(inputLevels, out levels))
        {
            Console.WriteLine($"Sie haben {levels} Etagen gewählt\n");
            Console.WriteLine("Geben Sie nun die Anzahl an Parkplätzen je Etage an:");
            string inputSpots = Console.ReadLine();

            // check validity like before
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

        if (garage != null) // garage != null: placeholders replaced with actual values -> valid inputs
        {

            // loop to enable continuos usage of application
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("Was möchten Sie tun? Geben Sie die Ziffer der Funktion ein, die Sie nutzen möchten:\n");
                Console.WriteLine("1 = Fahrzeug einparken\n2 = Fahrzeug ausparken\n3 = Fahrzeug finden\n4 = Anzahl verfügbarer Parkplätze anzeigen\n5 = Ebenenstruktur verändern\n6 = Programm beenden");
                string inputMode = Console.ReadLine();


                // switch for function selection
                switch (inputMode)
                {

                    // parking new vehicle
                    case "1":
                        Console.WriteLine("Fahrzeug einparken\nBitte geben Sie das Kennzeichen des Fahrzeugs an:");
                        string licensePlate = Console.ReadLine();

                        Console.WriteLine("Auto oder Motorrad? (A/M eingeben)");
                        string vehicleType = Console.ReadLine();
                        Vehicle vehicle = null;

                        // create car
                        if (vehicleType.Equals("A"))
                        {
                            vehicle = new Car(licensePlate);
                        }

                        // create motorcycle
                        else if (vehicleType.Equals("M"))
                        {
                            vehicle = new Motorcycle(licensePlate);
                        }

                        // invalid input
                        else
                        {
                            Console.WriteLine("Ungültige Eingabe");
                        }

                        // park at coordinates
                        var (level, spot) = garage.ParkVehicle(vehicle);

                        if (level != -1 && spot != -1)

                        // correct potential output of "level 0"
                        {
                            Console.WriteLine($"Fahrzeug geparkt auf Ebene {level + 1}, Parkplatz {spot + 1}");
                        }

                        else
                        {
                            Console.WriteLine("Fahrzeug konnte nicht geparkt werden");
                        }
                        break;
                    
                    // unpark vehicle
                    case "2":
                        Console.WriteLine("Fahrzeug ausparken\nMöchten Sie das Fahrzeug über das Kennzeichen oder die Ebene und den Parkplatz ausparken? (K für Kennzeichen, E für Ebene und Parkplatz)");
                        string unparkMethod = Console.ReadLine();

                        // check for unparking method
                        switch (unparkMethod)
                        {
                            // by license plate
                            case "K":
                                Console.WriteLine("Bitte geben Sie das Kennzeichen des auszuparkenden Fahrzeugs an:");
                                string licensePlateToUnpark = Console.ReadLine();
                                if (garage.UnparkVehicle(licensePlateToUnpark))
                                {
                                    Console.WriteLine("Fahrzeug erfolgreich ausgeparkt.");
                                }
                                else
                                {
                                    Console.WriteLine("Fahrzeug konnte nicht ausgeparkt werden. Kennzeichen nicht gefunden.");
                                }
                                break;

                            // by level/spot
                            case "E":
                                Console.WriteLine("Bitte geben Sie die Ebene des auszuparkenden Fahrzeugs an:");
                                string levelStr = Console.ReadLine();
                                Console.WriteLine("Bitte geben Sie den Parkplatz des auszuparkenden Fahrzeugs an:");
                                string spotStr = Console.ReadLine();

                                // check input
                                if (int.TryParse(levelStr, out int unparkLevel) && int.TryParse(spotStr, out int unparkSpot))
                                {
                                    if (garage.UnparkVehicle(unparkLevel - 1, unparkSpot - 1))  // correct user input for right indices
                                    {
                                        Console.WriteLine("Fahrzeug erfolgreich ausgeparkt.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Fahrzeug konnte nicht ausgeparkt werden. Ungültige Ebene oder Parkplatz.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Ungültige Eingabe für Ebene oder Parkplatz.");
                                }
                                break;

                            default:
                                Console.WriteLine("Ungültige Auswahl.");
                                break;
                        }
                        break;

                    // find vehicle by license plate
                    case "3":
                        Console.WriteLine("Fahrzeug finden\nBitte geben Sie das Kennzeichen des zu findenden Fahrzeugs an:");
                        string licensePlateToFind = Console.ReadLine();
                        var (foundLevel, foundSpot) = garage.FindVehicle(licensePlateToFind);

                        if (foundLevel != -1 && foundSpot != -1)
                        {
                            //correction for user output
                            Console.WriteLine($"Fahrzeug gefunden auf Ebene {foundLevel + 1}, Parkplatz {foundSpot + 1}");
                        }
                        else
                        {
                            Console.WriteLine("Fahrzeug konnte nicht gefunden werden.");
                        }
                        break;

                    // count free spaces
                    case "4":
                        Console.WriteLine($"Anzahl verfügbarer Parkplätze: {garage.CountFreeSpaces()}");
                        break;

                    // edit levels
                    case "5":
                        Console.WriteLine("Anzahl der Ebenen ändern\nBitte geben Sie die neue Anzahl der Ebenen ein:");
                        string newLevelCountStr = Console.ReadLine();

                        if (newLevelCountStr != null && int.TryParse(newLevelCountStr, out int newLevelCount))
                        {
                            if (garage.ModifyLevels(newLevelCount))
                            {
                                Console.WriteLine($"Die Anzahl der Ebenen wurde erfolgreich auf {newLevelCount} geändert.");
                            }
                            else
                            {
                                Console.WriteLine("Die Anzahl der Ebenen konnte nicht geändert werden. Stellen Sie sicher, dass ausreichend Parkplätze zum Umparken vorhanden sind.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ungültige Eingabe.");
                        }
                        break;

                    // sets boolean that keeps while-loop running to false
                    case "6":
                        keepRunning = false;
                        break;
                    default:
                        Console.WriteLine("Ungültige Auswahl.");
                        break;
                }
            }
        }
    }
}
