namespace Algorithms.AdventOfCode.Y2023.Day20
{
    internal class StringDataModel
    {
        public string DataToProcess { get; set; } = string.Empty;
    }
}

// need a parse method that returns a dataModel
// need functions that takes a dataModel and return an IEnumerable de string.
// le data model n'a pas besoin d'être visible, donc ca peut etre encapsulé dans une classe générique <TDataModel>
// qui prend une unique methode Solve avec 2 parametres input & strategy.

