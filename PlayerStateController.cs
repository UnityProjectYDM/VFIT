using UnityEngine;
using System.Collections;

using TMPro;
using UnityEngine.UI;


public class PlayerStateController : MonoBehaviour
{
    public int score;
    public int scorectr;
    public TextMeshPro ScoreText;
    private Animator animator;


    public GameObject[] buttons; // Array for buttons
    public GameObject popupmessage;
    public GameObject Achievement1;
    public GameObject Achievement2;
    public GameObject Achievement3;


    private PopupMessageController popupMessageController;


    // Function to update PlayerPrefs for a given key
    void UpdateTimes(string NameOfEx) // to update how many tiems the player has done an exercise
    {
        // Retrieve the current value from PlayerPrefs
        int currentValue = PlayerPrefs.GetInt(NameOfEx);

        // Increment the value by 1
        currentValue++;

        // Set the updated value back to PlayerPrefs
        PlayerPrefs.SetInt(NameOfEx, currentValue);

        // Save the changes
        PlayerPrefs.Save();

        Debug.Log($"Updated {NameOfEx}: {currentValue}"); // for debugging purposes, to track the values
    }

    // Defining a delegate type that matches the signature of the function
    public delegate void MyFunctionDelegate(); // a delegate is a pointer to a function 

    private IEnumerator WaitSec(float duration, MyFunctionDelegate ExToIdle)
    { // function to wait for a set amount of seconds
        yield return new WaitForSeconds(duration); //WaitForSeconds is a built in class, new is used to create an instance of it
        ExToIdle(); // to call the Exercise-To-Idle Animational function
    }

    // function to hide the game buttons , we transform use to manipulate and navigate the relationships between parent and child objects.
    private void DisableMeshRenderersRecursively(Transform parent)
    {
        // Disable MeshRenderers in the current GameObject
        MeshRenderer meshRenderer = parent.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

        // Disable MeshRenderers in all child GameObjects
        foreach (Transform child in parent)
        {
            DisableMeshRenderersRecursively(child);
        }
    }

    private void DisableAllMeshRenderers()
    {
        foreach (GameObject parent in buttons)
        {
            if (parent != null)
            {
                DisableMeshRenderersRecursively(parent.transform);
            }
            else
            {
                Debug.LogError("One or more parent GameObjects are not assigned in the Unity Editor.");
            }
        }
    }

    private void EnableMeshRenderers(Transform parent)
    {
        // Enable MeshRenderers in the current GameObject
        MeshRenderer meshRenderer = parent.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true;
        }

        // Enable MeshRenderers in all child GameObjects
        foreach (Transform child in parent)
        {
            EnableMeshRenderers(child);
        }
    }

    /*
     enxTimes = name of the exercise to updatee how many times it has been played
    exName = the exercise name
    time = how many secodns you want the exervise to be played before going to thte idle state
    exToIdle = the exercise to idle animation's name

    corotuine is a function to manipulate time
    The lambda expression () => is used as an argument to specify 
    a callback or action to be executed after a certain waiting period. 
     */
    public void playEx(string nameofex, string exName, float time, string exToIdle)
    {
        UpdateTimes(nameofex);
        SetExerciseState(exName);
        StartCoroutine(WaitSec(time, () => ExtoIdle(exToIdle)));
    }

    // function to unlock exercises, where "element" is the button's name
    public void progress(string exTimes, int exPlayNum, int element)
    {
        if (PlayerPrefs.GetInt(exTimes) >= exPlayNum)
        {
            if (buttons.Length > element)
            {
                EnableMeshRenderers(buttons[element].transform);
            }
        }
    }

    private void Start()
    {
        score = -1; 

        // Get the Animator component attached to the same GameObject
        animator = GetComponent<Animator>();
        popupMessageController = GetComponent<PopupMessageController>();

        PlayerPrefs.SetInt("PushupTimes", 0);
        PlayerPrefs.SetInt("BurpeeTimes", 0);
        PlayerPrefs.SetInt("PlankTimes", 0);
        PlayerPrefs.SetInt("SitupTimes", 0);
        PlayerPrefs.SetInt("SquatTimes", 0);
        Achievement1.SetActive(false);
        Achievement2.SetActive(false);
        Achievement3.SetActive(false);
        popupmessage.SetActive(false);

        DisableAllMeshRenderers();

        // Ensure that the array has elements
        if (buttons == null || buttons.Length == 0)
        { 
            Debug.LogError("No parent GameObjects assigned in the Unity Editor.");
        }


        // Ensure that the Animator component is not null
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
            return;
        }

        // Set the initial state to idle
        setidlestate();

    }


        private void Update()
    {
        ScoreText.text = "Score " + score;
        progress("PushupTimes", 3, 0);
        progress("BurpeeTimes", 3, 1);
        progress("PlankTimes", 3, 2);
        progress("SitupTimes", 3, 3);
        if (PlayerPrefs.GetInt("SquatTimes") >= 2)
        {
            if (buttons.Length > 4)
            {
                EnableMeshRenderers(buttons[4].transform);
                popupmessage.SetActive(true);
            }

        }



        if (scorectr > 10)
        {
            showobject(Achievement1);
        }
        if (scorectr > 20)
        {
            showobject(Achievement2);
        }
        if (scorectr > 30)
        {
            showobject(Achievement3);
        }

        // this is for testing purposes with a keyboard
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Alpha 1 is keycode for 1 on the keyboard
         {
            playEx("PushupTimes", "Ex1", 6.6f, "PushupsToIdle"  );

         }
        else if (Input.GetKeyDown(KeyCode.Alpha2))// Alpha 2 is keycode for 1 on the keyboard
        {
            playEx("BurpeeTimes", "Ex2", 7.2f, "Burpeetoidle");

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))// Alpha 3 is keycode for 1 on the keyboard
        {
            playEx("PlankTimes", "Ex3", 6.7f, "EndPlank");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))// Alpha 4 is keycode for 1 on the keyboard
        {
            playEx("SitupTimes", "Ex4", 8.0f, "SitupToIdle");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))// Alpha 5 is keycode for 1 on the keyboard
        {
            playEx("SquatTimes", "Ex5", 8.7f, "AirSquattoIdle");

        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))// Alpha 6 is keycode for 1 on the keyboard
        {
            SetExerciseState("Ex6");
            StartCoroutine(WaitSec(11.4f, () => ExtoIdle("BicycletoIdle")));

        }


    }
    public void ex1()
    {
        playEx("PushupTimes", "Ex1", 6.6f, "PushupsToIdle");
    }
    public void ex2()
    {
        playEx("BurpeeTimes", "Ex2", 7.2f, "Burpeetoidle");

    }
    public void ex3()
    {
        UpdateTimes("PlankTimes");
        SetExerciseState("Ex3");
        StartCoroutine(WaitSec(6.7f, () => ExtoIdle("EndPlank")));
    }
    public void ex4()
    {
        playEx("SitupTimes", "Ex4", 8.0f, "SitupToIdle");

    }
    public void ex5()
    {
        playEx("SquatTimes", "Ex5", 8.7f, "AirSquattoIdle");

    }
    public void ex6()
    {
        SetExerciseState("Ex6");
        StartCoroutine(WaitSec(11.4f, () => ExtoIdle("BicycletoIdle")));

    }



    public void showobject(GameObject Object)
    {
        Object.SetActive(true);
    }
    public void hideobject(GameObject Object)
    {
        Object.SetActive(false);
    }


    public void ExtoIdle(string ExName)
    {
        animator.Play(ExName);
    }

    private void SetExerciseState(string exerciseState)
    {
        // Play the exercise state
        animator.Play(exerciseState); 
        updatescore(0);
    }

    public void setidlestate()
    {
        SetExerciseState("Idle");
    }
    public void updatescore(int newscore)
    {
        newscore++;
        score += newscore;
        scorectr++;
    }

    public void Exiting()
    {
        Application.Quit(); 
    }
}
