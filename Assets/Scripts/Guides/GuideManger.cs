using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using System;

public class EventManager : MonoBehaviour
{
    public GameObject trochar_1, bladeProximal_1, scalpelGuide, awlGuide, THandleGuide, guideWireGuide, guideWireRemovalDetector, wire1, wire2, nailGuide, drill_1, ScrewdriverProximal_1, trochar_2, bladeProximal_2, drill_2, ScrewdriverProximal_2, pladeDistal, DrillDistal, ScrewdriverDistal;
    public GameObject drill, screw1, screw2, screw3, trochar, cuttingBlade, screwDriver, carm;
    public GameObject THandle, awl, nail;
    public StepManager textDisplay; // Drag ObjectA (with ScriptA) here in the inspector
    public TextMeshProUGUI cornerText;
    public naimte animateScript; // Assign in inspector
    public bool isDistalLocking = false;

    private void Start()
    {
        THandle.GetComponent<Animator>().enabled = false;
        wire1.GetComponent<Animator>().enabled = false;
        awl.GetComponent<Animator>().enabled = false;
        nail.GetComponent<Animator>().enabled = false;
    }
    private void Update()
    {
        // this is just temprory till i integrat my part with salma
        // anyway here you dipslay this message "Place trochar on bone and  mark skin"
        if (Input.GetKey(KeyCode.O))
        {

            onEventGuideWire();
            // textDisplay.ShowTask("Place trochar on bone and  mark skin");
            // // animate the trocher shit
            // if (trochar_1.gameObject != null)
            // {

            //     StartCoroutine(ActivateWithDelay(trochar_1, 2f)); // 2 seconds delay
            //     // trochar_1.SetActive(true);

            // }
        }
    }
    private IEnumerator DelayCoroutine(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }

    // this is called when the skin is cut
    public void OnEventReductionDone()
    {
        // Display the task to the user
        // textDisplay.ShowTask("<b><color=#2A7FFF>Task 1: </color></b>Use the scalpel to make the initial incision and open the entry site over the proximal tibia");

        if (scalpelGuide.gameObject != null)
        {
            StartCoroutine(ActivateWithDelay(scalpelGuide, 2f)); // 2 seconds delay
        }
    }
    public void OnEventSkinCut()
    {
        textDisplay.HideTask();
        StartCoroutine(DelayCoroutine(2f, () => { textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b> Use the T-handle to insert the guide wire through the entry site."); ; }));

        // Display the task to the user
        // textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b> Use the T-handle to insert the guide wire through the entry site.");

        if (THandleGuide.gameObject != null)
        {
            StartCoroutine(ActivateWithDelay(THandleGuide, 2f)); // 2 seconds delay
        }
    }

    // this is called when the player is done using the T-Handle
    public void OnEventTHandleUsed()
    {
        textDisplay.HideTask();
        StartCoroutine(DelayCoroutine(2f, () => { textDisplay.ShowTask("<b><color=#2A7FFF>Task 3:</color></b> Use the awl to open the medullary canal at the guide wire entry point."); ; }));
        // Display the task to the user
        // textDisplay.ShowTask("<b><color=#2A7FFF>Task 3:</color></b> Use the awl to open the medullary canal at the guide wire entry point.");

        if (awlGuide.gameObject != null)
        {
            StartCoroutine(ActivateWithDelay(awlGuide, 2f)); // 2 seconds delay
        }
    }
    // this is called when the player is done using the Awl
    public void OnEventAwlUsed()
    {
        textDisplay.EntrySiteCompleted();
        // Display the task to the user
        // textDisplay.ShowTask("<b><color=#2A7FFF>Task 1:</color></b> Advance the guide wire through the medullary canal to the distal end of the tibia.");

        if (guideWireGuide.gameObject != null)
        {
            StartCoroutine(ActivateWithDelay(guideWireGuide, 2f)); // 2 seconds delay
        }
    }
    // this is called when the player is done inserting the guide wire to the distal end of the bone
    public void OnEventGuideWireUsed()
    {
        guideWireGuide.gameObject.SetActive(false);
        textDisplay.HideTask();
        StartCoroutine(DelayCoroutine(2f, () => { textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b> Insert the intramedullary nail over the guide wire down the canal."); ; }));
        // Display the task to the user
        // textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b> Insert the intramedullary nail over the guide wire down the canal.");

        if (nailGuide.gameObject != null)
        {
            StartCoroutine(ActivateWithDelay(nailGuide, 2f)); // 2 seconds delay
        }
    }
    // this is called when the player is done inserting the Nail to the distal end of the bone
    public void OnEventNailUsed()
    {
        textDisplay.HideTask();
        StartCoroutine(DelayCoroutine(2f, () => { textDisplay.ShowTask("<b><color=#2A7FFF>Task 3:</color></b> Remove the guide wire carefully after nail insertion."); ; }));
        // Display the task to the user
        // textDisplay.ShowTask("<b><color=#2A7FFF>Task 3:</color></b> Remove the guide wire carefully after nail insertion.");
        guideWireRemovalDetector.SetActive(true);
        wire1.SetActive(false);
        wire2.SetActive(true);

        if (guideWireGuide.gameObject != null)
        {
            // StartCoroutine(ActivateWithDelay(nailGuide, 2f)); // 2 seconds delay
            // Activate the guide wire Guide
            guideWireGuide.SetActive(true);

            // Get the Animator component
            Animator animator = guideWireGuide.GetComponent<Animator>();

            if (animator != null)
            {
                // Play the animation in reverse
                animator.Play("GuideWireGuide", 0, 1f); // Start at the end (normalizedTime = 1f)
                animator.speed = -1; // Play backward

                // Start coroutine to deactivate after 2 seconds
                StartCoroutine(DeactivateAfterSeconds(guideWireGuide, 2f));
            }
        }
    }

    public void onEventGuideWire()
    {
        textDisplay.NailInsertionCompleted();
        drill.SetActive(true);
        trochar.SetActive(true);
        screw1.SetActive(true);
        screw2.SetActive(true);
        screw3.SetActive(true);
        cuttingBlade.SetActive(true);
        screwDriver.SetActive(true);

        // textDisplay.ShowTask("Place trochar on bone and  mark skin");
        // animate the trocher shit
        if (trochar_1.gameObject != null)
        {
            textDisplay.ShowTask("<b><color=#2A7FFF>Task 1:</color></b>Place trochar on bone and  mark skin");
            StartCoroutine(ActivateWithDelay(trochar_1, 2f)); // 2 seconds delay
                                                              // trochar_1.SetActive(true);

        }
    }

    private IEnumerator DeactivateAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }

    public void OnEventProximalTrochar_1()
    {
        //this functuion is called when the actual trochar  is placed and made a mark on the skin   
        // now here display the following message    "use a blade through skin, spread down to bone then place trochar of sleeve on bone"
        textDisplay.HideTask();
        StartCoroutine(DelayCoroutine(2f, () => { textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b> Use a blade through skin, spread down to bone then place trochar of sleeve on bone"); ; }));
        // textDisplay.ShowTask("use a blade through skin, spread down to bone then place trochar of sleeve on bone");

        // animate the skin cutting
        if (bladeProximal_1.gameObject != null)
        {

            StartCoroutine(ActivateWithDelay(bladeProximal_1, 2f)); // 2 seconds delay
            // bladeProximal_1.SetActive(true);

        }
    }

    public void OnEventProximalCut_1()
    {
        // this function is CallerLineNumberAttribute when the skin cut is displyed on the skin
        // now display this meesage "now drill to the bone"
        textDisplay.HideTask();
        StartCoroutine(DelayCoroutine(2f, () => { textDisplay.ShowTask("<b><color=#2A7FFF>Task 3:</color></b> Now drill to the bone"); ; }));
        // textDisplay.ShowTask("now drill to the bone");

        // animiate the bone drilling
        if (drill_1.gameObject != null)
        {

            StartCoroutine(ActivateWithDelay(drill_1, 2f)); // 2 seconds delay
            // drill_1.SetActive(true);

        }

    }

    public void OnEventProximalDrill_1()
    {
        // called whent the bone is drilled
        // now display the messege" insert screw be careful not to over tighten screws as they can sink into bone easily in metaphyseal bone"
        textDisplay.HideTask();
        StartCoroutine(DelayCoroutine(2f, () => { textDisplay.ShowTask("<b><color=#2A7FFF>Task 4:</color></b> Insert a screw, be careful not to over tighten screws as they can sink into bone easily in metaphyseal bone"); ; }));
        // textDisplay.ShowTask("insert screw be careful not to over tighten screws as they can sink into bone easily in metaphyseal bone");

        // animete the screw locking
        if (ScrewdriverProximal_1.gameObject != null)
        {

            StartCoroutine(ActivateWithDelay(ScrewdriverProximal_1, 2f)); // 2 seconds delay
            // ScrewdriverProximal_1.SetActive(true);

        }
    }

    public void OnEventProximalScrew_1()
    {
        // called when the first proximal screw is attached to the bone
        // display the message "Repeat process for another screw"
        textDisplay.HideTask();
        StartCoroutine(DelayCoroutine(2f, () => { textDisplay.ShowTask("<b><color=#2A7FFF>Task 5:</color></b>Repeat the process for another screw"); ; }));
        // textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b>Repeat process for another screw");

        //animate tthe tcher inside second hole
        if (trochar_2.gameObject != null)
        {

            StartCoroutine(ActivateWithDelay(trochar_2, 2f)); // 2 seconds delay
            // trochar_2.SetActive(true);

        }
    }

    //============================================================================================================================================================================================================================================================================================================

    public void OnEventProximalTrochar_2()
    {
        //this functuion is called when the actual trochar  is placed and made a mark on the skin
        // animate the skin cutting
        textDisplay.HideTask();
        StartCoroutine(DelayCoroutine(2f, () => { textDisplay.ShowTask("<b><color=#2A7FFF>Task 6:</color></b>Make a cut."); ; }));
        // textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b>make a cut");

        if (bladeProximal_2.gameObject != null)
        {

            StartCoroutine(ActivateWithDelay(bladeProximal_2, 2f)); // 2 seconds delay
            // drill_1.SetActive(true);

        }
    }

    public void OnEventProximalCut_2()
    {
        // this function is CallerLineNumberAttribute when the skin cut is displyed on the skin
        // animiate the bone drilling
        textDisplay.HideTask();
        StartCoroutine(DelayCoroutine(2f, () => { textDisplay.ShowTask("<b><color=#2A7FFF>Task 7:</color></b>Now drill to the bone"); ; }));
        // textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b>now drill to the bone");

        if (drill_2.gameObject != null)
        {

            StartCoroutine(ActivateWithDelay(drill_2, 2f)); // 2 seconds delay
            // drill_1.SetActive(true);

        }
    }

    public void OnEventProximalDrill_2()
    {
        // called whent the bone is drilled
        // animete the screw locking
        textDisplay.ShowTask("<b><color=#2A7FFF>Task 8:</color></b>apply second screw proximal lock");

        if (ScrewdriverProximal_2.gameObject != null)
        {

            StartCoroutine(ActivateWithDelay(ScrewdriverProximal_2, 2f)); // 2 seconds delay
            // drill_1.SetActive(true);

        }
    }

    // public void OnEventProximalScrew_2()
    // {
    //     // called when the first proximal screw is attached to the bone
    //     // display the message "Repeat process for another screw"
    //     textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b>Repeat process for another screw");

    //     //animate tthe tcher inside second hole
    //     if (trochar_2.gameObject != null)
    //     {

    //         StartCoroutine(ActivateWithDelay(trochar_2, 2f)); // 2 seconds delay
    //         // trochar_2.SetActive(true);

    //     }
    //     // called when the screw is attached to the bone
    //     //animate tthe tcher inside second hole
    // }

    //==============================================================================================================================================================================================================
    public void OnEventProximalLockingDone()
    {


        //this is called when thte proximal locking is done AKA after the second screw is palced
        // message ==> "Remove targeting guide and jig from nail and bring the knee into full extension"
        textDisplay.ShowTask("<b><color=#2A7FFF>Task 9:</color></b>bring the knee into full extension");


        // animeate spreading the leg  and hide the aiming guide or just animate the real one and remove all tringle shit 
        StartCoroutine(waitThenAnimate());

        isDistalLocking = true;
        OnEventKneeExtnsion();

        // and replace the sheet and attach bones and screws and all that shit to the rig then unattach it after the animation


    }

    public void OnEventKneeExtnsion()
    {
        StartCoroutine(KneeExtensionSequence());

        // called when the knee is in full extension now 
        // textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b>now move to distal tibia and get perfect circles of interlock screws  "); // add delay here
        // StartCoroutine(justWait(3f)); // 2 seconds delay;

        // textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b>Move C-arm to get perfect distal tibia screw circles without rotating the leg. ");
        // StartCoroutine(justWait(3f)); // 2 seconds delay;


        // messege ==> get the Use C-arm to get perfect distal circles—don’t rotate leg
        // no animation here

        textDisplay.ShowTask("<b><color=#2A7FFF>Task 10:</color></b>use scalpel to locate the nailhole on medial distal tibia, and incise through skin");

        //message ==>Use a blade to locate nail hole, make incision, and spread to bone.
        if (pladeDistal.gameObject != null)
        {

            // animate the distal skin cut
            StartCoroutine(ActivateWithDelay(pladeDistal, 5f)); // 2 seconds delay

        }
    }



    public void OnEventDistalCut()
    {
        // called wqhrn the distal cut is done
        textDisplay.ShowTask("<b><color=#2A7FFF>Task 11:</color></b>drill toward center of C-arm beam");
        // message ==>Drill toward C-arm center
        if (DrillDistal.gameObject != null)
        {

            // animate the distal drilling 
            StartCoroutine(ActivateWithDelay(DrillDistal, 2f)); // 2 seconds delay

        }
    }

    public void OnEventDistalDrilling()
    {
        // called whren distal drilling is done
        textDisplay.ShowTask("<b><color=#2A7FFF>Task 12:</color></b>remove drill quickly and insert screw");
        //message==> insert screw
        if (ScrewdriverDistal.gameObject != null)
        {

            // animate the distal screww
            StartCoroutine(ActivateWithDelay(ScrewdriverDistal, 2f)); // 2 seconds delay

        }
    }


    public void OnEventDistalDistalLockin()
    {
        // called whren distal lock is done
        textDisplay.ShowTask("good boy");
        textDisplay.Locking_ClosureCompleted();
        //message==> insert screw
        //  if (ScrewdriverDistal.gameObject != null)
        // {

        // // animate the distal screww
        //     StartCoroutine(ActivateWithDelay(ScrewdriverDistal, 2f)); // 2 seconds delay

        // }
    }


    private IEnumerator ActivateWithDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
        // yield return new WaitForSeconds(4);
        // obj.SetActive(false);
    }




    private IEnumerator waitThenAnimate()
    {
        yield return new WaitForSeconds(6f);
        carm.transform.SetPositionAndRotation(new Vector3(-0.426999986f, 0f, -2.91000009f), Quaternion.Euler(0f, 198.909882f, 0f));
        yield return new WaitForSeconds(6f);
        animateScript.showHideAnimate();
        yield return new WaitForSeconds(3f); // if needed after the animation
    }
    private IEnumerator justWait(float delay)
    {
        yield return new WaitForSeconds(delay);
        // obj.SetActive(true);
    }

    private IEnumerator KneeExtensionSequence()
    {
        // textDisplay.ShowTask("Now move to distal tibia and get perfect circles of interlock screws.");
        // yield return new WaitForSeconds(3f);

        textDisplay.ShowTask("<b><color=#2A7FFF>Task 2:</color></b>Move C-arm to get perfect distal tibia screw circles without rotating the leg.");
        yield return new WaitForSeconds(3f);
    }
}