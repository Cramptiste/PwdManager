using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using static PswGest.outils;

namespace PswGest
{
    public class Identifiant
    {
        public string Pseudo { get; set; }
        public string Mdp { get; set; }

        public Identifiant(string pseudo, string mdp)
        {
            Pseudo = pseudo;
            Mdp = mdp;
        }

        public void GetId()
        {
            Console.WriteLine($"Utilisateur : {Pseudo}, Mot de passe : {Mdp}");
        }
    }

    public class Gestionnaire
    {
        public List<Identifiant> Ids { get; set; } = new List<Identifiant>();
        private static int nb = 0;

        public Gestionnaire()
        {
            nb++;
        }

        public void Ajouter()
        {
            string pseudo = "";
            string mdp = "";
            string choice = "";

            while (true)
            {
                Console.Write("Veuillez entrer un nom d'utilisateur : ");
                pseudo = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(pseudo))
                {
                    Console.WriteLine("Vous devez entrer quelque chose.");
                    continue;
                }
                Console.Write("Voulez vous generer un mot de passe (Y/N) : ");
                while (true)
                {
                    choice = Console.ReadLine();
                    if (choice == "Y")
                    {
                        mdp = GeneratePassword();
                        break;
                    }
                    else
                    {
                        Console.Write("Veuillez entrer un mot de passe : ");
                        mdp = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(mdp))
                        {
                            Console.WriteLine("Vous devez entrer quelque chose.");
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                break;
            }

            var id = new Identifiant(pseudo, mdp);

            try
            {
                Ids.Add(id);
                Console.WriteLine("Le nouvel identifiant a bien été ajouté.");
            }
            catch
            {
                Console.WriteLine("ERREUR : L'ajout a échoué ou l'identifiant existe déjà.");
            }
        }
        public string GeneratePassword()
        {
            const int NB_MOT_DE_PASSE = 1;
            int longueurMotDePasse = outils.DemanderNombrePositifNonNul("Longueur du mot de passe : ");

            string miniscules = "abdcefghijklmnopqrstuvwxyz";
            string majuscules = miniscules.ToUpper();
            string chiffres = "0123456789";
            string caracteresSpeciaux = "#&+-*/_@~.";
            string alphabet = "";
            while (true)
            {
                Console.WriteLine("Vous voulez un mot de passe avec : ");
                Console.WriteLine("1 - Uniquement des caractères en minuscules");
                Console.WriteLine("2 - Des caractères minuscules et majuscules");
                Console.WriteLine("3 - Des caractères et des chiffres");
                Console.WriteLine("4 - Caractères, chiffres et caractères spéciaux");
                int choix = outils.DemanderNombreEntre("Veuillez rentrer un nombre : ", 1, 4);
                try
                {
                    switch (choix)
                    {
                        case 1:
                            alphabet = miniscules;
                            break;
                        case 2:
                            alphabet = miniscules + majuscules;
                            break;
                        case 3:
                            alphabet = miniscules + majuscules + chiffres;
                            break;
                        case 4:
                            alphabet = miniscules + majuscules + chiffres + caracteresSpeciaux;
                            break;
                            //default:
                            //  continue;
                    }
                    break;
                }
                catch
                {
                    Console.WriteLine("vous devez rentrer un nombre");
                    Console.WriteLine();
                }
            }
            string motDePasse = "";
            int longueurAlphabet = alphabet.Length;
            Random rand = new Random();
            for (int j = 0; j < NB_MOT_DE_PASSE; j++)
            {
                for (int i = 0; i < longueurMotDePasse; i++)
                {
                    //Console.WriteLine(alphabet[index]);
                    int index = rand.Next(0, longueurAlphabet);
                    motDePasse += alphabet[index];
                }
                motDePasse += "\n";
            }
            return motDePasse;
        }

        public void AfficherLesId()
        {
            Console.WriteLine("Nombre d'utilisateurs : " + nb);
            foreach (var id in Ids)
            {
                id.GetId();
            }
            Console.WriteLine();
        }
    }

    class Program
    {
        static Identifiant CreateAccount()
        {
            string pseudo = "";
            string mdp = "";

            while (true)
            {
                Console.Write("Veuillez entrer un nom d'utilisateur : ");
                pseudo = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(pseudo))
                {
                    Console.WriteLine("Vous devez entrer quelque chose.");
                    continue;
                }

                Console.Write("Veuillez entrer un mot de passe : ");
                mdp = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(mdp))
                {
                    Console.WriteLine("Vous devez entrer quelque chose.");
                    continue;
                }

                return new Identifiant(pseudo, mdp);
            }
        }

        static void Choice(Gestionnaire gestionnaire)
        {
            var choix = "";

            while (true)
            {
                Console.WriteLine("1 - Ajouter un utilisateur");
                Console.WriteLine("2 - Afficher les utilisateurs");
                Console.WriteLine("3 - Quitter");
                choix = Console.ReadLine();

                if (choix == "1")
                {
                    gestionnaire.Ajouter();
                    WriteJson("data.json", gestionnaire);
                    continue;
                }
                else if (choix == "2")
                {
                    gestionnaire.AfficherLesId();
                    continue;
                }
                else if (choix == "3")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Veuillez choisir un nombre parmi les propositions.");
                    continue;
                }
            }
        }

        static void WriteJson(string filename, Gestionnaire gestionnaire)
        {
            var json = JsonConvert.SerializeObject(gestionnaire);
            File.WriteAllText(filename, json);
        }

        static Gestionnaire LoadGestionnaire(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    var json = File.ReadAllText(filename);
                    return JsonConvert.DeserializeObject<Gestionnaire>(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors de la lecture du fichier JSON : " + ex.Message);
                }
            }

            return new Gestionnaire();
        }

        
        static void Main(string[] args)
        {
            var filename = "data.json";
            var gestionnaire = LoadGestionnaire(filename);

            var compte = CreateAccount();
            var rpass = compte.Mdp;
            var pass = "";

            while (true)
            {
                Console.WriteLine("Entrez votre mot de passe");
                pass = Console.ReadLine();

                if (pass == rpass)
                {
                    Choice(gestionnaire);
                    break;
                }
                else
                {
                    Console.WriteLine("Mot de passe incorrect");
                    continue;
                }
            }
        }
    }
}
