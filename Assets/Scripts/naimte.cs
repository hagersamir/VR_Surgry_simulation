using System.Collections;
using UnityEngine;

public class naimte : MonoBehaviour
{
    public GameObject oldPatient, animPatient, strightLegPatient, bone1, bone2, feumer, tibiaRigParent, feumerRigParent, pateintCover1, pateintCover2, foam, pillow, nail;
    //dont foragett to add the other attechs of the bione like the nail and the screws

    // Start is called before the first frame update
    void Start()
    {
        // showHideAnimate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void showHideAnimate()
    {
        bone2.transform.SetParent(bone1.transform);
        nail.transform.SetParent(bone1.transform);
        nail.transform.localPosition = new Vector3(-0.201704949f, 6.21703529f, 11.6516876f);
        bone1.transform.SetParent(tibiaRigParent.transform);
        feumer.transform.SetParent(feumerRigParent.transform);
        oldPatient.SetActive(false);
        animPatient.SetActive(true);


        // Start a coroutine for the 3-minute delay
        StartCoroutine(WaitAndSwitchPatient());
    }

    // Coroutine to handle the 3-minute delay
    IEnumerator WaitAndSwitchPatient()
    {
        yield return new WaitForSeconds(4); // Wait for 3 minutes (180 seconds)

        pateintCover1.SetActive(false);
        foam.SetActive(false);
        pateintCover2.SetActive(true);
        pillow.SetActive(true);
        bone1.transform.SetParent(strightLegPatient.transform);
        // bone2.transform.SetParent(strightLegPatient.transform);
        // nail.transform.SetParent(strightLegPatient.transform);
        feumer.transform.SetParent(strightLegPatient.transform);
        animPatient.SetActive(false);
        strightLegPatient.SetActive(true);
    }
}
