using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1_Controller : MonoBehaviour
{
    // Geometry
    private float[] f_columnPos = { -3.5f, -2, -0.5f, 1, 2.5f };    // x-coord of each column
    private float[] f_rowPos = { 2.5f, 1, -0.5f, -2, -3.5f };       // z-coord of each row
    private GameObject[,] go_locand = new GameObject[5, 5];         // holder for all the locands

    // PPs
    public GameObject go_PP0;
    public GameObject go_PP1;
    public GameObject go_PP2;
    public GameObject go_PP3;
    public GameObject go_PP4;

    private GameObject[] go_PPList;     // a holder array to refer to PPs sequentially

    public int[] PPCurrRow = new int[5];   // row indices of each PP
    public int[] PPCurrCol = new int[5];   // column indeices of each PP

    private int[] PPSolRow = { 4, 3, 2, 1, 0 };     // the solution rows
    private int[] PPSolCol = { 0, 1, 2, 3, 4 };     // the solution columns
    private bool[] b_PPSol;                         // are the solutions met?

    // Highlighters
    public int i_SelectedRow;
    public int i_SelectedCol;
    public bool b_isRow;        // is the Row currently selected?
    public bool b_highlighterIsOn;
    private GameObject[] go_columnHiList;   // a holder array
    private GameObject[] go_rowHiList;      // a holder array

    private Color32 col_gold = new Color32(236, 255, 74, 100);
    private Color32 col_dull = new Color32(176, 176, 176, 255);
    private Color32 col_grn = new Color32(48, 210, 49, 255);
    private Color32 col_blk = new Color32(0, 0, 0, 255);
    private float f_time = 0.0f;
    public float f_flashPeriod = 0.3f;

    // Control arrows
    public GameObject arrow_Col0;
    public GameObject arrow_Col1;
    public GameObject arrow_Col2;
    public GameObject arrow_Col3;
    public GameObject arrow_Col4;
    public GameObject arrow_Row0;
    public GameObject arrow_Row1;
    public GameObject arrow_Row2;
    public GameObject arrow_Row3;
    public GameObject arrow_Row4;

    public int[] i_effectRow = { -1, -1, -1, -1, -1 };
    public int[] i_effectCol = { 1, 1, 1, 1, 1 };


    public bool b_isMoving = false;

    // Use this for initialization
    void Start()
    {
        SetUpScene();
    }

    // Update is called once per frame
    void Update()
    {
        // Do the flash
        if (Time.time > f_time)
        {
            f_time = Time.time + f_flashPeriod;
            Anim_flashCurrentController();
        }

        // Accept input
        if (!b_isMoving) { Control_checkInput(); }

        // Check green
        Anim_CheckGreen();
    }

    void Control_checkInput()
    {
        if (b_isRow)
        {
            if (Input.GetKeyDown("down"))
            {
                i_SelectedRow = (i_SelectedRow + 1) % go_rowHiList.Length;  // increase currentRow by 1 (loop around if exceeds end of array)
                ResetHighlightColours();                            // reset all colours
                Anim_flashCurrentController();                      // highlight the new colour (failure to do this step here results in a noticeable delay in the new arrow beginning to flash
            }
            if (Input.GetKeyDown("up"))
            {
                i_SelectedRow = (i_SelectedRow - 1) % go_rowHiList.Length;
                if (i_SelectedRow == -1) { i_SelectedRow = go_rowHiList.Length - 1; }   // loop from start of array back to end
                ResetHighlightColours();
                Anim_flashCurrentController();
            }
            if (Input.GetKeyDown("enter") || Input.GetKeyDown("space") || Input.GetKeyDown("return"))
            {
                Control_confirmInput();
            }
        }
        else
        {
            if (Input.GetKeyDown("right"))
            {
                i_SelectedCol = (i_SelectedCol + 1) % go_columnHiList.Length;
                ResetHighlightColours();
                Anim_flashCurrentController();
            }
            if (Input.GetKeyDown("left"))
            {
                i_SelectedCol = (i_SelectedCol - 1) % go_columnHiList.Length;
                if (i_SelectedCol == -1) { i_SelectedCol = go_columnHiList.Length - 1; }   // loop from start of array back to end
                ResetHighlightColours();
                Anim_flashCurrentController();
            }
            if (Input.GetKeyDown("enter") || Input.GetKeyDown("space") || Input.GetKeyDown("return"))
            {
                Control_confirmInput();
            }
        }
    }

    void Control_confirmInput()
    {
        if (b_isRow)
        {
            for (int i = 0; i < PPCurrRow.Length; i++)
            {
                if (PPCurrRow[i] == i_SelectedRow)
                {
                    PPCurrCol[i] += i_effectRow[i_SelectedRow];     // PP will never run into another PP as all the PP on that row are affected at the same time (no collisions until the blocks are introduced)
                    if (PPCurrCol[i] == -1) { PPCurrCol[i] = PPCurrCol.Length; }    // if you fall off the end of the array, loop around
                    if (PPCurrCol[i] == PPCurrCol.Length) { PPCurrCol[i] = 0; }    // if you fall off the end of the array, loop around
                }
            }

            Control_setNewPositions();
            b_isRow = false;
        }
        else
        {
            for (int i = 0; i < PPCurrCol.Length; i++)
            {
                if (PPCurrCol[i] == i_SelectedCol)
                {
                    PPCurrRow[i] += i_effectCol[i_SelectedCol];     // PP will never run into another PP as all the PP on that column are affected at the same time (no collisions until the blocks are introduced)
                    if (PPCurrRow[i] == -1) { PPCurrRow[i] = PPCurrRow.Length; }    // if you fall off the end of the array, loop around
                    if (PPCurrRow[i] == PPCurrRow.Length) { PPCurrRow[i] = 0; }    // if you fall off the end of the array, loop around
                }
            }

            Control_setNewPositions();
            b_isRow = true;
        }
    }

    void Control_setNewPositions()
    {
        go_PP0.transform.position = new Vector3(f_columnPos[PPCurrCol[0]], 0, f_rowPos[PPCurrRow[0]]);
        go_PP1.transform.position = new Vector3(f_columnPos[PPCurrCol[1]], 0, f_rowPos[PPCurrRow[1]]);
        go_PP2.transform.position = new Vector3(f_columnPos[PPCurrCol[2]], 0, f_rowPos[PPCurrRow[2]]);
        go_PP3.transform.position = new Vector3(f_columnPos[PPCurrCol[3]], 0, f_rowPos[PPCurrRow[3]]);
        go_PP4.transform.position = new Vector3(f_columnPos[PPCurrCol[4]], 0, f_rowPos[PPCurrRow[4]]);
    }

    void SetUpScene()
    {
        // Array of locands
        for (int i = 0; i < 5; i++)         // over each row
        {
            for (int j = 0; j < 5; j++)     // over each column
            {
                go_locand[i, j] = GameObject.Find(i.ToString() + j.ToString());
            }
        }

        // Array of PPs
        go_PPList = new GameObject[] { go_PP0, go_PP1, go_PP2, go_PP3, go_PP4 };

        // Record & set inital positions
        PPCurrRow = new int[5] { 0, 1, 2, 3, 4 };
        PPCurrCol = new int[5] { 4, 1, 2, 3, 4 };
        Control_setNewPositions();

        // Initial arrow selection
        go_columnHiList = new GameObject[] { arrow_Col0, arrow_Col1, arrow_Col2, arrow_Col3, arrow_Col4 };
        go_rowHiList = new GameObject[] { arrow_Row0, arrow_Row1, arrow_Row2, arrow_Row3, arrow_Row4 };
        i_SelectedCol = 0;
        i_SelectedRow = 0;
        b_isRow = true;
        b_highlighterIsOn = true;

        ResetHighlightColours();

        // Initial check of correct positions
        Anim_CheckGreen();
    }

    void ResetHighlightColours()
    {
        // Decolour all controller arrows
        arrow_Col0.GetComponent<Renderer>().material.color = col_dull;
        arrow_Col1.GetComponent<Renderer>().material.color = col_dull;
        arrow_Col2.GetComponent<Renderer>().material.color = col_dull;
        arrow_Col3.GetComponent<Renderer>().material.color = col_dull;
        arrow_Col4.GetComponent<Renderer>().material.color = col_dull;
        arrow_Row0.GetComponent<Renderer>().material.color = col_dull;
        arrow_Row1.GetComponent<Renderer>().material.color = col_dull;
        arrow_Row2.GetComponent<Renderer>().material.color = col_dull;
        arrow_Row3.GetComponent<Renderer>().material.color = col_dull;
        arrow_Row4.GetComponent<Renderer>().material.color = col_dull;
    }

    void Anim_flashCurrentController()
    {
        if (b_isRow)
        {
            if (b_highlighterIsOn)
            {
                go_rowHiList[i_SelectedRow].GetComponent<Renderer>().material.color = col_dull;
                b_highlighterIsOn = false;
            }
            else
            {
                go_rowHiList[i_SelectedRow].GetComponent<Renderer>().material.color = col_gold;
                b_highlighterIsOn = true;
            }
        }
        else
        {
            if (b_highlighterIsOn)
            {
                go_columnHiList[i_SelectedCol].GetComponent<Renderer>().material.color = col_dull;
                b_highlighterIsOn = false;
            }
            else
            {
                go_columnHiList[i_SelectedCol].GetComponent<Renderer>().material.color = col_gold;
                b_highlighterIsOn = true;
            }
        }
    }

    void Anim_CheckGreen()
    {
        int i_countCorrect = 0;
        b_PPSol = new bool[] { false, false, false, false, false };     // reset correct check

        for (int i = 0; i < PPCurrRow.Length; i++)  // find all those sweet sweet correct spots ... so sweet
        {
            for (int j = 0; j < PPSolRow.Length; j++)
            {
                //go_locand[PPSolRow[j], PPSolCol[j]].GetComponent<Renderer>().material.color = col_grn;

                if (PPCurrRow[i] == PPSolRow[j] && PPCurrCol[i] == PPSolCol[j])
                {
                    b_PPSol[j] = true;
                    Debug.LogFormat("Correct @: {0},{1} for {2}", PPSolRow[j], PPSolCol[j], j);
                    i_countCorrect++;
                }
            }
        }

        for (int i = 0; i < b_PPSol.Length; i++)    // apply the colour to the correct ones and recolour the wrong ones
        {
            if (b_PPSol[i])
            {
                go_locand[PPSolRow[i], PPSolCol[i]].GetComponent<Renderer>().material.color = col_grn;
            }
            else
            {
                go_locand[PPSolRow[i], PPSolCol[i]].GetComponent<Renderer>().material.color = col_blk;
            }
        }
    }
}

