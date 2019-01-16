using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_QUATRO_0
{
    class Program
    {
        static private Random _rng = new Random();

        /// <summary>
        /// Change de joueur (0 lorsque c'est à l'ordinateur de jouer, 1 lorsque c'est à l'utilisateur)
        /// </summary>
        /// <param name="joueur"></param>
        public static void ChangerJoueur(ref int joueur)
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

        /// <summary>
        /// Retire element du tableau tab
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="element"></param>
        public static void ActualiserTableau(ref string[] tab, string element)
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

        /// <summary>
        /// Retourne true si element est dans le tableau tab, false sinon
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool TrouverValeur(string[] tab, string element)
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

        /// <summary>
        /// Affiche à l'utilisateur la liste des pièces qu'il reste
        /// </summary>
        /// <param name="pieces"></param>
        public static void ListerPieces(string[] pieces)
        {
            int cpt = 0;
            for (int i = 0; i < pieces.Length; i++)
            {
                Console.Write(" {0} : ", i);
                RepresenterPiece(pieces[i]);
                Console.Write("\t");
                cpt++;
                if (cpt == 5)
                {
                    cpt = 0;
                    Console.WriteLine("\n");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Choisit aléatoirement piece parmi le tableau pieces
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="pieces"></param>
        /// <param name="cases"></param>
        /// <param name="joueur"></param>
        /// <returns></returns>
        public static string ChoisirPieceAleatoire(string[] pieces)
        {
            //Choix d'une pièce aléatoire dans le tableau pieces
            int indice = _rng.Next(pieces.Length);
            string piece = pieces[indice];
            return piece;
        }

        /// <summary>
        /// Place aléatoire l'élément piece sur le plateau
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="pieces"></param>
        /// <param name="cases"></param>
        /// <param name="joueur"></param>
        /// <param name="piece"></param>
        public static void PlacerAleatoire(ref string[,] plateau, ref string[] pieces, ref string[] cases, ref int joueur, string piece)
        {
            //Choix d'une position aléatoire dans le tableau cases pour placer piece
            int indice = _rng.Next(cases.Length);
            string position = cases[indice];

            //On actualise les données: plateau, cases, pieces
            plateau[(int)char.GetNumericValue(position[0]), (int)char.GetNumericValue(position[1])] = piece;
            ActualiserTableau(ref cases, position);
            ActualiserTableau(ref pieces, piece);
        }

        /// <summary>
        /// Supprime les pièces de piecesPossibles qui permettraient à l'utilisateur de faire un quarto en un coup sur les lignes
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="piecesPossibles"></param>
        public static void ChoisirPieceLignes(string[,] plateau, ref string[] piecesPossibles)
        {
            int cptCaracteristique; //compte le nombre de pièces alignées ayant une même caractéristique
            int cptCaseVide; //compte le nombre de cases vides sur l'alignement testé (lignes, colonnes, diagonales)
            char carac;
            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    cptCaracteristique = 0;
                    cptCaseVide = 0;
                    carac = ' ';
                    for (int j = 0; j < plateau.GetLength(1); j++)
                    {
                        if (plateau[i, j][k] == '0')
                        {
                            cptCaseVide++;
                        }
                        else
                        {
                            if (carac == ' ')
                            {
                                carac = plateau[i, j][k];
                            }
                            if (plateau[i, j][k] == carac)
                            {
                                cptCaracteristique++;
                            }
                        }
                    }
                    if (cptCaracteristique == 3 && cptCaseVide == 1)
                    {
                        for (int indice = 0; indice < piecesPossibles.Length; indice++) //on élimine les piece qui possède la caractéristique carac
                        {
                            if (piecesPossibles[indice] != null && piecesPossibles[indice][k] == carac)
                            {
                                piecesPossibles[indice] = null;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Supprime les pièces de piecesPossibles qui permettraient à l'utilisateur de faire un quarto en un coup sur les colonnes
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="piecesPossibles"></param>
        public static void ChoisirPieceColonnes(string[,] plateau, ref string[] piecesPossibles)
        {
            int cptCaracteristique, cptCaseVide;
            char carac;
            for (int j = 0; j < plateau.GetLength(1); j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    cptCaracteristique = 0;
                    cptCaseVide = 0;
                    carac = ' ';
                    for (int i = 0; i < plateau.GetLength(0); i++)
                    {
                        if (plateau[i, j][k] == '0')
                        {
                            cptCaseVide++;
                        }
                        else
                        {
                            if (carac == ' ')
                            {
                                carac = plateau[i, j][k];
                            }
                            if (plateau[i, j][k] == carac)
                            {
                                cptCaracteristique++;
                            }
                        }
                    }
                    if (cptCaracteristique == 3 && cptCaseVide == 1)
                    {
                        for (int indice = 0; indice < piecesPossibles.Length; indice++) //on élimine les piece qui possède la caractéristique carac
                        {
                            if (piecesPossibles[indice] != null && piecesPossibles[indice][k] == carac)
                            {
                                piecesPossibles[indice] = null;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Supprime les pièces de piecesPossibles qui permettraient à l'utilisateur de faire un quarto en un coup sur la diagonale \ lorsque diagonale=true, et sur la diagonale / lorsque diagonale=false
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="piecesPossibles"></param>
        /// <param name="diagonale"></param>
        public static void ChoisirPieceDiagonales(string[,] plateau, ref string[] piecesPossibles, bool diagonale)
        {
            int cptCaracteristique, cptCaseVide, indice;
            char carac;
            for (int k = 0; k < 4; k++)
            {
                cptCaracteristique = 0;
                cptCaseVide = 0;
                carac = ' ';
                for (int i = 0; i < plateau.GetLength(0); i++)
                {
                    if (diagonale == true) //on s'occupe de la diagonale \
                    {
                        indice = i;
                    }
                    else //false --> on s'occupe de la diagonale /
                    {
                        indice = 3 - i;
                    }
                    if (plateau[i, indice][k] == '0')
                    {
                        cptCaseVide++;
                    }
                    else
                    {
                        if (carac == ' ')
                        {
                            carac = plateau[i, indice][k];
                        }
                        if (plateau[i, indice][k] == carac)
                        {
                            cptCaracteristique++;
                        }
                    }
                    if (cptCaracteristique == 3 && cptCaseVide == 1)
                    {
                        for (indice = 0; indice < piecesPossibles.Length; indice++) //on élimine les piece qui possède la caractéristique carac
                        {
                            if (piecesPossibles[indice] != null && piecesPossibles[indice][k] == carac)
                            {
                                piecesPossibles[indice] = null;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retourne une piece qui ne permet pas à l'utilisateur de faire quarto, lorsque cela est possible
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="pieces"></param>
        /// <param name="cases"></param>
        /// <param name="joueur"></param>
        /// <returns></returns>
        public static string ChoisirPieceIntelligent(string[,] plateau, string[] pieces, string[] cases, ref int joueur) 
        {
            //On recopie le tableau pieces dans piecesPossibles
            string[] piecesPossibles = new string[pieces.Length];
            int indice;
            for (indice = 0; indice < piecesPossibles.Length; indice++)
            {
                piecesPossibles[indice] = pieces[indice];
            }

            //On élimine les pièces qui permettraient à l'utilisateur de faire un quarto sur les lignes
            ChoisirPieceLignes(plateau, ref piecesPossibles);

            //On élimine les pièces qui permettraient à l'utilisateur de faire un quarto sur les colonnes
            ChoisirPieceColonnes(plateau, ref piecesPossibles);

            //On élimine les pièces qui permettraient à l'utilisateur de faire un quarto sur les diagonales
            //Pour la diagonale \
            bool diagonale = true;
            ChoisirPieceDiagonales(plateau, ref piecesPossibles, diagonale);
            //Pour la diagonale /
            diagonale = false;
            ChoisirPieceDiagonales(plateau, ref piecesPossibles, diagonale);

            //Choix de la pièce que l'ordinateur donne à l'utilisateur, intelligemment si possible, aléatoire sinon
            return ChoisirValeurNonNulle(piecesPossibles, pieces);
        }

        /// <summary>
        /// Choisis aléatoirement une valeur différente de null dans piecesPossibles lorsqu'il y en a, ou aléatoirement dans pieces sinon
        /// </summary>
        /// <param name="piecesPossibles"></param>
        /// <param name="pieces"></param>
        /// <returns></returns>
        public static string ChoisirValeurNonNulle(string[] piecesPossibles, string[] pieces)
        {
            int indice;
            int cpt = 0;
            for (indice = 0; indice < piecesPossibles.Length; indice++)
            {
                if (piecesPossibles[indice] != null)
                {
                    cpt++;
                }
            }
            if (cpt != 0)
            {
                string[] tab = new string[cpt];
                cpt = 0;
                for (indice = 0; indice < piecesPossibles.Length; indice++)
                {
                    if (piecesPossibles[indice] != null)
                    {
                        tab[cpt] = piecesPossibles[indice];
                        cpt++;
                    }
                }
                indice = _rng.Next(tab.Length);
                return tab[indice];
            }
            else //aucune pièce n'est présente dans piecesPossibles, on choisit une piece à donner aléatoirement dans pieces
            {
                indice = _rng.Next(pieces.Length);
                return pieces[indice];
            }
        }

        /// <summary>
        /// Retourne les coordonnées qui permettent de gagner avec piece sur une ligne si elles existent
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="place"></param>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static void PlacerIntelligentLigne(string[,] plateau, ref int[] place, string piece)
        {
            int i=0;
            int k = 0;
            int cpt, col;
            char carac;
            while (i < plateau.GetLength(0) && place[0] == -1) //on a pas trouvé de place "intelligente" ou placer la pièce
            {
                while (k < 4)
                {
                    cpt = 0; //compte le nombre de pièces ayant la caractéristique k sur la ligne i
                    col = -1; //col représente la colonne vide de la ligne i quand trois pièces de même caractéristiques sont présentes sur cette ligne
                    carac = piece[k];
                    for (int j = 0; j < plateau.GetLength(1); j++)
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
                    if (cpt == 3 && col != -1)  //trois pièces de mêmes caractéristiques présentes sur la ligne i et une place disponible pour aligner la pièce
                    {
                        place[0] = i;
                        place[1] = col;
                    }
                    k++;
                }
                i++;
                k = 0;
            }
        }

        /// <summary>
        /// Retourne les coordonnées qui permettent de gagner avec piece sur une colonne si elles existent
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="place"></param>
        /// <param name="piece"></param>
        /// <returns></returns>
        public static void PlacerIntelligentColonne(string[,] plateau, ref int[] place, string piece)
        {
            int j = 0;
            int k = 0;
            int cpt, ligne;
            char carac;
            while (j < plateau.GetLength(1))
            {
                while (k < 4 && place[0] == -1)
                {
                    cpt = 0; //compte le nombre de pièces ayant la caractéristique k sur la colonne j
                    ligne = -1; //ligne représente la ligne vide de la colonne j quand trois pièces de même caractéristiques sont présentes sur cette colonne
                    carac = piece[k];
                    for (int i = 0; i < plateau.GetLength(0); i++)
                    {
                        if (plateau[i, j][k] == carac)
                        {
                            cpt++;
                        }
                        if (plateau[i, j][k] == '0')
                        {
                            ligne = i;
                        }
                    }
                    if (cpt == 3 && ligne != -1) //trois pièces de mêmes caractéristiques présentes sur la colonne j
                    {
                        place[0] = ligne;
                        place[1] = j;
                    }
                    k++;
                }
                j++;
                k = 0;
            }
        }

        /// <summary>
        /// Retourne les coordonnées qui permettent de gagner avec piece sur une diagonale si elles existent. diagonale=true traite la diagonale \ et diagonale=false traite la diagonale /
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="place"></param>
        /// <param name="piece"></param>
        /// <param name="diagonale"></param>
        public static void PlacerIntelligentDiagonale(string[,] plateau, ref int[] place, string piece, bool diagonale)
        {
            int k = 0;
            int cpt, ligne;
            int indice = 0;
            char carac;
            while (k < 4 && place[0] == -1) //place[0]==-1 --> on a pas trouvé de place "intelligente" ou placer la pièce
            {
                cpt = 0; //compte le nombre de pièces ayant la caractéristique k sur la diagonale 
                ligne = -1; 
                carac = piece[k];
                for (int i = 0; i < plateau.GetLength(0); i++)
                {
                    if (diagonale==true) //on traite la diagonale \
                    {
                        indice = i;
                    }
                    else //on traite la diagonale /
                    {
                        indice = 3 - i;
                    }
                    if (plateau[i, indice][k] == carac)
                    {
                        cpt++;
                    }
                    if (plateau[i, indice][k] == '0')
                    {
                        ligne = i;
                    }
                }
                if (cpt == 3 && ligne != -1) //trois pièces de mêmes caractéristiques présentes sur la diagonale 
                {
                    place[0] = ligne;
                    place[1] = indice;
                }
                k++;
            }
        }
        /// <summary>
        /// Gagne en un coup avec la piece donnée si la configuration du plateau le permet
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="pieces"></param>
        /// <param name="cases"></param>
        /// <param name="joueur"></param>
        /// <param name="piece"></param>
        public static void PlacerIntelligent(ref string[,] plateau, ref string[] pieces, ref string[] cases, string piece) 
        {
            //On cherche si trois pièces ayant une même caractéristique sont alignées
            int[] place = { -1, -1 }; //place sert à donner, quand elle existe, la position (ligne, colonne) de la place vide qui permet d'aligner 4 pièces
            
            //On cherche à faire un quarto sur une des lignes
            PlacerIntelligentLigne(plateau, ref place, piece);

            //On cherche à faire un quarto sur une des colonnes
            PlacerIntelligentColonne(plateau, ref place, piece);


            //On cherche à faire un quarto sur la diagonale \
            bool diagonale = true;
            PlacerIntelligentDiagonale(plateau, ref place, piece, diagonale);

            //On cherche à faire un quarto sur la diagonale /
            diagonale = false;
            PlacerIntelligentDiagonale(plateau, ref place, piece, diagonale);

            //On place la pièce sur le plateau de manière à gagner quand cela est possible, de manière aléatoire sinon
            string position;
            if (place[0] != -1) //cas où il y a une case qui permet à l'ordinateur de gagner
            {
                plateau[place[0], place[1]] = piece;
                position = string.Format("{0}{1}", place[0], place[1]);
                Console.WriteLine(position);
            }
            else //si aucune case ne permet à l'ordinateur de gagner, on choisit une position aléatoire
            {
                int indice = _rng.Next(cases.Length);
                position = cases[indice];
                plateau[(int)char.GetNumericValue(position[0]), (int)char.GetNumericValue(position[1])] = piece;
            }

            //On actualise les données: cases, pieces
            ActualiserTableau(ref cases, position);
            ActualiserTableau(ref pieces, piece);
        }

        /// <summary>
        /// Réalise l'action pour un tour, selon si l'utilisateur ou l'ordinateur doit jouer, et selon le niveau (1:aléatoire ou 2:intelligent)
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="pieces"></param>
        /// <param name="cases"></param>
        /// <param name="joueur"></param>
        /// <param name="niveau"></param>
        public static void Jouer(ref string[,] plateau, ref string[] pieces, ref string[] cases, ref int joueur, int niveau)
        {
            if (joueur == 1) //si c'est au tour de l'utilisateur de jouer
            {
                //Choix de la pièce par l'ordinateur
                string piece;
                if (niveau == 1) //cas du niveau aléatoire
                {
                    piece = ChoisirPieceAleatoire(pieces);
                }
                else //cas du niveau intelligent
                {
                    piece = ChoisirPieceIntelligent(plateau, pieces, cases, ref joueur);
                }
                //Choix d'une case pour placer la pièce par l'utilisateur parmi les cases disponibles
                string position;
                do
                {
                    Console.Write("\n L'ordinateur a choisi la pièce ");
                    RepresenterPiece(piece);
                    Console.WriteLine(" pour vous. A quelle position souhaitez-vous la mettre ? \n (pour la case à la ligne i et la colonne j, entrez : ij)");
                    position = Console.ReadLine();
                }
                while (TrouverValeur(cases, position) == false);
                //On actualise les données: plateau, cases, pieces, joueur
                plateau[(int)char.GetNumericValue(position[0]), (int)char.GetNumericValue(position[1])] = piece;
                ActualiserTableau(ref cases, position);
                ActualiserTableau(ref pieces, piece);
            }
            else //si c'est au tour de l'ordinateur de jouer
            {
                //Choix par l'utilisateur d'une pièce que l'ordinateur doit placer parmi les pièces restantes
                int indice_piece; //indice de la pièce choisie par l'utilisateur pour l'ordinateur
                string piece = "";
                do
                {
                    Console.WriteLine("\n Choisissez parmi les pièces suivantes celle que l'ordinateur va devoir placer: \n (entrez le numéro situé en face de la pièce voulue)\n");
                    ListerPieces(pieces);
                    indice_piece = int.Parse(Console.ReadLine());
                }
                while (indice_piece < 0 || indice_piece >= pieces.Length || TrouverValeur(pieces, pieces[indice_piece]) == false);
                piece = pieces[indice_piece];
                if (niveau == 1) //Cas du niveau aléatoire
                {
                    PlacerAleatoire(ref plateau, ref pieces, ref cases, ref joueur, piece);
                }
                else //Cas du niveau intelligent
                {
                    PlacerIntelligent(ref plateau, ref pieces, ref cases, piece);
                }
            }
            //On passe au joueur suivant
            ChangerJoueur(ref joueur);
            Console.Clear();
        }

        /// <summary>
        /// Retourne true s'il y a quarto sur le plateau, false sinon. En cas de quarto, indique sa position grâce à la variable quarto (Li pour la ligne i, Cj pour la colonne j, D1 pour la diagonale \ et D2 pour la diagonale /)
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="quarto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Affiche la représentation graphique de 'piece'
        /// </summary>
        /// <param name="piece"></param>
        public static void RepresenterPiece(string piece)
        {
            if (piece == "0000")
            {
                Console.Write("     ");
            }
            else
            {
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
                if (piece[3] == 'C') //si la pièce a la caractéristique "claire" claire (C), on la représente en rouge
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else //si la pièce a la caractéristique "foncée" (F), on la représente en bleu
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                Console.Write(representation);
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
        }
        
        /// <summary>
        /// Affichage du plateau et des pièces qu'il contient
        /// </summary>
        /// <param name="plateau"></param>
        public static void AfficherPlateau(string[,] plateau)
        {
            Console.WriteLine("  \t0\t\t1\t\t2\t\t3\t");
            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                Console.WriteLine("   --------------------------------------------------------------");
                Console.Write(" {0} ", i);
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

        /// <summary>
        /// Affiche le plateau avec le quarto mis en évidence
        /// </summary>
        /// <param name="plateau"></param>
        /// <param name="quarto"></param>
        public static void AfficherQuarto(string[,] plateau, string quarto)
        {
            Console.WriteLine("\n\n\n  \t0\t\t1\t\t2\t\t3\t");
            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                Console.WriteLine("   --------------------------------------------------------------");
                Console.Write(" {0} ", i);
                for (int j = 0; j < plateau.GetLength(1); j++)
                {
                    Console.Write("|\t");
                    if ((quarto[0] == 'L' && i == (int)char.GetNumericValue(quarto[1])) || (quarto[0] == 'C' && j == (int)char.GetNumericValue(quarto[1])) || (quarto == "D1" && i == j) || (quarto == "D2" && i == 3 - j))
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        RepresenterPiece(plateau[i, j]);
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        RepresenterPiece(plateau[i, j]);
                    }
                    Console.Write("\t");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("   --------------------------------------------------------------\n");
        }

        /// <summary>
        /// Affiche l'en-tête et les règles du jeu au début de la partie
        /// </summary>
        public static void AfficherDebutPartie()
        {
            Console.WriteLine();
            Console.WriteLine("     ,o888888o.     8 8888      88        .8.          8 888888888o. 8888888 8888888888 ,o888888o.     ");
            Console.WriteLine("  . 8888     `88.   8 8888      88       .888.         8 8888    `88.      8 8888    . 8888     `88.   ");
            Console.WriteLine(" ,8 8888       `8b  8 8888      88      :88888.        8 8888     `88      8 8888   ,8 8888       `8b  ");
            Console.WriteLine(" 88 8888        `8b 8 8888      88     . `88888.       8 8888     ,88      8 8888   88 8888        `8b ");
            Console.WriteLine(" 88 8888         88 8 8888      88    .8. `88888.      8 8888.   ,88'      8 8888   88 8888         88 ");
            Console.WriteLine(" 88 8888     `8. 88 8 8888      88   .8`8. `88888.     8 888888888P'       8 8888   88 8888         88 ");
            Console.WriteLine(" 88 8888      `8,8P 8 8888      88  .8' `8. `88888.    8 8888`8b           8 8888   88 8888        ,8P ");
            Console.WriteLine(" `8 8888       ;8P  ` 8888     ,8P .8'   `8. `88888.   8 8888 `8b.         8 8888   `8 8888       ,8P  ");
            Console.WriteLine("  ` 8888     ,88'8.   8888   ,d8P .888888888. `88888.  8 8888   `8b.       8 8888    ` 8888     ,88'   ");
            Console.WriteLine("     `8888888P'  `8.   `Y88888P' .8'       `8. `88888. 8 8888     `88.     8 8888       `8888888P'     ");
            Console.WriteLine("\n\n");

            //Règles
            Console.WriteLine(" Le jeu est composé de 16 pièces, toutes différentes, ayant 4 caractéristiques chacunes : \n\t- ronde ou carrée \n\t- pleine ou creusée \n\t- haute ou basse \n\t- rouge ou bleue\n");
            Console.WriteLine(" Le but du jeu est de créer un alignement de 4 pièces ayant au moins une caractéristique en commun, sur \n une ligne, une colonne ou une diagonale.");
            Console.WriteLine(" Chacun à son tour choisit et donne une pièce à l'adversaire, qui doit la placer sur une case libre.");
            Console.WriteLine(" Le joueur qui commence est tiré au sort.\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Commençons la partie ! \n");
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        /// <summary>
        /// Affiche le résultat de la partie : Gagné, perdu, ou égalité
        /// </summary>
        /// <param name="joueur"></param>
        /// <param name="fin"></param>
        public static void AfficherFinPartie(int joueur, string fin)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (fin == "egalite")
            {
                Console.WriteLine(@"  _____            _ _ _    __   _ ");
                Console.WriteLine(@" | ____|__ _  __ _| (_) |_ /_/  | |");
                Console.WriteLine(@" |  _| / _` |/ _` | | | __/ _ \ | |");
                Console.WriteLine(@" | |__| (_| | (_| | | | ||  __/ |_|");
                Console.WriteLine(@" |_____\__, |\__,_|_|_|\__\___| (_)");
                Console.WriteLine(@"       |___/                       ");
            }
            else
            {
                if (joueur == 0)
                {
                    Console.WriteLine(@"  ____              _         _ ");
                    Console.WriteLine(@" |  _ \ ___ _ __ __| |_   _  | |");
                    Console.WriteLine(@" | |_) / _ \ '__/ _` | | | | | |");
                    Console.WriteLine(@" |  __/  __/ | | (_| | |_| | |_|");
                    Console.WriteLine(@" |_|   \___|_|  \__,_|\__,_| (_)");
                    Console.WriteLine();

                }
                else 
                {
                    Console.WriteLine(@"   ____                     __   _ ");
                    Console.WriteLine(@"  / ___| __ _  __ _ _ __   /_/  | |");
                    Console.WriteLine(@" | |  _ / _` |/ _` | '_ \ / _ \ | |");
                    Console.WriteLine(@" | |_| | (_| | (_| | | | |  __/ |_|");
                    Console.WriteLine(@"  \____|\__,_|\__, |_| |_|\___| (_)");
                    Console.WriteLine(@"              |___/                ");
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGray; 
        }
        static void Main(string[] args)
        {
            //Initialisation du plateau, des pièces et des positions
            string[] pieces = new string[] { "RPHC", "RPBC", "RTHC", "RTHF", "RPHF", "RPBF", "RTBC", "RTBF", "CPHC", "CPBC", "CTHC", "CTHF", "CPHF", "CPBF", "CTBC", "CTBF" }; //tableau contenant les pieces restantes à placer
            string[] cases = new string[] { "00", "01", "02", "03", "10", "11", "12", "13", "20", "21", "22", "23", "30", "31", "32", "33" }; //tableau contenant les cases libres du plateau
            //Initialisation du plateau: tableau 4x4 avec des "0000"                                                                                                                             
            string[,] plateau = new string[4, 4];
            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                for (int j = 0; j < plateau.GetLength(1); j++)
                {
                    plateau[i, j] = "0000";
                }
            }

            //Paramétrer la taille de la fenêtre et les couleurs
            Console.SetWindowSize(105, 40);
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;

            //Présentation du jeu
            AfficherDebutPartie();

            //Choix du niveau
            int niveau;
            do
            {
                Console.WriteLine(" Choisissez le niveau: (entrez 1 ou 2) \n\n\t- Niveau 1 : l'ordinateur joue aléatoirement \n\t- Niveau 2 : l'ordinateur joue intelligemment");
                niveau = int.Parse(Console.ReadLine()); 
            }
            while (niveau != 1 && niveau != 2);
            Console.Clear();

            //Choix aléatoire du premier joueur: 0 correspond à l'ordinateur, et 1 à l'utilisateur du programme
            int joueur; 
            joueur = _rng.Next(2);

            //Début de la partie
            Console.ForegroundColor = ConsoleColor.Red;
            if (joueur == 0)
            {
                Console.WriteLine("\n L'adversaire commence à placer une pièce.\n");
            }
            else
            {
                Console.WriteLine("\n Vous commencez à placer une pièce.\n");
            }
            Console.ForegroundColor = ConsoleColor.DarkGray; 

            AfficherPlateau(plateau);

            //Déroulement de la partie
            string quarto = "";
            while ((TesterQuarto(plateau, out quarto) == false) && (pieces.Length > 0))
            {
                Jouer(ref plateau, ref pieces, ref cases, ref joueur, niveau);
                Console.WriteLine("\n\n");
                AfficherPlateau(plateau);
            }

            //Fin de la partie
            string fin = "";
            if (TesterQuarto(plateau, out quarto) == true)
            {
                ChangerJoueur(ref joueur);
                Console.Clear();
                AfficherQuarto(plateau, quarto);
                AfficherFinPartie(joueur, fin);
            }
            else
            {
                fin = "egalite";
                AfficherFinPartie(joueur, fin);
            }
        }
    }
}

