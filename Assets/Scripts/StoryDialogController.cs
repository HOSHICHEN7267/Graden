using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryDialogController : MonoBehaviour
{

    public Text storyText;

    [TextArea(5, 30)]
    public string[] lines;
    public float textSpeed;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        storyText.text = string.Empty;
        StartDialog();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(storyText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                storyText.text = lines[index];
            }
        }
    }

    void StartDialog()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            storyText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 2)
        {
            index++;
            storyText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else if(index < lines.Length - 1)
        {
            index++;
            StopAllCoroutines();
            storyText.text = lines[index];
            storyText.fontSize = 38;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
