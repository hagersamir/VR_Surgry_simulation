using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class EventManager : MonoBehaviour
{
    public GameObject trochar_1, bladeProximal_1, drill_1, ScrewdriverProximal_1, trochar_2, bladeProximal_2, drill_2, ScrewdriverProximal_2;
    public StepManager textDisplay; // Drag ObjectA (with ScriptA) here in the inspector

    private void Update()
    {
        // this is just temprory till i integrat my part with salma
        // anyway here you dipslay this message "Place trochar on bone and  mark skin"
        if (Input.GetKey(KeyCode.Q))
        {


            textDisplay.ShowTask("Place trochar on bone and  mark skin");
            // animate the trocher shit
            trochar_1.SetActive(true);
        }
    }
    public void OnEventProximalTrochar_1()
    {
        //this functuion is called when the actual trochar  is placed and made a mark on the skin   
        // now here display the following message    "use a blade through skin, spread down to bone then place trochar of sleeve on bone"
        textDisplay.ShowTask("use a blade through skin, spread down to bone then place trochar of sleeve on bone");

        // animate the skin cutting
        bladeProximal_1.SetActive(true);
    }

    public void OnEventProximalCut_1()
    {
        // this function is CallerLineNumberAttribute when the skin cut is displyed on the skin
        // now display this meesage "now drill to the bone"
        textDisplay.ShowTask("now drill to the bone");

        // animiate the bone drilling
        drill_1.SetActive(true);

    }

    public void OnEventProximalDrill_1()
    {
        // called whent the bone is drilled
        // now display the messege" insert screw be careful not to over tighten screws as they can sink into bone easily in metaphyseal bone"
        textDisplay.ShowTask("insert screw be careful not to over tighten screws as they can sink into bone easily in metaphyseal bone");

        // animete the screw locking
        ScrewdriverProximal_1.SetActive(true);
    }

    public void OnEventProximalScrew_1()
    {
        // called when the screw is attached to the bone
        // display the message "Repeat process for another screw"
        textDisplay.ShowTask("Repeat process for another screw");

        //animate tthe tcher inside second hole
        trochar_2.SetActive(true);
    }

//============================================================================================================================================================================================================================================================================================================

    public void OnEventProximalTrochar_2()
    {
        //this functuion is called when the actual trochar  is placed and made a mark on the skin
        // animate the skin cutting
        textDisplay.ShowTask("now drill to the bone");

        drill_1.SetActive(true);
    }

    public void OnEventProximalCut_2()
    {
        // this function is CallerLineNumberAttribute when the skin cut is displyed on the skin
        // animiate the bone drilling
        textDisplay.ShowTask("now drill to the bone");

        drill_1.SetActive(true);
    }

    public void OnEventProximalDrill_2()
    {
        // called whent the bone is drilled
        // animete the screw locking
        textDisplay.ShowTask("now drill to the bone");

        drill_1.SetActive(true);
    }

    public void OnEventProximalScrew_2()
    {
        // called when the screw is attached to the bone
        //animate tthe tcher inside second hole
    }


    public void OnEventProximalLockingDone()
    {
        //this is called when thte proximal locking is done AKA after the second screw is palced
        // message ==> "Remove targeting guide and jig from nail and bring the knee into full extension"
        // animeate spreading the leg 
    }

    public void OnEventKneeExtnsion()
    {
        // called when the knee is in full extension now 
        // messege ==> get the Use C-arm to get perfect distal circles—don’t rotate leg
        // no animation here
    }

    public void onEventXrayShot()
    {
        // called when the xray shot is taken
        //message ==>Use a blade to locate nail hole, make incision, and spread to bone.
        // animate the distal skin cut
    }

    public void OnEventDistalCut()
    {
        // called wqhrn the distal cut is done
        // message ==>Drill toward C-arm center
        // animate the distal drilling 
    }

    public void OnEventDistalDrilling()
    {
        // called whren distal drilling is done
        //message==> insert screw
    }
}
