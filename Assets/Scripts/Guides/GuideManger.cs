using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class EventManager : MonoBehaviour
{
    public GameObject trochar_1, bladeProximal_1, awlGuide, THandleGuide, guideWireGuide, nailGuide, drill_1, ScrewdriverProximal_1, trochar_2, bladeProximal_2, drill_2, ScrewdriverProximal_2, pladeDistal, DrillDistal, ScrewdriverDistal;

    public StepManager textDisplay; // Drag ObjectA (with ScriptA) here in the inspector
    public naimte animateScript; // Assign in inspector
    public bool isDistalLocking = false;


    private void Update()
    {
        // this is just temprory till i integrat my part with salma
        // anyway here you dipslay this message "Place trochar on bone and  mark skin"
        if (Input.GetKey(KeyCode.Q))
        {


            textDisplay.ShowTask("Place trochar on bone and  mark skin");
            // animate the trocher shit
            if (trochar_1.gameObject != null)
            {

                StartCoroutine(ActivateWithDelay(trochar_1, 2f)); // 2 seconds delay
                // trochar_1.SetActive(true);

            }
        }
    }

    // -----------------------------Added part salma ----------------------------------
    // this is called when the player is done using the skin is cut
    public void OnEventSkinCut()
    {
        // Display the task to the user
        textDisplay.ShowTask("Use The T-Handle Guide Wire");

        if (THandleGuide.gameObject != null)
        {
            StartCoroutine(ActivateWithDelay(THandleGuide, 2f)); // 2 seconds delay
        }
    }

    // this is called when the player is done using the T-Handle
    public void OnEventTHandleUsed()
    {
        // Display the task to the user
        textDisplay.ShowTask("Use The awl");

        if (awlGuide.gameObject != null)
        {
            StartCoroutine(ActivateWithDelay(awlGuide, 2f)); // 2 seconds delay
        }
    }
    // this is called when the player is done using the Awl
    public void OnEventAwlUsed()
    {
        // Display the task to the user
        textDisplay.ShowTask("Insert The Guide Wire");

        if (guideWireGuide.gameObject != null)
        {
            StartCoroutine(ActivateWithDelay(guideWireGuide, 2f)); // 2 seconds delay
        }
    }
    // this is called when the player is done inserting the guide wire to the distal end of the bone
    public void OnEventGuideWireUsed()
    {
        // Display the task to the user
        textDisplay.ShowTask("Insert The Nail");

        if (nailGuide.gameObject != null)
        {
            StartCoroutine(ActivateWithDelay(nailGuide, 2f)); // 2 seconds delay
        }
    }
    // this is called when the player is done inserting the Nail to the distal end of the bone
    public void OnEventNailUsed()
    {
        // Display the task to the user
        textDisplay.ShowTask("Remove The Guide Wire");

        if (guideWireGuide.gameObject != null)
        {
            // StartCoroutine(ActivateWithDelay(nailGuide, 2f)); // 2 seconds delay
            // Activate the Awl Guide
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

    private IEnumerator DeactivateAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }

    // -----------------------------Added part salma ----------------------------------
    public void OnEventProximalTrochar_1()
    {
        //this functuion is called when the actual trochar  is placed and made a mark on the skin   
        // now here display the following message    "use a blade through skin, spread down to bone then place trochar of sleeve on bone"
        textDisplay.ShowTask("use a blade through skin, spread down to bone then place trochar of sleeve on bone");

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
        textDisplay.ShowTask("now drill to the bone");

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
        textDisplay.ShowTask("insert screw be careful not to over tighten screws as they can sink into bone easily in metaphyseal bone");

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
        textDisplay.ShowTask("Repeat process for another screw");

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
        textDisplay.ShowTask("now drill to the bone");

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
        textDisplay.ShowTask("now drill to the bone");

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
        textDisplay.ShowTask("now drill to the bone");

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
    //     textDisplay.ShowTask("Repeat process for another screw");

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
        textDisplay.ShowTask("bring the knee into full extension");


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
        // textDisplay.ShowTask("now move to distal tibia and get perfect circles of interlock screws  "); // add delay here
        // StartCoroutine(justWait(3f)); // 2 seconds delay;

        // textDisplay.ShowTask("Move C-arm to get perfect distal tibia screw circles without rotating the leg. ");
        // StartCoroutine(justWait(3f)); // 2 seconds delay;


        // messege ==> get the Use C-arm to get perfect distal circles—don’t rotate leg
        // no animation here
    }

    public void onEventXrayShot()
    {
        // called when the xray shot is taken
        textDisplay.ShowTask("use scalpel to locate the nailhole on medial distal tibia, and incise through skin");

        //message ==>Use a blade to locate nail hole, make incision, and spread to bone.
        if (pladeDistal.gameObject != null)
        {

            // animate the distal skin cut
            StartCoroutine(ActivateWithDelay(pladeDistal, 2f)); // 2 seconds delay

        }
    }

    public void OnEventDistalCut()
    {
        // called wqhrn the distal cut is done
        textDisplay.ShowTask("drill toward center of C-arm beam");
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
        textDisplay.ShowTask("remove drill quickly and insert screw");
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
    }




    private IEnumerator waitThenAnimate()
    {
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
        textDisplay.ShowTask("Now move to distal tibia and get perfect circles of interlock screws.");
        yield return new WaitForSeconds(3f);

        textDisplay.ShowTask("Move C-arm to get perfect distal tibia screw circles without rotating the leg.");
        yield return new WaitForSeconds(3f);
    }
}