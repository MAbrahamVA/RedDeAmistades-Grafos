using System;
using System.Collections.Generic;

class Grafo
{
    // Diccionario: persona → lista de amigos
    private Dictionary<string, List<string>> amigos = new Dictionary<string, List<string>>();

    // 1. Agregar persona (nodo)
    public void AgregarPersona(string nombre)
    {
        if (!amigos.ContainsKey(nombre))
        {
            amigos[nombre] = new List<string>();
            Console.WriteLine($"Persona '{nombre}' agregada a la red.");
        }
        else
        {
            Console.WriteLine($"La persona '{nombre}' ya existe en la red.");
        }
    }

    // 2. Conectar dos personas (arista no dirigida)
    public void Conectar(string p1, string p2)
    {
        if (!amigos.ContainsKey(p1) || !amigos.ContainsKey(p2))
        {
            Console.WriteLine("Una o ambas personas no existen en la red.");
            return;
        }

        if (!amigos[p1].Contains(p2))
        {
            amigos[p1].Add(p2);
            amigos[p2].Add(p1);
            Console.WriteLine($"{p1} y {p2} ahora son amigos.");
        }
        else
        {
            Console.WriteLine($"{p1} y {p2} ya estaban conectados.");
        }
    }

    // 3. Mostrar todas las conexiones
    public void MostrarConexiones()
    {
        Console.WriteLine("\nRed de Amistades:");
        foreach (var persona in amigos)
        {
            Console.Write($"{persona.Key}: ");
            Console.WriteLine(string.Join(", ", persona.Value));
        }
    }

    // 4. Recorrer grafo (BFS)
    public void RecorrerGrafo(string inicio)
    {
        if (!amigos.ContainsKey(inicio))
        {
            Console.WriteLine("Esa persona no existe en la red.");
            return;
        }

        Console.WriteLine($"\nRecorrido desde {inicio}:");
        var visitados = new HashSet<string>();
        var cola = new Queue<string>();
        cola.Enqueue(inicio);
        visitados.Add(inicio);

        while (cola.Count > 0)
        {
            string actual = cola.Dequeue();
            Console.Write(actual + " ");

            foreach (var amigo in amigos[actual])
            {
                if (!visitados.Contains(amigo))
                {
                    visitados.Add(amigo);
                    cola.Enqueue(amigo);
                }
            }
        }
        Console.WriteLine();
    }

    // 5. Verificar si dos personas están conectadas (BFS)
    public bool EstanConectados(string origen, string destino)
    {
        if (!amigos.ContainsKey(origen) || !amigos.ContainsKey(destino))
        {
            Console.WriteLine("Una o ambas personas no existen en la red.");
            return false;
        }

        var visitados = new HashSet<string>();
        var cola = new Queue<string>();
        cola.Enqueue(origen);
        visitados.Add(origen);

        while (cola.Count > 0)
        {
            string actual = cola.Dequeue();
            if (actual == destino)
                return true;

            foreach (var vecino in amigos[actual])
            {
                if (!visitados.Contains(vecino))
                {
                    visitados.Add(vecino);
                    cola.Enqueue(vecino);
                }
            }
        }
        return false;
    }

    // 6. Mostrar grado de cada persona
    public void MostrarGradoDeCadaPersona()
    {
        Console.WriteLine("\nGrado de cada persona:");
        foreach (var persona in amigos)
        {
            Console.WriteLine($"{persona.Key} tiene {persona.Value.Count} amistad(es).");
        }
    }

    // 7. Camino más corto entre dos personas
    public void CaminoMasCorto(string origen, string destino)
    {
        if (!amigos.ContainsKey(origen) || !amigos.ContainsKey(destino))
        {
            Console.WriteLine("Una o ambas personas no existen.");
            return;
        }

        var cola = new Queue<string>();
        var previo = new Dictionary<string, string>();
        var visitados = new HashSet<string>();

        cola.Enqueue(origen);
        visitados.Add(origen);

        while (cola.Count > 0)
        {
            string actual = cola.Dequeue();
            if (actual == destino)
                break;

            foreach (var vecino in amigos[actual])
            {
                if (!visitados.Contains(vecino))
                {
                    visitados.Add(vecino);
                    previo[vecino] = actual;
                    cola.Enqueue(vecino);
                }
            }
        }

        if (!visitados.Contains(destino))
        {
            Console.WriteLine($"No hay conexión entre {origen} y {destino}.");
            return;
        }

        var camino = new List<string>();
        string nodo = destino;
        while (nodo != null)
        {
            camino.Insert(0, nodo);
            previo.TryGetValue(nodo, out nodo);
        }

        Console.WriteLine($"Camino más corto entre {origen} y {destino}:");
        Console.WriteLine(string.Join(" -> ", camino));
    }

    // 8. Mostrar grupos de amistades (componentes conexos)
    public void MostrarGrupos()
    {
        var visitados = new HashSet<string>();
        int grupo = 1;

        foreach (var persona in amigos.Keys)
        {
            if (!visitados.Contains(persona))
            {
                var grupoActual = new List<string>();
                var cola = new Queue<string>();
                cola.Enqueue(persona);
                visitados.Add(persona);

                while (cola.Count > 0)
                {
                    string actual = cola.Dequeue();
                    grupoActual.Add(actual);

                    foreach (var vecino in amigos[actual])
                    {
                        if (!visitados.Contains(vecino))
                        {
                            visitados.Add(vecino);
                            cola.Enqueue(vecino);
                        }
                    }
                }

                Console.WriteLine($"\nGrupo {grupo++}: {string.Join(", ", grupoActual)}");
            }
        }
    }
}

class GrafoAmistades
{
    static void Main()
    {
        Grafo red = new Grafo();
        int opcion;

        do
        {
            Console.WriteLine("\nMenú de Opciones");
            Console.WriteLine("1. Agregar persona");
            Console.WriteLine("2. Conectar personas");
            Console.WriteLine("3. Mostrar red completa");
            Console.WriteLine("4. Recorrer grafo (BFS)");
            Console.WriteLine("5. Verificar si dos personas están conectadas");
            Console.WriteLine("6. Mostrar grado de cada persona");
            Console.WriteLine("7. Camino más corto entre dos personas");
            Console.WriteLine("8. Mostrar grupos de amistades");
            Console.WriteLine("9. Salir");
            Console.Write("Seleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("Ingrese un número válido.");
                continue;
            }

            switch (opcion)
            {
                case 1:
                    Console.Write("Nombre de la persona: ");
                    red.AgregarPersona(Console.ReadLine());
                    break;

                case 2:
                    Console.Write("Primera persona: ");
                    string p1 = Console.ReadLine();
                    Console.Write("Segunda persona: ");
                    string p2 = Console.ReadLine();
                    red.Conectar(p1, p2);
                    break;

                case 3:
                    red.MostrarConexiones();
                    break;

                case 4:
                    Console.Write("Persona de inicio: ");
                    red.RecorrerGrafo(Console.ReadLine());
                    break;

                case 5:
                    Console.Write("Persona origen: ");
                    string o1 = Console.ReadLine();
                    Console.Write("Persona destino: ");
                    string o2 = Console.ReadLine();
                    bool conectado = red.EstanConectados(o1, o2);
                    Console.WriteLine(conectado
                        ? $"{o1} y {o2} están conectados (directa o indirectamente)."
                        : $"{o1} y {o2} NO están conectados.");
                    break;

                case 6:
                    red.MostrarGradoDeCadaPersona();
                    break;

                case 7:
                    Console.Write("Persona origen: ");
                    string c1 = Console.ReadLine();
                    Console.Write("Persona destino: ");
                    string c2 = Console.ReadLine();
                    red.CaminoMasCorto(c1, c2);
                    break;

                case 8:
                    red.MostrarGrupos();
                    break;

                case 9:
                    Console.WriteLine("Saliendo del programa...");
                    break;

                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }

        } while (opcion != 9);
    }
}