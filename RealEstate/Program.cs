using System;
using System.Collections.Generic;


public interface IRentable
{
    bool IsRented { get; set; }
    double MonthlyRent { get; set; }
}


public abstract class Property
{
    public string Address { get; set; }
    public double IndoorArea { get; set; }
    public double PropertyValue { get; set; }

    public Property(string address, double indoorArea, double propertyValue)
    {
        Address = address;
        IndoorArea = indoorArea;
        PropertyValue = propertyValue;
    }

    public abstract override string ToString();
}


public class House : Property
{
    public double OutdoorArea { get; set; }

    
    public double TotalArea => IndoorArea + OutdoorArea;

    public House(string address, double indoorArea, double outdoorArea, double propertyValue)
        : base(address, indoorArea, propertyValue)
    {
        OutdoorArea = outdoorArea;
    }

    public override string ToString()
    {
        return $"House:\n Address: {Address}\n Indoor Area: {IndoorArea} m^2\n Outdoor Area: {OutdoorArea} m^2\n" +
               $"Total Area: {TotalArea} m^2\n Value: ${PropertyValue:N2}";
    }
}


public class Apartment : Property
{
    public int Floor { get; set; }
    public bool HasElevator { get; set; }

    public Apartment(string address, double indoorArea, double propertyValue, int floor, bool hasElevator)
        : base(address, indoorArea, propertyValue)
    {
        Floor = floor;
        HasElevator = hasElevator;
    }

    public override string ToString()
    {
        return $"Apartment:\n Address: {Address}\n Floor: {Floor}\n Indoor Area: {IndoorArea} m^2\n " +
               $"Has Elevator: {(HasElevator ? "Yes" : "No")}\n Value: ${PropertyValue:N2}";
    }
}


public class RentableApartment : Apartment, IRentable
{
    public bool IsRented { get; set; }
    public double MonthlyRent { get; set; }

    public RentableApartment(string address, double indoorArea, double propertyValue, int floor, bool hasElevator, double monthlyRent)
        : base(address, indoorArea, propertyValue, floor, hasElevator)
    {
        MonthlyRent = monthlyRent;
        IsRented = false;
    }

    public override string ToString()
    {
        return $"Rentable Apartment:\n Address: {Address}\n Floor: {Floor}\n Indoor Area: {IndoorArea} m^2\n " +
               $"Has Elevator: {(HasElevator ? "Yes" : "No")}\n Value: ${PropertyValue:N2}\n " +
               $"Monthly Rent: ${MonthlyRent:N2}\n Currently Rented: {(IsRented ? "Yes" : "No")}";
    }
}


public class RealEstateAgency
{
    private List<Property> properties = new List<Property>();

    public void AddProperty(Property property)
    {
        properties.Add(property);
        Console.WriteLine($"Added: {property.Address}");
    }

    public void RentProperty(string address)
    {
        Property property = properties.Find(p => p.Address.Equals(address, StringComparison.OrdinalIgnoreCase));

        if (property == null)
        {
            Console.WriteLine("Property not found.");
            return;
        }

        if (property is IRentable rentable)
        {
            if (rentable.IsRented)
            {
                Console.WriteLine("Property is already rented.");
            }
            else
            {
                rentable.IsRented = true;
                Console.WriteLine($"Property at {address} has been rented for ${rentable.MonthlyRent:N2} per month.");
            }
        }
        else
        {
            Console.WriteLine("This property is not rentable.");
        }
    }

    public void ShowAllProperties()
    {
        foreach (var property in properties)
        {
            Console.WriteLine(" ");
            Console.WriteLine(property.ToString());
        }
    }
}


public class Program
{
    public static void Main()
    {
        RealEstateAgency agency = new RealEstateAgency();

        
        House house = new House("123 A", 120, 80, 350000);
        Apartment apartment = new Apartment("45 B", 90, 250000, 3, true);
        RentableApartment rentableApt = new RentableApartment("78 C", 85, 280000, 5, true, 1500);

        agency.AddProperty(house);
        agency.AddProperty(apartment);
        agency.AddProperty(rentableApt);

        Console.WriteLine("\n All Properties");
        agency.ShowAllProperties();

        Console.WriteLine("\n Renting a Property");
        agency.RentProperty("78 C");
        agency.RentProperty("123 A"); 
    }
}
