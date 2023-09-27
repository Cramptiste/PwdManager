using System;
using System.Collections.Generic;
using System.Text;

namespace PswGest
{
    static class outils
    {
        public static int DemanderNombrePositifNonNul(string question)
        {
            return DemanderNombreEntre(question, 1, int.MaxValue, "ERREUR : le nombre doit etre positif et non nul");
        }

        public static int DemanderNombreEntre(string question, int min, int max, string messageErreurPersonnalise = null)
        {

            int nombre = DemanderNombre(question);

            if (nombre >= min && nombre <= max)
            {
                return nombre;
            }
            if (messageErreurPersonnalise == null)
            {
                Console.WriteLine($"Le nombre doit etre entre {min} et {max}");
            }
            else
            {
                Console.WriteLine(messageErreurPersonnalise);
            }


            Console.WriteLine();
            return DemanderNombreEntre(question, min, max, messageErreurPersonnalise);



        }
        public static int DemanderNombre(string question)
        {
            Console.Write(question);
            int reponse = 0;
            while (true)
            {
                try
                {
                    reponse = int.Parse(Console.ReadLine());
                    return reponse;
                }
                catch
                {
                    Console.WriteLine("ERREUR : vous devez rentrer un nombre");
                    Console.WriteLine();
                    Console.Write(question);
                }
            }
        }


    }
}
