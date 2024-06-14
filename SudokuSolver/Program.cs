using System.Collections.Generic;
using System.Data.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SudokuSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {

            static bool inputValidation(string[,] input)
            {
                string[] validCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "." };

                if (input.GetLength(0) != 9 || input.GetLength(1) != 9)
                {
                    return false;
                }

                for (int i = 0; i < input.GetLength(0); i++)
                {
                    for (int j = 0; j < input.GetLength(1); j++)
                    {
                        for (int k = 0; k < validCharacters.Length; k++)
                        {
                            if (input[i, j] == validCharacters[k])
                            {
                                break;
                            }
                            else if (k == validCharacters.Length - 1)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }

            static List<string> deductPossibleNumbers(List<string> impossibleNumbers)
            {
                string[] numbers = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                List<string> possibleNumbers = new List<string>();
                for (int i = 0; i < numbers.Length; i++)
                {
                    for (int j = 0; j < impossibleNumbers.Count; j++)
                    {
                        if (numbers[i] == impossibleNumbers[j])
                        {
                            break;
                        }
                        else if (j == impossibleNumbers.Count - 1)
                        {
                            possibleNumbers.Add(numbers[i]);
                        }
                    }
                }
                return possibleNumbers;
            }

            static List<string> getRowNumbers(int row, int column, string[,] sudokuArray)
            {
                List<string> numbersFound = new List<string>();

                for (int i = 0; i < sudokuArray.GetLength(1); i++)
                {
                    if (sudokuArray[row, i] != ".")
                    {
                        numbersFound.Add(sudokuArray[row, i]);
                    }
                }

                return numbersFound;
            }

            static List<string> getColumnNumbers(int row, int column, string[,] sudokuArray)
            {
                List<string> numbersFound = new List<string>();

                for (int i = 0; i < sudokuArray.GetLength(0); i++)
                {
                    if (sudokuArray[i, column] != ".")
                    {
                        numbersFound.Add(sudokuArray[i, column]);
                    }
                }

                return numbersFound;
            }

            static List<string> getSquareNumbers(int row, int column, string[,] sudokuArray)
            {
                List<string> numbersFound = new List<string>();
                int squareRow = (row / 3) * 3;
                int squareColumn = (column / 3) * 3;

                for (int i = squareRow; i < squareRow + 3; i++)
                {
                    for (int j = squareColumn; j < squareColumn + 3; j++)
                    {
                        if (sudokuArray[i, j] != ".")
                        {
                            numbersFound.Add(sudokuArray[i, j]);
                        }
                    }
                }
                return numbersFound;
            }

            static List<EmptyCase> emptyCasesListDefiner(string[,] sudokuArray)
            {
                List<EmptyCase> emptyCasesList = new List<EmptyCase>();

                for (int i = 0; i < sudokuArray.GetLength(0); i++)
                {
                    for (int j = 0; j < sudokuArray.GetLength(1); j++)
                    {
                        if (sudokuArray[i, j] == ".")
                        {
                            List<string> impossibleNumbers = new List<string>();
                            impossibleNumbers.AddRange(getRowNumbers(i, j, sudokuArray));
                            impossibleNumbers.AddRange(getColumnNumbers(i, j, sudokuArray));
                            impossibleNumbers.AddRange(getSquareNumbers(i, j, sudokuArray));
                            impossibleNumbers = impossibleNumbers.Distinct().ToList();
                            List<string> possibleNumbers = deductPossibleNumbers(impossibleNumbers);
                            emptyCasesList.Add(new EmptyCase([i, j], impossibleNumbers, possibleNumbers, 0));
                        }
                    }
                }
                return emptyCasesList;
            }

            static bool isNumberExistsInTheRow(int[] coordinates, string value, string[,] sudokuArray)
            {
                for (int i = 0; i < sudokuArray.GetLength(1); i++)
                {
                    if (coordinates[1] != i && value == sudokuArray[coordinates[0], i])
                    {
                        return true;
                    }
                }
                return false;
            }

            static bool isNumberExistsInTheColumn(int[] coordinates, string value, string[,] sudokuArray)
            {
                for (int i = 0; i < sudokuArray.GetLength(0); i++)
                {
                    if (coordinates[0] != i && value == sudokuArray[i, coordinates[1]])
                    {
                        return true;
                    }
                }
                return false;
            }

            static bool isNumberExistsInThe3x3(int[] coordinates, string value, string[,] sudokuArray)
            {
                int squareRow = (coordinates[0] / 3) * 3;
                int squareColumn = (coordinates[1] / 3) * 3;

                for (int i = squareRow; i < squareRow + 3; i++)
                {
                    for (int j = squareColumn; j < squareColumn + 3; j++)
                    {
                        if ((coordinates[0] != i || coordinates[1] != j) && sudokuArray[i, j] == value)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            static bool isNumberExistsInTheRowColumn3x3(int[] coordinates, string value, string[,] sudokuArray)
            {
                if (isNumberExistsInTheRow(coordinates, value, sudokuArray) || isNumberExistsInTheColumn(coordinates, value, sudokuArray) || isNumberExistsInThe3x3(coordinates, value, sudokuArray))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            static bool isNumberInDoubleInTheRow(int rowIndex, string[,] sudokuArray)
            {
                for (int i = 0; i < sudokuArray.GetLength(1); i++)
                {
                    for (int j = i; j < sudokuArray.GetLength(1); j++)
                    {
                        if (i != j && sudokuArray[rowIndex, i] != "." && sudokuArray[rowIndex, i] == sudokuArray[rowIndex, j])
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            static bool isNumberInDoubleInTheColumn(int columnIndex, string[,] sudokuArray)
            {
                for (int i = 0; i < sudokuArray.GetLength(0); i++)
                {
                    for (int j = i; j < sudokuArray.GetLength(0); j++)
                    {
                        if (i != j && sudokuArray[i, columnIndex] != "." && sudokuArray[i, columnIndex] == sudokuArray[j, columnIndex])
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            static bool isNumberInDoubleInThe3x3(int rowIndex, int columnIndex, string[,] sudokuArray)
            {
                int squareRow = (rowIndex / 3) * 3;
                int squareColumn = (columnIndex / 3) * 3;

                for (int i = squareRow; i < squareRow + 3; i++)
                {
                    for (int j = squareColumn; j < squareColumn + 3; j++)
                    {
                        for (int k = squareRow; k < squareRow + 3; k++)
                        {
                            for (int l = squareColumn; l < squareColumn + 3; l++)
                            {
                                if ((i != k || j != l) && sudokuArray[i, j] != "." && sudokuArray[i, j] == sudokuArray[k, l])
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }

            static bool isNumberInDoubleInTheRowColumn3x3(int rowIndex, int columnIndex, string[,] sudokuArray)
            {
                if (isNumberInDoubleInTheRow(rowIndex, sudokuArray) || isNumberInDoubleInTheColumn(columnIndex, sudokuArray) || isNumberInDoubleInThe3x3(rowIndex, columnIndex, sudokuArray))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            static bool isThereADoubleInTheArray(string[,] sudokuArray)
            {
                for (int i = 0; i < sudokuArray.GetLength(0); i++)
                {
                    for (int j = 0; j < sudokuArray.GetLength(1); j++)
                    {
                        if (isNumberInDoubleInTheRowColumn3x3(i, j, sudokuArray))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            static bool isThereAnEmptyCaseInTheArray(string[,] sudokuArray)
            {
                for (int i = 0; i < sudokuArray.GetLength(0); i++)
                {
                    for (int j = 0; j < sudokuArray.GetLength(1); j++)
                    {
                        if (sudokuArray[i, j] == ".")
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            // Comble chaque case vide avec le premier nombre possible
            static void initEmptyCases(List<EmptyCase> emptyCasesList, string[,] sudokuArray)
            {
                for (int i = 0; i < emptyCasesList.Count; i++)
                {
                    sudokuArray[emptyCasesList[i].getCoordinates()[0], emptyCasesList[i].getCoordinates()[1]] = emptyCasesList[i].getPossibleNumbers()[0];
                    emptyCasesList[i].setIt(emptyCasesList[i].getIt() + 1);
                }
            }


            // Main fucntion
            static string[,] sudokuSolver(string[,] input)
            {

                if (inputValidation(input))
                {
                    string[,] sudokuArray = input;
                    List<EmptyCase> emptyCasesList = emptyCasesListDefiner(sudokuArray);

                    int iteration = 0;

                    for (int i = 0; i < emptyCasesList.Count; i++)
                    {

                        foreach (string possibleNumber in emptyCasesList[i].getPossibleNumbers())
                        {

                            //if (emptyCasesList.Last() == emptyCase && emptyCase.getPossibleNumbers().Last() == possibleNumber)
                            //{
                            //    Console.WriteLine("Aucune solution possible pour cette grille.");
                            //    return null;
                            //}

                            if (isNumberInDoubleInTheRowColumn3x3(emptyCasesList[i].getCoordinates()[0], emptyCasesList[i].getCoordinates()[1], sudokuArray) || sudokuArray[emptyCasesList[i].getCoordinates()[0], emptyCasesList[i].getCoordinates()[1]] == ".")
                            {
                                sudokuArray[emptyCasesList[i].getCoordinates()[0], emptyCasesList[i].getCoordinates()[1]] = possibleNumber;
                                if (possibleNumber == emptyCasesList[i].getPossibleNumbers().Last())
                                {
                                    i = i - 1;
                                }

                                //if (!isNumberInDoubleInTheRowColumn3x3(emptyCasesList[i].getCoordinates()[0], emptyCasesList[i].getCoordinates()[1], sudokuArray))
                                //{
                                //    break;
                                //}
                            }

                            iteration = iteration + 1;
                            Console.WriteLine(iteration);
                        }


                    }

                    Console.WriteLine("Solution trouvée !");
                    for (int i = 0; i < sudokuArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < sudokuArray.GetLength(1); j++)
                        {
                            if (j == sudokuArray.GetLength(1) - 1)
                            {
                                Console.WriteLine(sudokuArray[i, j]);
                            }
                            else
                            {
                                Console.Write(sudokuArray[i, j]);

                            }
                        }
                    }
                    return sudokuArray;
                }
                else
                {
                    Console.WriteLine("Le tableau doit avoir une taille de 9x9 et contenir uniquement des chiffres de 1 à 9 ou des . pour les cases vides.");
                    return null;
                }
            }


            string[,] sudoku1 = {   { "5", "3", ".", ".", "7", ".", ".", ".", "." },
                                    { "6", ".", ".", "1", "9", "5", ".", ".", "." },
                                    { ".", "9", "8", ".", ".", ".", ".", "6", "." },
                                    { "8", ".", ".", ".", "6", ".", ".", ".", "3" },
                                    { "4", ".", ".", "8", ".", "3", ".", ".", "1" },
                                    { "7", ".", ".", ".", "2", ".", ".", ".", "6" },
                                    { ".", "6", ".", ".", ".", ".", "2", "8", "." },
                                    { ".", ".", ".", "4", "1", "9", ".", ".", "5" },
                                    { ".", ".", ".", ".", "8", ".", ".", "7", "9" } };
            Console.WriteLine(sudokuSolver(sudoku1));
        }
    }
}
