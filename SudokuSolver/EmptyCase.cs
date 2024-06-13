using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    internal class EmptyCase
    {
        private int[] _coordinates = new int[2];
        private List<string> _impossibleNumbers = new List<string>();
        private List<string> _possibleNumbers = new List<string>();
        private int _it = 0;

        public EmptyCase(int[] coordinates, List<string> impossibleNumbers, List<string> possibleNumbers, int it)
        {
            this._coordinates = coordinates;
            this._impossibleNumbers = impossibleNumbers;
            this._possibleNumbers = possibleNumbers;
            this._it = it;
        }

        public int[] getCoordinates() { return _coordinates; }
        public List<string> getImpossibleNumbers() { return _impossibleNumbers; }
        public List<string> getPossibleNumbers() { return _possibleNumbers; }
        public int getIt() { return _it; }

        public void logAllInfos()
        {
            Console.Write("Coordinates : ");
            foreach (int coordinate in _coordinates)
            {
                Console.Write(coordinate + " ");
            }
            Console.WriteLine(" ");

            Console.Write("Nombres impossibles : ");
            foreach (string impossibleNumber in _impossibleNumbers)
            {
                Console.Write(impossibleNumber + " ");
            }
            Console.WriteLine(" ");

            Console.Write("Nombres possibles : ");
            foreach (string possibleNumber in _possibleNumbers)
            {
                Console.Write(possibleNumber + " ");
            }
            Console.WriteLine(" ");

            Console.Write("Itération : ");
            Console.WriteLine(_it);
            Console.WriteLine("____________________");
        }

        public void setCoordinates(int[] coordinates) { _coordinates = coordinates; }
        public void setImpossibleNumbers(List<string> impossibleNumbers) { _impossibleNumbers = impossibleNumbers; }
        public void setPossibleNumbers(List<string> possibleNumbers) { _possibleNumbers = possibleNumbers; }
        public void setIt(int it) { _it = it; }
    }
}
