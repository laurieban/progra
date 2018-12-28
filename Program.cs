﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_QUATRO_0
{
    class Program
    {
        public static void ChangerJoueur(ref int joueur) //ref pour modifier directement la valeur de joueur
        {
            if (joueur == 0)
            {
                joueur = 1;
            }
            else //dans le cas où joueur=1, car joueur appartient à {0,1}
            {
                joueur = 0;
            }
        }
        //FONCTION OK

        public static void ActualiserTableau(ref string[] tab, string element) //retire "element" du tableau "tab"
        {
            string[] tab_temp = new string[tab.Length - 1];
            int cpt = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] != element)
                {
                    tab_temp[cpt] = tab[i];
                    cpt++;
                }
            }
            tab = tab_temp;
        }
        //FONCTION OK

        public static bool TrouverValeur(string[] tab, string element) //retourne true si element est dans tab, false sinon
        {
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] == element)
                {
                    return true;
                }
            }
            return false;
        }
        //FONCTION OK

        public static void AfficherTableau(object[] tab)
        {
            for (int i = 0; i < tab.Length; i++)
            {
                Console.Write(tab[i] + "\t");
            }
            Console.WriteLine();
        }
        //TROUVER UN AFFICHAGE PLUS SYMPA POUR LISTER LES PIECES RESTANTES A L'UTILISATEUR

        public static void JouerUtilisateurAleatoire(ref string[,] plateau, ref string[] pieces, ref string[] cases, ref int joueur) //réalise un tour effectué par l'utilisateur
        {
            //Choix d'une pièce aléatoire dans le tableau pieces
            Random aleatoire = new Random();
            int indice = aleatoire.Next(pieces.Length);
            string piece = pieces[indice];

            //Choix d'une case pour placer la pièce par l'utilisateur parmi les cases disponibles
            string position;
            do
            {
                Console.Write("Vous devez placer la pièce ");
                RepresenterPiece(piece);
                Console.WriteLine(" sur le plateau. A quelle position souhaitez-vous la mettre ?");
                position = Console.ReadLine();
            }
            while (TrouverValeur(cases, position) == false);

            //On actualise les données: plateau, cases, pieces, joueur
            plateau[(int)char.GetNumericValue(position[0]), (int)char.GetNumericValue(position[1])] = piece;
            ActualiserTableau(ref cases, position);
            ActualiserTableau(ref pieces, piece);
            ChangerJoueur(ref joueur);
        }
        //FONCTION OK

        public static void JouerOrdinateurAleatoire(ref string[,] plateau, ref string[] pieces, ref string[] cases, ref int joueur) //réalise un tour effectué par l'utilisateur
        {
            //Choix par l'utilisateur d'une pièce que l'ordinateur doit placer parmi les pièces restantes
            int indice_piece; //indice de la pièce choisie par l'utilisateur pour l'ordinateur
            string piece=""; //piece choisie par l'utilisateur
            do
            {
                Console.WriteLine("Choisissez parmi les pièces suivantes celle que l'adversaire doit placer.");
                for(int i=0;i<pieces.Length;i++)
                {
                    Console.Write("{0} : ", i);
                    RepresenterPiece(pieces[i]);
                    Console.Write("\t");
                }
                Console.WriteLine();
                indice_piece = int.Parse(Console.ReadLine());
            }
            while (indice_piece<0 || indice_piece >= pieces.Length || TrouverValeur(pieces, pieces[indice_piece]) == false);

            piece = pieces[indice_piece];

            //Choix d'une position aléatoire dans le tableau cases
            Random aleatoire = new Random();
            int indice = aleatoire.Next(cases.Length);
            string position = cases[indice];

            //On actualise les données: plateau, cases, pieces, joueur
            plateau[(int)char.GetNumericValue(position[0]), (int)char.GetNumericValue(position[1])] = piece;
            ActualiserTableau(ref cases, position);
            ActualiserTableau(ref pieces, piece);
            ChangerJoueur(ref joueur);
        }
        //FONCTION OK

        public static void JouerUtilisateurIntelligent(ref string[,] plateau, ref string[] pieces, ref string[] cases, ref int joueur) //réalise un tour effectué par l'utilisateur
        {
            JouerUtilisateurAleatoire(ref plateau, ref pieces, ref cases, ref joueur);
        }
        //Comportement aléatoire en attendant de trouver la fonction intelligente

        public static void JouerOrdinateurIntelligent(ref string[,] plateau, ref string[] pieces, ref string[] cases, ref int joueur) //réalise un tour effectué par l'utilisateur
        {
            //JouerOrdinateurAleatoire(ref plateau, ref pieces, ref cases, ref joueur);

            //Choix par l'utilisateur d'une pièce que l'ordinateur doit placer parmi les pièces restantes
            int indice_piece; //indice de la pièce choisie par l'utilisateur pour l'ordinateur
            string piece = ""; //piece choisie par l'utilisateur
            do
            {
                Console.WriteLine("Choisissez parmi les pièces suivantes celle que l'adversaire doit placer.");
                for (int ind = 0; ind < pieces.Length; ind++) //ind parcourt le tableau pieces
                {
                    Console.Write("{0} : ", ind);
                    RepresenterPiece(pieces[ind]);
                    Console.Write("\t");
                }
                Console.WriteLine();
                indice_piece = int.Parse(Console.ReadLine());
            }
            while (indice_piece < 0 || indice_piece >= pieces.Length || TrouverValeur(pieces, pieces[indice_piece]) == false);

            piece = pieces[indice_piece];

            //On cherche si trois pièces ayant une même caractéristique sont alignées
            int[] place = { -1, -1 }; //place sert à donner, quand elle existe, la position (ligne, colonne) de la place vide qui permet d'aligner 4 pièces
            //Pour les lignes
            int i = 0; //parcourt les lignes
            int j; //parcourt les colonnes
            int k = 0; //parcourt les caractéristiques des pièces
            int cpt;
            int col;
            char carac;
            while (i < plateau.GetLength(0) && place[1] == -1) //place[0]==-1 --> on a pas trouvé de place "intelligente" ou placer la pièce
            {
                while (k < 4 && place[1] == -1)
                {
                    cpt = 0; //compte le nombre de pièces ayant la caractéristique k sur la ligne i
                    col = -1; //col représente la colonne vide de la ligne i quand trois pièces de même caractéristiques sont présentes sur cette ligne
                    carac = piece[k];
                    
                    for (j = 0; j < plateau.GetLength(1); j++)
                    {
                        if (plateau[i, j][k] == carac)
                        {
                            cpt++;
                        }
                        if (plateau[i, j][k] == '0')
                        {
                            col = j;
                        }
                    }
                    if (cpt == 3) //trois pièces de mêmes caractéristiques présentes sur la ligne i
                    {
                        place[0] = i;
                        place[1] = col;
                    }
                    k++;
                }
                i++;
            }
            Console.WriteLine("i:" + i); 
            Console.WriteLine("k:" + k);
            Console.WriteLine(piece[k - 1]);
            Console.WriteLine(place[0] +""+ place[1]);

            //On place la pièce sur le plateau de manière à gagner quand cela est possible, de manière aléatoire sinon
            string position;
            if (place[0] != -1)
            {
                plateau[place[0], place[1]] = piece;
                position = string.Format("{0}{1}",place[0],place[1]);
            }
            else
            {
                Random aleatoire = new Random();
                int indice = aleatoire.Next(cases.Length);
                position = cases[indice];
                plateau[(int)char.GetNumericValue(position[0]), (int)char.GetNumericValue(position[1])] = piece;
            }
            Console.WriteLine(position);
            
            //On actualise les données: cases, pieces, joueur
            ActualiserTableau(ref cases, position);
            ActualiserTableau(ref pieces, piece);
            ChangerJoueur(ref joueur);
        }
        //Gagne en un coup avec la piece donnée si la configuration du plateau le permet

        public static void Jouer(ref string[,] plateau, ref string[] pieces, ref string[] cases, ref int joueur, int niveau) //Réalise l'action pour un tour, selon si l'utilisateur ou l'ordinateur doit jouer, et selon le niveau (1:aléatoire ou 2:intelligent)
        {
            if (niveau == 1)
            {
                if (joueur == 0) //c'est au tour de l'ordinateur
                {
                    JouerOrdinateurAleatoire(ref plateau, ref pieces, ref cases, ref joueur);
                }
                else //c'est au tour de l'utilisateur
                {
                    JouerUtilisateurAleatoire(ref plateau, ref pieces, ref cases, ref joueur);
                }
            }
            else //Dans le cas du niveau 2: l'ordinateur joue intelligemment
            {
                if (joueur == 0) //c'est au tour de l'ordinateur
                {
                    JouerOrdinateurIntelligent(ref plateau, ref pieces, ref cases, ref joueur);
                }
                else //c'est au tour de l'utilisateur
                {
                    JouerUtilisateurIntelligent(ref plateau, ref pieces, ref cases, ref joueur);
                }
            }
        }

        public static bool TesterQuarto(string[,] plateau, out string quarto)
        {
            //On teste si quatre pièces avec des similitudes sont alignées sur une des lignes du plateau
            for (int i = 0; i < plateau.GetLength(0); i++) //i parcourt les lignes du plateau
            {
                for (int k = 0; k < 4; k++) //k parcourt les quatre caractères des string présents dans les cases du plateau
                {
                    int j = 1; //j parcourt les colonnes de la ligne i
                    while ((j < plateau.GetLength(1)) && (plateau[i, j][k] != '0') && (plateau[i, j - 1][k] == plateau[i, j][k]))
                    {
                        j++;
                    }
                    if (j == plateau.GetLength(1)) //la ligne i est composée de quatre pièces similaires sur le paramètre k : il y a QUARTO
                    {
                        quarto = "L" + i;
                        return true;
                    }
                }
            }

            //On teste si quatre pièces avec des similitudes sont alignées sur une des colonnes du plateau
            for (int j = 0; j < plateau.GetLength(1); j++) //j parcourt les colonnes du plateau
            {
                for (int k = 0; k < 4; k++) //k parcourt les caractères des string du plateau
                {
                    int i = 1; //i parcourt les lignes de la colone j
                    while ((i < plateau.GetLength(0)) && (plateau[i, j][k] != '0') && (plateau[i - 1, j][k] == plateau[i, j][k]))
                    {
                        i++;
                    }
                    if (i == plateau.GetLength(0)) //la colonne j est composée de quatre pièces similaires sur le paramètre k : il y a QUARTO
                    {
                        quarto = "C" + j;
                        return true;
                    }
                }
            }

            //On teste si quatre pièces avec des similitudes sont alignées sur la diagonale \ du plateau
            for (int k = 0; k < 4; k++) //k parcourt les caractères des string du plateau
            {
                int i = 1;
                while ((i < plateau.GetLength(0)) && (plateau[i, i][k] != '0') && (plateau[i - 1, i - 1][k] == plateau[i, i][k]))
                {
                    i++;
                }
                if (i == plateau.GetLength(0)) //la diagonale \ est composée de quatre pièces similaires sur le paramètre k : il y a QUARTO
                {
                    quarto = "D1";
                    return true;
                }
            }

            //On teste si quatre pièces avec des similitudes sont alignées sur la diagonale / du plateau
            for (int k = 0; k < 4; k++)
            {
                int i = 1;
                while ((i < plateau.GetLength(0)) && (plateau[i, 3 - i][k] != '0') && (plateau[i - 1, 3 - (i - 1)][k] == plateau[i, 3 - i][k]))
                {
                    i++;
                }
                if (i == plateau.GetLength(0)) //la diagonale / est composée de quatre pièces similaires sur le paramètre k : il y a QUARTO
                {
                    quarto = "D2";
                    return true;
                }
            }

            //Si aucun des cas ci-dessus n'est vrai, il n'y a pas QUARTO
            quarto = "";
            return false;
        }
        //FONTION OK

        public static void RepresenterPiece(string piece) //Affiche la représentation graphique de 'piece'
        {
            if (piece == "0000")
            {
                Console.Write("     ");
            }
            else
            {
                //Console.Write(plateau[i, j]);
                string representation = ""; //on crée les représentations de la forme des pièces
                switch (piece.Substring(0, 2))
                {
                    case "RT":
                        representation = "(O) ";
                        break;
                    case "RP":
                        representation = "( ) ";
                        break;
                    case "CT":
                        representation = "[O] ";
                        break;
                    case "CP":
                        representation = "[ ] ";
                        break;
                }
                representation = representation + piece[2]; //on ajoute la hauteur (H ou B) à la représentation de la pièce
                if (piece[3] == 'C') //si la pièce est de couleur claire (C), on la représente en cyan clair
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                else //si la pièce est de couleur foncée (F), on la représente en cyan foncé
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                Console.Write(representation);

                //Une fois la pièce affichée, on réinitialise la couleur d'écriture en la couleur d'origine
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static void AfficherPlateau(string[,] plateau)
        {
            Console.WriteLine("  \t0\t\t1\t\t2\t\t3\t");
            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                Console.WriteLine("   --------------------------------------------------------------");
                Console.Write(" {0} ",i);
                for (int j = 0; j < plateau.GetLength(1); j++)
                {
                    Console.Write("|\t");
                    RepresenterPiece(plateau[i, j]);
                    Console.Write("\t");
                }
                Console.WriteLine("|");  
            }
            Console.WriteLine("   --------------------------------------------------------------");
        }
        public static void AfficherQuarto(string[,] plateau, string quarto)
        {
            Console.WriteLine("  \t0\t\t1\t\t2\t\t3\t");
            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                Console.WriteLine("   --------------------------------------------------------------");
                Console.Write(" {0} ", i);
                for (int j = 0; j < plateau.GetLength(1); j++)
                {
                    Console.Write("|\t");
                    if ((quarto[0] == 'L' && i == (int)char.GetNumericValue(quarto[1])) || (quarto[0] == 'C' && j == (int)char.GetNumericValue(quarto[1])) || (quarto == "D1" && i == j) || (quarto == "D2" && i == 3 - j))
                    {
                        Console.BackgroundColor = ConsoleColor.White; //DarkBlue DarkRed --> revoir les couleurs ?
                        RepresenterPiece(plateau[i, j]);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        RepresenterPiece(plateau[i, j]);
                    }
                    Console.Write("\t");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("   --------------------------------------------------------------");
            }

        static void Main(string[] args)
        {
            Random aleatoire = new Random();

            //Initialisation du plateau, des pièces et des positions
            string[] pieces = new string[] { "RPHC", "RPBC", "RTHC", "RTHF", "RPHF", "RPBF", "RTBC", "RTBF", "CPHC", "CPBC", "CTHC", "CTHF", "CPHF", "CPBF", "CTBC", "CTBF" }; //tableau contenant les pieces restantes à placer
            //string[] representation = new string[] { "( ) HC", "( ) BC", "(O) HC", "(O) HF", "( ) BF","(O) BC","[ ] HC", }
            string[] cases = new string[] { "00", "01", "02", "03", "10", "11", "12", "13", "20", "21", "22", "23", "30", "31", "32", "33" }; //tableau contenant les cases libres du plateau
            //Initialisation du plateau: tableau 4x4 avec des "0000"                                                                                                                             
            string[,] plateau = new string[4, 4];
            for (int i = 0; i<plateau.GetLength(0); i++)
            {
                for (int j = 0; j<plateau.GetLength(1); j++)
                {
                    plateau[i, j] = "0000";
                }
            }

            //Choix du niveau: aléatoire ou intelligent
            Console.WriteLine("Choisissez votre niveau: \n1 : l'ordinateur joue aléatoirement \n2 : l'ordinateur joue intelligemment");
            int niveau = int.Parse(Console.ReadLine()); //variable permettant le choix du niveau par l'utilisateur

            //Début du jeu
            int joueur; //variable qui indique le joueur qui va jouer : 0 correspond à l'ordinateur, et 1 à l'utilisateur du programme
            joueur = aleatoire.Next(2); //
            if (joueur == 0)
            {
                Console.WriteLine("L'adversaire commence à placer une pièce.");
            }
            else //joueur = 1
            {
                Console.WriteLine("Vous commencez à placer une pièce.");
            }
            AfficherPlateau(plateau);

            //Déroulement de la partie
            string quarto = "";
            while ((TesterQuarto(plateau, out quarto) == false) && (pieces.Length > 0))
            {
                Jouer(ref plateau, ref pieces, ref cases, ref joueur, niveau);
                AfficherPlateau(plateau);
            }

            //Fin de la partie
            Console.WriteLine("\n-------------------------------------------------------------------------------\n");
            Console.ForegroundColor = ConsoleColor.White;
            if (TesterQuarto(plateau, out quarto) == true)
            {
                ChangerJoueur(ref joueur);
                if (joueur == 0)
                {
                    Console.WriteLine("QUARTO ! VOUS AVEZ PERDU !\n");
                }
                else //joueur =1
                {
                    Console.WriteLine("QUARTO ! VOUS AVEZ GAGNE !\n");
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                AfficherQuarto(plateau, quarto);
            }
            else //si le tableau contenant les pieces est vide
            {
                Console.WriteLine("FIN DE LA PARTIE : EGALITE !\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            

            //A FAIRE
            //Trouver une seule fonction pour AfficherPlateau et AfficherQuarto
            //Jeu intelligent
        }
    }
}