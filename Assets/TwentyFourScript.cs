using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;
using KModkit;

public class TwentyFourScript : MonoBehaviour
{
    static int _moduleIdCounter = 1;
    int _moduleID = 0;

    public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMBombModule Module;
    public KMSelectable[] Buttons;

    private Dictionary<string, string> NumsToSolution = new Dictionary<string, string>
    {
        { "1234", "(1+2+3)*4" },
        { "1235", "(1+2+5)*3" },
        { "1236", "(2+3-1)*6" },
        { "1237", "(1+2)*7+3" },
        { "1238", "(1+3+8)*2" },
        { "1239", "(1+2)*9-3" },
        { "1245", "(2+5-1)*4" },
        { "1246", "(2+6)*(4-1)" },
        { "1247", "(1+4+7)*2" },
        { "1248", "(1+4-2)*8" },
        { "1249", "(1+9)*2+4" },
        { "1256", "(1+5+6)*2" },
        { "1257", "(7-2)*5-1" },
        { "1258", "(5+8-1)*2" },
        { "1259", "(1+2)*5+9" },
        { "1267", "(6+7-1)*2" },
        { "1268", "(1+8)*2+6" },
        { "1269", "(1*2*9+6)" },
        { "1278", "(1+2*8)+7" },
        { "1279", "(1+2*7)+9" },
        { "1289", "(2*8+9)-1" },
        { "1345", "(1+3)*5+4" },
        { "1346", "6/(1-3/4)" },
        { "1347", "(1+3)*7-4" },
        { "1348", "(1+3)*4+8" },
        { "1349", "(1+3*9)-4" },
        { "1356", "(1+3*6)+5" },
        { "1357", "(1+5)*(7-3)" },
        { "1358", "(1+5-3)*8" },
        { "1359", "(1*3*5+9)" },
        { "1367", "(3*6+7)-1" },
        { "1368", "(8-1-3)*6" },
        { "1369", "(1+9)*3-6" },
        { "1378", "(7-1-3)*8" },
        { "1379", "(1+7)*9/3" },
        { "1389", "1*8*9/3" },
        { "1457", "(1+4*7)-5" },
        { "1458", "(5-1)*4+8" },
        { "1459", "(4-1)*5+9" },
        { "1467", "(1+7-4)*6" },
        { "1468", "(1+6-4)*8" },
        { "1469", "(9-1-4)*6" },
        { "1478", "(1+7)*4-8" },
        { "1479", "(1-9)*(4-7)" },
        { "1489", "(1+4*8)-9" },
        { "1567", "(1+5*6)-7" },
        { "1568", "(1+8-5)*6" },
        { "1569", "(1*9-5)*6" },
        { "1578", "(1+7-5)*8" },
        { "1579", "(1-7)*(5-9)" },
        { "1589", "(9-1-5)*8" },
        { "1679", "(1+7)*(9-6)" },
        { "1689", "1+6+8+9" },
        { "1789", "(1*7+8)+9" },
        { "2345", "(3+4+5)*2" },
        { "2346", "(2+3*6)+4" },
        { "2347", "(2+7-3)*4" },
        { "2348", "(2+4-3)*8" },
        { "2349", "(3+9)*4/2" },
        { "2356", "(2+5-3)*6" },
        { "2357", "(2+3*5)+7" },
        { "2358", "(2*8+3)+5" },
        { "2359", "(2+9*3)-5" },
        { "2367", "(2*7-6)*3" },
        { "2368", "(2+8)*3-6" },
        { "2369", "(3+2*6)+9" },
        { "2378", "(7+8-3)*2" },
        { "2379", "(7-2)*3+9" },
        { "2389", "(9-2*3)*8" },
        { "2456", "(2+4)*5-6" },
        { "2457", "(4-2)*(5+7)" },
        { "2458", "(2+5-4)*8" },
        { "2459", "(2+9-5)*4" },
        { "2467", "(2*7+4)+6" },
        { "2468", "(2*6+8)+4" },
        { "2469", "(4-2)*9+6" },
        { "2478", "(2*7-8)*4" },
        { "2479", "(2*4+7)+9" },
        { "2489", "(9-2-4)*8" },
        { "2567", "(2*6+5)+7" },
        { "2568", "(2*5+8)+6" },
        { "2569", "(5*6/2+9)" },
        { "2578", "(2*5-7)*8" },
        { "2579", "(5*7-2)-9" },
        { "2589", "2+5+8+9" },
        { "2678", "(2+7-6)*8" },
        { "2679", "2+6+7+9" },
        { "2689", "(2*6-9)*8" },
        { "2789", "(7+9)*2-8" },
        { "3456", "(3+5-4)*6" },
        { "3457", "(3*4+5)+7" },
        { "3458", "(3+5)*4-8" },
        { "3459", "(4+9-5)*3" },
        { "3468", "(3*4-8)*6" },
        { "3469", "(3+9-6)*4" },
        { "3478", "(7-3)*4+8" },
        { "3479", "(3*9+4)-7" },
        { "3489", "3+4+8+9" },
        { "3567", "(6+7-5)*3" },
        { "3568", "(5-6/3)*8" },
        { "3569", "3*(5+6)-9" },
        { "3578", "(3*7+8)-5" },
        { "3579", "3+5+7+9" },
        { "3589", "(3*9+5)-8" },
        { "3678", "3+6+7+8" },
        { "3679", "(3*7+9)-6" },
        { "3689", "(3+9-8)*6" },
        { "3789", "(7+9-8)*3" },
        { "4567", "(5+7-6)*4" },
        { "4568", "(4+5-6)*8" },
        { "4569", "4+5+6+9" },
        { "4578", "4+5+7+8" },
        { "4579", "(4*7+5)-9" },
        { "4589", "(5+9-8)*4" },
        { "4678", "(4+6-7)*8" },
        { "4679", "(7+9)*6/4" },
        { "4689", "(4*6)*(9-8)" },
        { "4789", "(7+8-9)*4" },
        { "5678", "(5+7-8)*6" },
        { "5679", "(7-5)*9+6" },
        { "5689", "(5+8-9)*6" },
        { "5789", "(5*8-7)-9" },
        { "6789", "6*8/(9-7)" }
    };
    private List<List<int>> Banned = new List<List<int>>() { new List<int>() { 1, 3, 4, 6 }, new List<int>() { 1, 4, 5, 6 }, new List<int>() { 1, 6, 7, 8 }, new List<int>() { 3, 4, 6, 7 } };
    private List<int> SelectedPriority = new List<int>();
    private int[] Numbers;
    private bool[] Selected = new bool[8];
    private bool[] Inselectables = new bool[9];
    private bool Animating = false;

    private List<int> ReadEquation(string equation)
    {
        var output = new List<int>();
        var buttons = Buttons.Select(x => x.GetComponentInChildren<TextMesh>().text).Take(4).ToList();
        var temp = "";
        for (int i = 0; i < equation.Length; i++)
            temp += buttons.Contains(equation[i].ToString()) ? buttons.IndexOf(equation[i].ToString()).ToString() : equation[i].ToString();
        equation = temp;
        Debug.Log(equation);
        var groups = Regex.Matches(equation, @"\(.+?\)");
        for (int i = 0; i < groups.Count; i++)
        {
            var expression = groups[i].Value.Replace("(", "").Replace(")", "");
            while (true)
            {
                var matches = Regex.Matches(expression, @"[0-3](\*|/)[0-3]");

                if (matches.Count == 0)
                    break;
                else
                {
                    output.Add(int.Parse(matches[0].Value.First().ToString()));
                    output.Add(matches[0].Value[1] == '*' ? 6 : 7);
                    output.Add(int.Parse(matches[0].Value.Last().ToString()));
                    output.Add(8);
                    expression = expression.Replace(matches[0].Value, int.Parse(matches[0].Value.First().ToString()) < int.Parse(matches[0].Value.Last().ToString()) ? matches[0].Value.First().ToString() : matches[0].Value.Last().ToString());
                }
            }
            while (true)
            {
                var matches = Regex.Matches(expression, @"[0-3](\+|\-)[0-3]");

                if (matches.Count == 0)
                    break;
                else
                {
                    output.Add(int.Parse(matches[0].Value.First().ToString()));
                    output.Add(matches[0].Value[1] == '+' ? 4 : 5);
                    output.Add(int.Parse(matches[0].Value.Last().ToString()));
                    output.Add(8);
                    expression = expression.Replace(matches[0].Value, int.Parse(matches[0].Value.First().ToString()) < int.Parse(matches[0].Value.Last().ToString()) ? matches[0].Value.First().ToString() : matches[0].Value.Last().ToString());
                }
            }
            equation = equation.Replace(groups[i].Value, expression);
        }
        while (true)
        {
            var matches = Regex.Matches(equation, @"[0-3](\*|/)[0-3]");

            if (matches.Count == 0)
                break;
            else
            {
                output.Add(int.Parse(matches[0].Value.First().ToString()));
                output.Add(matches[0].Value[1] == '*' ? 6 : 7);
                output.Add(int.Parse(matches[0].Value.Last().ToString()));
                output.Add(8);
                equation = equation.Replace(matches[0].Value, int.Parse(matches[0].Value.First().ToString()) < int.Parse(matches[0].Value.Last().ToString()) ? matches[0].Value.First().ToString() : matches[0].Value.Last().ToString());
            }
        }
        while (true)
        {
            var matches = Regex.Matches(equation, @"[0-3](\+|\-)[0-3]");

            if (matches.Count == 0)
                break;
            else
            {
                output.Add(int.Parse(matches[0].Value.First().ToString()));
                output.Add(matches[0].Value[1] == '+' ? 4 : 5);
                output.Add(int.Parse(matches[0].Value.Last().ToString()));
                output.Add(8);
                equation = equation.Replace(matches[0].Value, int.Parse(matches[0].Value.First().ToString()) < int.Parse(matches[0].Value.Last().ToString()) ? matches[0].Value.First().ToString() : matches[0].Value.Last().ToString());
            }
        }
        return output;
    }

    void Awake()
    {
        _moduleID = _moduleIdCounter++;
        for (int i = 0; i < Buttons.Length; i++)
        {
            int x = i;
            Buttons[x].OnInteract += delegate { if (!Inselectables[x] && !Animating) ButtonPress(x); return false; };
        }
        Generate();
        var temp = Numbers.ToList();
        temp.Sort();
    }

    void ButtonPress(int pos)
    {
        if (pos != 8)
        {
            Audio.PlaySoundAtTransform("press", Buttons[pos].transform);
            Selected[pos] = !Selected[pos];
            if (Selected[pos])
            {
                Buttons[pos].GetComponent<MeshRenderer>().material.color = new Color(0.8f, 1, 0.8f);
                Buttons[pos].GetComponentInChildren<TextMesh>().color = new Color(0, 0, 0, 1);
                if (pos >= 4)
                {
                    for (int i = 4; i < 8; i++)
                    {
                        Selected[i] = i == pos;
                        if (i != pos)
                        {
                            Buttons[i].GetComponent<MeshRenderer>().material.color = new Color(0, 120f / 255, 0);
                            Buttons[i].GetComponentInChildren<TextMesh>().color = new Color(1, 1, 1, 1);
                        }
                    }
                }
                else
                {
                    SelectedPriority.Add(pos);
                    if (SelectedPriority.Count() == 3)
                    {
                        Selected[SelectedPriority.First()] = false;
                        Buttons[SelectedPriority.First()].GetComponent<MeshRenderer>().material.color = new Color(0, 120f / 255, 0);
                        Buttons[SelectedPriority.First()].GetComponentInChildren<TextMesh>().color = new Color(1, 1, 1, 1);
                        SelectedPriority.Remove(SelectedPriority.First());
                    }
                }
            }
            else
            {
                SelectedPriority.Remove(pos);
                Buttons[pos].GetComponent<MeshRenderer>().material.color = new Color(0, 120f / 255, 0);
                Buttons[pos].GetComponentInChildren<TextMesh>().color = new Color(1, 1, 1, 1);
            }
        }
        else
        {
            Audio.PlaySoundAtTransform("tap", Buttons[8].transform);
            var nums = Enumerable.Range(0, 4).Where(x => Selected[x]).Select(x => int.Parse(Buttons[x].GetComponentInChildren<TextMesh>().text)).ToList();
            nums.Sort();
            if (Selected.Where((x, ix) => x && ix < 4).Count() == 2 && Selected.Where((x, ix) => x && ix >= 4).Count() == 1)
                if (!Selected[7] || (nums[0] > 0 && nums[1] % nums[0] == 0))
                    StartCoroutine(Merge());
        }
    }

    void Generate()
    {
        var nums = Banned[0].ToList();
        while (Banned.Where(x => x.Where((y, ix) => y == nums[ix]).Count() == 4).Count() > 0)
        {
            nums = Enumerable.Range(1, 9).ToList();
            nums.Shuffle();
            nums = nums.Take(4).ToList();
            nums.Sort();
        }
        var answer = NumsToSolution[nums.Join("")];
        nums.Shuffle();
        Numbers = nums.ToArray();
        Debug.LogFormat("[24 #{0}] The displayed numbers are: {1}.", _moduleID, Numbers.Join(", "));
        Debug.LogFormat("[24 #{0}] One possible solution: {1}.", _moduleID, answer);
        for (int i = 0; i < 4; i++)
        {
            Buttons[i].GetComponentInChildren<TextMesh>().text = nums[i].ToString();
            Buttons[i].GetComponent<MeshRenderer>().material.color = new Color(0, 120f / 255, 0);
            Buttons[i].GetComponentInChildren<TextMesh>().color = new Color(1, 1, 1, 1);
        }
    }

    private IEnumerator Merge(float inDuration = 0.2f, float outDuration = 0.4f)
    {
        Animating = true;
        var buts = Selected.Select((x, ix) => x ? ix : -1).Where(x => x > -1).ToList();
        List<Vector3> originals = new List<Vector3>();
        for (int i = 0; i < 3; i++)
            originals.Add(Buttons[buts[i]].transform.localPosition);
        Vector3 destination = new Vector3(0, 0.01094f, -0.0201f);
        bool sounded = false;
        float timer = 0;
        while (timer < inDuration)
        {
            yield return null;
            timer += Time.deltaTime;
            for (int i = 0; i < 3; i++)
            {
                var position = new Vector3(Easing.InQuint(timer, originals[i].x, destination.x, inDuration), 0.01094f, Easing.InQuint(timer, originals[i].z, destination.z, inDuration));
                Buttons[buts[i]].transform.localPosition = position;
            }
            if (inDuration - timer <= 0.1f && !sounded)
            {
                sounded = true;
                Audio.PlaySoundAtTransform("merge", Module.transform);
            }
        }
        for (int i = 0; i < 3; i++)
            Buttons[buts[i]].transform.localPosition = destination;
        int answer = 0;
        int num1 = int.Parse(Buttons[buts[0]].GetComponentInChildren<TextMesh>().text);
        int num2 = int.Parse(Buttons[buts[1]].GetComponentInChildren<TextMesh>().text);
        bool greatest = num2 > num1;
        switch (buts.Last())
        {
            case 4:
                answer = num1 + num2;
                break;
            case 5:
                answer = Mathf.Abs(num1 - num2);
                break;
            case 6:
                answer = num1 * num2;
                break;
            default:
                answer = greatest ? num2 / num1 : num1 / num2;
                break;
        }
        Buttons[buts[0]].GetComponentInChildren<TextMesh>().text = answer.ToString();
        Buttons[buts[1]].GetComponentInChildren<TextMesh>().text = "";
        Buttons[buts[1]].GetComponent<MeshRenderer>().material.color = new Color(0, 0.125f, 0);
        Buttons[buts[0]].GetComponent<MeshRenderer>().material.color = new Color(0, 120f / 255, 0);
        Buttons[buts[2]].GetComponent<MeshRenderer>().material.color = new Color(0, 120f / 255, 0);
        Buttons[buts[0]].GetComponentInChildren<TextMesh>().color = new Color(1, 1, 1, 1);
        Buttons[buts[2]].GetComponentInChildren<TextMesh>().color = new Color(1, 1, 1, 1);
        Inselectables[buts[1]] = true;
        Debug.LogFormat("[24 #{0}] Operation performed: {1}.", _moduleID, (greatest ? num2.ToString() : num1.ToString()) + new[] { "+", "-", "*", "/" }[buts.Last() - 4] + (!greatest ? num2.ToString() : num1.ToString()) + "=" + answer.ToString());
        timer = 0;
        while (timer < outDuration)
        {
            yield return null;
            timer += Time.deltaTime;
            for (int i = 0; i < 3; i++)
            {
                var position = new Vector3(Easing.OutQuint(timer, destination.x, originals[i].x, outDuration), 0.01094f, Easing.OutQuint(timer, destination.z, originals[i].z, outDuration));
                Buttons[buts[i]].transform.localPosition = position;
            }
        }
        for (int i = 0; i < 3; i++)
            Buttons[buts[i]].transform.localPosition = originals[i];
        Selected = new bool[8];
        SelectedPriority = new List<int>();
        if (Buttons[0].GetComponentInChildren<TextMesh>().text == "24" && Buttons.Where((x, ix) => new[] { 1, 2, 3 }.Contains(ix)).Select(x => x.GetComponentInChildren<TextMesh>().text).Where(x => x != "").Count() == 0)
        {
            timer = 0;
            while (timer < 0.25f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            Solve();
            yield return "solve";
        }
        else if (Buttons.Where((x, ix) => new[] { 1, 2, 3 }.Contains(ix)).Select(x => x.GetComponentInChildren<TextMesh>().text).Where(x => x != "").Count() == 0)
        {
            timer = 0;
            while (timer < 0.25f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            Module.HandleStrike();
            Debug.LogFormat("[24 #{0}] That's {1}, not 24! Strike!", _moduleID, answer);
            yield return "strike";
            for (int i = 0; i < 4; i++)
            {
                Buttons[i].GetComponentInChildren<TextMesh>().text = Numbers[i].ToString();
                Buttons[i].GetComponent<MeshRenderer>().material.color = new Color(0, 120f / 255, 0);
                Buttons[i].GetComponentInChildren<TextMesh>().color = new Color(1, 1, 1, 1);
            }
            Animating = false;
            Inselectables = new bool[9];
        }
        else
            Animating = false;
    }

    void Solve()
    {
        Module.HandlePass();
        Debug.LogFormat("[24 #{0}] That's 24! Module solved.", _moduleID);
        Audio.PlaySoundAtTransform("solve", Module.transform);
        Animating = true;
        for (int i = 0; i < 8; i++)
        {
            if (i < 4)
                Buttons[i].GetComponentInChildren<TextMesh>().text = "NICE"[i].ToString();
            Buttons[i].GetComponent<MeshRenderer>().material.color = new Color(0.8f, 1, 0.8f) * (1 - (i / 4));
            Buttons[i].GetComponentInChildren<TextMesh>().color = new Color(0, 0, 0, 1);
        }
        Buttons[8].GetComponentInChildren<TextMesh>().text = "GG";
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = "Use '!{0} 1 2 * > 4 / 2 >' to multiply 1 by 2, then press the display, then divide 4 by 2, then press the display.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        var commandArray = command.Split(' ');
        foreach (var part in commandArray)
            if (!Regex.IsMatch(part, @"[0-9]+|\+|\-|\*|/|>"))
            {
                yield return "sendtochaterror Invalid command.";
                yield break;
            }
        foreach (var part in commandArray)
        {
            while (Animating)
                yield return null;
            if (part == "+")
                Buttons[4].OnInteract();
            else if (part == "-")
                Buttons[5].OnInteract();
            else if (part == "*")
                Buttons[6].OnInteract();
            else if (part == "/")
                Buttons[7].OnInteract();
            else if (part == ">")
                Buttons[8].OnInteract();
            else
            {
                var buts = Buttons.Take(4).Select(x => x.GetComponentInChildren<TextMesh>().text).ToList();
                if (!buts.Contains(part))
                {
                    yield return "sendtochaterror I can't find any buttons with the number " + part + "!";
                    yield break;
                }
                else
                    Buttons[buts.IndexOf(part)].OnInteract();
            }
            float timer = 0;
            while (timer < 0.1f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        for (int i = 0; i < 4; i++)
        {
            Buttons[i].GetComponentInChildren<TextMesh>().text = Numbers[i].ToString();
            Buttons[i].GetComponent<MeshRenderer>().material.color = new Color(0, 120f / 255, 0);
            Buttons[i].GetComponentInChildren<TextMesh>().color = new Color(1, 1, 1, 1);
        }
        Animating = false;
        Inselectables = new bool[9];
        var temp = Numbers.ToList();
        temp.Sort();
        var answer = ReadEquation(NumsToSolution[temp.Select(x => x.ToString()).Join("")]);
        Debug.Log(_moduleID + ": " + answer.Join(", "));
        foreach (var pos in answer)
        {
            yield return true;
            Buttons[pos].OnInteract();
            while (Animating)
                yield return true;
        }
    }
}
