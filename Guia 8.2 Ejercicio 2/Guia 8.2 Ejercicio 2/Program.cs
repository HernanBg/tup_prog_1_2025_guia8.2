using System;

class Program
{
    static Servicio servicio = new Servicio();

    static void MostrarPantallaSolicitarOpcionMenu()
    {
        Console.WriteLine("1. Ingresar monto a repartir");
        Console.WriteLine("2. Ingresar edades de las niñas");
        Console.WriteLine("3. Calcular y mostrar montos y porcentajes");
        Console.WriteLine("4. Salir");
        Console.Write("Seleccione una opción: ");
    }

    static void MostrarPantallaSolicitarMontoARepartir()
    {
        Console.Write("Ingrese el monto total a repartir: ");
        double monto = double.Parse(Console.ReadLine());
        servicio.RegistrarMontoARepartir(monto);
    }

    static void MostrarPantallaSolicitarEdadesNiñas()
    {
        for (int i = 0; i < 4; i++)
        {
            Console.Write($"Ingrese la edad de la niña {i + 1}: ");
            int edad = int.Parse(Console.ReadLine());
            servicio.RegistrarEdad(edad, i);
        }
    }

    static void MostrarPantallaCalcularMostrarMontoYPorcentajePorNiña()
    {
        servicio.CalcularMontosYPorcentajesARepartir();
        for (int i = 0; i < 4; i++)
        {
            double porcentaje = servicio.GetType().GetField($"Porcentaje{i}").GetValue(servicio) as double? ?? 0;
            double monto = servicio.GetType().GetField($"Monto{i}").GetValue(servicio) as double? ?? 0;

            Console.WriteLine($"Niña {i + 1}: {porcentaje:P2} del total, recibe ${monto:F2}");
        }
    }

    static void Main(string[] args)
    {
        int opcion;
        do
        {
            MostrarPantallaSolicitarOpcionMenu();
            opcion = int.Parse(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    MostrarPantallaSolicitarMontoARepartir();
                    break;
                case 2:
                    MostrarPantallaSolicitarEdadesNiñas();
                    break;
                case 3:
                    MostrarPantallaCalcularMostrarMontoYPorcentajePorNiña();
                    break;
                case 4:
                    Console.WriteLine("Saliendo...");
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }

            Console.WriteLine();
        } while (opcion != 4);
    }
}
public class Servicio
{
    public int Edad0, Edad1, Edad2, Edad3;
    public double Monto;
    public double Porcentaje0, Porcentaje1, Porcentaje2, Porcentaje3;
    public double Monto0, Monto1, Monto2, Monto3;

    public void RegistrarMontoARepartir(double monto)
    {
        Monto = monto;
    }

    public void RegistrarEdad(int edad, int nroNiña)
    {
        switch (nroNiña)
        {
            case 0: Edad0 = edad; break;
            case 1: Edad1 = edad; break;
            case 2: Edad2 = edad; break;
            case 3: Edad3 = edad; break;
        }
    }

    public void CalcularMontosYPorcentajesARepartir()
    {
        int sumaEdades = Edad0 + Edad1 + Edad2 + Edad3;

        Porcentaje0 = (double)Edad0 / sumaEdades;
        Porcentaje1 = (double)Edad1 / sumaEdades;
        Porcentaje2 = (double)Edad2 / sumaEdades;
        Porcentaje3 = (double)Edad3 / sumaEdades;

        Monto0 = Monto * Porcentaje0;
        Monto1 = Monto * Porcentaje1;
        Monto2 = Monto * Porcentaje2;
        Monto3 = Monto * Porcentaje3;
    }
}