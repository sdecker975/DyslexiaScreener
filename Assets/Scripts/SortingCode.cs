using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingCode : MonoBehaviour {

    //static List<TestItem> quicksort(List<int> posList, List<TestItem> testItems)
    //{
    //    if (testItems.Count <= 1) return testItems;
    //    int pivotPosition = posList.Count / 2;
    //    int pivotValue = posList[pivotPosition];
    //    TestItem pivotTestItem = testItems[pivotPosition];
    //    posList.RemoveAt(pivotPosition);
    //    testItems.RemoveAt(pivotPosition);

    //    List<int> smaller = new List<int>();
    //    List<TestItem> smallerTestItems = new List<TestItem>();
    //    List<int> greater = new List<int>();
    //    List<TestItem> greaterTestItems = new List<TestItem>();

    //    int index = 0;
    //    foreach (int item in posList)
    //    {
    //        if (item < pivotValue)
    //        {
    //            smaller.Add(item);
    //            smallerTestItems.Add(testItems[index]);
    //        }
    //        else
    //        {
    //            greater.Add(item);
    //            greaterTestItems.Add(testItems[index]);
    //        }
    //        index++;
    //    }
    //    List<TestItem> sorted = quicksort(smaller, testItems);
    //    sorted.Add(pivotTestItem);
    //    sorted.AddRange(quicksort(greater, testItems));
    //    return sorted;
    //}

    //public void SortItems()
    //{
    //    List<int> posValues = new List<int>();
    //    foreach (WITestItem l in testItemFrontEnd)
    //    {
    //        posValues.Add(l.pos);
    //        print(l.pos);
    //    }
    //    IEnumerator position = posValues.GetEnumerator();
    //    testItemBackEnd = testItemBackEnd.OrderBy(p =>
    //    {
    //        position.MoveNext();
    //        return position.Current;
    //    }
    //    ).ToList();
    //    testItemFrontEnd = testItemFrontEnd.OrderBy(o => o.pos).ToList();
    //}

    //public void ExportItems()
    //{
    //    int counter = 1;
    //    string filePath = "testItems/" + testAbbrev + "_items.csv";
    //    System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
    //    fileInfo.Directory.Create(); // If the directory already exists, this method does nothing.
    //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
    //    {
    //        file.Write("Item Number, Event Number, Correct Number, Display Text, Correct Item, Option 1, Option 2, Option 3, Option 4, Option 5, Option 6, Option 7,\n");

    //    }
    //    int eventSystemCounter = 0;
    //    foreach (WITestItem l in testItemFrontEnd)
    //    {
    //        if (!l.isExample)
    //        {
    //            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
    //            {
    //                file.Write(counter + ",");
    //                file.Write(eventSystemCounter + ",");
    //                counter++;
    //                for (int i = 0; i < l.isCorrect.Length; i++)
    //                {
    //                    if (l.isCorrect[i])
    //                    {
    //                        file.Write((i + 1) + ",");
    //                        file.Write(l.displayedText);
    //                        if (l.isSounds)
    //                            file.Write(l.testSounds[i] + ",");
    //                        else if (l.isWords)
    //                            file.Write(l.wordPictures[i].name);
    //                    }
    //                }
    //                if (l.isSounds)
    //                {
    //                    for (int i = 0; i < l.testSounds.Length; i++)
    //                    {
    //                        file.Write(l.testSounds[i] + ",");
    //                    }
    //                }
    //                if (l.isWords)
    //                {
    //                    for (int i = 0; i < l.testSounds.Length; i++)
    //                    {
    //                        file.Write(l.wordPictures[i].name + ",");
    //                    }
    //                }
    //                file.Write("\n");
    //            }
    //        }
    //        eventSystemCounter++;
    //    }
    //}

    //public void ResetItemPositions()
    //{
    //    int counter = 0;
    //    foreach (WITestItem l in testItemFrontEnd)
    //    {
    //        l.pos = counter;
    //        counter++;
    //    }
    //}
}
